/**
 * \file      Bobine.cs
 * \author    TM	
 * \version   1.0
 * \date      02 mars 2015
 * \brief     Fait qu'une bobine génère un éclair lorsqu'une autre bobine est proche
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bobine : MonoBehaviour {

	public float dureeDeVieBobine = 10f;
	float rayonAction = 5f;
	public GameObject ligneDeLaMort;
//	public GameObject ligneVisuel;

	public float cooldown = 3f; //Temps entre deux éclairs consécutifs;
	float tempsDuDernierEclair = 0f; //Tout est dans le nom

//	bool commencerEclairGenerator = false;
	List<GameObject> bobinesProches;

	private LineRenderer lineRenderer;
	private float maxZ = 8f;
	private int noSgmts = 10;
	private Color color = Color.white;
	private float posRange = 0.1f;

	private bool lignesDejaFaites = false; //Pour ne mettre de lignes qu'une seule fois entre les bobines

	// Use this for initialization
	void Start () {
		bobinesProches = new List<GameObject> (){};
	}
	
	 //Update is called once per frame
	void Update () {
//		if (commencerEclairGenerator) {
//			EclairGenerator ();
//				}
	}

//	void EclairGenerator (){
//		if(Time.time>tempsDuDernierEclair+cooldown)
//		{   tempsDuDernierEclair=Time.time;
//			foreach (GameObject go in bobinesProches)
//			{
//				if (go)
//				{   LineRenderer copieLine = gameObject.AddComponent<LineRenderer>() ;
//					lineRenderer = GetComponent<LineRenderer>();
//					copieLine.SetVertexCount (noSgmts);
//					for (int i = 1; i < noSgmts; ++i) {
//						copieLine.SetPosition(i,gameObject.transform.position+ (go.transform.position-gameObject.transform.position)*i/noSgmts +new Vector3(Random.Range (-posRange,posRange),Random.Range(-posRange,posRange),Random.Range (-posRange,posRange)));
//					}
//					copieLine.SetPosition (0, gameObject.transform.position);
//
////			Instantiate (ligneDeLaMort, this.transform.position, Quaternion.identity);
//
//		}
//			}
//		}
//	}

	// Lorsque la bobine rencontre un collider, on l'immobilise 
	void OnCollisionEnter (Collision col)
	{   
		if (lignesDejaFaites) return; //Seule la première collision est prise en compte
		lignesDejaFaites = true;
		ContactPoint contact = col.contacts[0]; //point de contact
		Vector3 pos = contact.point; //on convertit en coordonnées

		transform.rotation = Quaternion.AngleAxis (90, Vector3.left); // bobine orientée vers le haut
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;

		Collider[] hitColliders = Physics.OverlapSphere(pos, rayonAction); //on prend tous ceux près de la bobine et on génère un champ électrique
		foreach (Collider collid in hitColliders) {
			GameObject objetTouche = collid.gameObject;
			if (objetTouche.tag == ("BobineMagie") && objetTouche!=gameObject) //Si l'objet est une BobineMagie et n'est pas la bobine sur laquelle ce script est attaché
			{
				bobinesProches.Add (objetTouche);
//				commencerEclairGenerator=true;
				GameObject cloneLigne;
				cloneLigne = Instantiate(ligneDeLaMort,gameObject.transform.position+new Vector3(0f,0.45f,0f),Quaternion.LookRotation(gameObject.transform.position-objetTouche.transform.position)*Quaternion.AngleAxis(90,Vector3.left)) as GameObject;
				Debug.Log (cloneLigne.transform.localScale);
				cloneLigne.transform.localScale =new Vector3(0.03f,0f,0.03f)+ new Vector3(0f,0.5f,0f)*Vector3.Distance(gameObject.transform.position,objetTouche.transform.position);
				Debug.Log (0.285f*Vector3.Distance(gameObject.transform.position,objetTouche.transform.position));
				Destroy (cloneLigne, dureeDeVieBobine-0.3f);

			}
		}


		Destroy (gameObject, dureeDeVieBobine); // on détruit la bobine 10 sec plus tard


	
	}
}
