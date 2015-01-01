#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    // Initialisation
    ui->setupUi(this);
    setWindowTitle( "Eclementary2, Gestion des dialogues");
    changesSaved = true;
    dialogManager = NULL;
    dialogLayout = NULL;
    startLayout = new QVBoxLayout();
    dialogWidget = new QWidget();
    dialogWidget->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);

    displayMenuBar();
    displayToolbar();
    centralArea = new QScrollArea;

    displayStartLayout();
    // Interface d'ajout de dialogue

    centralArea->setFixedSize( 530, 600 );
    setCentralWidget( centralArea );
}

void MainWindow::displayMenuBar() {
    QMenu* fileMenu = menuBar()->addMenu("&Fichier");
    QMenu* newMenu = fileMenu->addMenu("&Nouveau");
    QMenu* openMenu = fileMenu->addMenu("&Ouvrir");

    // Actions du menu "Nouveau"
    QAction* quitAction = new QAction("Quitter", this);
    newReplicaAction = new QAction("Réplique", this);
    newDialogAction = new QAction("Dialogue", this);
    openDialogAction = new QAction("Dialogue existant", this);
    QString newReplicaIconPath = "icones/add.png";
    newReplicaAction->setIcon( QIcon(WORKING_DIRECTORY+ newReplicaIconPath) );
    newReplicaAction->setEnabled( false ); // Désactivé au lancement du programme

    // Action d'enregistrement
    saveAction = new QAction("Enregistrer le dialogue", this);
    saveAction->setShortcut(QKeySequence("Ctrl+S"));
    QString iconName = "icones/save.png";
    saveAction->setIcon( QIcon( WORKING_DIRECTORY + iconName ) );
    saveAction->setEnabled( false );

    newDialogAction->setShortcut(QKeySequence("Ctrl+N"));
    openDialogAction->setShortcut(QKeySequence("Ctrl+O"));
    quitAction->setShortcut(QKeySequence("Ctrl+Q"));

    fileMenu->addAction( saveAction );
    newMenu->addAction( newDialogAction );
    newMenu->addAction( newReplicaAction );
    openMenu->addAction( openDialogAction );
    fileMenu->addAction( quitAction );

    connect( saveAction, SIGNAL(triggered()), this, SLOT(saveDialogFile()) );
    connect( newReplicaAction, SIGNAL(triggered()), this, SLOT(newEmptyReplica())); // Permet un ajout en queue
    connect( newReplicaAction, SIGNAL(triggered()), this, SLOT(enableSaveAction()));
    connect( newDialogAction, SIGNAL(triggered()), this, SLOT(displayFileBrowserToCreateDialog()) );
    connect( openDialogAction, SIGNAL(triggered()), this, SLOT(displayFileBrowserToOpenDialog()) );
    connect( quitAction, SIGNAL(triggered()), qApp, SLOT(quit()));
}

void MainWindow::displayToolbar() {
    QToolBar* saveToolbar = addToolBar("Enregistrer le dialogue");
    QToolBar* addReplicaToolbar = addToolBar("Nouvelle réplique");

    saveToolbar->addAction( saveAction );
    addReplicaToolbar->addAction( newReplicaAction );
}

void MainWindow::displayStartLayout() {
    // Écran de démarrage
    newDialogButton = new QPushButton("Créer un nouveau dialogue");
    editDialogButton = new QPushButton("Modifier un dialogue");

    connect( editDialogButton, SIGNAL(clicked()), this, SLOT(displayFileBrowserToOpenDialog()) );
    connect( newDialogButton, SIGNAL(clicked()), this, SLOT(displayFileBrowserToCreateDialog()) );

    startLayout->addWidget( newDialogButton );
    startLayout->addWidget( editDialogButton );

    centralArea->setLayout( startLayout );
}

void MainWindow::displayFileBrowserToOpenDialog() {
    askForSavingChanges();
    QString filter = QString("Dialogues (*.json)");
    QString filename = QFileDialog::getOpenFileName(this, "Ouvrir un dialogue", QString(), filter);
    if( !filename.isEmpty() ) {
        setWindowTitle(QString("Eclementary2, Gestion des dialogues: " + filename));
        initializeDialog( filename );
        displayDialogLayout();
    }
}

