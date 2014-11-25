#ifndef ADDREPLICACOMBO_H
#define ADDREPLICACOMBO_H

#include <QComboBox>
#include <QStringListModel>
#include <QDebug>

class AddReplicaCombo : public QComboBox
{
    Q_OBJECT
    QStringListModel* model;
public:
    explicit AddReplicaCombo( QStringListModel* _model );
    void updateNextReplicasId( QStringList newIdList ); /* Permet la mise à jour de la liste
                                                         * des id des répliques suivantes dans
                                                         * une liste déroulante et sa textview associée,
                                                         * via le "model". Doit etre appelée après une mise à jour
                                                         * du fichier du dialogue
                                                         */

signals:
    void currentIndexChanged( int, QStringListModel*, QComboBox* );
public slots:
    void emitModel( int index );
};

#endif // ADDREPLICACOMBO_H
