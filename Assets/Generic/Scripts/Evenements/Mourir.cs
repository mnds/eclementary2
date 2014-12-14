using UnityEngine;

public class Mourir : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		StateManager.getInstance().BasculerEtat( new EtatMort( StateManager.getInstance() ) );
	}
}
