/**
 * \file      ActiverEcranFin.cs
 * \author    
 * \version   1.0
 * \date      3 février 2015
 * \brief	  Est appelé à la fin du jeu, lorsque le joueur a atteint l'objectif final
 */

using UnityEngine;

public class ActiverEcranFin : Evenement {
	public override void DeclencherEvenement( params Item[] items ) {
		StateManager stateManager = StateManager.getInstance ();
		stateManager.BasculerEtat ( new EtatFin( stateManager ) );
	}
}