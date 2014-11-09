using UnityEngine;
using System.Collections;

public class Couleurs {
	private Color returnColor (int r, int g, int b) {return new Color (r / 256F, g / 256F, b / 256F);}

	public Color abricot () {return returnColor (230, 126, 48);}
	public Color ambreJaune () {return returnColor (240,195,0);}
	public Color banane () {return returnColor (209, 182, 6);}
	public Color bleuMinuit () {return returnColor (0,51,102);}
	public Color blanc () {return returnColor (255,255,255);}
	public Color bleuRoi () {return returnColor (49,140,231);}
	public Color bronze () {return returnColor (97,60,17);}
	public Color citron () {return returnColor (247,255,60);}
	public Color coquelicot () {return returnColor (198,8,0);}
	public Color creme () {return returnColor (253,241,184);}
	public Color emeraude () {return returnColor (1, 215, 88);}
	public Color feu () {return returnColor (255,73,1);}
	public Color melon () {return returnColor (222,152,22);}
	public Color miel () {return returnColor (218,179,10);}
	public Color noirCharbon () {return returnColor (0,0,16);}
	public Color ocreJaune () {return returnColor (223, 175, 44);}
	public Color opale () {return returnColor (102,204,204);}
	public Color or () {return returnColor (255,215,0);}
	public Color orange () {return returnColor (255,127,0);}
	public Color orangeBrulee () {return returnColor (204, 85, 0);}
	public Color orchidee () {return returnColor (218,112,214);}
	public Color paille () {return returnColor (254,227,71);}
	public Color prune () {return returnColor (129,20,83);}
	public Color rose () {return returnColor (253,108,158);}
	public Color rougeSang () {return returnColor (133,6,6);}
	public Color rouille () {return returnColor (152,87,23);}
	public Color rubis () {return returnColor (224,17,95);}
	public Color safre () {return returnColor (1,49,180);}
	public Color soufre () {return returnColor (255,255,107);}
	public Color violet () {return returnColor (127,0,255);}
}