void MainWindow::displayFileBrowserToCreateDialog() {
    askForSavingChanges();
    QString filter = QString("Dialogues (*.json)");
    QString filename = QFileDialog::getSaveFileName(this, "Enregistrer un fichier", QString(), filter);
    if( !filename.isEmpty() ) {
        initializeDialog( filename );
        displayDialogLayout();
    }
}

void MainWindow::askForSavingChanges() {
    if( !changesSaved ) { // La demande est faite si les modifications n'ont pas été enregistrées
        int answer = QMessageBox::question(this, "Enregistrement des modifications", "Voulez-vous enregistrer vos dernières modifications avant d'ouvrir un nouveau dialogue ?");
        if( answer == QMessageBox::Yes ) { // Enregistrement si réponse affirmative de l'utilisateur
            saveDialogFile();
            changesSaved = true;
        }
    }
}

void MainWindow::initializeDialog( QString dialogFilename ) {
    if( !dialogFilename.isEmpty()) {
        if( dialogManager != NULL ) {
            delete dialogManager;
            dialogManager = NULL;
        }
        if( startLayout != NULL ) {
            deleteLayout( startLayout);
            startLayout = NULL;
        }
        if( dialogLayout != NULL ) {
            deleteLayout( dialogLayout);
            dialogLayout = NULL;
        }
        dialogManager = new DialogManager( dialogFilename );
        dialogLayout = new QVBoxLayout();
    }
}

void MainWindow::displayDialogLayout() {
    // Écran d'édition des dialogues
    QJsonArray replicas = dialogManager->getReplicas();
    if( dialogManager->isValidDialog() ) {
        int pos = 0; // Ligne correspondant à la réplique dans la présentation
        if( !dialogManager->getReplicas().isEmpty()) {
            foreach( const QJsonValue & value, replicas ) {
                QLayout* replicaLayout = newReplicaLayout( value );
                if( replicaLayout != NULL ) {
                    dialogLayout->addLayout( replicaLayout );
                    pos++;
                }
            }
        }
        else
            newEmptyReplica(); // Création d'une réplique vide si le fichier du dialogue est vide
        // Activation de la possibilité d'ajouter une réplique (dans la barre d'outils et le menu)
        newReplicaAction->setEnabled( true );
        // Changement de layout
        updateCentralArea();
    }
    else
        QMessageBox::information( NULL, "Notification", "Aucune réplique n'a été trouvée dans le fichier?");

}

