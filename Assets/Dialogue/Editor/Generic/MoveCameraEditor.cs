using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(MoveCamera))]
public class MoveCameraEditor : Editor {
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();//Garder les champs d'avant pour le script
		
		if (GUILayout.Button ("Ajouter Position GameObject Actuelle")) {
			MoveCamera moveCamera = (MoveCamera)target; //Récupérer le MoveCamera
			moveCamera.AjouterTransform(false);
		}
		if (GUILayout.Button ("Ajouter Position Camera Actuelle")) {
			MoveCamera moveCamera = (MoveCamera)target; //Récupérer le MoveCamera
			moveCamera.AjouterTransform(true);
		}
		if (GUILayout.Button ("Effectuer le trajet")) { //Disponible seulement en Play
			MoveCamera moveCamera = (MoveCamera)target; //Récupérer le MoveCamera
			moveCamera.EffectuerCoroutineTrajet();
		}
	}
	
}
