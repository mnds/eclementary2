using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ContactPrefabScript))]
public class ContactPrefabScriptEditor : Editor {
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();//Garder les champs d'avant pour le script
		
		if (GUILayout.Button ("Actualiser paramètres")) {
			ContactPrefabScript cas = (ContactPrefabScript)target; //Récupérer le MoveCamera
			cas.Initialiser();
		}
	}
	
}
