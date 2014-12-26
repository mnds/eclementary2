#include <string>
#include <iostream>
#include <fstream>
#include <locale>
#include <QString>
#include <QUrl>
#include <QDesktopServices>
#include <QCoreApplication>
#include <QApplication>

using namespace std;


bool is_number(const std::string& s)
{
    std::string::const_iterator it = s.begin();
    while (it != s.end() && std::isdigit(*it)) ++it;
    return !s.empty() && it == s.end();
}

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    std::locale locale;
    //Fichier
    ofstream fichier2("debug/temp.txt", ios::out | ios::trunc);
    fichier2.close();
    ofstream fichier("debug/temp.txt", ios::out | ios::app); //S'il existe deja, on le vide

		cout<<"Numero du flag de depart : ";
        int numeroFlagActuel;
        cin>>numeroFlagActuel;
        while(cin)
        {
            cout<<endl;
            QString qnumero=QString::number(numeroFlagActuel);
            string numeroFlag=qnumero.toStdString();

            //Description
            string description="";
            while(description=="")
            {
                cout<<"Description : "<<endl;
                getline(cin,description);
            }

            cout<<endl;

            //Etat initial
            cout<<"Tapez T si le flag est a true au depart, si tapez autre chose.";
            string isFlagTrue;
            cin>>isFlagTrue;
            if(isFlagTrue=="T"||isFlagTrue=="t"||isFlagTrue=="true"||isFlagTrue=="True")
                isFlagTrue="true";
            else
                isFlagTrue="false";

            //Predecesseurs
            cout<<endl;
            string predecesseurs="";
            string predecesseurActuel="1";
            while(is_number(predecesseurActuel)) {

                cout<<"Entrez les numeros des predecesseurs. Entrez une série de lettres pour arreter.";
                cin>>predecesseurActuel;
                if(is_number(predecesseurActuel)) {
                    if(predecesseurs=="") {
                        predecesseurs=predecesseurActuel;
                    }
                    else //On ajoute la virgule
                    {
                        predecesseurs+=","+predecesseurActuel;
                    }
                }
            }

            //Evenements
            cout<<endl;
            string evenements="";
            string evenementActuel="s";
            while(!is_number(evenementActuel)) {
                cout<<"Entrez les noms des evenements. Entrez une série de chiffres pour arreter.";
                cin>>evenementActuel;
                if(!is_number(evenementActuel)) {
                    if(evenements=="") {
                        evenements="new "+evenementActuel+"()";
                    }
                    else //On ajoute la virgule
                    {
                        evenements+=",new "+evenementActuel="()";
                    }
                }
            }

            //Affichage a l'ecran du flag
            string ecritureFlag;
            ecritureFlag="new Flag("+numeroFlag+","+isFlagTrue+",\""+description+"\","
                    +"new List<int>(){"+predecesseurs+"}"+",new List<Evenement>(){"+evenements+"})";
            cout<<"Le flag est : "+ecritureFlag<<endl;

            //Nouvelle tournee
            numeroFlagActuel++;
            fichier<<"flags.Add ("<<ecritureFlag<<");"<<endl;

            string continuer="";
            while(continuer!="Y"&&continuer!="y"&&continuer!="N"&&continuer!="n")
            {
                cout<<"Continuer ? Y/N : ";
                cin>>continuer;
            }
            if(continuer=="N"||continuer=="n") {
                fichier.close(); //Et on le ferme
                QString emplacementFichier = QCoreApplication::applicationDirPath();
                emplacementFichier+="/temp.txt";
                QDesktopServices::openUrl(QUrl(emplacementFichier));
                break;
            }
        }

    return 0;
}

