/**
 * \file      EnvoyerMessage.cs
 * \author    
 * \version   1.0
 * \date      12 février 2015
 * \brief     Envoie un message au joueur
 */

using UnityEngine;

public class EnvoyerMessage : Evenement
{
	Item message;
	
	public EnvoyerMessage(Item message_) {
		message=message_;
		Debug.Log ("Message : "+message);
	}
<<<<<<< HEAD

=======
	
>>>>>>> master
	public override void DeclencherEvenement (params Item[] items)
	{
		Messager messager = ControlCenter.GetMessager ();
		Debug.Log ("Declencher evenement");
		//Si on a appelé DeclencherEvenement par le FlagManager, il n'y a pas d'items et message est non null
		if((items.Length==0||items==null) && message!=null) {
			Debug.Log ("Coucou");
			messager.EnvoyerMessage( message.GetNomItem() ); // nomItem est à un item ce que l'id est à un message
			return;
		}
		for (int i = 0; i < items.Length; i++) {
			Debug.Log ("Items : "+items);
			Debug.Log ("Message : "+message);
			if( items[i].GetNom().Equals( NomItem.Message ) )
				messager.EnvoyerMessage( items[i].GetNomItem() ); // nomItem est à un item ce que l'id est à un message
		}
	}
	
}


