using UnityEngine;

public class RemplirEndurance : Evenement {
	
	public override void DeclencherEvenement( params Item[] items ) {
		ControlCenter.GetJoueurPrincipal().GetComponent<ControllerJoueur>().RemplirJaugeEndurance();
	}
}
