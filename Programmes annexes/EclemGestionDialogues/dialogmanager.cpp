#include "dialogmanager.h"

DialogManager::DialogManager( QString filename ):
    filename( filename )
{
    QFile dialogFile( filename );
    validDialog = false;
    if( dialogFile.open( QIODevice::ReadWrite | QIODevice::Text ) ) {
        QByteArray fileContent = dialogFile.readAll();
        if( fileContent.isEmpty() )
            validDialog = true; // Un fichier vide est considéré comme valide
        else {
            QJsonDocument jsonContent = QJsonDocument::fromJson( fileContent ); // Conversion du contenu du fichier en objet json
            if( !jsonContent.isNull() ) {
                QJsonObject jsonObject = jsonContent.object();
                if( jsonObject["repliques"].isArray() ) {
                    validDialog = true;  // Un fichier avec la structure Json attendue est considéré comme valide
                    jsonReplicas = jsonObject["repliques"].toArray();
                }
            }
        }
        dialogFile.close();
    }
}

bool DialogManager::isValidDialog() {
    return validDialog;
}

QJsonArray DialogManager::getReplicas() {
    return jsonReplicas;
}

QStringList DialogManager::getIdList() {
    QStringList idList = QStringList();
    foreach( const QJsonValue & value, jsonReplicas ) {
        QJsonObject replicaObj = value.toObject();
        if( replicaObj["id"].toString() != "0")
            idList.append( replicaObj["id"].toString() );
    }

    return idList;
}

QStringList DialogManager::getGoList() {
    if( goList.isEmpty() ) {
        goList = QStringList();
        foreach( const QJsonValue & value, jsonReplicas ) {
            QJsonObject replicaObj = value.toObject();
            QString goName = replicaObj["goAssocie"].toString();
            if( !goName.isEmpty() && !goList.contains( goName )) // Ajout si seulement il n'est pas déjà présent dans la liste
                goList.append( goName );
        }
    }
    return goList;
}

QJsonObject DialogManager::createReplica(QString id, QString go, QString text, QStringList nextReplicasId) {
    QJsonObject replica = QJsonObject();
    QJsonArray nextId = QJsonArray();

    // Conversion des QString en QJsonValue
    foreach( QString id, nextReplicasId )
        nextId.append( QJsonValue( id ) );

    replica.insert( "id", QJsonValue( id ) );
    replica.insert( "goAssocie", go );
    replica.insert( "texte", QJsonValue( text ) );
    replica.insert( "repSuivantes", QJsonValue(nextId) );

    return replica;
}

bool DialogManager::updateReplica( QJsonObject newReplica, int position) {
    bool done = false;

    if( jsonReplicas.isEmpty() ) // On vérifie si le document n'est pas vide
        jsonReplicas.append( QJsonValue(newReplica) );
    else if( position >=0 && position < jsonReplicas.size() ) { // On vérifie que l'index est valide
        jsonReplicas.removeAt( position ); // Suppression de l'ancienne réplique
        jsonReplicas.insert( position, QJsonValue( newReplica ) ); // Insertion de la nouvelle réplique, à la meme position que l'ancienne

        done = true;
    }

    return done;
}

bool DialogManager::updateDialogFile() {
    bool done = false;

    QFile dialogFile( filename );
    QJsonObject root = QJsonObject();
    root.insert( "repliques", QJsonValue(jsonReplicas) );
    // Réécriture complète du fichier
    if( dialogFile.exists() && dialogFile.open( QIODevice::WriteOnly | QIODevice::Truncate | QIODevice::Text ) ) {
        QTextStream stream( &dialogFile );
        QJsonDocument jsonDocument = QJsonDocument( root );
        stream << jsonDocument.toJson();
        dialogFile.close();

        done = true;
    }
    else
        QMessageBox::critical( NULL, "Erreur", "Le fichier du dialogue n'a pu etre trouvé.");

    return done;
}

void DialogManager::insertReplica(QJsonObject replicaObj, int index) {
    if( index >= jsonReplicas.count() )
        jsonReplicas.append(QJsonValue(replicaObj));
    else
        jsonReplicas.insert( index, QJsonValue(replicaObj));
}

void DialogManager::deleteReplicaFromJson( QString id ) {
    int i = 0;
    bool found = false;

    // Recherche de la réplique correspondante
    while( i < jsonReplicas.count() && !found ) {
        QJsonObject replicaObj = jsonReplicas[i].toObject();
        found = id == replicaObj["id"].toString();
        i++;
    }

    // Suppression de la réplique du document Json
    if( found )
        jsonReplicas.removeAt(i-1);
}
