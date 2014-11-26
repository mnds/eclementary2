#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    changesSaved = true;
    dialogWidget = new QWidget;
    dialogWidget->setSizePolicy( QSizePolicy::Expanding, QSizePolicy::Expanding);

    displayMenuBar();
    displayToolbar();
    centralArea = new QScrollArea;

    displayStartLayout();
    // Interface d'ajout de dialogue

    centralArea->setFixedSize( 500, 600 );
    setCentralWidget( centralArea );
}

void MainWindow::displayMenuBar() {
    QMenu* fileMenu = menuBar()->addMenu("&Fichier");
    QMenu* newMenu = fileMenu->addMenu("&Nouveau");

    // Actions du menu "Nouveau"
    newReplicaAction = new QAction("Réplique", this);
    newDialogAction = new QAction("Dialogue", this);
    QString newReplicaIconPath = "icones/add.png";
    newReplicaAction->setIcon( QIcon(WORKING_DIRECTORY+ newReplicaIconPath) );
    newReplicaAction->setEnabled( false ); // Désactivé au lancement du programme

    // Action d'enregistrement
    saveAction = new QAction("Enregistrer le dialogue", this);
    saveAction->setShortcut(QKeySequence("Ctrl+S"));
    QString iconName = "icones/save.png";
    saveAction->setIcon( QIcon( WORKING_DIRECTORY + iconName ) );
    saveAction->setEnabled( false );

    fileMenu->addAction( saveAction );
    newMenu->addAction( newDialogAction );
    newMenu->addAction( newReplicaAction );

    connect( saveAction, SIGNAL(triggered()), this, SLOT(saveDialogFile()) );
    connect( newReplicaAction, SIGNAL(triggered()), this, SLOT(newEmptyReplica())); // Permet un ajout en queue
    connect( newReplicaAction, SIGNAL(triggered()), this, SLOT(enableSaveAction()));
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

    startLayout = new QVBoxLayout;
    startLayout->addWidget( newDialogButton );
    startLayout->addWidget( editDialogButton );

    centralArea->setLayout( startLayout );
}

void MainWindow::displayFileBrowserToOpenDialog() {
    QString filter = QString("Dialogues (*.json)");
    QString filename = QFileDialog::getOpenFileName(this, "Ouvrir un dialogue", QString(), filter);
    if( !filename.isEmpty() ) {
        dialogManager = new DialogManager( filename );
        displayDialogLayout();
    }
}

void MainWindow::displayFileBrowserToCreateDialog() {
    QString filter = QString("Dialogues (*.json)");
    QString filename = QFileDialog::getSaveFileName(this, "Enregistrer un fichier", QString(), filter);
    if( !filename.isEmpty()) {
        dialogManager = new DialogManager( filename );
        displayDialogLayout();
    }
}

void MainWindow::displayDialogLayout() {
    // Écran d'édition des dialogues
    dialogLayout = new QVBoxLayout();

    QJsonArray replicas = dialogManager->getReplicas();
    if( dialogManager->isValidDialog() ) {
        int pos = 0; // Ligne correspondant à la réplique dans la présentation
        if( !dialogManager->getReplicas().isEmpty()) {
            foreach( const QJsonValue & value, replicas ) {
                QVBoxLayout* replicaLayout = newReplicaLayout( value );
                if( replicaLayout != NULL ) {
                    dialogLayout->addLayout( replicaLayout );
                    pos++;
                }
            }
        }
        else
            newEmptyReplica(); // Création d'une réplique vide si le fichier du dialogue est vide

        // Changement de layout
        deleteLayout( startLayout );
        dialogWidget->setLayout( dialogLayout );
        centralArea->setWidget(dialogWidget);
        // Activation de la possibilité d'ajouter une réplique (dans la barre d'outils et le menu)
        newReplicaAction->setEnabled( true );
    }
    else
        QMessageBox::information( NULL, "Notification", "Aucune réplique n'a été trouvée dans le fichier?");

}

