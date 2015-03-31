using UnityEngine;

public class Mourir : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		//Particularite : le joueur doit avoir une vitesse de déplacement nulle en mourant. Donc on lui impose ça.
		ControlCenter.GetJoueurPrincipal ().GetComponent<ControllerJoueur>().SetVitesseNonVerticaleActuelle (0f);
		ControlCenter.GetJoueurPrincipal ().GetComponent<ControllerJoueur>().setEnabled (false);
		StateManager.getInstance().etatActif.DesactiverEtat(); // Désactivation de l'état précédent
		StateManager.getInstance().BasculerEtat( new EtatMort( StateManager.getInstance() ) );

		// Cas où le joueur est mort est lors d'un combat avec le boss final
		FlagManager.Flag flag431 = FlagManager.ChercherFlagParId (431);
		FlagManager.Flag flag435 = FlagManager.ChercherFlagParId (435);
		FlagManager.Flag flag436 = FlagManager.ChercherFlagParId (436);
		if (flag431.actif && (flag435.actif ^ flag436.actif)) {
			flag431.actif = false;
			flag435.actif = false;
			flag436.actif = false;
		}
		//StateManager stateManager = StateManager.getInstance ();
		//Debug.Log ( "Mort: Scène de l'ancien état " + stateManager.ancienEtat.getSceneCorrespondante());
	}
}
