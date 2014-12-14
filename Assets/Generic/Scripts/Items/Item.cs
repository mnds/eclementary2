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
public enum NomItem { Key, Sound};

public abstract class Item {

	private NomItem nom;

	public Item( NomItem nom ) {
		this.nom = nom;
	}
}