QLayout* MainWindow::newReplicaLayout(const QJsonValue &value ) {
    QJsonObject replicaObj;
    QString id, goName, text;
    QVBoxLayout* replicaLayout = NULL ;
    QJsonArray nextReplicasIdArray, requiredFlagsArray, blockingFlagsArray, enabledFlagsArray;
    bool editState = false;

    if( value != 0 ) {
        replicaObj = value.toObject();
        id = replicaObj["id"].toString();
        goName = replicaObj["goAssocie"].toString();
        text = replicaObj["texte"].toString();
        nextReplicasIdArray = replicaObj["repSuivantes"].toArray();
        requiredFlagsArray = replicaObj["flagsRequis"].toArray();
        blockingFlagsArray = replicaObj["flagsBloquants"].toArray();
        enabledFlagsArray = replicaObj["flagsActives"].toArray();
    }
    else
        editState = true; // Création d'une réplique vide, modification possible dès sa création


    QLayout* replicaIdLayout = newReplicaIdLayout( id, editState );
    QLayout* goLayout = newGoLayout( goName, editState );
    QTextEdit* textArea = new QTextEdit();
    QLayout* nextReplicasLayout = newNextReplicasLayout( nextReplicasIdArray, editState );
    QGridLayout* flagsLayout = newFlagsLayout( requiredFlagsArray, blockingFlagsArray, enabledFlagsArray, editState);

    textArea->setText( text );
    textArea->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);

    // Mise en forme des QTextEdit
    setHeight( textArea, 5 );

    // Les widgets ne sont pas modifiables, jusqu'à ce que l'utilisateur le demande explicitement
    textArea->setReadOnly( !editState ); // setReadOnly ne se lit pas dans le meme sens que les autres propriétés

    replicaLayout = new QVBoxLayout();
    replicaLayout->addLayout( replicaIdLayout );
    replicaLayout->addLayout( goLayout );
    replicaLayout->addWidget( textArea );
    replicaLayout->addLayout( nextReplicasLayout );
    replicaLayout->addLayout( flagsLayout );

    // Boutons d'édition
    QString addPath = "icones/add.png",editPath = "icones/edit.png", delPath = "icones/del.png";
    ActionButton* insertButton = new ActionButton( replicaLayout, WORKING_DIRECTORY + addPath ); // Permet d'insérer une réplique juste en dessous de l'emplacement de la réplique à laquelle il est associé
    ActionButton* editButton = new ActionButton( replicaLayout, WORKING_DIRECTORY + editPath, editState ); // Permet de modifier la réplique
    ActionButton* delButton = new ActionButton( replicaLayout, WORKING_DIRECTORY + delPath ); // Permet de supprimer la réplique

    // Connection des boutons avec les traitements associés
    connect( textArea, SIGNAL(textChanged()), this, SLOT(enableSaveAction()) );
    connect( insertButton, SIGNAL(clicked()), insertButton, SLOT(sendPosition()) );
    connect( insertButton, SIGNAL(position(QLayout*)), this, SLOT(newEmptyReplica(QLayout*)));
    connect( insertButton, SIGNAL(clicked()), this, SLOT(enableSaveAction()) );
    connect( editButton, SIGNAL(clicked()), editButton, SLOT(sendAllDetails()) );
    connect( editButton, SIGNAL(allDetails(QLayout*,bool,ActionButton*)), this, SLOT(setReplicaEditState(QLayout*,bool,ActionButton*)));
    connect( delButton, SIGNAL(clicked()), this, SLOT(enableSaveAction()) );
    connect( delButton, SIGNAL(clicked()), delButton, SLOT(sendPosition()) );
    connect( delButton, SIGNAL(position(QLayout*)), this, SLOT(delReplica(QLayout*)) );

    // Placement des boutons
    QHBoxLayout* buttonsLayout = new QHBoxLayout;
    buttonsLayout->addWidget( insertButton );
    buttonsLayout->addWidget( editButton );
    buttonsLayout->addWidget( delButton );

    replicaLayout->addLayout( buttonsLayout );
    replicaLayout->setSizeConstraint( QLayout::SetMinimumSize);

    return replicaLayout;
}

QLayout* MainWindow::newReplicaIdLayout( QString replicaId, bool editState ) {
    QLayout* idLayout = new QHBoxLayout();

    QLabel* idLabel = new QLabel("ID: ");
    QLineEdit* idLineEdit = new QLineEdit( replicaId );

    // Règlage de la taille des widget
    idLabel->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);
    idLineEdit->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);

    idLineEdit->setEnabled( editState ); // Réglage de l'état d'activation

    idLayout->addWidget( idLabel );
    idLayout->addWidget( idLineEdit );

    idLayout->setAlignment( Qt::AlignLeft); // Alignement à gauche

    //idLayout->setSizeConstraint(QLayout::SetMinimumSize);

    return idLayout;
}


