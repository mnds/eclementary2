using UnityEngine;
using System.Collections;

public class MazeDeathZone : MonoBehaviour {
	private MazeManager mazeManager;
	private GameObject fpc; //Cherche le fpc de MazeManager, initialisé dans MazeManager
	public AudioClip sonChute;

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject==fpc) //Si le fpc rentre dans le collider, il est à la position i,j
		{
			StartCoroutine(Chute ());
		}
	}

	private IEnumerator Chute () {
		//Debug.Log ("Chute");
		if(sonChute) //on met un son si on en a un
		{
			AudioSource sourceAudio = GetComponent<AudioSource>();
			sourceAudio.PlayOneShot(sonChute);
		}
		yield return new WaitForSeconds (2.0F);
		mazeManager.PlacerPersonnages();
	}

	public void SetMazeManager (MazeManager mazeManager_) { //Utilisé dans MazeManager
		mazeManager = mazeManager_;
	}
	
	public void SetFPC (GameObject fpc_) { //Utilisé dans MazeManager
		fpc = fpc_;
	}
}
