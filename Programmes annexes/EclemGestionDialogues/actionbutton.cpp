#include "actionbutton.h"

ActionButton::ActionButton( QLayout* _replicaLayout, QString iconPath, bool _editState) :
    replicaLayout(_replicaLayout), editState(_editState)
{
    QPixmap pixmap( iconPath );
    QIcon buttonIcon(pixmap);
    setIcon( buttonIcon );
}

void ActionButton::setEditState(bool state) {
    editState = state;
}

void ActionButton::sendPosition() {
    emit position( replicaLayout );
}

void ActionButton::sendAllDetails() {
    emit allDetails( replicaLayout, editState, this );
}

void ActionButton::enable() {
    if( !isEnabled() )
        setEnabled( true );
}
