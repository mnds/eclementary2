using UnityEngine;
using System.Collections;

public class BasketGagnerPoint : MonoBehaviour {
	public int score=0;

	void OnTriggerEnter(Collider other)
	{   Debug.Log("OnTrigger");
		if (other.gameObject.transform.position.y > gameObject.transform.position.y) {
            
			score += 1;		
		}

	}

	void OnGUI()
	{
		//GUI.Label(new Rect(0,Screen.height-50,100,50),"Score : " + score);
	}
}
