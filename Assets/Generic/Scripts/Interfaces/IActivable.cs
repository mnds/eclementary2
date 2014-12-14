/**
 * \file      IActivable.cs
 * \author    
 * \version   1.0
 * \date      14 décembre 2014
 * \brief     Interface implémentée par tous les objets pouvant être activés/désactivés au cours du jeu
 */

public interface IActivable
{
	bool isActif();
	void setActif( bool ok );
}
