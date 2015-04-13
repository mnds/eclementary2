using UnityEngine;
using System.Collections;

/**
 * \file      Sauvegarde.cs
 * \author    
 * \version   1.0
 * \date      25 février 2015
 * \brief     Interaction qui permet la sauvegarde de l'état actuel du jeu, lorsque le joueur interagit avec le lit
 */

public class SauvegardeLit : MonoBehaviour, Interactif {
	public GUITexture gt; //Pour l'écran noir
	ControllerJoueur cj; //Pour désactiver le joueur
	float tempsEcranNoir = 2f; //Temps de l'écran
	float tempsEcoule = 0f; //Temps déjà écoulé
	bool ecranEnCours = false;

	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet

	void Start () {
		gt.color=new Color(0f,0f,0f,0f);
		cj = ControlCenter.GetJoueurPrincipal().GetComponent<ControllerJoueur>();
	}

	// Use this for initialization
	void Update () {
		if(!ecranEnCours) //Rien à faire
			return; 
		//Ecran noir
		tempsEcoule+=Time.deltaTime;
		if(tempsEcoule<tempsEcranNoir/2) //Fade out
		{
			float alpha = Mathf.Min (2f*tempsEcoule/tempsEcranNoir,255f);
			gt.color=new Color(0f,0f,0f,alpha);
		}
		else //fade in
		{
			float alpha = Mathf.Max (2f*(tempsEcranNoir-tempsEcoule)/tempsEcranNoir,0f);
			gt.color=new Color(0f,0f,0f,alpha);
		}
		//Ensuite on teste
		if(tempsEcoule>tempsEcranNoir) {
			gt.color=new Color(0f,0f,0f,0f);
			ecranEnCours=false;
			tempsEcoule=0f;
			cj.SetFreeze(false);
		}
	}

	// Sauvegarde lorsque le joueur interagit avec le lit
	public void DemarrerInteraction() {
		if(ecranEnCours)
			return;
		cj.SetFreeze(true);
		Debug.Log ("Sauvegarde du jeu...");
		//On fait en sorte de restaurer la vie, l'endurance, et le mana
		cj.RemplirJaugeEndurance();
		Health h = ControlCenter.GetJoueurPrincipal().GetComponent<Health>();
		h.Soigner(h.GetPointsDeVieMax());
		h.SoignerMana(h.GetPointsDeManaMax());

		ecranEnCours=true; //Lancer l'écran noir 
		new SauverGameData ().DeclencherEvenement (); //Sauvegarder les données
	}

	public void ArreterInteraction() {

	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_) {
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}

	public float GetDistanceMinimaleInteraction () {
		return distanceMinimaleInteraction;
	}
}
