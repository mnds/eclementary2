using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ContactArmeScript))]
public class ContactArmeScriptEditor : Editor {
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();//Garder les champs d'avant pour le script
		
		if (GUILayout.Button ("Actualiser paramètres")) {
			ContactArmeScript cas = (ContactArmeScript)target; //Récupérer le MoveCamera
			cas.Initialiser();
		}
	}
	
}
