~~~ Fonctionnement des dialogues

REMARQUES 
1) Faire attention � ce que les DialogueHolder aient bien toutes les r�pliques associ�es dans leur script Dialogue. Sinon �a ne marche pas.
2) Il est conseill� de mettre toutes les phrases du DialogueJoueur en AccessibleDebut. En effet, elles ne sont accessibles que par les 
r�ponses que les objets leur donnent, donc il n'y a pas de s�curit� � mettre du c�t� du joueur.

REMPLIR LES DIALOGUES
Chaque objet qui peut parler a un objet "DialogueHolder" qui contient le script Dialogue.
Chacune de ses r�pliques est enfant de cet objet.

Mettre manuellement toutes les r�pliques dans des gameObject diff�rents.
Mettre manuellement tous ces gameObject dans le vecteur RepliquesObjet du dialogue.
Faire �a pour tout le monde.


Un dialogue peut �tre fait de deux fa�ons. Soit on clique sur un objet, et il r�pond de lui-m�me
Soit on a un choix de r�pliques (pour le joueur), on en choisit une et voil�.
Chacune des r�pliques du joueur (RJ) entraine une r�plique de l'objet (RO).
Plusieurs RJ pourraient donner la m�me RO (penser � des formulations diff�rentes d'une m�me question)

Dans chaque RO, mettre dans "RepliquesPrecedentes" le/les RJ associ�s.
Pour remplir les r�pliques, leur donner un texteReplique (le texte dit), un son �ventuellement, ne PAS cocher EnCours
Si la r�plique est accessible au d�but de la sc�ne (c'est � dire que la RJ associ�e est disponible au d�but)
	cocher AccessibleDebut
Si la r�plique part d'elle-m�me, sans que le joueur ait besoin de cliquer sur la touche d'interaction (E par d�faut)
	cocher DesactivationAutomatique

Penser un dialogue en forme d'arbre. Chaque question peut entrainer d'autres questions, et peut en "�viter" d'autres.
Remplir les deux vecteurs suivants (RepliquesRendues(In)Accessibles) comme �a.
Attention : on ne rend pas les dialogues du joueur accessibles ou inaccessibles. On rend ceux d'objets autres que lui. Voir l'exemple.

Replique suivante :
Apr�s la fin de cette r�plique, la r�plique suivante est enclench�e automatiquement. Attention � prendre en compte la notion de d�sactivation auto.
Imaginons qu'elle soit � false ; le son fini, l'autre r�plique est d�clench�e imm�diatement.
A true, une fois E cliqu�, l'autre s'enclenche.
Sans replique suivante, avec true, on fait E sur l'objet, la r�plique s'arr�te. Encore une fois E, l'autre r�plique s'enclenche.
L'int�r�t est de pouvoir couper en plusieurs morceaux de longues r�pliques, afin d'avoir la place par exemple d'afficher le texte � l'�cran.

Replique pr�c�dente :
Utilisation sp�cifique. Un objet envoie une R�ponse � une Question du joueur. La r�ponse est une replique li�e � l'objet, la question une r�plique li�e au joueur.
Dans le dialogue de l'objet, on met dans RepliquePrecedente la question (a). Et dans le dialogue du joueur, on met la r�ponse dans r�pliqueSuivante (b).
En effet, lorsqu'on appuie sur E, on regarde l'objet en face. On prend son dialogue. On regarde toutes ses r�pliques accessibles, et on liste toutes
les r�ponsesPrecedentes, ie les questions. Et c'est ces questions qu'on va afficher. Il faut donc savoir quelles sont les questions (a), et pouvoir
aller de la question � la r�ponse (b).
