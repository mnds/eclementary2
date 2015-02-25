/**
 * \file      FlagReduit.cs
 * \author    
 * \version   1.0
 * \date      24 février 2015
 * \brief     Mini-flag, utilisé pour la sauvegarde
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlagReduit
{
	public int id;
	public bool actif;

	public FlagReduit( int id, bool actif ) {
		this.id = id;
		this.actif = actif;
	}

}

