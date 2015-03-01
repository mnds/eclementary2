using UnityEngine;

public class ActiverInventaire : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		Inventaire inventaire = StateManager.getInstance ().GetJoueur ().GetComponent<Inventaire> ();
		if (inventaire != null)
			inventaire.setEnabled (true);
		else
			Debug.Log ("Erreur: inventaire non trouvé");
	}
}