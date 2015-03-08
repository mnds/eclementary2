using UnityEngine;
using System.Collections;

public class ZombieHealth : Health {
     
	Zombie_Comportement m_zombie;
	
void Start ()

	{
		m_zombie = this.GetComponent<Zombie_Comportement>();
	}
	
	override public void DeclencherMort ()
	{
		m_zombie.isdead = true;
	} 

	override protected void OnChangementPointsDeVie () {
		
	}
	
}