using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Ne sert à rien pour l'instant. Il pourrait servir à savoir quelle est la réplique actuelle, si jamais on voulait
//ne permettre d'avoir qu'une seule réplique.
//On désactiverait alors fpcscript pour que le joueur soit incapable de bouger pendant un dialogue.
public class DialogueManager : MonoBehaviour {

	Replique repliqueActuelle; //La réplique en train d'etre jouée sur toute la 
	public FPCClassic fpcscript; //Le script du joueur. Si une réplique est en train d'etre jouée, il est bloqué.

	//Affichage des repliques selectionnables
	bool enCoursDeSelection = false ; //Si on est en train d'afficher les répliques à l'écran, true
	List<Replique> repliquesASelectionner; //Les répliques sélectionnables par le joueur

	public void DemandeSelectionPhrase (List<Replique> repliques)
	{
		if (repliques == null || repliques.Count <= 0 || enCoursDeSelection)
			return;
		repliquesASelectionner = repliques;
		enCoursDeSelection = true;
		fpcscript.SetFreeze (true);
	}

	void Update () {

	}

	void OnGUI () {
		if (DebugManager.mode == ModesJeu.Normal)
			OnGUINormal ();
		else
			OnGUIDebug ();
	
	}

	void OnGUINormal () {
		string texteReplique = "";
		if (repliqueActuelle)
			texteReplique = repliqueActuelle.GetTexteReplique ();
		GUI.Label (new Rect (25, 25, 800, 35), texteReplique);
	}

	void OnGUIDebug () {
		string texteReplique = "";
		if (repliqueActuelle)
			texteReplique = repliqueActuelle.GetTexteReplique ();
		GUI.Label (new Rect (25, 25, 800, 35), texteReplique);
	}

	void LancerReplique(Replique replique) {
		repliquesASelectionner = null;
		enCoursDeSelection = false;
		fpcscript.SetFreeze (false);
	}

	public void SetRepliqueActuelle (Replique ra) {
		repliqueActuelle = ra;
	}
}
