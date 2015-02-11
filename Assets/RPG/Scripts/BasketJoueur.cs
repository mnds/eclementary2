﻿using UnityEngine;
using System.Collections;

public class BasketJoueur : MonoBehaviour {
     
	public GameObject ballonBasket;
	private bool right=true;
	GameObject bar;
	GameObject arrow;
	public Vector3 minForceDeLance = new Vector3(0,25f,20f);
	public Vector3 maxForceDeLance = new Vector3(0,50f,40f);
	public Vector3 arrowVitesse = new Vector3 (0.05f,0,0);
	public Vector3 forceDeLancer = new Vector3();
	public GameObject arrow2;
	GameObject ballPosition;
	Camera mainCamera;

	public Vector3 posInitialeArrow = new Vector3();
	public Transform posArrow;
	float t = 0;
	public float relativePos;

	private bool enabled = false; // Permet l'activation/désactivation du script

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
	}

	void OnLevelWasLoaded( int level ) {
		arrow = GameObject.Find ("Arrow");
		arrow2 = GameObject.Find ("Arrow2");
		bar = GameObject.Find("Bar");
		ballPosition = GameObject.Find("BallPosition");
		if (arrow != null) {
			posArrow = arrow.transform; 
			posInitialeArrow = arrow.transform.position;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!enabled) // Lorsque le minijeu n'est pas activé
			return;
		if (t > -1)  t -= Time.deltaTime;
		relativePos = posArrow.position.x - posInitialeArrow.x;
		
		float amplForceY = maxForceDeLance.y - minForceDeLance.y;
		float amplForceZ = maxForceDeLance.z - minForceDeLance.z;
		
		forceDeLancer = (minForceDeLance + maxForceDeLance)/2.0f;
		forceDeLancer.y += relativePos / 50f * (amplForceY / 2f);
		forceDeLancer.z += relativePos / 50f * (amplForceZ / 2f);
		
		BougeArraw (relativePos);
		
		if (t<=0 && Input.GetMouseButtonDown(0)) {
			t = 1.0f;
			GameObject basketBall =Instantiate(ballonBasket,ballPosition.transform.position,mainCamera.transform.rotation) as GameObject; 
			if(basketBall !=null)  
				basketBall.rigidbody.AddRelativeForce(forceDeLancer, ForceMode.Impulse)	;
			float xx = posArrow.position.x;
			float yy = arrow2.transform.position.y;
			float zz = arrow2.transform.position.z;
			arrow2.transform.position = new Vector3(xx, yy, zz);
			Destroy(basketBall,5);
		}
	}

	void BougeArraw (float relativePos)
	{
		if ( relativePos <  50f && right) {
			posArrow.position  += arrowVitesse;
		}
		if ( relativePos >= 50f ) {
			right = false;
			posArrow.position -= arrowVitesse;
		}
		if ( relativePos >  -50f && !right) {
			posArrow.position -= arrowVitesse;
		}
		if ( relativePos <= -50f) {
			posArrow.position += arrowVitesse;
			right = true;
		}

	}

	// Implémentation de IScriptEtatJouable
	public bool isEnabled() {
		return enabled;
	}

	public void setEnabled( bool ok ) {
		enabled = ok;
	}
}
