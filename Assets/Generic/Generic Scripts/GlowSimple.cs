using UnityEngine;
using System.Collections.Generic;

public class GlowSimple : MonoBehaviour {
	private bool glow; //Activate or not
	private List<Shader> shaders = new List<Shader>(){};
	private List<Color> couleurs = new List<Color>(){};
	private Material mat;

	// Use this for initialization
	void Start () {
		foreach (Material material in renderer.sharedMaterials) { //On cherche tous les materiaux de l'objet
			shaders.Add (material.shader);
			couleurs.Add (material.color);

		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("Glow");
			ActivateGlow();
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log ("UnGlow");
			DesactivateGlow();
		}
	}

	public void ActivateGlow () {
		int i = 0;
		Couleurs couleursScript = new Couleurs();
		foreach (Material material in renderer.sharedMaterials) { //On cherche tous les materiaux de l'objet

			Shader shad = material.shader;
			material.shader=Shader.Find ("Self-Illumin/"+ReturnTypeShad(shad));
			material.color=couleursScript.bleuRoi();
			i++;
		}
	}

	public void DesactivateGlow () {
		int i = 0;
		foreach (Material material in renderer.sharedMaterials) { //On cherche tous les materiaux de l'objet
			material.shader=shaders[i];
			material.color=couleurs[i];
			i++;
		}
	}

	public string ReturnTypeShad (Shader shader) {
		if(shader==Shader.Find ("Bumped Diffuse")) 
			return "Bumped Diffuse";
		if(shader==Shader.Find ("Bumped Specular")) 
			return "Bumped Specular";
		if(shader==Shader.Find ("Diffuse")) 
			return "Diffuse";
		if(shader==Shader.Find ("Parallax Diffuse")) 
			return "Parallax Diffuse";
		if(shader==Shader.Find ("Parallax Specular")) 
			return "Parallax Specular";
		if(shader==Shader.Find ("Specular")) 
			return "Specular";
		if(shader==Shader.Find ("VertexLit")) 
			return "VertexLit";
		return "Diffuse";

	}
}
