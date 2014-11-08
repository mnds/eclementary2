using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeTriggerManager : MonoBehaviour {
	public int i; //Position i selon MazeManager
	public int j; //Position j selon MazeManager
	private MazeManager mazeManager; //Pour lui donner toutes les informations sur la position du fpc
	private GameObject fpc; //Cherche le fpc de MazeManager, initialisé dans MazeManager
	//Evenements
	public AudioClip sonParticulesExplosion;
	private GameObject labyrintheLampe; //La lampe
	public AudioClip sonAllumageLampe;

	void Start () {
		labyrintheLampe = GameObject.Find ("LabyrintheLampe");
	}

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject==fpc) //Si le fpc rentre dans le collider, il est à la position i,j
		{
			mazeManager.SetCoordFPC(i,j);
			//On tente de déclencher un événement si on n'est pas sur les cases d'entrée ou de sortie
			if((mazeManager.GetJEntree()!=j || i!=0) && 
			   (mazeManager.GetJSortie()!=j||i!=mazeManager.GetNombreMursParCote()-1))
			{
				int determineLesEvenements = Random.Range(0,100);
				if(determineLesEvenements<5) {DeclencherExplosion();}
				else if(determineLesEvenements<10) {StartCoroutine(ClignoterLampe());}
				else if(determineLesEvenements<20) {StartCoroutine(ChangerCouleurLampe());}
				else if(determineLesEvenements<30) {StartCoroutine(ReduireRayonLampe());}
				else if(determineLesEvenements<40) {StartCoroutine(ReduireRangeLampe());}
			}
		}
	}


//Evénements possibles

	//Explosion
	private void DeclencherExplosion () {
		//Lancer un son s'il est assigné
		if(sonParticulesExplosion)
		{
			AudioSource sourceAudio = GetComponent<AudioSource>();
			sourceAudio.PlayOneShot(sonParticulesExplosion);
		}
		Debug.Log ("Explosion");
	}

	//Clignoter la lampe
	private IEnumerator ClignoterLampe () {
		Debug.Log ("Clignotement");
		//On liste les durées entre l'activation et la désactivation de la lampe
		//List<float> listeTempsClignotements = new List<float> (){0.2f,0.1f,0.1f,0.2f,0.4f,0.3f};
		//On lit la liste pour allumer/éteindre la lampe
		for(int i = 0;i<6;i++)
		{
			labyrintheLampe.light.enabled = !labyrintheLampe.light.enabled; //Changement de l'état de la lampe
			if(sonAllumageLampe) //on met un son si on en a un
			{
				AudioSource sourceAudio = GetComponent<AudioSource>();
				sourceAudio.PlayOneShot(sonAllumageLampe);
			}
//			yield return new WaitForSeconds(listeTempsClignotements[i]);
			float temps = Random.Range(0.5f,1f);
			Debug.Log("temps = "+temps.ToString());
			yield return new WaitForSeconds(temps);
		}
	}

	//La lampe change de couleur
	private IEnumerator ChangerCouleurLampe () {
		//Choix de la couleur finale
		Couleurs couleurs = new Couleurs(); //Contient une liste de couleurs
		List<Color> listeCouleursPossibles = new List<Color> (){Color.yellow,Color.red,couleurs.abricot(),
			couleurs.citron(), couleurs.rouille(), couleurs.rougeSang(), Color.grey};
		Color startColor = labyrintheLampe.light.color;
		Color endColor = listeCouleursPossibles[Random.Range(0,listeCouleursPossibles.Count)];

		float avancement = 0.0f; //Entre 0 et 1
		float startTime = Time.time;
		float changeTime = 2.0f;

		//On change graduellement
		while (avancement < 1.0f) {// tant que ça n'est pas changé
			//On utilise Lerp pour graduellement changer la couleur
			labyrintheLampe.light.color = Color.Lerp(startColor, endColor, avancement);
			//On avance la variable avancement
			avancement = (Time.time-startTime)/changeTime;
			yield return new WaitForSeconds(0.05f); //on n'actualise que toutes les 0.05s
		}

		Debug.Log (endColor);
	}

	//Reduire le rayon de la lampe pendant 60 secondes
	private IEnumerator ReduireRayonLampe () {
		int reductionDuRayon = Random.Range (1, 4);
		if(labyrintheLampe.light.spotAngle-reductionDuRayon>30)
			labyrintheLampe.light.spotAngle -= reductionDuRayon;
		Debug.Log ("Réduction de " + reductionDuRayon.ToString () + "du rayon");
		yield return new WaitForSeconds (Random.Range (45, 75));
		if (labyrintheLampe.light.range + reductionDuRayon < 66)
			labyrintheLampe.light.range += reductionDuRayon;
		Debug.Log ("Augmentation de " + reductionDuRayon.ToString () + "du rayon");
	}

	//Reduire le range de la lampe pendant 45 à 75 secondes
	private IEnumerator ReduireRangeLampe () {
		int reductionDuRange = Random.Range (1, 4);
		if(labyrintheLampe.light.range-reductionDuRange>4)
			labyrintheLampe.light.range -= reductionDuRange;
		Debug.Log ("Réduction de " + reductionDuRange.ToString () + "du range");
		yield return new WaitForSeconds (Random.Range (45, 75));
		if (labyrintheLampe.light.range + reductionDuRange < 31)
			labyrintheLampe.light.range += reductionDuRange;
		Debug.Log ("Réduction de " + reductionDuRange.ToString () + "du range");
	}
	

//Fonctions autres
	public void SetCoordinates (int i_,int j_) {
		i = i_;
		j = j_;
	}
	
	public bool DoesMatchCoordinates (int i_, int j_) {
		return (i == i_ && j == j_);
	}
	
	public void SetMazeManager (MazeManager mazeManager_) { //Utilisé dans MazeManager
		mazeManager = mazeManager_;
	}
	
	public void SetFPC (GameObject fpc_) { //Utilisé dans MazeManager
		fpc = fpc_;
	}
}