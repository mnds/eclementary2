#include "customdelegate.h"

CustomDelegate::CustomDelegate(QStringListModel* _model) :
    model(_model)
{
}

QWidget * CustomDelegate::createEditor ( QWidget * parent, const QStyleOptionViewItem & option, const QModelIndex & index ) const {
    model->removeRow( index.row() );
    return NULL;
}
