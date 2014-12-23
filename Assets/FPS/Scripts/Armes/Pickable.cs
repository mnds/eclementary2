/**
 * \file      Pickable.cs
 * \author    
 * \version   1.0
 * \date      19 octobre 2014
 * \brief     Attaché aux objets pouvant etre ramassés.
 *
 * \details   Contient deux attributs : le premier est un booléan qui dit s'il est possible de ramasser l'objet. Le deuxième dit à quelle distance on peut le ramasser.
 *
 */

using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {
	//Remarque : ne pas mettre de bypassInitial, sinon, l'instruction bypass=bypassInitial se fera après les Instantiate de Lancer par exemple
	public bool bypass; //Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	public bool isPickableDebut = false; //Si false, on ne peut pas le prendre, true sinon
	public float pickableDistanceDebut = 3.0f;

	public int nombreMunitions = 1; //Nombre de munitions que donne l'objet
	bool isPickable ;
	float pickableDistance;

	public GUITexture texture;
	//Une fois ramassé, on ajoutera à l'inventaire 

	void Start() {
		isPickable = isPickableDebut; //On initialise
		pickableDistance = pickableDistanceDebut;

	}

	public void SetPickable(bool isPickable_)
	{
		isPickable = isPickable_;
	}

	public void SetPickableDebut(bool isPickableDebut_)
	{
		isPickableDebut = isPickableDebut_;
	}

	public bool GetPickable()
	{
		return isPickable;
	}

	public void SetBypass(bool bypass_) {
		bypass = bypass_;
	}

	public float GetPickableDistance() {
		return pickableDistance;
	}
}
