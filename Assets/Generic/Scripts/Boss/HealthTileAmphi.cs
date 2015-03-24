/**
 * \file      HealthTileAmphi.cs
 * \author    
 * \version   1.0
 * \date      7 mars 2015
 * \brief     Permet de changer les dalles au sol pendant le combat dans l'amphi.
 */


using UnityEngine;
using System.Collections;

public class HealthTileAmphi : HealthDestroy {
	public enum TypeTile {
		PremiereCouche,
		DeuxiemeCouche,
		TroisiemeCouche,
		QuatriemeCouche
	}

	public TypeTile type;


	private float degatsParContactPremiereCouche = 0f; //Dégats subis quand on touche une case en feu
	private float degatsParContactDeuxiemeCouche = 0f; //Dégats subis quand on touche une case en feu
	private float degatsParContactTroisiemeCouche = 0f; //Dégats subis quand on touche une case en feu
	private float degatsParContactQuatriemeCouche = 1000000f; //Dégats subis quand on touche une case tout en bas
	private float degatsParContact; //Prendra la bonne valeur dans Start

	private float pointsDeViePremiereCouche = 750000f; //Points de vie de la couche du dessus
	private float pointsDeVieDeuxiemeCouche = 250000f; //Points de vie de la couche au milieu
	private float pointsDeVieTroisiemeCouche = 100000f; //Points de vie de la couche du bas
	private float pointsDeVieQuatriemeCouche = 100000000000000f; //Points de vie de la couche tout en bas. Impossible à tuer

	private float ratioPremierChangementTexture = 0.8f; //Lorsque la case est à 80%, elle change de texture
	private float ratioAvantMiseEnFeu = 0.5f; //Lorsque la case à est 50%, elle se met en feu
	private float ratioTroisiemeChangement = 0.2f; //Lorsque la case est à 20%, elle devient noire
	private bool infligerDegats = false; //Savoir quand la dalle inflige des dégats

	public Material texture1; //Normal
	public Material texture2; //Premier changement
	public Material texture3; //Passage en feu
	public Material texture4; //Va casser
	public Material texture5; //Dalle finale



	// Use this for initialization
	void Start () {
		switch(type){
		case(TypeTile.PremiereCouche):
			pointsDeVieMax=pointsDeViePremiereCouche;
			degatsParContact=degatsParContactPremiereCouche;
			break;
		case(TypeTile.DeuxiemeCouche):
			pointsDeVieMax=pointsDeVieDeuxiemeCouche;
			degatsParContact=degatsParContactDeuxiemeCouche;
			break;
		case(TypeTile.TroisiemeCouche):
			pointsDeVieMax=pointsDeVieTroisiemeCouche;
			degatsParContact=degatsParContactTroisiemeCouche;
			break;
		case(TypeTile.QuatriemeCouche):
			pointsDeVieMax=pointsDeVieQuatriemeCouche;
			degatsParContact=degatsParContactQuatriemeCouche;
			pointsDeVieActuels=pointsDeVieMax;
			gameObject.renderer.material=texture5; //Initialisation à la texture finale
			infligerDegats=true;
			return;
			break;
		}

		pointsDeVieActuels=pointsDeVieMax;
		gameObject.renderer.material=texture1; //Initialisation à la texture de base
	}

	/**
	 * @brief Pour interagir avec le trigger externe de Joueur
	 */
	void OnTriggerEnter (Collider c) {
		Health h = c.gameObject.GetComponentInParent<Health>() ; //On récupère qui a touché
		//Si l'objet a effectivement un script Health
		if(infligerDegats && h) {
			Debug.Log(degatsParContact + "!");
			h.SubirDegats(degatsParContact);
		}
		
	}

	override protected void OnChangementPointsDeVie () {
		if(type==TypeTile.QuatriemeCouche)
			return; //Pas de changement pour la quatrieme

		//Changement textures. On commence par le plus exigeant.
		if(pointsDeVieActuels/pointsDeVieMax<=ratioTroisiemeChangement) {
			gameObject.renderer.material=texture4;
			return;
		}
		if(pointsDeVieActuels/pointsDeVieMax<=ratioAvantMiseEnFeu) {
			gameObject.renderer.material=texture3;
			infligerDegats=true; //On peut infliger des dégats
			return;
		}
		if(pointsDeVieActuels/pointsDeVieMax<=ratioPremierChangementTexture) {
			gameObject.renderer.material=texture2;
			return;
		}
	}

	public void SetTypeTile (TypeTile tt) {
		type=tt;
	}
}