QLayout* MainWindow::newNextReplicasLayout( QJsonArray nextReplicasArray, bool editState ) {
    QLayout* layout = new QHBoxLayout();
    QLabel* label = new QLabel();
    QListView* listView = new QListView();
    QStringList idList = QStringList();
    QStringListModel* model = new QStringListModel();

    label->setText("Répliques<br/> suivantes");
    label->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);
    //if( nextReplicasArray  ) // si ce n'est pas une réplique vide
    foreach( const QJsonValue & nextValue, nextReplicasArray )
        idList.append( nextValue.toString() );

    model->setStringList( idList );
    listView->setModel( model );
    listView->setItemDelegate( new CustomDelegate(model) );
    listView->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);
    setHeight(listView, 3);

    AddReplicaCombo* cb = new AddReplicaCombo( model );
    cb->addItem("Lier à une réplique");
    cb->addItems( dialogManager->getIdList());
    cb->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);

    // Modification des widgets possible ou non, selon la valeur de editState
    QAbstractItemView::EditTrigger editTrigger = editState ? QAbstractItemView::DoubleClicked : QAbstractItemView::NoEditTriggers;
    listView->setEditTriggers( editTrigger );
    cb->setEnabled( editState );

    connect( listView, SIGNAL(doubleClicked(const QModelIndex &)), this, SLOT(enableSaveAction()) ); // Un double clic, et donc la suppression d'un id, active le bouton d'enregistrement
    connect( cb, SIGNAL(currentIndexChanged( int )), this, SLOT(enableSaveAction()) ); // Un ajout d'id entraine l'activation du bouton d'enregistrement
    connect( cb, SIGNAL( currentIndexChanged( int ) ), cb, SLOT(emitModel( int )) );
    connect( cb, SIGNAL( currentIndexChanged( int, QStringListModel*, QComboBox* ) ), this, SLOT(addId(int, QStringListModel*, QComboBox*)) );

    layout->addWidget( label );
    layout->addWidget( listView );
    layout->addWidget( cb );
    //layout->setSizeConstraint( QLayout::SetMinimumSize);

    return layout;
}

QList<QWidget*> MainWindow::newEditFlagWidgets( QJsonArray flagsArray, QString flagType, bool editState ) {
    QList<QWidget*> widgetsList;
    QLabel* label = new QLabel();
    QListView* listView = new QListView();
    QStringList flagsList = QStringList();
    QStringListModel* model = new QStringListModel();

    QString labelText;
    // Texte du label trouvé en fonction du type de flag
    if( flagType == "required" )
        labelText = "Flags <br/> requis";
    else if( flagType == "blocking" )
        labelText = "Flags <br/> bloquants";
    else if( flagType == "enabled" )
        labelText = "Flags <br/> activés";

    label->setText( labelText );
    label->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);

    // Constitution de la liste des flags à afficher
    foreach( const QJsonValue & flagJson, flagsArray ) {
        QString flag = flagJson.toString();
        int flagIndex = dialogManager->getIdList("flags").indexOf( flag ); // Récupération de l'index du flag
        if( flagIndex != -1 ) // Si le flag a été trouvé
            flagsList.append( dialogManager->getToDisplayFlagsList().at( flagIndex ) ); // Ajout de la chaine correspondante
    }

    // Paramétrage du modèle
    model->setStringList( flagsList );
    listView->setModel( model );
    listView->setItemDelegate( new CustomDelegate(model) );
    listView->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);
    setHeight(listView, 3);

    // Configuration de la liste déroulante des suggestions de flags (pour l'ajout)
    AddReplicaCombo* cb = new AddReplicaCombo( model );
    cb->addItem("Lier à un flag");
    cb->addItems( dialogManager->getToDisplayFlagsList());
    cb->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);

    // Modification des widgets possible ou non, selon la valeur de editState
    QAbstractItemView::EditTrigger editTrigger = editState ? QAbstractItemView::DoubleClicked : QAbstractItemView::NoEditTriggers;
    listView->setEditTriggers( editTrigger );
    cb->setEnabled( editState );

    // Connexion des boutons
    connect( listView, SIGNAL(doubleClicked(const QModelIndex &)), this, SLOT(enableSaveAction()) ); // Un double clic, et donc la suppression d'un id, active le bouton d'enregistrement
    connect( cb, SIGNAL(currentIndexChanged( int )), this, SLOT(enableSaveAction()) ); // Un ajout de flag entraine l'activation du bouton d'enregistrement
    connect( cb, SIGNAL( currentIndexChanged( int ) ), cb, SLOT(emitModel( int )) );
    connect( cb, SIGNAL( currentIndexChanged( int, QStringListModel*, QComboBox* ) ), this, SLOT(addFlag(int, QStringListModel*, QComboBox*)) );

    widgetsList.append( label );
    widgetsList.append( listView );
    widgetsList.append( cb );

    return widgetsList;
}

