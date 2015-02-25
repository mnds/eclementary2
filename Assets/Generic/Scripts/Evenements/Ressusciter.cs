using UnityEngine;

public class Ressusciter : Evenement {

	public override void DeclencherEvenement( params Item[] items ) {
		StateManager stateManager = StateManager.getInstance ();
		Health healthPlayer = stateManager.GetJoueur ().GetComponent<HealthPlayer> ();
		if (healthPlayer != null) {
			healthPlayer.SetPointsDeVie( healthPlayer.GetPointsDeVieMax() );
			healthPlayer.SetMort( false );
			Debug.Log ("Points de vie: " + healthPlayer.pointsDeVieActuels);
		}
		stateManager.BasculerEtat ( stateManager.ancienEtat );
	}
}

