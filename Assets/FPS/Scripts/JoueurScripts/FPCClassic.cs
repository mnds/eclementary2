using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FPCClassic : MonoBehaviour {
	CharacterController cc;
	//Sprint
	public float vitesseMarche = 6.0f; //Vitesse maximale de marche
	public float vitesseCourse = 12.0f; //Vitesse de course
	float jauge = 10.0f; //Temps maximum pendant lequel on peut courir
	float limiteBasseJauge = 2.0f; //Si la jauge se vide, il n'est plus possible de courir avant ce laps de temps

	float vitesseMouvement; //Vitesse actuelle max de mouvement selon qu'on marche ou qu'on court
	//Sensibilités pour la vitesse
	public float vitesseRotation = 3.0f; //Liée à la sensibilité de la souris
	public float vitesseSaut = 7.0f;
	//Angle de rotation de la camera en vertical
	float rotationVerticale = 0; //Relève la position de la caméra. Initialisé à 0.
	public float angleVerticalMax = 60.0f; //Pour limiter l'angle avec lequel on peut regarder vers le haut et le bas
	//Saut
	float velociteVerticale = 0; //Tient en compte de la gravité
	int nombreSautsFaits = 0; //Pour un double saut, il faut prendre en compte le nombre d'appuis sur la touche saut
	public int nombreSautsMax = 2; //Nombre de sauts maximum que le joueur peut faire. 1 pour saut, 2 pour double saut, 0 si interdit de sauter
	//S'accroupir
	public float characterControllerHeightDebout = 2.0f;
	public float characterControllerHeightAccroupi = 1.2f;

	//Bypass
	bool sprintPossible = true; //true si appuyer sur Sprint fait quelque chose, false sinon
	bool rendreImmobile = false; //Si true, les touches directionnelles sont bloquées
	bool bloquerTete = false; //La camera ne bouge plus
	bool freeze = false; //Tout bloquer. Attention, le FPC tombe pendant ce temps.

	// Use this for initialization
	void Start () {
		//Screen.lockCursor = true;
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!freeze) { //Si on peut bouger
			Sprint (); //On regarde la vitesse à donner au joueur
			BougerTete(); //On change la caméra
			Crouch (); //Hauteur des yeux
			MouvementCorps(); //Motion du joueur
		}
	}

	//Change la vitesse du mouvement
	void Sprint () {
		//Sprint
		if (Input.GetButton("Sprint") && sprintPossible)
		{
			vitesseMouvement=vitesseCourse;
			jauge-=Time.deltaTime;
			if(jauge<=0) {
				jauge=0; //On remet à 0
				sprintPossible=false; //On ne peut plus faire le sprint pendant un certain temps
			}
		}
		else
		{
			vitesseMouvement=vitesseMarche;
			if(jauge>limiteBasseJauge) sprintPossible=true;
			if(jauge<10.0f) jauge+=Time.deltaTime/3;
		}
		//Debug.Log (jauge);
	}

	void BougerTete () {
		if(bloquerTete) return; //Si on ne peut pas bouger la camera

		//Rotation latérale
		float rotationLaterale = Input.GetAxis ("Mouse X") * vitesseRotation;
		transform.Rotate (0, rotationLaterale, 0);
		
		//Rotation verticale
		rotationVerticale += Input.GetAxis ("Mouse Y") * vitesseRotation;
		rotationVerticale = Mathf.Clamp (rotationVerticale, -angleVerticalMax, angleVerticalMax);
		Camera.main.transform.localRotation = Quaternion.Euler (-rotationVerticale, 0, 0);
	}

	void Crouch () {
		//S'accroupir
		if (Input.GetButtonDown("Crouch"))
		{
			if(cc.height==characterControllerHeightDebout) {
				cc.height=characterControllerHeightAccroupi;
			}
			else { //On remet le joueur debout en faisant garde à ce qu'il ne passe pas à travers le terrain
				float nouveauY = cc.transform.position.y + (characterControllerHeightDebout-characterControllerHeightAccroupi)/2; //Pour ne pas tomber à travers le décor
				cc.transform.position = new Vector3 (cc.transform.position.x, nouveauY, cc.transform.position.z);
				cc.height=characterControllerHeightDebout;
			}
		}

	}

	void MouvementCorps () {
		if (rendreImmobile) return; //Si on ne veut pas pouvoir bouger

		//Mouvement
		float vitesseVerticale = Input.GetAxis ("Vertical") * vitesseMouvement;
		float vitesseHorizontale = Input.GetAxis ("Horizontal") * vitesseMouvement;
		
		//Saut
		if(!cc.isGrounded) //Si on est en l'air, on augmente la vitesse de chute
		{
			if(nombreSautsFaits==0) nombreSautsFaits++; //Tomber compte pour un saut.
			velociteVerticale += Physics.gravity.y * Time.deltaTime; //La vitesse augmente lorsqu'on chute
		}
		else 
		{
			nombreSautsFaits = 0; //Si le cc est sur le sol, alors il peut faire autant de sauts qu'il veut
			velociteVerticale = -1; //Sinon la remet à un nombre négatif fixe pour éviter qu'elle se cumule. Négatif pour que isGrounded marche
		}
		if (Input.GetButtonDown("Jump") && nombreSautsFaits<nombreSautsMax //On veut sauter, on n'a pas trop sauté
		    && !(nombreSautsMax==0 && !cc.isGrounded)) //si on n'a pas encore sauté et qu'on est en l'air, pas le droit de sauter
		{
			nombreSautsFaits++; //On augmente le nombre de sauts déjà faits
			velociteVerticale = vitesseSaut; //On se met en vitesse de saut
		}
		
		
		Vector3 vitesse = new Vector3 (vitesseHorizontale, velociteVerticale ,vitesseVerticale);
		//Coupler la rotation avec le mouvement
		vitesse = transform.rotation * vitesse;
		
		cc.Move (vitesse*Time.deltaTime); //On multiplie la vitesse par la temps écoulé depuis le dernier appel à Update
	}


	//Set/Get
	public void SetRendreImmobile (bool rendreImmobile_) {
		rendreImmobile = rendreImmobile_;
	}
	
	public bool GetRendreImmobile () {
		return rendreImmobile;
	}
	
	public void SetBloquerTete (bool bloquerTete_) {
		bloquerTete = bloquerTete_;
	}
	
	public bool GetBloquerTete () {
		return bloquerTete;
	}

	public void SetFreeze (bool freeze_) {
		freeze = freeze_;
	}
	
	public bool GetFreeze () {
		return freeze;
	}

}
