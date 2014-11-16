using UnityEngine;
using System.Collections;

public class Roadfinding_Enemy : MonoBehaviour {

	//Component pour trouver le chemin
	NavMeshAgent m_agent;
	GameObject m_player;
	Transform m_transform;

	//vitesse de mouvement
	float m_movSpeed = 0.5f;

	// Use this for initialization
	void Start () {
	
		m_transform = this.transform;
		m_agent = this.GetComponent<NavMeshAgent> ();

		// localiser Player!
		m_player = GameObject.FindGameObjectWithTag ("Player");




	}
	
	// Update is called once per frame
	void Update () {


		//Definir la destination de la recherche de chemin
		m_agent.SetDestination (m_player.transform.position);

	
		float speed = m_movSpeed * Time.deltaTime;
		m_agent.Move (m_transform.TransformDirection (new Vector3 (0, 0, speed)));

	}


}
