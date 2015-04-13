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
	private Text experience_texte;
	private Text vie_texte;
	private Text endurance_texte;
	private Text mana_texte;
	private JoueurCaracteristiques carac;
	private ControllerJoueur carac_endu;
	public Sprite imageTransparente;
	public Sprite imageBackground;

	private bool enabled;
	
	
	
	// Use this for initialization
	void Start () 
	{
		//On récupère les deux scripts portant les infos qu'on veut
		carac = ControlCenter.GetJoueurPrincipal ().GetComponent<JoueurCaracteristiques> ();
		carac_endu = ControlCenter.GetJoueurPrincipal ().GetComponent<ControllerJoueur> ();
		
		//On stock les objets que l'on va modifier
		experience = gameObject.transform.FindChild ("Expérience").gameObject.GetComponent<Image> ();
		vie = gameObject.transform.FindChild ("Vies").gameObject.GetComponent<Image> ();
		endurance = gameObject.transform.FindChild ("Endurance").gameObject.GetComponent<Image> ();
		mana = gameObject.transform.FindChild ("Mana").gameObject.GetComponent<Image> ();

		//On stock aussi les textes
		experience_texte = GameObject.Find("Expérience_Texte_B").GetComponent<Text> ();
		vie_texte = GameObject.Find ("Vies_Texte_B").GetComponent<Text> ();
		endurance_texte = GameObject.Find ("Endurance_Texte_B").GetComponent<Text> ();
		mana_texte = GameObject.Find ("Mana_Texte_B").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//On met à jour les différentes barres
		if (!enabled)
			return;

		//XP
		experience.fillAmount = (float)(carac.experience_entre_deux_niveaux - carac.experience_avant_niveau_suivant) / carac.experience_entre_deux_niveaux;
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
		mana.fillAmount = carac.GetH ().GetPointsDeManaActuels () / carac.GetH ().GetPointsDeManaMax ();
		mana.color = Color.blue;
	}

	public void setEnabled(bool ok)
	{
		//Si elles sont desactivées et qu'on veut les activer
		if(ok)
		{
			experience.sprite = imageBackground;
			vie.sprite = imageBackground;
			mana.sprite = imageBackground;
			endurance.sprite = imageBackground;
			experience_texte.text = "Expérience";
			vie_texte.text = "Vie";
			mana_texte.text = "Mana";
			endurance_texte.text = "Endurance";
		}
		//Si elles sont activées et qu'on veut les désactiver
		else
		{
			Debug.Log(vie_texte);
			experience.sprite = imageTransparente;
			vie.sprite = imageTransparente;
			mana.sprite = imageTransparente;
			endurance.sprite = imageTransparente;
			experience_texte.text = "";
			vie_texte.text = "";
			mana_texte.text = "";
			endurance_texte.text = "";
		}

		enabled = ok;
	}
}

