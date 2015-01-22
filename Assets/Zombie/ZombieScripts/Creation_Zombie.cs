/**
 * \file      Creation_Zombie.cs
 * \author    Zep
 * \version   2.0
 * \date      20 decembre 2014
 * \brief     Creation d'un zombie toutes les m_periodeZombie secondes
 *
 * \details   Creation d'un zombie toutes les m_periodeZombie secondes, le choix de zombie est aleatoire dans la liste "Listezombie", 
 *            si le point de creation (m_transform) est dans la vue du joueur, alors il ne peut pas creer de zombies. Ceci parce qu'on ne veut
 *             pas que le joueur voie apparaitre le zombie.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creation_Zombie : MonoBehaviour {

	public List<GameObject> ZombieListe;
	public float m_periodeZombie=3.0f;
	public static int ZombieNombreMax = 10;
	public static int ZombieNombre=0;

	Transform m_camTransform;
	float periodeZombie ;  //periode de creation de zombie 
	int k ;                //nombre d'elements dans Zombieliste
	Transform m_transform;   //Transform du objet auquel on attache le script
	GameObject m_player;  // le joueur  

	public int m;
	

	// Use this for initialization
	void Awake () {
		periodeZombie = m_periodeZombie;
		k = ZombieListe.Count;
		m_transform = this.transform;
		m_player = GameObject.FindGameObjectWithTag("Player"); 
		m_camTransform = Camera.main.transform;
	}

	// Update is called once per frame
	void Update () {
		if(periodeZombie> 0) periodeZombie -= Time.deltaTime;
		else
		{   
			Create();
		}
	}
	
	void OnDrawGizmos() 
	{
		Gizmos.DrawIcon (this.transform.position, "balle.png", true);
	}


	void Create(){
		if (ZombieNombre >= ZombieNombreMax)
						return;
		m_transform.LookAt (m_player.transform);
		Vector3 lookatPoint = -m_transform.TransformDirection (Vector3.forward);
		Vector3 cameraForward =  m_camTransform.TransformDirection (Vector3.forward);
		float cos = Vector3.Dot (lookatPoint, cameraForward);  //On calcule le cosinus de l'angle entre le vecteur forward du joueur et le vecteur unitaire qui pointe du joueur a l'objet 
		
         if (cos < 0.5f) {						
			 m = (int)(Random.value * (float)k);
			if(m<k)  {
				Instantiate (ZombieListe[m], this.transform.position, Quaternion.identity); 
				ZombieNombre++;
				periodeZombie = m_periodeZombie;
			}
		}	
	}
}

/// Camera.main不能用  原因   因为之前找 “Player” 出了问题
/// TransformDirection 不能用  还是因为之前都没有附上值！
/// cos 有问题   还是没有
///如果定义成public的话  那么声明时赋的值就没用了