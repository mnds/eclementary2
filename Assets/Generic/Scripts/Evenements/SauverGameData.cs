/**
 * \file      SauverGameData.cs
 * \author    
 * \version   1.0
 * \date      24 février 2015
 * \brief     Evenement qui permet de déclencher la sauvegarde du gameData
 */

using UnityEngine;
public class SauverGameData : Evenement
{
	public override void DeclencherEvenement( params Item[] items ) {
		StateManager manager = StateManager.getInstance ();
		manager.gameData.SauvegarderGameData ();
	}
}


