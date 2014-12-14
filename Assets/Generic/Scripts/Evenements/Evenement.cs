/**
 * \file      Evenement.cs
 * \author    
 * \version   1.0
 * \date      14 décembre 2014
 * \brief	Classe dont héritent toutes les classes qui déclenchent des évènements
 */

public abstract class Evenement {
	// Fonction à nombre d'arguments variables
	public abstract void DeclencherEvenement( params Item[] items );	
}