using UnityEngine;

public class DesactiverInventaire : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		Inventaire inventaire = StateManager.getInstance ().GetJoueur ().GetComponent<Inventaire> ();
		if (inventaire != null)
			inventaire.setEnabled (false);
		else
			Debug.Log ("Erreur: inventaire non trouvé");
	}
}