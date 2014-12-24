using UnityEngine;

public class Ressusciter : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		StateManager.getInstance().BasculerEtat ( StateManager.getInstance().ancienEtat );
	}
}

