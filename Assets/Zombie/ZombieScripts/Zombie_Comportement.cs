/**
 * \file      Zombie_Comportement.cs
 * \author    ZepengLI
 * \version   1.0
 * \date      24 novembre 2014
 * \brief     1.  Aide le zombie a trouver son chemin. Et faire correspondre les animatations aux comportement du zombie.
 *            2.  Calculer les degats au joueur si le zombie l'attaque
 * 
 * \details   Si le zombie est loin du joueur, il se balade aleatoirement, une fois le joueur rentre dans une distance du zombie, zombie va suivre le jouer jusqu'a sa mort.
 * 
 * 
 */

using UnityEngine;
using System.Collections;

public class Zombie_Comportement : MonoBehaviour {
	
	//Component pour trouver le chemin
	public float dis;
	NavMeshAgent m_agent;
	GameObject m_player;
	Transform m_transform;
	Animator m_ani;
	HealthPlayer healthPlayer;
	ZombieCaracteristiques zombieCarac;
	Caracteristiques caracJoueur;
	public float tempsResteChangeDesti;
	
	public float periodeChangementDesti=10.0f;
	float vitesseMarche = 1.0f;
	float vitesseCourse = 5.0f;
	public bool isdead = false;  // Variable boolenne qu'on fera changer dans le script "HealthZombie"
	
	bool isFollowingPlayer=false;  //Paramètre qui designe si le pathfinding est declenché. 
	public float tempsAttentePathFinding;
	// Use this for initialization
	void Start () {
		
		m_transform = this.transform;
		m_agent = this.GetComponent<NavMeshAgent> ();
		// localiser Player!
		m_player = GameObject.FindGameObjectWithTag ("Player");
		m_ani = this.GetComponent<Animator> ();
		
		healthPlayer = m_player.GetComponent<HealthPlayer>(); 
		zombieCarac = this.GetComponent<ZombieCaracteristiques>();
		caracJoueur = m_player.GetComponent<Caracteristiques>();
	}
	
	// Update is called once per frame
	void Update () {
		
		AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo (0);
		if (isdead) {
			m_ani.SetBool("die",true);
			if(stateInfo.nameHash==Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0) && stateInfo.normalizedTime>=1.0f) 
				//Si l'animation "death" est fini et qu'on n'est pas dans le processus de transition, on detruit le zombie
			{
				Destroy(gameObject);
				caracJoueur.AjouterExperience(zombieCarac.GetScoreZombie());   //On ajoute le score dans scoreDuJoueur
				Creation_Zombie.ZombieNombre--;
			}
			return;
		} 
		
		if (isFollowingPlayer) {
			m_agent.SetDestination (m_player.transform.position); //Definir la destination de la recherche de chemin,cad la position du joueur

			if (Vector3.Distance (m_player.transform.position, m_transform.position) < 2.0f) { //On rentre dans la mode d'attaque
				Rotateto (); //Faire tourner la zombie vers le joueur
				m_ani.SetBool ("attack", true);
				m_ani.SetBool ("rerun", false);
				healthPlayer.SubirDegats (zombieCarac.GetAttaque () * Time.deltaTime, gameObject.GetComponent<Caracteristiques>()); //Diminuer la vie du jouer.
				
			} else {
				m_ani.SetBool ("attack", false);
				m_ani.SetBool ("rerun", true);
				
			}
			return; 
		} else {
			dis = Vector3.Distance (m_player.transform.position, m_transform.position);//Distance entre le joueur et le zombie
			
			if (dis > 5) {             //Le joueur se situe tjr en dehors des 5 metres du zombie
				SetDestinationAleatoire();
				
			} else {                     // Le joueur entre dans 5 metres du zombie.
				isFollowingPlayer = true; 
				m_agent.speed = vitesseCourse;
				m_ani.SetBool ("run", true);
			}
		}
	}
	
	void SetDestinationAleatoire(){
		if (tempsResteChangeDesti <= 0) {
			tempsResteChangeDesti = periodeChangementDesti; 
			Vector3 positionRadom = new Vector3 (930+Random.value * 300, 200, 830 + Random.value * 350); //On dirige le zombie vers un endroit quelconque
			m_agent.SetDestination (positionRadom);
			tempsAttentePathFinding = 0;
			m_agent.speed = vitesseMarche;
		} else {
			tempsAttentePathFinding += Time.deltaTime;
			tempsResteChangeDesti -= Time.deltaTime; 
			if(tempsAttentePathFinding >= 2 && !m_agent.hasPath){  //Si au bout dee 2 secondes les zombies trouvent toujours pas de chemin, alors on change!
				Vector3 positionRadom = new Vector3 (930+Random.value * 300, 200, 830 + Random.value * 350); //On dirige le zombie vers un endroit quelconque
				m_agent.SetDestination (positionRadom);
			    tempsAttentePathFinding = 0 ;
				tempsResteChangeDesti = periodeChangementDesti;
			}
		}

		
	}
	void Rotateto()
	{
		//Angle actuel
		Vector3 oldangle = m_transform.eulerAngles;
		
		m_transform.LookAt (m_player.transform);
		float taget = m_transform.eulerAngles.y;
		
		//Faire tourner le zombie vers le joueur
		float angle = Mathf.MoveTowardsAngle(oldangle.y, taget, 120.0f * Time.deltaTime);
		m_transform.eulerAngles = new Vector3 (0, angle, 0);
		
	}
	
}
