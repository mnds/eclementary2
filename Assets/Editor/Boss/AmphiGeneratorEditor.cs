using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(AmphiGenerator))]
public class AmphiGeneratorEditor : Editor {
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();//Garder les champs d'avant pour le script
		if (GUILayout.Button ("Clear")) {
			AmphiGenerator ag = (AmphiGenerator)target; //Récupérer le MoveCamera
			ag.ClearParentTiles();
		}
		if (GUILayout.Button ("Créer layout")) {
			AmphiGenerator ag = (AmphiGenerator)target; //Récupérer le MoveCamera
			ag.CreerTiles();
		}
	}
	
}
