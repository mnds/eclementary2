/**
 * \file      Couleurs.cs
 * \author    BC
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Contient les valeurs RGB de plusieurs couleurs.
 */

using UnityEngine;
using System.Collections;

static public class Couleurs {
	static private Color returnColor (int r, int g, int b) {return new Color (r / 256F, g / 256F, b / 256F);}

	static public Color abricot () {return returnColor (230, 126, 48);}
	static public Color ambreJaune () {return returnColor (240,195,0);}
	static public Color banane () {return returnColor (209, 182, 6);}
	static public Color bleuMinuit () {return returnColor (0,51,102);}
	static public Color blanc () {return returnColor (255,255,255);}
	static public Color bleuRoi () {return returnColor (49,140,231);}
	static public Color bronze () {return returnColor (97,60,17);}
	static public Color citron () {return returnColor (247,255,60);}
	static public Color coquelicot () {return returnColor (198,8,0);}
	static public Color creme () {return returnColor (253,241,184);}
	static public Color emeraude () {return returnColor (1, 215, 88);}
	static public Color feu () {return returnColor (255,73,1);}
	static public Color melon () {return returnColor (222,152,22);}
	static public Color miel () {return returnColor (218,179,10);}
	static public Color noirCharbon () {return returnColor (0,0,16);}
	static public Color ocreJaune () {return returnColor (223, 175, 44);}
	static public Color opale () {return returnColor (102,204,204);}
	static public Color or () {return returnColor (255,215,0);}
	static public Color orange () {return returnColor (255,127,0);}
	static public Color orangeBrulee () {return returnColor (204, 85, 0);}
	static public Color orchidee () {return returnColor (218,112,214);}
	static public Color paille () {return returnColor (254,227,71);}
	static public Color prune () {return returnColor (129,20,83);}
	static public Color rose () {return returnColor (253,108,158);}
	static public Color rougeSang () {return returnColor (133,6,6);}
	static public Color rouille () {return returnColor (152,87,23);}
	static public Color rouge () {return returnColor (255,0,0);}
	static public Color rubis () {return returnColor (224,17,95);}
	static public Color safre () {return returnColor (1,49,180);}
	static public Color soufre () {return returnColor (255,255,107);}
	static public Color violet () {return returnColor (127,0,255);}
}
