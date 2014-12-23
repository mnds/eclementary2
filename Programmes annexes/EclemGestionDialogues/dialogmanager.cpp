#include "dialogmanager.h"

DialogManager::DialogManager( QString filename ):
    filename( filename )
{
    jsonReplicas = loadJson( filename, "repliques" ); // Le tableau de répliques dans un fichier de dialogue doit etre nommé "repliques"
    jsonFlags = loadJson( WORKING_DIRECTORY + QString("flags"), "flags"); // les flags sont renseignés dans le fichier flags et le tableau de flags dans le fichier de flags doit etre nommé "flags"
}

QJsonArray DialogManager::loadJson(QString filename, QString arrayName) {
    QJsonArray arrayData;
    QFile dialogFile( filename );

    if( dialogFile.open( QIODevice::ReadWrite | QIODevice::Text ) ) { // ouverture du fichier en mode lecture écriture
        QByteArray fileContent = dialogFile.readAll(); // lecture du contenu du fichier
        if( fileContent.isEmpty() ) // vérification du contenu du fichier
            validDialog = true; // Un fichier vide est considéré comme valide
        else {
            QJsonDocument jsonContent = QJsonDocument::fromJson( fileContent ); // Conversion du contenu du fichier en objet json
            if( !jsonContent.isNull() ) {
                QJsonObject jsonObject = jsonContent.object(); // Récupération de l'objet englobant
                if( jsonObject[arrayName].isArray() ) { // Vérification de l'existence du tableau dont le nom a été donné en paramètre
                    validDialog = true;  // Un fichier avec la structure Json attendue est considéré comme valide
                    arrayData = jsonObject[arrayName].toArray(); // Récupération du tableau de données
                }
                else
                    validDialog = false;
            }
            else
                validDialog = false;
        }
        dialogFile.close(); // Fermeture du fichier
    }

    return arrayData;
}

bool DialogManager::isValidDialog() {
    return validDialog;
}

QJsonArray DialogManager::getReplicas() {
    return jsonReplicas;
}

QStringList DialogManager::getIdList( QString arrayName ) {
    QJsonArray jsonArray;
    QStringList idList = QStringList();

    if( arrayName == "repliques" )
        jsonArray = jsonReplicas; // Les id des répliques sont récupérés
    else if( arrayName == "flags" )
        jsonArray = jsonFlags; // Sinon les id des flags sont récupérés

    foreach( const QJsonValue & value, jsonArray ) {
        QJsonObject object = value.toObject();
        if( object["id"].toString() != "0") // Par convention, l'id 0 n'est jamais renvoyé
            idList.append( object["id"].toString() );
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

QStringList DialogManager::getToDisplayFlagsList( QString separator ) {
    if( flagsList.isEmpty() ) {
        flagsList = QStringList();
        foreach( const QJsonValue & value, jsonFlags ) {
            QJsonObject jsonObject = value.toObject();
            QString line = jsonObject["id"].toString() + separator + jsonObject["description"].toString(); // Ligne du tableau
            flagsList.append( line );
        }
    }

    return flagsList;
}

QJsonObject DialogManager::createReplica( QString id, QString go, QString text, QStringList nextReplicasId, QStringList requiredFlags, QStringList blockingFlags, QStringList enabledFlags ) {
    QJsonObject replica = QJsonObject();
    QJsonArray nextId = QJsonArray(), rFlagsArray = QJsonArray(), bFlagsArray = QJsonArray(), eFlagsArray = QJsonArray();
    // Conversion des QString en QJsonValue
    foreach( QString id, nextReplicasId )
        nextId.append( QJsonValue( id ) );
    foreach( QString flag, requiredFlags )
        rFlagsArray.append( QJsonValue(flag) );
    foreach( QString flag, blockingFlags )
        bFlagsArray.append( QJsonValue(flag) );
    foreach( QString flag, enabledFlags )
        eFlagsArray.append( QJsonValue(flag) );

    replica.insert( "id", QJsonValue( id ) );
    replica.insert( "goAssocie", go );
    replica.insert( "texte", QJsonValue( text ) );
    replica.insert( "repSuivantes", QJsonValue(nextId) );
    replica.insert( "flagsRequis", QJsonValue(rFlagsArray) );
    replica.insert( "flagsBloquants", QJsonValue(bFlagsArray) );
    replica.insert( "flagsActives", QJsonValue(eFlagsArray) );

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
