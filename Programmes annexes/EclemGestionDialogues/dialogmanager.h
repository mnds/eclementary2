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
#define WORKING_DIRECTORY "D:\Centrale Lyon\Année 2\PA - Jeu vidéo\Programmes annexes\EclemGestionDialogues"

class DialogManager
{
    QString filename;
    QJsonArray jsonReplicas;
    QStringList goList; // Liste des gameObject du document
    bool validDialog; // Renseigne sur la validité du fichier choisi comme fichier de dialogue

public:
    DialogManager( QString filename );
    static QJsonObject createReplica( QString id, QString go, QString text, QStringList nextReplicasId );
    QJsonArray getReplicas(); // Retourne le tableau Json de répliques
    QStringList getIdList(); // Liste des id de toutes les répliques
    QStringList getGoList();
    bool updateReplica( QJsonObject newReplica, int position ); // Met à jour la réplique située à l'index position de la liste des répliques
    bool updateDialogFile();
    void deleteReplicaFromJson( QString id );
    void insertReplica( QJsonObject replicaObj, int index );
    bool isValidDialog();
};

#endif // FILEMANAGER_H
