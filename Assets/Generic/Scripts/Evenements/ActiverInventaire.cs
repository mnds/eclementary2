using UnityEngine;

public class ActiverInventaire : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		Inventaire inventaire = StateManager.getInstance ().GetJoueur ().GetComponent<Inventaire> ();
		bool inventaireActif = FlagManager.ChercherFlagParId (130).actif;
		if (inventaire != null) {
			inventaire.setEnabled (inventaireActif);
			Debug.Log("Activation de l'inventaire");
		}
		else
			Debug.Log ("Erreur: inventaire non trouvé");
	}
}