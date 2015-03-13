/**
 * \file      ActiverInventaire.cs
 * \author    
 * \version   1.0
 * \date      12 février 2015
 * \brief     Permet d'activer l'inventaire et le radar
 */

using UnityEngine;

public class ActiverInventaire : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		Inventaire inventaire = StateManager.getInstance ().GetJoueur ().GetComponent<Inventaire> ();
		bool inventaireActif = FlagManager.ChercherFlagParId (ControlCenter.idFlagEtatInventaire).actif;
		GameObject radar = GameObject.Find ("Radar");
		// A récupérer pour permettre l'activation des points jaunes et rouges du radar
		DetectionPersonnes dp = null;

		if (radar != null)
			dp = radar.GetComponent<DetectionPersonnes> ();
		else
			Debug.Log ("Radar non trouvé");

		if (inventaire != null && dp != null ) {
			inventaire.setEnabled (inventaireActif);
			dp.SetEnabled(inventaireActif);
			Debug.Log("Activation de l'inventaire et du radar");
		}
		else
			Debug.Log ("Erreur: inventaire ou script DetectionPersonnnes non trouvé");
	}
}