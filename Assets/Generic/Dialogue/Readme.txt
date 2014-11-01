~~~ Fonctionnement des dialogues

REMARQUES 
1) Faire attention à ce que les DialogueHolder aient bien toutes les répliques associées dans leur script Dialogue. Sinon ça ne marche pas.
2) Il est conseillé de mettre toutes les phrases du DialogueJoueur en AccessibleDebut. En effet, elles ne sont accessibles que par les 
réponses que les objets leur donnent, donc il n'y a pas de sécurité à mettre du côté du joueur.

REMPLIR LES DIALOGUES
Chaque objet qui peut parler a un objet "DialogueHolder" qui contient le script Dialogue.
Chacune de ses répliques est enfant de cet objet.

Mettre manuellement toutes les répliques dans des gameObject différents.
Mettre manuellement tous ces gameObject dans le vecteur RepliquesObjet du dialogue.
Faire ça pour tout le monde.


Un dialogue peut être fait de deux façons. Soit on clique sur un objet, et il répond de lui-même
Soit on a un choix de répliques (pour le joueur), on en choisit une et voilà.
Chacune des répliques du joueur (RJ) entraine une réplique de l'objet (RO).
Plusieurs RJ pourraient donner la même RO (penser à des formulations différentes d'une même question)

Dans chaque RO, mettre dans "RepliquesPrecedentes" le/les RJ associés.
Pour remplir les répliques, leur donner un texteReplique (le texte dit), un son éventuellement, ne PAS cocher EnCours
Si la réplique est accessible au début de la scène (c'est à dire que la RJ associée est disponible au début)
	cocher AccessibleDebut
Si la réplique part d'elle-même, sans que le joueur ait besoin de cliquer sur la touche d'interaction (E par défaut)
	cocher DesactivationAutomatique

Penser un dialogue en forme d'arbre. Chaque question peut entrainer d'autres questions, et peut en "éviter" d'autres.
Remplir les deux vecteurs suivants (RepliquesRendues(In)Accessibles) comme ça.
Attention : on ne rend pas les dialogues du joueur accessibles ou inaccessibles. On rend ceux d'objets autres que lui. Voir l'exemple.

Replique suivante :
Après la fin de cette réplique, la réplique suivante est enclenchée automatiquement. Attention à prendre en compte la notion de désactivation auto.
Imaginons qu'elle soit à false ; le son fini, l'autre réplique est déclenchée immédiatement.
A true, une fois E cliqué, l'autre s'enclenche.
Sans replique suivante, avec true, on fait E sur l'objet, la réplique s'arrête. Encore une fois E, l'autre réplique s'enclenche.
L'intérêt est de pouvoir couper en plusieurs morceaux de longues répliques, afin d'avoir la place par exemple d'afficher le texte à l'écran.

Replique précédente :
Utilisation spécifique. Un objet envoie une Réponse à une Question du joueur. La réponse est une replique liée à l'objet, la question une réplique liée au joueur.
Dans le dialogue de l'objet, on met dans RepliquePrecedente la question (a). Et dans le dialogue du joueur, on met la réponse dans répliqueSuivante (b).
En effet, lorsqu'on appuie sur E, on regarde l'objet en face. On prend son dialogue. On regarde toutes ses répliques accessibles, et on liste toutes
les réponsesPrecedentes, ie les questions. Et c'est ces questions qu'on va afficher. Il faut donc savoir quelles sont les questions (a), et pouvoir
aller de la question à la réponse (b).
