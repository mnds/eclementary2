using UnityEngine;
using System.Collections;

public class ActiverFlag : MonoBehaviour, Interactif {
	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet
	public int flagActive = 0;

	public void DemarrerInteraction() {
		
	}

	public void ArreterInteraction() {

	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_)
	{
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}

	public float GetDistanceMinimaleInteraction ()
	{
		return distanceMinimaleInteraction;
	}
}
