using UnityEngine;

public class Mourir : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		//Particularite : le joueur doit avoir une vitesse de déplacement nulle en mourant. Donc on lui impose ça.
		ControlCenter.GetJoueurPrincipal ().GetComponent<ControllerJoueur>().SetVitesseNonVerticaleActuelle (0f);
		ControlCenter.GetJoueurPrincipal ().GetComponent<ControllerJoueur>().setEnabled (false);
		StateManager.getInstance().BasculerEtat( new EtatMort( StateManager.getInstance() ) );
		//StateManager stateManager = StateManager.getInstance ();
		//Debug.Log ( "Mort: Scène de l'ancien état " + stateManager.ancienEtat.getSceneCorrespondante());
	}
}
