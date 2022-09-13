## Build API

Pour lancer le code, j'ai utilisé le débugger de visual studio 2022 avec .NET 6.0.

Le fichier d'entrée peut être modifié dans le fichier Program.cs à la constante : INPUT_FILE_NAME

Dans la version actuelle, le fichier "payload01.json" est lu.

## Petite étude avant lancement du code

J'ai pris le temps de tester un peut le fonctionnement de la lecture et l'écriture des fichiers json.

Date de début de la tentative : 12h40.

Architecture : 

1) Créer une interface IPhysicFactor : utilisé pour modifier la valeur de création d'énergie d'un producteur via un facteur
2) Créer un objet WindFactor dépendant du PhysicFactor : utilisé pour modifier la valeur de la création d'énergie d'un producteur via un facteur.
3) Créer un objet Productor : utilisé pour gérer les données des producteurs.
4) Créer un objet Input : utilisé pour gérer les données d'entrées de la production.
5) Créer un objet InputFuel : utilisé pour gérer les données des carburants en entrées de la producation.
6) Créer un objet InputProductor : utilisé pour gérer les données des producteurs.
7) Créer un objet OutputProductor : utilisé pour gérer et sauvegarder les données des producteurs.

Fonction : 

1) Lire le json d'entrée.
2) Parser les données du json dans les classes créées.
3) Calculer les producteurs à activer. - Point difficile, sera découpé plus tard
4) Remplire les données de sorties 
5) Écrire le fichier de sortie.

Fin du code : 15h58