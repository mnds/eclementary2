/**
 * \file      EclairRender.cs
 * \author    TM	
 * \version   1.0
 * \date      27 février 2015
 * \brief     Permet de render des éclairs avec Line Renderer pour les effets visuels du sort éclair
 */

using UnityEngine;
using System.Collections;

public class EclairRender : MonoBehaviour {

	private LineRenderer lineRenderer;
	private float maxZ = 8f;
	private int noSgmts = 10;
	private Color color = Color.white;
	private float posRange = 1f;
	private Camera mainCamera;


	void Start () {
		mainCamera = ControlCenter.GetCameraPrincipale ();

		Vector3 axis = mainCamera.transform.forward;

		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount (noSgmts);
		for (int i = 1; i < noSgmts; ++i) {
//			float z = ((float)i)*(maxZ)/(float)(noSgmts-1);
			lineRenderer.SetPosition(i,mainCamera.transform.position+ axis*i*20f/noSgmts +new Vector3(Random.Range (-posRange,posRange),Random.Range(-posRange,posRange),Random.Range (-posRange,posRange)));
		}
		lineRenderer.SetPosition (0, mainCamera.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		color.a -= 3f * Time.deltaTime;
		lineRenderer.SetColors (color, color);
		if (color.a <= 0f)
			Destroy (this.gameObject);
	}
}
