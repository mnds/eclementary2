#ifndef ACTIONBUTTON_H
#define ACTIONBUTTON_H

#include <QPushButton>
#include <QLayout>
#include <QString>
#include <dialogmanager.h> // Pour récupérer la constante WORKING_DIRECTORY
#include <QPixmap>

class ActionButton : public QPushButton
{
    Q_OBJECT
    QLayout* replicaLayout; // Position de la réplique dans la présentation
    bool editState; // true si la réplique associée est actuellement en état de modification, false sinon
public:
    explicit ActionButton( QLayout* _replicaLayout, QString iconPath, bool _editState = false);
    void setEditState( bool state );

signals:
    void position( QLayout* );
    void allDetails( QLayout*, bool, ActionButton* );
public slots:
    void sendPosition();
    void sendAllDetails();
    void enable();
};

#endif // ACTIONBUTTON_H
