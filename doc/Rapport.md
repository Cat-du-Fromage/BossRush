HEIG-VD MCR
# Projet Builder - Rapport d'implémentation
Florian Duruz, Mathieu Rabot, Raphael Perret

## Introduction

Dans le cadre du module MCR à la HEIG-VD, nous avons développé un jeu vidéo de type Boss Rush, dans lequel le joueur doit affronter des vagues successives d’ennemis. Le gameplay repose sur une montée progressive en difficulté, avec, toutes les cinq vagues, l’apparition d’un boss plus puissant que les ennemis standards. L’objectif principal pour le joueur est de survivre le plus longtemps possible en améliorant ses compétences et en maîtrisant les mécaniques de jeu.

Ce projet a été réalisé à l’aide de MonoGame, un framework de développement de jeux multiplateforme en C#, qui nous a permis de créer une base technique solide tout en augmentant notre niveau de compétence en programmation orientée objet et en design patterns.

## Instruction de Compilation

Pour compiler et exécuter le projet, vous devez disposer de l’environnement de développement .NET 8.0 SDK ou supérieur ainsi que de MonoGame 3.8 correctement installés sur votre machine. Le projet peut être ouvert et compilé avec Visual Studio 2022 ou toute autre IDE compatible avec C# et .NET.

## Contexte de Mise en Oeuvre

L'implémentation du jeu a été réalisée en utilisant le design pattern Builder, qui nous a permis de structurer le code de manière modulaire et extensible. Ce choix a été motivé par la nécessité de créer des objets complexes (comme les ennemis) avec de nombreuses variations sans alourdir la hiérarchie des classes.

## Architecture du Projet
Le projet est organisé en plusieurs classes et namespaces, chacun ayant une responsabilité spécifique. Voici un aperçu des principales classes et de leur rôle, ce qui nous a permis de se répartir le code sans trop de conflits :

### Animations:
Les animations sont gérées par la classe `Animation`, qui permet de créer des animations à partir de spritesheets. Chaque animation est définie par une série de frames, une durée et un état (active ou inactive). Les animations sont surtout utilisées pour le joueur, en lui donnant une animation de marche, attente et dash.

### Joueur:
```csharp
// Todo
```
### Dash:
Le dash est une mécanique de jeu qui permet au joueur de se déplacer rapidement sur une courte distance, esquivant ainsi les attaques ennemies. Il est implémenté dans la classe `Dash`, qui gère la logique du dash, y compris la durée, la distance et les frames d'invincibilité. Le dash utilise une courbe sinusoïdale pour simuler un mouvement fluide et naturel.

### Scenes Menu et Game:
Les scènes ont été séparées en deux classes distinctes : `Menu` et `Game`. La `Menu` gère l'interface utilisateur du menu principal, permettant au joueur de démarrer le jeu, ou de le quitter. La classe `Game` gère la logique du jeu, y compris le joueur, les ennemis, les vagues et les interactions.

### Game/SceneManager:
Le `GameManager` est responsable de la gestion globale du jeu, y compris le lancement des vagues et leurs logique. Tandis que le `SceneManager` gère les différentes scènes du jeu, comme le menu principal et l'écran du jeu. Il permet de charger, décharger et changer de scène en douceur.

### Particules:
Les particules sont divisées en quatre types : `Particle`, `ParticleSystem`, `ParticleEmitter` et `ParticlePreserts`. Elles sont utilisées pour créer des effets visuels dynamiques, comme les explosions ou les impacts. Chaque type de particule peut être configuré avec des propriétés spécifiques, telles que la durée de vie, la taille et la couleur.

Les textures des particules sont chargées à partir de fichiers PNG qui sont chargé dans le dossier `Content/Particles`. Les particules sont ensuite créées et gérées par le `ParticleSystem`, qui utilise des émetteurs pour générer des particules à des positions spécifiques dans le jeu.

#### Particle:
La classe `Particle` représente une particule individuelle, avec des propriétés telles que la position, la vitesse et la durée de vie. Elle est utilisée pour créer des effets visuels simples.

#### ParticleEmitter:
La classe `ParticleEmitter` est responsable de l'émission des particules. Elle définit la position, la direction et la vitesse initiale des particules. Les émetteurs peuvent être configurés pour émettre des particules en continu ou à intervalles réguliers.

Elle contient aussi un système d'object pooling pour réutiliser les particules existantes au lieu de créer de nouvelles instances, ce qui améliore les performances du jeu.

#### ParticlePresets:
La classe `ParticlePresets` contient des configurations prédéfinies pour les particules, telles que les explosions, les impacts de balles, etc. Elle permet de créer rapidement des particules avec des paramètres cohérents.

#### ParticleSystem:
La classe `ParticleSystem` fait le lien entre le `ParticleEmitter` et le `ParticlePresets`. Elle gère la création et la mise à jour des particules, en utilisant les émetteurs.

### UIComponents:
Les composants d'interface utilisateur (UI) sont gérés dans le dossier `UIComponents`. Chaque composant est responsable d'une partie spécifique de l'interface, comme les boutons ou la barre de vie. Ces composants ont été conçus pour être réutilisables et configurables au besoin, permettant une personnalisation facile de l'interface utilisateur.

### Ennemies:
les ennemies apparaissent à chaque niveau, leur nombre, types et puissances augmentent en fonction du niveau de la vague.

### Global:
La classe `Global` contient des constantes et des paramètres globaux utilisés dans tout le jeu, tels que les dimensions de la fenêtre, les couleurs et les ressources partagées. Elle permet de centraliser la configuration du jeu et d'éviter la duplication de code.

## Utilisation du pattern

### Ennemies:
Le patterne Builder a été utile pour les enemies pour créer un grands nombre d'ennemies avec des caractéristique différentes sans pour antant passer par de l'héritage simplifiant grandement l'architecture du programme.

## Conclusion
Notre projet de jeu vidéo a été une expérience enrichissante qui nous a permis de mettre en pratique nos compétences apprises en cours, notamment en programmation orientée objet et en design patterns. L'utilisation du pattern Builder a été particulièrement bénéfique pour structurer le code de manière modulaire et extensible, facilitant ainsi l'ajout de nouvelles fonctionnalités et la maintenance du projet.
Nous sommes assurement fiers du résultat final, qui offre une expérience de jeu fluide et engageante