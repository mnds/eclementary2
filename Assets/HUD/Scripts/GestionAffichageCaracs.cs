/**
 * \file      GestionAffichageCaracs.cs
 * \author    
 * \version   1.0
 * \date      24 février 2015
 * \brief     Met à jour les objets sur la fenetre des caractéristiques
 *
 * \details   Gère les barres de vie, xp, mana, endurance et les affichages des niveaux, attaque,
 * 			  attaque magique et défense.
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GestionAffichageCaracs : MonoBehaviour {


	private Text niveau;
	private Text defense;
	private Text attaque_magique;
	private Text attaque;
	private Image experience;
	private Image vie;
	private Image endurance;
	private Image mana;
	private Caracteristiques carac;
	private ControllerJoueur carac_endu;



	// Use this for initialization
	void Start () 
	{
		//On récupère les deux scripts portant les infos qu'on veut
		carac = ControlCenter.GetJoueurPrincipal ().GetComponent<Caracteristiques> ();
		carac_endu = ControlCenter.GetJoueurPrincipal ().GetComponent<ControllerJoueur> ();

		//On stock les objets que l'on va modifier
		niveau = gameObject.transform.FindChild ("Niveau").gameObject.GetComponent<Text> ();
		defense = gameObject.transform.FindChild ("Defense").gameObject.GetComponent<Text> ();
		attaque_magique = gameObject.transform.FindChild ("Attaque Magique").gameObject.GetComponent<Text> ();
		attaque = gameObject.transform.FindChild ("Attaque").gameObject.GetComponent<Text> ();
		experience = gameObject.transform.FindChild ("Expérience").gameObject.GetComponent<Image> ();
		vie = gameObject.transform.FindChild ("Vies").gameObject.GetComponent<Image> ();
		endurance = gameObject.transform.FindChild ("Endurance").gameObject.GetComponent<Image> ();
		mana = gameObject.transform.FindChild ("Mana").gameObject.GetComponent<Image> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		//On met à jour les valeurs qui doivent s'afficher (textes)
		//Attaque magique n'existe pas encore ...
		defense.text = carac.GetDefense ().ToString ();
		niveau.text = carac.GetNiveau ().ToString ();
		attaque.text = carac.GetAttaque ().ToString ();
		attaque_magique.text = carac.GetAttaque ().ToString ();

		//On met à jour les différentes barres

		//XP
		experience.fillAmount = carac.GetXp ()+5f / carac.GetXp ();
		experience.color = new Color (0.5f,0.20f,0.83f);

		//Vie
		vie.fillAmount = carac.GetH ().GetPointsDeVieActuels () / carac.GetH ().GetPointsDeVieMax ();
		vie.color = Color.red;

		//Endurance
		float rapport_endurance;
		rapport_endurance = carac_endu.GetJauge () / carac_endu.GetJaugeMax ();
		endurance.fillAmount = rapport_endurance;
		endurance.color = Color.Lerp (Color.black, Color.green, rapport_endurance);


		//Mana (Mana n'xiste pas encore ...)
		mana.fillAmount = carac.GetH ().GetPointsDeVieActuels () / carac.GetH ().GetPointsDeVieMax ();
		mana.color = Color.blue;
	}
}
