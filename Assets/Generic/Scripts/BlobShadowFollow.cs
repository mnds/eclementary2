using UnityEngine;
using System.Collections;

//Empeche un blob shadow projector de tourner en meme temps qu'une sphere
public class BlobShadowFollow : MonoBehaviour {
	Vector3 distanceInitialeBlobObjet;
	Transform parent;
	// Use this for initialization
	void Start () {
		parent = transform.parent;
		distanceInitialeBlobObjet = transform.position - parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parent.position + distanceInitialeBlobObjet;
		transform.rotation = Quaternion.LookRotation (-distanceInitialeBlobObjet, parent.forward);
	}
}
