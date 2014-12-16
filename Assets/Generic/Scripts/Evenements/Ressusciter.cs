using UnityEngine;

public class Ressusciter : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		StateManager.getInstance().BasculerEtat( StateManager.getInstance().ancienEtat ); // On reprend à la scène à l'état auquel le joueur était avant sa mort
	}
}

