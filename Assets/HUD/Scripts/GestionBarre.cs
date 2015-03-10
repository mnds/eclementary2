/**
 * \file      Gestionbarre.cs
 * \author    
 * \version   1.0
 * \date      10 mars 2015
 * \brief     Met à jour les barres de vies
 *
 * \details   Gère les barres de vie, xp, mana, endurance
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GestionBarre : MonoBehaviour {
	

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
		experience = gameObject.transform.FindChild ("Expérience").gameObject.GetComponent<Image> ();
		vie = gameObject.transform.FindChild ("Vies").gameObject.GetComponent<Image> ();
		endurance = gameObject.transform.FindChild ("Endurance").gameObject.GetComponent<Image> ();
		mana = gameObject.transform.FindChild ("Mana").gameObject.GetComponent<Image> ();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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

