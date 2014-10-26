using UnityEngine;
using System.Collections;

public class DetonateOnHit : MonoBehaviour {

	public GameObject explosionPrefab;

	public float damage = 200f; //Dégats max, au centre
	public float explosionRadius = 3f; //Zone d'impact
	public Vector3 detonationPoint;

	void OnTriggerEnter (Collider objet) {
		Detonate (objet);
	}

	/* void FixedUpdate () {
		Ray ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (ray, speed * Time.deltaTime)) { //Si on va toucher quelque chose à la frame suivante, on explose
			Detonate ();
		}
	} */

	void Detonate(Collider objet) {
		Vector3 explosionPoint = transform.position + detonationPoint; //pour que l'explosion commence au niveau du bout du missile
		if (explosionPrefab != null) {
			Instantiate(explosionPrefab,explosionPoint,Quaternion.identity);
		}
		Destroy (gameObject);

		Collider[] collidersInRadius = Physics.OverlapSphere (explosionPoint, explosionRadius);
		foreach(Collider collider in collidersInRadius) {
			Health health = collider.GetComponent<Health>();
			if(health!=null){
				float dist = Vector3.Distance(explosionPoint,collider.transform.position);
				Debug.Log(dist);
				float damageRatio = Mathf.Clamp(1f - (dist/explosionRadius),0f,1f);
				Debug.Log (collider.gameObject+" subit "+damage*damageRatio+" degats");
				health.SubirDegats(damage*damageRatio);
			}
		}
	}
}
