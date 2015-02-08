/**
 * \file      EtatJouable.cs
 * \author    
 * \version   1.0
 * \date      8 février 2015
 * \brief     Script Interactif chargé du lancement et de l'arrêt de l'état EtatBasket
 */

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MinijeuBasket : MonoBehaviour, Interactif {

	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet

	public Sprite imgBarre;
	public Sprite imgArrow1;
	public Sprite imgArrow2;
	public Sprite imgTransparente;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DemarrerInteraction() {
		StateManager manager = StateManager.getInstance ();
		// Affichage de la jauge de lancer
		GameObject.Find ("Bar").transform.GetComponent<Image> ().sprite = imgBarre;
		GameObject.Find ("Arrow").transform.GetComponent<Image> ().sprite = imgArrow1;
		GameObject.Find ("Arrow2").transform.GetComponent<Image> ().sprite = imgArrow2;

		manager.BasculerEtat ( new EtatBasket( manager, this ) );
	}

	public void ArreterInteraction() {
		StateManager manager = StateManager.getInstance ();
		// Masquage de la jauge de lancer
		GameObject.Find ("Bar").transform.GetComponent<Image> ().sprite = imgTransparente;
		GameObject.Find ("Arrow").transform.GetComponent<Image> ().sprite = imgTransparente;
		GameObject.Find ("Arrow2").transform.GetComponent<Image> ().sprite = imgTransparente;

		manager.BasculerEtat ( manager.ancienEtat ); // Retour à l'état précédent
	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_) {
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}

	public float GetDistanceMinimaleInteraction () {
		return distanceMinimaleInteraction;
	}
	
}
