using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ObjetEnSceneScript))]
public class ObjetEnSceneScriptEditor : Editor {
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();//Garder les champs d'avant pour le script
		
		if (GUILayout.Button ("Actualiser paramètres")) {
			ObjetEnSceneScript cas = (ObjetEnSceneScript)target; //Récupérer le MoveCamera
			cas.Initialiser();
		}
	}
	
}
