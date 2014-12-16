/**
 * \file      EtatJouable.cs
 * \author    
 * \version   1.1
 * \date      16 décembre 2014
 * \brief     Type d'état du jeu où le joueur est actif
 */

using UnityEngine;

public abstract class EtatJouable : Etat {

	public EtatJouable( StateManager manager ) : base( manager ) {

	}

	public override void ConfigurerScripts ()
	{
		// Récupération de tous les scripts de l'état jouable
		Component[] listeScriptsEtatJouable = StateManager.getInstance().GetJoueur().GetComponents (typeof(IScriptEtatJouable));
		// Activation des scripts récupérés
		for (int i = 0; i < listeScriptsEtatJouable.Length; i++) {
			IScriptEtatJouable script = (IScriptEtatJouable)listeScriptsEtatJouable [i];
			script.setEnabled (true);
		}
	}
}