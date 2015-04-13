﻿/**
 * \file      FPCClassic.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Controle le joueur.
 *
 * \details   Utilise un CharacterController pour bouger le gameObject associé. Similaire au FPSInputController.
 * 			  Le joueur peut courir pendant un certain temps, se baisser, sauter un nombre de fois défini, regarder autour de lui avec la souris.
 */

/*
 * Utilisé dans TerrainSoundManager , AnimationChute , MoveCamera
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FPCClassic : ControllerJoueur {

	private bool enabled = true; // variable booléenne qui servira à l'implémentation des méthodes de IScriptEtatJouable
	
	// Use this for initialization
	void Start () {
		Initialiser ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.M)&&ControlCenter.mode==ModesJeu.Debug) {
			Debug.Log ("Recharge endurance");
			vitesseCourse = 100f;
			vitesseSaut = 10f;
			jauge=jaugeMax;
		}
		if (!enabled) {
			Debug.Log ("Not enabled");
			SetVitesseNonVerticaleActuelle (0f);
			MouvementCorps(); //Motion du joueur
			return;
		}
		if(bypass) {
			return;
			Debug.Log ("Bypass");
		}
		if (!freeze) { //Si on peut bouger
			Sprint (); //On regarde la vitesse à donner au joueur
			BougerTete(); //On change la caméra
			Crouch (); //Hauteur des yeux
			MouvementCorps(); //Motion du joueur
		}
		else
		{
			vitesseNonVerticaleActuelle=0;
		}
	}
	
	/**
	 * @brief Change la vitesse du mouvement.
	 * @details Appelé toutes les frames dans Update. Si le bouton de sprint est appuyé, on change la vitesse de mouvement du charactercontroller.
	 * 	        La jauge d'endurance descend. Si elle est trop basse, sprintPossible passe à false et la course est impossible pendant un temps court.
	 * 			Quand le bouton Sprint n'est pas appuyé, cette jauge augmente jusqu'à retrouver son maximum.
	 */
	protected override void Sprint () {
		//Sprint
		if (Input.GetButton("Sprint") && sprintPossible && vitesseNonVerticaleActuelle>0)
		{
			vitesseMouvement=vitesseCourse;
			jauge = Mathf.Max (0,jauge-Time.deltaTime);
			if(jauge<=0) {
				jauge=0; //On remet à 0
				sprintPossible=false; //On ne peut plus faire le sprint pendant un certain temps
			}
		}
		else
		{
			vitesseMouvement=vitesseMarche;
			if(jauge>limiteBasseJauge) sprintPossible=true;
			if(jauge<jaugeMax) jauge = Mathf.Min (jauge+Time.deltaTime/3,jaugeMax);
		}
//		Debug.Log (jauge);
	}
	
	/**
	 * @brief Gère la caméra.
	 * @details Vérifie la position de la souris pour tourner la caméra dans un champ réduit.
	 */
	protected override void BougerTete () {
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
	protected override void Crouch () {
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
	
	/**
	 * @brief Gère le mouvement du character controller.
	 * @details S'il est possible de bouger, on repère l'appui des touches de mouvements, ainsi que les demandes de saut.
	 */
	protected override void MouvementCorps () {
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
		
		
		Vector3 vitesse = new Vector3 (vitesseHorizontale, velociteVerticale ,vitesseVerticale);
		//Coupler la rotation avec le mouvement
		vitesse = transform.rotation * vitesse;
		vitesseNonVerticaleActuelle = Mathf.Sqrt(vitesseHorizontale*vitesseHorizontale + vitesseVerticale*vitesseVerticale);
		
		if (rendreImmobile) return; //Si on ne veut pas pouvoir bouger
		cc.Move (vitesse*Time.deltaTime); //On multiplie la vitesse par la temps écoulé depuis le dernier appel à Update
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.material==null) return; //S'il n'y a pas de physic material, on ne fait rien
		PhysicMaterial pm = hit.collider.material;

		float bounciness = pm.bounciness; //Pour savoir de combien on remonte
		bounce = Mathf.Abs ( velociteVerticale * bounciness );
		//		Debug.Log ("Bounce : " + bounce);
	}

	//Affichange de l'ancienne barre d'endurance
	/*void OnGUI () {
		if (!enabled)
			return;
		//Affichage de la barre d'endurance
		GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 2 / 10, barLength, barHeight), "Endurance"); // Endurance max
		if(! (jauge/jaugeMax < 0.1) )  // La barre n'est affichée qu'au delà d'un certain seuil	
			GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 2 / 10, jauge/jaugeMax * barLength, barHeight), enduranceBarTexture); // Etat de l'endurance du joueur
	}*/

}
