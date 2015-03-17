/**
 * \file      MajEtatInteraction.cs
 * \author    
 * \version   1.0
 * \date      14 mars 2015
 * \brief     Permet d'activer le booléen qui renseigne sur l'état des interactions
 *
 */

using UnityEngine;

public class MajEtatInteraction : Evenement
{
	private bool etatInteraction;
	
	public MajEtatInteraction ( bool etatInteraction )
	{
		this.etatInteraction = etatInteraction;
	}

	public override void DeclencherEvenement( params Item[] items ) {
		GameObject joueur = ControlCenter.GetJoueurPrincipal();
		InteractionManager im = null; // Récupération du gestionnaire d'interactions

		if (joueur != null) {
			im = joueur.GetComponent<InteractionManager> ();
			if( im != null )
				im.SetInteractionEnCours( etatInteraction ); // Mise à jour de l'état des interactions
		}
	}
}


