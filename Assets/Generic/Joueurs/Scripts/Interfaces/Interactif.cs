using UnityEngine;
using System.Collections;

public interface Interactif{
	void DemarrerInteraction();
	void ArreterInteraction();
	void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_);
	float GetDistanceMinimaleInteraction ();
}