QGridLayout* MainWindow::newFlagsLayout( QJsonArray requiredFlagsArray, QJsonArray blockingFlagsArray, QJsonArray enabledFlagsArray, bool editState ) {
    QGridLayout* layout = new QGridLayout();
    QList<QWidget*> requiredFlagsList = newEditFlagWidgets( requiredFlagsArray, "required", editState );
    QList<QWidget*> blockingFlagsList = newEditFlagWidgets( blockingFlagsArray, "blocking", editState );
    QList<QWidget*> enabledFlagsList = newEditFlagWidgets( enabledFlagsArray, "enabled", editState );

    layout->addWidget( requiredFlagsList.at(0), 0, 0 ); // label
    layout->addWidget( requiredFlagsList.at(1), 0, 1 ); // listView
    layout->addWidget( requiredFlagsList.at(2), 0, 2 ); // liste déroulante

    layout->addWidget( blockingFlagsList.at(0), 1, 0 ); // label
    layout->addWidget( blockingFlagsList.at(1), 1, 1 ); // listView
    layout->addWidget( blockingFlagsList.at(2), 1, 2 ); // liste déroulante

    layout->addWidget( enabledFlagsList.at(0), 2, 0 ); // label
    layout->addWidget( enabledFlagsList.at(1), 2, 1 ); // listView
    layout->addWidget( enabledFlagsList.at(2), 2, 2 ); // liste déroulante

    return layout;
}


void MainWindow::addId( int row, QStringListModel* model, QComboBox* cb ) {
    if( row > 0 ) { // La ligne 0 correspond au texte de description de la combobox (Lier à une réplique)
        QString idToBeInserted = dialogManager->getIdList()[row-1];
        if( !model->stringList().contains( idToBeInserted ) ) { // L'id ne doit pas non plus etre présent auparavant dans la liste des répliques suivantes de la réplique considérée
            model->insertRow(model->rowCount()); // Ajout d'une nouvelle ligne
            QModelIndex index = model->index(model->rowCount()-1); // récupération de l'index
            model->setData(index, idToBeInserted); // Remplissage de la nouvelle ligne avec la valeur sélectionnée dans la liste déroulante
        }
        cb->setCurrentIndex(0); // Le texte de la liste déroulante est remis à sa valeur par défaut (texte de description de la liste déroulante)
    }
}

void MainWindow::addFlag(int row, QStringListModel* model, QComboBox* transmitterCb) {
    if( row > 0 ) { // La ligne 0 correspond au texte de description de la combobox
        QString lineToBeInserted = dialogManager->getToDisplayFlagsList()[row-1]; // Récupération de la ligne à insérer
        if( !model->stringList().contains( lineToBeInserted ) ) { // La ligne à insérer ne doit pas etre présente auparavant dans le modèle
            model->insertRow(model->rowCount()); // Ajout d'une nouvelle ligne
            QModelIndex index = model->index(model->rowCount()-1); // récupération de l'index de la ligne nouvellement créée (en queue)
            model->setData(index, lineToBeInserted); // Remplissage de la nouvelle ligne avec la valeur sélectionnée dans la liste déroulante
        }
        transmitterCb->setCurrentIndex(0); // Le texte de la liste déroulante est remis à sa valeur par défaut (texte de description de la liste déroulante)
    }
}

QLayout* MainWindow::newGoLayout(QString goName, bool editState) {
    QHBoxLayout* layout = new QHBoxLayout();
    QLabel* label = new QLabel();
    QLineEdit* lineEdit = new QLineEdit();

    label->setText( "Game Object associé:" );
    lineEdit->setText( goName );
    label->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);
    lineEdit->setSizePolicy( QSizePolicy::Fixed, QSizePolicy::Fixed);
    lineEdit->setEnabled( editState );

    layout->addWidget( label );
    layout->addWidget( lineEdit );
    layout->setAlignment( Qt::AlignLeft);

    connect( lineEdit, SIGNAL(textChanged(const QString &)), this, SLOT(enableSaveAction()));

    return layout;
}

