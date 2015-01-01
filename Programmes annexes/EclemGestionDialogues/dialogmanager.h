#ifndef FILEMANAGER_H
#define FILEMANAGER_H

#include <QMessageBox>

#include <QTextEdit>
#include <QFile>
#include <QTextStream>
#include <QString>
#include <QJsonDocument>
#include <QJsonArray>
#include <QJsonObject>
#include <QStringList>
#include <QStringListModel>

#include <QDebug>

// Constante WORKING_DIRECTORY définie car les commandes sur QT permettant de trouver le dossier courant renvoient le dossier constitué par le .app
#define WORKING_DIRECTORY "/Users/mnds/QtProjects/build-EclemGestionDialogues-Desktop_Qt_5_3_clang_64bit-Debug/"

class DialogManager
{
    QString filename;
    QJsonArray jsonReplicas; // Encapsule l'ensemble des répliques liées à un dialogue
    QJsonArray jsonFlags; // Encapsule la liste exhaustive des flags utilisés dans le jeu
    QStringList goList; // Liste des gameObject du document
    QStringList flagsList; // Liste de tous les flags (id + description)
    bool validDialog; // Renseigne sur la validité du fichier choisi comme fichier de dialogue

public:
    DialogManager( QString filename );
    QJsonArray loadJson( QString filename, QString arrayName); // Charge le fichier Json donné en paramètre (filename), et récupère le tableau arrayName avant de le renvoyer
    static QJsonObject createReplica( QString id, QString go, QString text, QStringList nextReplicasId, QStringList requiredFlags, QStringList blockingFlags, QStringList enabledFlags );
    QJsonArray getReplicas(); // Retourne le tableau Json de répliques
    QStringList getIdList( QString data = "repliques" ); // Liste des tous les id de la structure dont le nom a été donné en paramètre (par défaut les répliques, mais peut aussi servir pour les flags)
    QStringList getGoList(); // Liste des game object
    QStringList getToDisplayFlagsList( QString separator = "-"); // Liste qui sera affichée à l'utilisateur, avec l'id du flag et la description de celui-ci séparés par "separator"
    static QStringList toFlagIdList( QStringList displayedFlagList ); // Transforme la liste des flags affichés (id+description) en simple liste d'id
    bool updateReplica( QJsonObject newReplica, int position ); // Met à jour la réplique située à l'index position de la liste des répliques
    bool updateDialogFile();
    void deleteReplicaFromJson( QString id );
    void insertReplica( QJsonObject replicaObj, int index );
    bool isValidDialog();
};

#endif // FILEMANAGER_H
