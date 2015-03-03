/**
 * \file      EtatJouable.cs
 * \author    
 * \version   1.0
 * \date      8 février 2015
 * \brief     Etat chargé lors du lancement du minijeu de basket
 */

using UnityEngine;

public class EtatBasket: Etat
{
	private MinijeuBasket minijeu;

	public EtatBasket ( StateManager manager, MinijeuBasket minijeuBasket ) : base( manager )
	{
		minijeu = minijeuBasket;
		ConfigurerScripts ();
	}

	public override void ConfigurerScripts() {
		/*Component[] listeScriptsEtatJouable = StateManager.getInstance().GetJoueur().GetComponents (typeof(IScriptEtatJouable));
		// Désactivation des scripts récupérés (on n'aura pas besoin des scripts Inventaire, Attaquer, Lancer... )
		for (int i = 0; i < listeScriptsEtatJouable.Length; i++) {
			IScriptEtatJouable script = (IScriptEtatJouable)listeScriptsEtatJouable [i];
			script.setEnabled (false);
		}*/
		HealthPlayer healthPlayer = StateManager.getInstance ().GetJoueur ().GetComponent<HealthPlayer> ();
		healthPlayer.setEnabled (false);
		new DesactiverInventaire ().DeclencherEvenement (); // on désactive l'inventaire

		// Activation du script BasketJoueur
		BasketJoueur scriptBasket = StateManager.getInstance().GetJoueur().GetComponent<BasketJoueur>();
		scriptBasket.setEnabled (true);
	}

	public override void UpdateEtat() {

	}

	public override void DesactiverEtat() {
		HealthPlayer healthPlayer = StateManager.getInstance ().GetJoueur ().GetComponent<HealthPlayer> ();
		healthPlayer.setEnabled (true);
		new ActiverInventaire().DeclencherEvenement(); // On réactive l'inventaire
		// Désactivation du script BasketJoueur
		BasketJoueur scriptBasket = StateManager.getInstance().GetJoueur().GetComponent<BasketJoueur>();
		scriptBasket.setEnabled (false);
		minijeu.ArreterInteraction(); // Fin du minijeu
	}

	public override void AfficherRendu() {
		// Actions à faire lorsqu'on quitte le jeu
		if (GUI.Button (new Rect (4*Screen.width / 5, 4*Screen.height / 5, 200, 100), "Quitter le minijeu ?")) {
			DesactiverEtat();
		}
	}
}


