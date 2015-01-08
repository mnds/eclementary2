using UnityEngine;

public class Ressusciter : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		StateManager stateManager = StateManager.getInstance ();
		HealthPlayer healthPlayer = stateManager.GetJoueur ().GetComponent<HealthPlayer> ();
		if (healthPlayer != null) {
			healthPlayer.Soigner( healthPlayer.GetPointsDeVieMax() );
			Debug.Log ("Points de vie: " + healthPlayer.pointsDeVieActuels);
		}
		stateManager.BasculerEtat ( stateManager.ancienEtat );
	}
}