QVBoxLayout* MainWindow::newReplicaLayout(const QJsonValue &value ) {
    QJsonObject replicaObj;
    QString id, goName, text;
    QVBoxLayout* replicaLayout = NULL ;
    QJsonArray nextReplicasIdArray;
    bool editState = false;

    if( value != 0 ) {
        replicaObj = value.toObject();
        id = replicaObj["id"].toString();
        goName = replicaObj["goAssocie"].toString();
        text = replicaObj["texte"].toString();
        nextReplicasIdArray = replicaObj["repSuivantes"].toArray();
    }
    else
        editState = true; // Création d'une réplique vide, modification possible dès sa création


    QLayout* replicaIdLayout = newReplicaIdLayout( id, editState );
    QHBoxLayout* goLayout = newGoLayout( goName, editState );
    QTextEdit* textArea = new QTextEdit();
    QGridLayout* nextReplicasLayout = newNextReplicasLayout( nextReplicasIdArray, editState );

    textArea->setText( text );

    // Mise en forme des QTextEdit
    setHeight( textArea, 5 );

    // Les widgets ne sont pas modifiables, jusqu'à ce que l'utilisateur le demande explicitement
    textArea->setReadOnly( !editState ); // setReadOnly ne se lit pas dans le meme sens que les autres propriétés

    replicaLayout = new QVBoxLayout();
    replicaLayout->addLayout( replicaIdLayout );
    replicaLayout->addLayout( goLayout );
    replicaLayout->addWidget( textArea );
    replicaLayout->addLayout( nextReplicasLayout );

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
    idLineEdit->setEnabled( editState );

    idLayout->addWidget( idLabel );
    idLayout->addWidget( idLineEdit );

    idLayout->setSizeConstraint(QLayout::SetMinimumSize);

    return idLayout;
}


QGridLayout* MainWindow::newNextReplicasLayout( QJsonArray nextReplicasArray, bool editState ) {
    QGridLayout* layout = new QGridLayout();
    QLabel* label = new QLabel();
    QListView* listView = new QListView();
    QStringList idList = QStringList();
    QStringListModel* model = new QStringListModel();

    label->setText("Répliques suivantes");
    //if( nextReplicasArray  ) // si ce n'est pas une réplique vide
    foreach( const QJsonValue & nextValue, nextReplicasArray )
        idList.append( nextValue.toString() );

    model->setStringList( idList );
    listView->setModel( model );
    listView->setItemDelegate( new CustomDelegate(model) );

    AddReplicaCombo* cb = new AddReplicaCombo( model );
    cb->addItem("Lier à une réplique");
    cb->addItems( dialogManager->getIdList());

    // Modification des widgets possible ou non, selon la valeur de editState
    QAbstractItemView::EditTrigger editTrigger = editState ? QAbstractItemView::DoubleClicked : QAbstractItemView::NoEditTriggers;
    listView->setEditTriggers( editTrigger );
    cb->setEnabled( editState );

    connect( listView, SIGNAL(doubleClicked(const QModelIndex &)), this, SLOT(enableSaveAction()) ); // Un double clic, et donc la suppression d'un id, active le bouton d'enregistrement
    connect( cb, SIGNAL(currentIndexChanged( int )), this, SLOT(enableSaveAction()) ); // Un ajout d'id entraine l'activation du bouton d'enregistrement
    connect( cb, SIGNAL( currentIndexChanged( int ) ), cb, SLOT(emitModel( int )) );
    connect( cb, SIGNAL( currentIndexChanged( int, QStringListModel*, QComboBox* ) ), this, SLOT(addReplicaId(int, QStringListModel*, QComboBox*)) );

    layout->addWidget( label, 0, 0 );
    layout->addWidget( listView, 0, 1 );
    layout->addWidget( cb, 1, 1 );
    layout->setSizeConstraint( QLayout::SetMinimumSize);

    return layout;
}

