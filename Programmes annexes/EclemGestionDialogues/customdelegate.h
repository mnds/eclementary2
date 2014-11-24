#ifndef CUSTOMDELEGATE_H
#define CUSTOMDELEGATE_H

#include <QItemDelegate>
#include <QStringListModel>

class CustomDelegate : public QItemDelegate
{
    Q_OBJECT
    QStringListModel* model;

public:
    explicit CustomDelegate(QStringListModel* _model);
    /*
     * createEditor est réimplémentée ici pour permettre la suppression de l'index sélectionné, pas la création d'un éditeur (qui ne nous servira pas ici)
     * */
    QWidget * createEditor ( QWidget * parent, const QStyleOptionViewItem & option, const QModelIndex & index ) const;

signals:

public slots:

};

#endif // CUSTOMDELEGATE_H
