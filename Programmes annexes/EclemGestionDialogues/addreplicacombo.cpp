#include "addreplicacombo.h"

AddReplicaCombo::AddReplicaCombo(QStringListModel* _model) :
    model(_model)
{
}

void AddReplicaCombo::emitModel( int index ) {
    emit currentIndexChanged( index, model, this );
}

void AddReplicaCombo::updateNextReplicasId( QStringList newIdList ) {
    this->clear();
    this->addItems( newIdList );
    QStringList modelStringList = model->stringList();
    QStringList newModelStringList = QStringList();
    for( int i = 0; i < modelStringList.size(); i++ ) { // Intersection des deux listes, qui permettra de ne pas garder les répliques qui ont été supprimées
        if( newIdList.contains(modelStringList.at(i)) )
            newModelStringList << modelStringList.at(i);
    }
    model->setStringList( newModelStringList );
}
