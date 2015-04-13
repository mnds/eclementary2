using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoueurCaracteristiques : Caracteristiques { 
	//C'est le niveau du joueur qui va reperer tous les propiété (endurence, attaque, défense, etc)
	public List<Niveau> listeNiveauJoueur = new List<Niveau> ();
	public int experience_entre_deux_niveaux; //Expérience pour passer d'un niveau à l'autre

    void Start()

	{   conditionsActuelles = new List<Condition> ();
		cj = gameObject.GetComponent<ControllerJoueur> ();
		//Debug.Log ("ControllerJoueur de " + this + " est défini : " + (cj == true));
		h = gameObject.GetComponent<Health> ();
		//Debug.Log ("Health de " + this + " est défini : " + (h == true));
		Actualiser ();

		
		listeNiveauJoueur.Add(new Niveau(1,0,10f,10f,1f,0f));
		listeNiveauJoueur.Add(new Niveau(2,50,15f,11f,2f,0f));
		listeNiveauJoueur.Add(new Niveau(3,150,20f,12f,3f,0f));
		listeNiveauJoueur.Add(new Niveau(4,300,25f,13f,4f,0f));
		listeNiveauJoueur.Add(new Niveau(5,500,30f,15f,5f,1f));
		listeNiveauJoueur.Add(new Niveau(6,800,40f,17f,6f,1f));
		listeNiveauJoueur.Add(new Niveau(7,1200,45f,18f,7f,1f));
		listeNiveauJoueur.Add(new Niveau(8,1700,55f,20f,8f,2f));
		listeNiveauJoueur.Add(new Niveau(9,2300,75f,22f,9f,3f));
		listeNiveauJoueur.Add(new Niveau(10,3000,100f,30f,12f,4f)); 
		SetNiveau ();
	}
	
	//Au cas où ...
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
		if (niveau < listeNiveauJoueur.Count) {
			experience_avant_niveau_suivant = listeNiveauJoueur [niveau].experience - experience;
			experience_entre_deux_niveaux = listeNiveauJoueur [niveau].experience - listeNiveauJoueur [niveau-1].experience;
		}
		else
			experience_avant_niveau_suivant = 0;
		Actualiser ();
		TesterNiveau ();

	}
	
	///FIn d ajout de Zepeng

}
