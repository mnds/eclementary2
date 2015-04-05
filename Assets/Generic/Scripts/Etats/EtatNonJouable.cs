/**
 * \file      EtatNonJouable.cs
 * \author    
 * \version   1.0
 * \date      16 décembre 2014
 * \brief     Type d'état du jeu où le joueur est inactif
 */

using UnityEngine;

public abstract class EtatNonJouable : Etat {
	
	public EtatNonJouable( StateManager manager ) : base( manager ) {
		etatJouable = false;
	}
	
	public override void ConfigurerScripts() {
		// Récupération de tous les scripts de l'état jouable
		/*Debug.LogWarning ("Instance state : "+StateManager.getInstance());
		Debug.LogWarning ("Joueur : "+StateManager.getInstance ().GetJoueur());
		Debug.LogWarning ("Components : "+StateManager.getInstance().GetJoueur().GetComponents (typeof(IScriptEtatJouable)));*/
		Component[] listeScriptsEtatJouable = StateManager.getInstance().GetJoueur().GetComponents (typeof(IScriptEtatJouable));
		// Désactivation des scripts récupérés
		for (int i = 0; i < listeScriptsEtatJouable.Length; i++) {
			IScriptEtatJouable script = (IScriptEtatJouable)listeScriptsEtatJouable [i];
			script.setEnabled (false);
		}
		// Désactivation de l'inventaire et du radar
		new DesactiverInventaire ().DeclencherEvenement ();
		new DesactiverBarresCaracs ().DeclencherEvenement ();
		// Désactivation du script BasketJoueur
		BasketJoueur scriptBasket = StateManager.getInstance().GetJoueur().GetComponent<BasketJoueur>();
		scriptBasket.setEnabled (false);
	}
	public override void UpdateEtat ()
	{		
		if (Input.GetButtonDown ("Pause")) {
			Time.timeScale = 0;
			StateManager manager = StateManager.getInstance();
			manager.BasculerEtat(new EtatPause(manager, manager.etatActif.getSceneCorrespondante()));
			}
	}
}