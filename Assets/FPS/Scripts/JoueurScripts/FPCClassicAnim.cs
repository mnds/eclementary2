/**
 * \file      FPCClassicAnim.cs
 * \author    
 * \version   1.0
 * \date      20 novembre 2014
 * \brief     Controle le joueur.
 *
 * \details   Meme principe que FPCClassic mais avec des animations.
 */

/*
 * Utilisé dans TerrainSoundManager , AnimationChute , MoveCamera
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FPCClassicAnim : ControllerJoueur {
	Animator anim; //Pour les animations

	// Use this for initialization
	void Start () {
		Initialiser ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(bypass) return;
		if (!freeze) { //Si on peut bouger
			Sprint (); //On regarde la vitesse à donner au joueur
			BougerTete(); //On change la caméra
			Crouch (); //Hauteur des yeux
			MouvementCorps(); //Motion du joueur
		}
	}
	
	/**
	 * @brief Change la vitesse du mouvement.
	 * @details Appelé toutes les frames dans Update. Si le bouton de sprint est appuyé, on change la vitesse de mouvement du charactercontroller.
	 * 	        La jauge d'endurance descend. Si elle est trop basse, sprintPossible passe à false et la course est impossible pendant un temps court.
	 * 			Quand le bouton Sprint n'est pas appuyé, cette jauge augmente jusqu'à retrouver son maximum.
	 */
	void Sprint () {
		//Sprint
		if (Input.GetButton("Sprint") && sprintPossible && vitesseNonVerticaleActuelle>0)
		{
			anim.SetBool("run",true); //On fait courir vers l'avant
			vitesseMouvement=vitesseCourse;
			jauge = Mathf.Max (0,jauge-Time.deltaTime);
			if(jauge<=0) {
				jauge=0; //On remet à 0
				anim.SetBool ("run",false); //On arrete de courir
				sprintPossible=false; //On ne peut plus faire le sprint pendant un certain temps
			}
		}
		else
		{
			anim.SetBool ("run",false); //On ne court pas
			vitesseMouvement=vitesseMarche;
			if(jauge>limiteBasseJauge) sprintPossible=true;
			if(jauge<jaugeMax) jauge = Mathf.Min (jauge+Time.deltaTime/3,jaugeMax);
		}
		//Debug.Log (jauge);
	}
	
	/**
	 * @brief Gère la caméra.
	 * @details Vérifie la position de la souris pour tourner la caméra dans un champ réduit.
	 */
	void BougerTete () {
		if(bloquerTete) return; //Si on ne peut pas bouger la camera
		
		//Rotation latérale
		float rotationLaterale = Input.GetAxis ("Mouse X") * vitesseRotation;
		transform.Rotate (0, rotationLaterale, 0);
		
		//Rotation verticale
		rotationVerticale += Input.GetAxis ("Mouse Y") * vitesseRotation;
		rotationVerticale = Mathf.Clamp (rotationVerticale, -angleVerticalMax, angleVerticalMax);
		camera.transform.localRotation = Quaternion.Euler (-rotationVerticale, 0, 0);
	}
	
	/**
	 * @brief Permet de se baisser.
	 * @details Quand la touche Crouch est appuyé, on s'accroupit. Rappuyé redonne la hauteur de caméra initiale.
	 */
	void Crouch () {
		//S'accroupir
		if (Input.GetButtonDown("Crouch"))
		{
			anim.SetBool("crouch",!anim.GetBool("crouch")); //On change de position.
			if(cc.height==characterControllerHeightDebout) {
				cc.center=new Vector3(0,characterControllerYCenterAccroupi,0);
				cc.height=characterControllerHeightAccroupi;
			}
			else { //On remet le joueur debout en faisant garde à ce qu'il ne passe pas à travers le terrain
				float nouveauY = cc.transform.position.y + (characterControllerHeightDebout-characterControllerHeightAccroupi)/2; //Pour ne pas tomber à travers le décor
				cc.transform.position = new Vector3 (cc.transform.position.x, nouveauY, cc.transform.position.z);
				cc.center=new Vector3(0,characterControllerYCenterDebout,0);
				cc.height=characterControllerHeightDebout;
			}
		}
		
	}
	
	/**
	 * @brief Gère le mouvement du character controller.
	 * @details S'il est possible de bouger, on repère l'appui des touches de mouvements, ainsi que les demandes de saut.
	 */
	void MouvementCorps () {
		if (rendreImmobile) return; //Si on ne veut pas pouvoir bouger
		
		//Mouvement
		float vitesseVerticale = Input.GetAxis ("Vertical"); //On le garde pour pouvoir tester si on bouge selon un axe autre que y (pour les sauts)
		float vitesseHorizontale = Input.GetAxis ("Horizontal"); //On le garde pour pouvoir tester si on bouge selon un axe autre que y (pour les sauts)
		
		//Saut
		if(!cc.isGrounded) //Si on est en l'air, on augmente la vitesse de chute
		{
			if(nombreSautsFaits==0) nombreSautsFaits++; //Tomber compte pour un saut.
			velociteVerticale += Physics.gravity.y * Time.deltaTime; //La vitesse augmente lorsqu'on chute
		}
		else 
		{
			//Rebond ?
			if(bounce<=0) {//Pas de rebond
				nombreSautsFaits = 0; //Si le cc est sur le sol, alors il peut faire autant de sauts qu'il veut
				velociteVerticale = -1; //Sinon la remet à un nombre négatif fixe pour éviter qu'elle se cumule. Négatif pour que isGrounded marche
			}
			else //On rebondit
			{
				velociteVerticale=bounce;
				bounce=0; //On remet à 0
			}
		}

		if (Input.GetButtonDown("Jump") && nombreSautsFaits<nombreSautsMax //On veut sauter, on n'a pas trop sauté
		    && !(nombreSautsMax==0 && !cc.isGrounded)) //si on n'a pas encore sauté et qu'on est en l'air, pas le droit de sauter
		{
			nombreSautsFaits++; //On augmente le nombre de sauts déjà faits
			velociteVerticale = vitesseSaut; //On se met en vitesse de saut
		}

		anim.SetBool ("walkForw", vitesseVerticale > 0); //Si positif, on va tout droit
		anim.SetBool("walkBack",vitesseVerticale<0);
		anim.SetBool ("strafeLeft", vitesseHorizontale < 0);
		anim.SetBool ("strafeRight", vitesseHorizontale > 0);

		Vector3 vitesse = new Vector3 (vitesseHorizontale, velociteVerticale ,vitesseVerticale);
		//Coupler la rotation avec le mouvement
		vitesse = transform.rotation * vitesse;
		vitesseNonVerticaleActuelle = Mathf.Sqrt(vitesseHorizontale*vitesseHorizontale + vitesseVerticale*vitesseVerticale);
		
		cc.Move (vitesse*Time.deltaTime); //On multiplie la vitesse par la temps écoulé depuis le dernier appel à Update
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(bypass) return;
		if(hit.collider.material==null) return; //S'il n'y a pas de physic material, on ne fait rien
		PhysicMaterial pm = hit.collider.material;

		float bounciness = pm.bounciness; //Pour savoir de combien on remonte
		bounce = Mathf.Abs ( velociteVerticale * bounciness );
		//		Debug.Log ("Bounce : " + bounce);
	}
	
	void OnGUI () {
		if(bypass) return;
		//Affichage de la barre d'endurance
		Debug.Log ("jauge :" + jauge / jaugeMax);
		GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 2 / 10, barLength, barHeight), "Endurance"); // Endurance max
		GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 2 / 10, jauge/jaugeMax * barLength, barHeight), enduranceBarTexture); // Etat de l'endurance du joueur
	}
	

}
