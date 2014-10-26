using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float pointsDeVieMax = 5f;
	public float pointsDeVieActuels = 5f;

	void Start () {
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax);
	}

	public void SubirDegats(float degats) {
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax); //Au cas où il y ait eu un problème
		pointsDeVieActuels -= degats;
		if (pointsDeVieActuels <= 0) {
			DeclencherMort ();
		}
	}

	void DeclencherMort () {
		Destroy (gameObject);
	}
}
