#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QWidget>
#include <QVBoxLayout>
#include <QGridLayout>
#include <QHBoxLayout>
#include <QTextEdit>
#include <QLabel>
#include <QScrollArea>
#include <QComboBox>
#include <QLineEdit>
#include <QTreeView>

#include <QListView>
#include <QStringListModel>
#include <QModelIndex>

#include <QFileDialog>

#include <QDebug>

#include "dialogmanager.h"
#include "actionbutton.h"
#include "addreplicacombo.h"
#include "customdelegate.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    QPushButton* newDialogButton; // Permet la création d'un nouveau dialogue
    QPushButton* editDialogButton; // Permet la modification d'un dialogue (déjà existant)
    QScrollArea* centralArea; // Zone centrale de la fenêtre principale
    QVBoxLayout* startLayout; // Layout de début
    QVBoxLayout* dialogLayout; // Layout d'ajout de dialogues

    DialogManager* dialogManager;

    bool changesSaved; // Renseigne sur l'état de sauvegarde des modifications

public:
    explicit MainWindow(QWidget *parent = 0);
        
    ~MainWindow();

private:
    Ui::MainWindow *ui;
    QWidget* dialogWidget;
    QLayout* newReplicaLayout( const QJsonValue & value = 0); // Construit le layout de la réplique donnée en argument | Champs vides si l'argument est nul
    QLayout* newGoLayout( QString goName, bool editState = false ); /* Construit le layout d'un game object,
                                                                         * avec le nom donné en paramètre étant sélectionné par défaut dans la liste déroulante
                                                                         * editState informe de la possibilité de modidifier ou non les widgets créés
                                                                         */
    QLayout* getGoLayout( QLayout* replicaLayout );
    QLayout* getNextReplicasIdLayout( QLayout* replicaLayout );
    QGridLayout* getFlagsLayout( QLayout* replicaLayout );
    QLayout* newNextReplicasLayout( QJsonArray nextReplicasArray, bool editState = false );
    QLayout* newReplicaIdLayout( QString replicaId, bool editState = false);
    QList<QWidget*> newEditFlagWidgets( QJsonArray flagsArray, QString flagType, bool editState = false ); /* Crée une liste de widget pour les flags de la réplique
                                                  * flagType peut avoir les valeurs "required", "blocking", "enabled"
                                                  */
    QGridLayout* newFlagsLayout( QJsonArray requiredFlagsArray, QJsonArray blockingFlagsArray, QJsonArray enabledFlagsArray, bool editState = false );
    QAction* saveAction;
    QAction* newReplicaAction;
    QAction* newDialogAction;
    QAction* openDialogAction;

    void displayStartLayout(); // Construit et affiche l'écran de démarrage
    void displayDialogLayout(); // instancie et rajoute tous les widgets nécessaires à l'interface d'édition des dialogues// Le paramètre est le layout qui était affiché avant et qui doit être supprimé
    void setHeight (QWidget* edit, int nRows);
    void displayMenuBar(); // Construit et affiche la barra des menus
    void displayToolbar(); // Construit et affiche la barre d'outils
    bool saveReplica( QLayout* replicaLayout );
    void setNextReplicasEnable( QLayout* replicaLayout, bool state );
    void setIdEnable( QLayout* replicaLayout, bool state );
    void setGoEnable( QLayout* replicaLayout, bool state );
    void setTextAreaEnable( QLayout* replicaLayout, bool state );
    void setFlagsEnable(QLayout* replicaLayout, bool state);
    void deleteLayout( QLayout* layout, QString tab = 0 ); // Fonction permettant la suppression récursive des layouts
    int replicaLayoutPosition( QLayout* replicaLayout );
    QString getReplicaId( QLayout* replicaLayout );
    void updateCentralArea(); // Permet un réajustement des tailles des widgets après une insertion/suppression de dialogue
    void updateNextReplicasLists(); // Met à jour la liste des id dans les listes déroulantes des répliques suivantes

public slots:
    void delReplica( QLayout* replicaLayout ); // Supprime la réplique donnée en paramètre de l'interface et du document Json
    QJsonObject getReplicaFromGUI( QLayout* replicaLayout ); // Récupère les informations des champs d'une replicaLayout pour créer un QJsonObject
    void saveDialogFile(); // Enregistre le document Json sur le fichier du dialogue
    void enableSaveAction(); // Active le bouton d'enregistrement des répliques sur le fichier
    void addId( int row, QStringListModel* model, QComboBox* ); // Ajoute l'id donné en paramètre au modèle "model"
    void addFlag(int row, QStringListModel* model, QComboBox* transmitterCb);
    void setReplicaEditState( QLayout* replicaLayout, bool editState, ActionButton* editButton); // Permet d'activer ou désactiver la modification sur la réplique donnée en paramètre
    void newEmptyReplica( QLayout* replicaBefore = 0 ); // Insère une réplique vide juste après la réplique donnée en paramètre
    void displayFileBrowserToOpenDialog(); // Navigateur de fichiers pour ouvrir un fichier dialogue
    void displayFileBrowserToCreateDialog(); // Navigateur de fichiers pour créer un fichier dialogue
    void initializeDialog( QString dialogFilename );
    void askForSavingChanges();
};

#endif // MAINWINDOW_H