QJsonObject MainWindow::getReplicaFromGUI( QLayout* replicaLayout ) {
    QJsonObject replica;
    QString id, text, goText;
    QStringList nextReplicasId = QStringList();
    QStringList requiredFlags = QStringList();
    QStringList blockingFlags = QStringList();
    QStringList enabledFlags = QStringList();

    if( replicaLayout != NULL) { // Recherche des widget contenant les informations à modifier
        id = getReplicaId( replicaLayout );

        QLayout* goLayout = getGoLayout( replicaLayout );
        if( goLayout != NULL ) {
            QLineEdit* goLineEdit = dynamic_cast<QLineEdit*>(goLayout->itemAt( 1 )->widget());
            if( goLineEdit != NULL )
                goText = goLineEdit->text();
        }

        QTextEdit* textEdit = dynamic_cast<QTextEdit*>(replicaLayout->itemAt( 2 )->widget()); // TextEdit où est écrit le text de la réplique
        if( textEdit != NULL )
            text = textEdit->toPlainText();

        QLayout* nextReplicasIdLayout = getNextReplicasIdLayout( replicaLayout );
        if( nextReplicasIdLayout != NULL ) {
            QListView* listView = dynamic_cast<QListView*>(nextReplicasIdLayout->itemAt( 1 )->widget()); // Label des répliques suivantes
            if( listView != NULL)
                nextReplicasId = static_cast<QStringListModel*>(listView->model())->stringList();
        }

        QGridLayout* flagsLayout = getFlagsLayout( replicaLayout );
        if( flagsLayout != NULL ) {
            QListView* requiredFlagsListView = dynamic_cast<QListView*>(flagsLayout->itemAtPosition(0, 1)->widget());
            QListView* blockingFlagsListView = dynamic_cast<QListView*>(flagsLayout->itemAtPosition(1, 1)->widget());
            QListView* enabledFlagsListView = dynamic_cast<QListView*>(flagsLayout->itemAtPosition(2, 1)->widget());

            if( requiredFlagsListView != NULL )
                requiredFlags = DialogManager::toFlagIdList( static_cast<QStringListModel*>(requiredFlagsListView->model())->stringList() );
            if( blockingFlagsListView != NULL )
                blockingFlags = DialogManager::toFlagIdList( static_cast<QStringListModel*>(blockingFlagsListView->model())->stringList() );
            if( enabledFlagsListView != NULL )
                enabledFlags = DialogManager::toFlagIdList( static_cast<QStringListModel*>(enabledFlagsListView->model())->stringList() );
        }

        replica = DialogManager::createReplica( id, goText, text, nextReplicasId, requiredFlags, blockingFlags, enabledFlags );
    }

    return replica;
}


QString MainWindow::getReplicaId( QLayout* replicaLayout ) {
    QString id;

    if( replicaLayout != NULL ) {
        QLayout* idLayout = replicaLayout->itemAt( 0 )->layout(); // Label de l'id de la réplique
        if( idLayout != NULL ) {
            QLineEdit* idLineEdit = dynamic_cast<QLineEdit*>(idLayout->itemAt(1)->widget());
            id = idLineEdit->text();
        }
    }

    return id;
}

QLayout* MainWindow::getGoLayout( QLayout* replicaLayout ) {
    QLayout* goLayout = NULL ;
    
    if( replicaLayout != NULL )
            goLayout = replicaLayout->itemAt( 1 )->layout();
    
    return goLayout;
}

QLayout* MainWindow::getNextReplicasIdLayout( QLayout* replicaLayout ) {
    QLayout* nextReplicasIdLayout = NULL;
    
    if( replicaLayout != NULL )
        nextReplicasIdLayout = replicaLayout->itemAt( 3 )->layout();

    return nextReplicasIdLayout;
}

QGridLayout* MainWindow::getFlagsLayout( QLayout* replicaLayout ) {
    QGridLayout* flagsLayout = NULL;

    if( replicaLayout != NULL )
        flagsLayout = dynamic_cast<QGridLayout*>(replicaLayout->itemAt( 4 )->layout());

    return flagsLayout;
}

void MainWindow::setReplicaEditState( QLayout* replicaLayout, bool editState, ActionButton* editButton) {
    setIdEnable( replicaLayout, !editState );
    setGoEnable( replicaLayout, !editState );
    setTextAreaEnable( replicaLayout, !editState );
    setNextReplicasEnable( replicaLayout, !editState);
    setFlagsEnable( replicaLayout, !editState );

    editButton->setEditState( !editState );
}

