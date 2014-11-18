using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

using System.ComponentModel;

using System.Diagnostics;   //Permet d'accéder à un .exe à l'extérieur
    class CrossThreadCommunication
    {

        //Variables globales : 
        private static EmotionStatus _status;           //On crée un objet Status
        private static Thread _thread;                  //On crée un objet Thread
        private static ISynchronizeInvoke _syn;       //Permet de synchroniser les threads
		private static StringBuilder emotionOutput;
        private static int numOutputLines = 0;          //Initialise le nombre de lignes output de l'application.exe
        private static StringBuilder sortOutput = null; //Ligne de caractère  : output de l'application.exe         

        //On crée un delegate (sorte de pointeur) qui nous donne un chiffre pour représenter le status émotionnel du joueur
        public delegate void EmotionStatus(DateTime now, int Status);

        //Constructeur : Il faudra donner les valeur de base
//        public CrossThreadCommunication (ISynchronizeInvoke syn, ProcessStatus notify) {
//            //On instancie les variables
//            _syn = syn;
//            _status = notify;
//        }

//        //on lance le thread
//        public void StartProcess () {
//
//            //Créer un nouveau thread avec une méthode de AreaObject
//            _thread = new System.Threading.Thread(EmotionAlert);
//            _thread.Priority = ThreadPriority.Highest;  //Attribut un degré de priorité
//        
//            _thread.IsBackground = true;
//            _thread.Start();  //Démarrer un nouveau Thread
//                _thread.IsAlive();     //Renvoie true si actif
//                _thread.IsBackground();//Renvoie true si le thread est en Background
//                _thread.ThreadState(); //Renvoie le statut du Thread
//       }


       //Fonction a effectuer dans le thread secondaire
       public static void EmotionAlert(){
           int Emotion;    //On défini une variable émotion : nombre qui sera envoyé au thread principal
           string winpath = Environment.GetEnvironmentVariable("windir");   //Récupère le chemin vers le dossier
		   emotionOutput = new StringBuilder("");   //Initialise un output vide
           
           Process EmotionProcess = new Process();  //défini le nom du process
           
           //Défini les données à lancer lors du Process
           EmotionProcess.StartInfo.FileName = "doxywizard.exe";
           EmotionProcess.StartInfo.UseShellExecute = false;         //Nécessaire d'être false pour que RedirectStandardOutput soit true
           EmotionProcess.StartInfo.RedirectStandardOutput = true;   //On peut récupérer les output du programme
           EmotionProcess.StartInfo.CreateNoWindow = false;          //Ouvre nouvelle fenetre   

           // Défini un gestionnaire d'événement pour recevoir les données de façon asynchrone
           EmotionProcess.OutputDataReceived += new DataReceivedEventHandler(EmotionOutputHandler);

           //Lance le process avec les données du startInfo
           EmotionProcess.Start(); //Lance l'application .exe

           //On récupère les donées de l'application
           EmotionProcess.BeginOutputReadLine();
           string error = EmotionProcess.StandardError.ReadToEnd(); //On écrit les error si on les récupères
           EmotionProcess.WaitForExit();    //Attendre que l'application écrive

       }

        //Gestionnaire d'évenements pour la communication asynchrone : 
        private static void EmotionOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collecte les outputs
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                numOutputLines++;
                // Ajoute le texte aux output
                emotionOutput.Append(Environment.NewLine + "[" + numOutputLines.ToString() + "] - " + outLine.Data);
            }
        }

    }
    

