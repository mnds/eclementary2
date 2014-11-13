/**
 * \file      GlowSimple.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Controle la surbrillance d'un objet.
 *
 * \details   Modifie les shaders du renderer de l'objet pour les rendre plus lumineux, à l'aide des méthodes ActivateGlow et DesactivateGlow.
 */

/*
 * Utilisé dans Inventaire
 */
using UnityEngine;
using System.Collections.Generic;

public class GlowSimple : MonoBehaviour {
	private bool glow; //Activate or not
	private List<Shader> shaders = new List<Shader>(){};
	private List<Color> couleurs = new List<Color>(){};
	private Material mat;
	private bool activated = false ;

	// Use this for initialization
	void Start () {
		foreach (Material material in renderer.materials) { //On cherche tous les materiaux de l'objet
			shaders.Add (material.shader);
			couleurs.Add (material.color);

		}
	}

	void Update () {
		/*if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("Glow");
			ActivateGlow();
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log ("UnGlow");
			DesactivateGlow();
		}*/
	}

	/**
	 * @brief Met l'objet en surbrillance.
	 * @details Les matériaux utilisés dans le renderer de l'objet sont mis en Self-Illumin. On peut changer aussi de couleur l'objet si nécessaire.
	 */
	public void ActivateGlow () {
		if (activated) return;
		activated = true;
		int i = 0;
//		Couleurs couleursScript = new Couleurs();
		foreach (Material material in renderer.materials) { //On cherche tous les materiaux de l'objet

			Shader shad = material.shader;
			material.shader=Shader.Find ("Self-Illumin/"+ReturnTypeShad(shad));
			//material.color=couleursScript.ocreJaune();
			i++;
		}
	}

	/**
	 * @brief Remet les Materials initiaux du renderer de l'objet.
	 */
	public void DesactivateGlow () {
		if (!activated) return;
		activated = false;
		int i = 0;
		foreach (Material material in renderer.materials) { //On cherche tous les materiaux de l'objet
			material.shader=shaders[i];
			material.color=couleurs[i];
			i++;
		}
	}

	/**
	 * @brief Renvoie le nom du shader donné en argument.
	 * @param shader Le shader à étudier
	 * @return Le nom du shader en argument
	 */
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
