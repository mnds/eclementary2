/**
 * \file      Item.cs
 * \author    
 * \version   1.0
 * \date      17 décembre 2014
 * \brief     Classe définissant tous les comportements et propriétés d'une clé
 */

public class Cle : Item {

	private string numero; // Numéro de la clé et de la porte qu'elle ouvre

	public Cle( NomItem nom ) : base( nom ) {

	}

	public Cle( NomItem nom, string numero ) : base( nom ) {
		this.numero = numero;
	}
}