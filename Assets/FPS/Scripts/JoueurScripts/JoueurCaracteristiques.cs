using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoueurCaracteristiques : Caracteristiques {
	/// <summary>
	/// Ajoute par Zep, a tester
	/// </summary>
	/// 

	//C'est le niveau du joueur qui va reperer tous les propiété (endurence, attaque, défense, etc)
	public List<Niveau> listeNiveauJoueur = new List<Niveau> ();
	/// Fin d ajout de Zep
	/// 

    void Start()

	{   conditionsActuelles = new List<Condition> ();
		cj = gameObject.GetComponent<ControllerJoueur> ();
		//Debug.Log ("ControllerJoueur de " + this + " est défini : " + (cj == true));
		h = gameObject.GetComponent<Health> ();
		//Debug.Log ("Health de " + this + " est défini : " + (h == true));
		Actualiser ();

		
		listeNiveauJoueur.Add (new Niveau (1, 0, 10f, 10f, 10f, 10f));
		listeNiveauJoueur.Add (new Niveau (2, 20, 20f, 20f, 20f, 20f));
		listeNiveauJoueur.Add (new Niveau (3, 30, 30f, 30f, 30f, 30f));
		SetNiveau ();
	}
	

	public void EnleverExperience (int experienceEnlevee){
		experience -= experienceEnlevee;
			TesterNiveau();
	}

	private void TesterNiveau () {  
		int i = 1;

		while ( listeNiveauJoueur[i-1]!=null && experience >= listeNiveauJoueur[i-1].experience)  i++;

		niveau = i-1;

		// Changer les attributs du script Caractéristiques
		Niveau _niveau = listeNiveauJoueur [niveau-1];
		SetAttaque (_niveau.attaque);
		SetPointsDeVie (_niveau.pointDeVie);
		SetPointsEndurance (_niveau.endurance);
		SetDefense (_niveau.endurance);
	

				}
	
	/// <summary>
	/// Ajoute par Zep 
	/// <param name="_expe">_expe.</param>
	public override void AjouterExperience(int _expe){
		experience += _expe;
		SetNiveau();
	}
	
	public void SetPointExperience(int _expe){
		experience = _expe;
		SetNiveau();
	}
	
	private void SetNiveau(){
		int i = 0;
		for (i=0; i < listeNiveauJoueur.Count; i++) {
			if( experience < listeNiveauJoueur[i].experience)  break;  		
		}
		niveau = i;
		if (niveau >= listeNiveauJoueur.Count)
						experience_avant_niveau_suivant = listeNiveauJoueur [niveau].experience - experience;
				else
						experience_avant_niveau_suivant = 0;

		Debug.Log ("Niveau du joueur est : " + niveau);
	}
	
	///FIn d ajout de Zepeng

}
