/**
 * \file      LireMusiques.cs
 * \author    
 * \version   1.0
 * \date      24 février 2015
 * \brief     Lit des musiques de façon aléatoire.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LireMusiques : MonoBehaviour {
	private AudioSource audioS;
	public List<AudioClip> acs;
	//Pour pouvoir imposer un changement de musique depuis ControlCenter
	private bool musiqueImposee = false; //Pour pouvoir empecher le changement de musique alors qu'on est dans une chanson imposée
	private List<AudioClip> acsInitial; //Pour si on veut changer
	// Use this for initialization
	void Awake () {
		ControlCenter.SetLireMusiques(this); //Dire au ControlCenter qui gère la musique d'ambiance.
		//On choisit un clip au hasard
		AudioClip ac=acs[Random.Range (0,acs.Count)];
		float duree=ac.length+1; //Cooldown

		//On le lit
		audioS=gameObject.GetComponent<AudioSource>();
		audioS.PlayOneShot(ac);
		//On attend le temps que le clip se finisse pour en envoyer un autre.
		StartCoroutine(AttendreFinSon(duree));
	}

	/**
	 * @brief Attend que le son actuel soit fini pour lancer un autre clip
	 * @param duree Temps à attendre avant le changement
	 */
	public IEnumerator AttendreFinSon(float duree) {
		yield return new WaitForSeconds(duree);
		ChangerSon ();
	}

	/**
	 * @brief Change de son
	 * @details Prend un clip au hasard, le lance et appelle la coroutine d'attente. Rien ne se passe si un clip
	 * 			est encore en train de tourner, ce qui arrive si ChangerClipAmbiance est appelé.
	 */
	private void ChangerSon() {
		if(audioS.isPlaying) return; //Pour empecher de couper alors que la chanson tourne
		AudioClip ac=acs[Random.Range (0,acs.Count)];
		float duree=ac.length;
		audioS.PlayOneShot(ac);
		StartCoroutine(AttendreFinSon(duree));
	}

	/**
	 * @brief Impose le changement du clip d'ambiance par une boucle du clip donné en argument.
	 * @param ac_ Clip à faire tourner en boucle.
	 * @details Cette fonction est appelée à travers ControlCenter par exemple.
	 */
	public void ChangerClipAmbiance (AudioClip ac_) {
		acs=new List<AudioClip>(){ac_};
		audioS.Stop(); //Pour pouvoir changer de chanson
		ChangerSon ();
	}
}
