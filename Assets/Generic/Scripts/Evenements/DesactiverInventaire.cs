/**
 * \file      DesactiverInventaire.cs
 * \author    
 * \version   1.0
 * \date      12 février 2015
 * \brief     Permet de désactiver l'inventaire et le radar
 */


using UnityEngine;

public class DesactiverInventaire : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		Inventaire inventaire = StateManager.getInstance ().GetJoueur ().GetComponent<Inventaire> ();
		GameObject radar = GameObject.Find ("Radar");
		// A récupérer pour permettre la désactivation des points jaunes et rouges du radar
		DetectionPersonnes dp = null;

		if (radar != null)
			dp = radar.GetComponent<DetectionPersonnes> ();
		else
			Debug.Log ("Radar non trouvé");

		if (inventaire != null) {
			inventaire.setEnabled (false);
			dp.SetEnabled(false);
			Debug.Log("Désactivation de l'inventaire et du radar");
		}
		else
			Debug.Log ("Erreur: inventaire ou script DetectionPersonnnes non trouvé");
	}
}