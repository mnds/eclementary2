/**
 * \file      Item.cs
 * \author    
 * \version   1.0
 * \date      14 décembre 2014
 * \brief     Objet utilisé avec un évènement pour déclencher une action dans le jeu
 */

using UnityEngine;
using System.Collections;

// Liste des noms que peut prendre un item
public enum NomItem { Key, Sound, SpawnPoint};


public class Item {

	private NomItem nom; //Catégorie de cet item
	private string nomItem; //Nom de cet item
	private string nomScene; //Nom de la scène
 	private bool teleportationImmediate; //Si true, téléportation immédiate

	public Item( NomItem nom_ ) {
		this.nom = nom_;
	}

	//Pour les spawnPoints
	public Item( NomItem nom_, string nomItem_, string nomScene_, bool tI) {
		this.nom = nom_;
		nomItem = nomItem_;
		nomScene=nomScene_;
		teleportationImmediate=tI;
	}

	public NomItem GetNom () {
		return nom;
	}

	public string GetNomScene () {
		return nomScene;
	}

	public string GetNomItem () {
		return nomItem;
	}

	public bool GetTeleportationImmediate () {
		return teleportationImmediate;
	}
}
