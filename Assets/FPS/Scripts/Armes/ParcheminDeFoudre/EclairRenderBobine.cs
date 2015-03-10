/**
 * \file      EclairRenderBobine.cs
 * \author    TM	
 * \version   1.0
 * \date      03 mars 2015
 * \brief     Permet de render des éclairs avec Line Renderer pour les effets visuels de la bobine
 */

using UnityEngine;
using System.Collections;

public class EclairRenderBobine : MonoBehaviour {
	
	private LineRenderer lineRenderer;
	private float maxZ = 8f;
	private int noSgmts = 10;
	private Color color = Color.white;
	private float posRange = 0.1f; //déclenche les "aléas" dans le mouvement des éclairss
	private Camera mainCamera;
		
	
	void Start () {
		mainCamera = ControlCenter.GetCameraPrincipale ();		
		Vector3 axis = mainCamera.transform.forward;

		// on place les points de manière aléatoire et on fixe les extrémités
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount (noSgmts);
		for (int i = 1; i < noSgmts-1; ++i) {
			lineRenderer.SetPosition(i,new Vector3(0f,1f,0f)*i*2f/noSgmts +new Vector3(Random.Range (-posRange,posRange),Random.Range(-posRange,posRange),Random.Range (-posRange,posRange)));
		}
		lineRenderer.SetPosition (0, new Vector3(0f,0f,0f));
		lineRenderer.SetPosition (noSgmts-1, new Vector3(0f,1f,0f));
	}
	
	// Update is called once per frame
	void Update () {
		// on fait décroitre la couleur et lorsque l'éclair est transparent on le supprime
		color.a -= 3f * Time.deltaTime;
		lineRenderer.SetColors (color, color);
		if (color.a <= 0f)
			Destroy (this.gameObject);
	}

}
