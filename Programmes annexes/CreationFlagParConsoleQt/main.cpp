#include <string>
#include <iostream>
#include <fstream>
#include <locale>
#include <QString>
#include <QUrl>
#include <QVector>
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

QVector<QString> recupererLignes()
{
    //Ouverture d'un fichier temp pour que l'utilisateur mette les lignes qu'il veut convertir
    cout<<"Remplir le fichier ouvert avec vos lignes en flags.Add(...) et entrer n'importe quelle commande pour signifier que vous avez fini"<<endl;
    ofstream fichierTemp("debug/temp.txt", ios::out | ios::app); //S'il existe deja, on le vide
    fichierTemp.close();

    QString emplacementFichier = QCoreApplication::applicationDirPath();
    emplacementFichier+="/temp.txt";
    QDesktopServices::openUrl(QUrl(emplacementFichier));
    //Récupération de la ligne donnée par l'utilisateur pour dire qu'il a fini
    string fini;
    cin>>fini;

    //On récupère maintenant les lignes du fichier
    fstream fichier("debug/temp.txt");
    vector <string> monTableau;

    //On remplit monTableau # http://www.commentcamarche.net/forum/affich-2485838-c-lire-un-fichier-texte-ligne-par-ligne
    if ( !fichier )
        cout << "fichier inexistant";
    else
    {
        while( !fichier.eof() )
        {
            monTableau.push_back("");//creation d'une ligne vide

            getline(fichier, monTableau.back());//lecture d'une ligne du fichier

            int ligne = monTableau.size() - 1;//je recupere la taille du tableau (-1 pour la ligne 0)

            if(monTableau[ligne].empty())//si la ligne est vide
                monTableau.pop_back();//on la retire du tableau
        }
        cout<<"Nombre de lignes : "<<monTableau.size();
    }

    //On le convertit en QVector<QString>
    QVector<QString> reponse;
    for(int i=0;i<monTableau.size();i++)
    {
        reponse.append(QString::fromStdString(monTableau[i]));
    }
    fichier.close();
    return reponse;
}

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    std::locale locale;
    cout<<"Choisir l'application : S pour creer des Set/Get, F pour les flags, f pour les flags simples (pas de choix d'evenements : conseille),a pour obtenir les lignes à mettre dans flags"<<endl;
    string reponse;
    cin>>reponse;

    //Fichier
    ofstream fichier2("debug/temp.txt", ios::out | ios::trunc);
    fichier2.close();


    if(reponse=="S") //On vide
    {
        while(cin)
        {
            ofstream fichier("debug/temp.txt", ios::out | ios::app); //S'il existe deja, on le vide
            cout<<"Type de l'objet : ";
            string type;
            cin>>type;
            if(type=="") break;
            cout<<"Nom : ";
            string nom;
            cin>>nom;
            if(nom=="") break;
            fichier<<endl;
            //Set
            fichier<<"public void Set";
            fichier<<toupper(nom[0],locale)<<nom.substr (1);
            fichier<<" ("<<type<<" "<<nom<<"_) {"<<endl;
            fichier<<"    "<<nom<<" = "<<nom<<"_;"<<endl;
            fichier<<"}"<<endl<<endl;
            //Get
            fichier<<"public "<<type<<" Get";
            fichier<<toupper(nom[0],locale)<<nom.substr (1);
            fichier<<" () {"<<endl;
            fichier<<"    return "<<nom<<";"<<endl;
            fichier<<"}"<<endl;
        }
    }
    if(reponse=="F"||reponse=="f")
    {
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
            string isFlagTrue="false";
            if(reponse=="F") {
                cout<<"Tapez T si le flag est a true au depart, si tapez autre chose.";
                cin>>isFlagTrue;
                if(isFlagTrue=="T"||isFlagTrue=="t"||isFlagTrue=="true"||isFlagTrue=="True")
                    isFlagTrue="true";
            }


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

            //Bloquants
            cout<<endl;
            string bloquants="";
            string bloquantActuel="1";
            while(is_number(bloquantActuel)) {

                cout<<"Entrez les numeros des bloquants. Entrez une série de lettres pour arreter.";
                cin>>bloquantActuel;
                if(is_number(bloquantActuel)) {
                    if(bloquants=="") {
                        bloquants=bloquantActuel;
                    }
                    else //On ajoute la virgule
                    {
                        bloquants+=","+bloquantActuel;
                    }
                }
            }

            //Evenements
            cout<<endl;
            string evenements="";
            string evenementActuel="s";
            if(reponse=="F") {
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
            }

            //Affichage a l'ecran du flag
            string ecritureFlag;
            ecritureFlag="new Flag("+numeroFlag+","+isFlagTrue+",\""+description+"\","
                    +"new List<int>(){"+predecesseurs+"}"
                    +",new List<int>(){"+bloquants+"}"
                    +",new List<Evenement>(){"+evenements+"})";
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
    }
    if(reponse=="a"||reponse=="A") { //Automatique
        QVector<QString> lignesTransformer=recupererLignes();

        fstream fichier("debug/temp.txt", ios::out | ios::trunc);
        for(int i=0;i<lignesTransformer.size();i++)
        {
            QString q=lignesTransformer[i];
            QRegExp rx("[(,]"); //Séparateur : ( et , pour flags.Add(new Flag(1,"description",...);
            QStringList qParse=q.split(rx,QString::SkipEmptyParts); //Séparation de q
            string id=qParse[2].toStdString();
            string description=qParse[4].toStdString();
            fichier<<"{"<<endl;
            fichier<<"    \"id\": \""<<id<<"\","<<endl;
            fichier<<"    \"description\": "<<description<<endl;
            fichier<<"},"<<endl;
        }
        //On ouvre le fichier
        fichier.close();
        QString emplacementFichier = QCoreApplication::applicationDirPath();
        emplacementFichier+="/temp.txt";
        QDesktopServices::openUrl(QUrl(emplacementFichier));
    }
    return 0;
}

