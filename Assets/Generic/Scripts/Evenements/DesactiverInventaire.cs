using UnityEngine;

public class DesactiverInventaire : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		Inventaire inventaire = StateManager.getInstance ().GetJoueur ().GetComponent<Inventaire> ();
		if (inventaire != null) {
			inventaire.setEnabled (false);
			Debug.Log("Désactivation de l'inventaire");
		}
		else
			Debug.Log ("Erreur: inventaire non trouvé");
	}
}