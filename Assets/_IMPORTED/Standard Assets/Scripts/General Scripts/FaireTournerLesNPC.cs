using UnityEngine;
using System.Collections;

public class FaireTournerLesNPC : MonoBehaviour {
	Transform m_transform;
	GameObject m_player;
	float distance = 4;
	// Use this for initialization
	void Start () {
		m_player = GameObject.FindGameObjectWithTag ("Player");
		m_transform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (m_player.transform.position, m_transform.position) < distance) {
			Rotateto ();		
		}

	}
	void Rotateto()
	{
		//Angle actuel
		Vector3 oldangle = m_transform.eulerAngles;

		m_transform.LookAt (m_player.transform);
		float taget = m_transform.eulerAngles.y;
		
		//Faire tourner le NPC vers le joueur
		float angle = Mathf.MoveTowardsAngle(oldangle.y, taget, 120.0f * Time.deltaTime);
		m_transform.eulerAngles = new Vector3 (0, angle, 0);
		
	}
}