void MainWindow::addReplicaId( int row, QStringListModel* model, QComboBox* cb ) {
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

QHBoxLayout* MainWindow::newGoLayout(QString goName, bool editState) {
    QHBoxLayout* layout = new QHBoxLayout();
    QLabel* label = new QLabel();
    QLineEdit* lineEdit = new QLineEdit();

    label->setText( "Game Object associé:" );
    lineEdit->setText( goName );
    lineEdit->setEnabled( editState );

    layout->addWidget( label );
    layout->addWidget( lineEdit );
    layout->setSizeConstraint( QLayout::SetMinimumSize);

    connect( lineEdit, SIGNAL(textChanged(const QString &)), this, SLOT(enableSaveAction()));

    return layout;
}

QJsonObject MainWindow::getReplicaFromGUI( QLayout* replicaLayout ) {
    QJsonObject replica;
    QString id, text, goText;
    QStringList nextReplicasId = QStringList();

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

        QGridLayout* nextReplicasIdLayout = getNextReplicasIdLayout( replicaLayout );
        if( nextReplicasIdLayout != NULL ) {
            QListView* listView = dynamic_cast<QListView*>(nextReplicasIdLayout->itemAtPosition( 0, 1 )->widget()); // Label des répliques suivantes
            if( listView != NULL)
                nextReplicasId = static_cast<QStringListModel*>(listView->model())->stringList();
        }
        replica = DialogManager::createReplica( id, goText, text, nextReplicasId );
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

QGridLayout* MainWindow::getNextReplicasIdLayout( QLayout* replicaLayout ) {
    QGridLayout* nextReplicasIdLayout = NULL;
    
    if( replicaLayout != NULL )
            nextReplicasIdLayout = dynamic_cast<QGridLayout*>(replicaLayout->itemAt( 3 )->layout());

    return nextReplicasIdLayout;
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
    QGridLayout* nextReplicasLayout = getNextReplicasIdLayout( replicaLayout );

    if( nextReplicasLayout != NULL ) {
        QListView* listView = dynamic_cast<QListView*>(nextReplicasLayout->itemAtPosition(0, 1)->widget());
        AddReplicaCombo* cb = dynamic_cast<AddReplicaCombo*>(nextReplicasLayout->itemAtPosition(1, 1)->widget());
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

void MainWindow::setHeight (QTextEdit* edit, int nRows)
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
        if( !saveReplica( replicaLayout ) )
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
        QGridLayout* layout = getNextReplicasIdLayout( dialogLayout->itemAt(i)->layout());
        AddReplicaCombo* cb = dynamic_cast<AddReplicaCombo*>(layout->itemAtPosition(1,1)->widget());
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

void MainWindow::setReplicaEditState( QLayout* replicaLayout, bool editState, ActionButton* editButton) {
    setGoEnable( replicaLayout, !editState );
    setTextAreaEnable( replicaLayout, !editState );
    setNextReplicasEnable( replicaLayout, !editState);

    editButton->setEditState( !editState );
}

void MainWindow::deleteLayout(QLayout *layout) {
    QLayoutItem* item;

    while( (item = layout->takeAt(0)) != 0 ) {
        if( item->layout() != NULL ) // Suppression récursive
            deleteLayout( item->layout() );
        else { // le widget est d'abord caché avant d'être supprimé
            item->widget()->hide();
            delete item;
        }
    }
    delete layout;
}

void MainWindow::delReplica( QLayout* replicaLayout ) {
    QString replicaId = getReplicaId( replicaLayout );
    deleteLayout( replicaLayout );
    dialogManager->deleteReplicaFromJson( replicaId ); // Suppression de la réplique du document Json
    updateCentralArea();
}

void MainWindow::newEmptyReplica( QLayout* replicaBefore ) {
    QLayout* newLayout = newReplicaLayout(); // Layout de réplique vide
    QJsonObject emptyReplica = dialogManager->createReplica("", "", "", QStringList()); // réplique vide
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
    //QWidget* replacementWidget = dialogWidget;
    dialogWidget = new QWidget();
    dialogWidget->setLayout( dialogLayout );
    centralArea->setWidget( dialogWidget );
    //delete replacementWidget;
}

MainWindow::~MainWindow()
{
    delete dialogLayout;
    delete centralArea;
    delete dialogWidget;
    delete saveAction;
    delete ui;
}
