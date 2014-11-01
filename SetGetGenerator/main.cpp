#include <string>
#include <iostream>
#include <fstream>
#include <locale>

using namespace std;

int main()
{
    std::locale locale;
    cout<<"Vider fichier ? Y/N"<<endl;
    string reponse;
    cin>>reponse;
    if(reponse=="Y") //On vide
    {
        ofstream fichier2("temp.txt", ios::out | ios::trunc);
        fichier2.close();
    }
    ofstream fichier("temp.txt", ios::out | ios::app); //S'il existe déjà, on le vide
    fichier<<endl;
    fichier<<"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"<<endl<<endl;
    while(cin)
    {
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


    fichier.close(); //Et on le ferme
    return 0;
}