void MainWindow::setIdEnable( QLayout* replicaLayout, bool state ) {
    QLineEdit* idLineEdit;

    if( replicaLayout != NULL ) {
        QLayout* idLayout = replicaLayout->itemAt( 0 )->layout(); // Layout de l'id de la réplique
        if( idLayout != NULL ) {
            idLineEdit = dynamic_cast<QLineEdit*>(idLayout->itemAt(1)->widget());
            idLineEdit->setEnabled( state );
        }
    }
}

void MainWindow::setGoEnable(QLayout* replicaLayout, bool state) {
    QLayout* goLayout = getGoLayout( replicaLayout );
    
    if( goLayout != NULL ) {
        QLineEdit* lineEdit = dynamic_cast<QLineEdit*>(goLayout->itemAt(1)->widget());
        if( lineEdit != NULL )
            lineEdit->setEnabled( state );
    }
}

void MainWindow::setNextReplicasEnable(QLayout* replicaLayout, bool state) {
    QLayout* nextReplicasLayout = getNextReplicasIdLayout( replicaLayout );

    if( nextReplicasLayout != NULL ) {
        QListView* listView = dynamic_cast<QListView*>(nextReplicasLayout->itemAt(1)->widget());
        AddReplicaCombo* cb = dynamic_cast<AddReplicaCombo*>(nextReplicasLayout->itemAt(2)->widget());
        if( listView != NULL ) {
            QAbstractItemView::EditTrigger editTriggers = state == true ? QAbstractItemView::DoubleClicked : QAbstractItemView::NoEditTriggers;
            listView->setEditTriggers( editTriggers );
        }
        if( cb != NULL)
            cb->setEnabled( state );
    }
}

void MainWindow::setFlagsEnable(QLayout* replicaLayout, bool state) {
    QGridLayout* flagsLayout = getFlagsLayout( replicaLayout );

    for( int i = 0; i < 3; i++ ) {
        QListView* listView = dynamic_cast<QListView*>( flagsLayout->itemAtPosition( i, 1)->widget() );
        AddReplicaCombo* cb = dynamic_cast<AddReplicaCombo*>( flagsLayout->itemAtPosition( i, 2)->widget() );

        if( listView != NULL ) {
            QAbstractItemView::EditTrigger editTriggers = state == true ? QAbstractItemView::DoubleClicked : QAbstractItemView::NoEditTriggers;
            listView->setEditTriggers( editTriggers );
        }
        if( cb != NULL)
            cb->setEnabled( state );
    }
}

void MainWindow::setTextAreaEnable(QLayout* replicaLayout, bool state) {
    QTextEdit* textArea = NULL;

    if( replicaLayout != NULL ) {
        textArea = dynamic_cast<QTextEdit*>(replicaLayout->itemAt( 2 )->widget());
        if( textArea != NULL )
            textArea->setReadOnly( !state );
    }
}

void MainWindow::setHeight (QWidget* edit, int nRows)
{
    QFontMetrics m (edit -> font()) ;
    int RowHeight = m.lineSpacing() ;
    edit -> setFixedHeight  (nRows * RowHeight) ;
}

bool MainWindow::saveReplica(QLayout* replicaLayout) {
    QJsonObject modifiedReplica = getReplicaFromGUI( replicaLayout );
    bool ok = true;
    int position = replicaLayoutPosition( replicaLayout );

    if( position == -1 || !dialogManager->updateReplica( modifiedReplica, position) )// position = -1 si la réplique n'a pas été trouvée
        ok = false;

    return ok;
}

int MainWindow::replicaLayoutPosition(QLayout *replicaLayout) {
    QLayout* layout;
    bool found = false;
    int i = 0;

    // Arrêt ssi le layout a été trouvé ou i a dépassé le nombre de fils de dialogLayout
    while( !found && (i < dialogLayout->count()) ) {
        layout = dialogLayout->itemAt(i)->layout();
        found = (layout == replicaLayout);
        i++;
    }

    if( found )
        i -= 1;
    else
        i = -1; // renvoie -1 si la réplique n'a pas été trouvée

    return i;
}

void MainWindow::saveDialogFile() {
    // Mise à jour du document JSON
    bool ok = true;
    int i = 0;
    // Transaction: Soit toutes les répliques sont mises à jour sur le fichier, soit aucune ne l'est
    while( ok && (i < dialogManager->getReplicas().count()) ){
        QLayout* replicaLayout = dialogLayout->itemAt( i )->layout();
        if( !saveReplica( replicaLayout ) ) // Enregistrement de la réplique et test de la valeur de retour
            ok = false;
        i++;
    }

    if( ok ) {
        dialogManager->updateDialogFile(); // Mise à jour sur le fichier
        saveAction->setEnabled( false ); // Désactivation du bouton d'enregistrement, jusqu'à la prochaine modification
        changesSaved = true;
        updateNextReplicasLists(); // Met à jour la liste des id dans les listes déroulantes des répliques suivantes
    }
    else
        QMessageBox::critical( NULL, "Erreur", "Erreur lors de la modification de l'enregistrement des répliques sur le fichier. Opération abandonnée.");

}

void MainWindow::updateNextReplicasLists() {
    for( int i = 0; i < dialogLayout->count(); i++ ) {
        QLayout* layout = getNextReplicasIdLayout( dialogLayout->itemAt(i)->layout());
        AddReplicaCombo* cb = dynamic_cast<AddReplicaCombo*>(layout->itemAt(2)->widget());
        QStringList comboText = QStringList() << "Lier une réplique" << dialogManager->getIdList();
        cb->updateNextReplicasId( comboText );
    }
}

void MainWindow::enableSaveAction() {
    if( !saveAction->isEnabled() ) {
        saveAction->setEnabled(true);
        changesSaved = false;
    }
}

void MainWindow::deleteLayout(QLayout *layout, QString tab) {
    QLayoutItem* item;
    //qDebug() << "Fils " << layout->count();
    if( layout != NULL ) {
        //qDebug() << tab + "Layout entrée";
        while( (item = layout->takeAt(0)) != 0 ) {
            if( item->layout() != 0 ) {// Suppression récursive si c'est un layout
                deleteLayout( item->layout(), tab+"     " );
            }
            else if( item->widget() != 0 ) { // le widget est d'abord caché avant d'être supprimé
                //qDebug() << tab + "     " + "Widget";
                item->widget()->hide();
                delete item->widget();
            }
        }
        //qDebug() << tab + "Layout sortie";
        delete layout;
    }
}

void MainWindow::delReplica( QLayout* replicaLayout ) {
    QString replicaId = getReplicaId( replicaLayout );
    deleteLayout( replicaLayout );
    dialogManager->deleteReplicaFromJson( replicaId ); // Suppression de la réplique du document Json
    updateCentralArea();
}

void MainWindow::newEmptyReplica( QLayout* replicaBefore ) {
    QLayout* newLayout = newReplicaLayout(); // Layout de réplique vide
    QJsonObject emptyReplica = dialogManager->createReplica("", "", "", QStringList(), QStringList(), QStringList(), QStringList()); // réplique vide
    int position; // position à laquelle sera insérée la réplique dans le document Json

    if( replicaBefore != 0 ) { // Insertion si une position est spécifiée
        position = replicaLayoutPosition( replicaBefore ) + 1;
        dialogLayout->insertLayout( position, newLayout);
    }
    else {// Ajout à la queue s'il n'y a pas de position spécifiée
        position = dialogLayout->count();
        dialogLayout->addLayout( newLayout );
    }
    updateCentralArea(); // Mise à jour de l'affichage

    // Mise à jour du document Json
    dialogManager->insertReplica( emptyReplica, position); // Insertion à la queue pour le moment
}

void MainWindow::updateCentralArea() {
    dialogWidget = new QWidget();
    dialogWidget->setLayout( dialogLayout );
    centralArea->setWidget( dialogWidget );
}

MainWindow::~MainWindow()
{
    delete dialogLayout;
    delete centralArea;
    delete dialogWidget;
    delete saveAction;
    delete ui;
}
