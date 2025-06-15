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
Le système d'ennemis est un pilier central de notre jeu de type Boss Rush. Nous avons implémenté une architecture modulaire permettant de générer des vagues d'ennemies dont la difficulté évolue progressivement. Le système repose sur trois types d'ennemis distincts :

1. Ennemis Mêlée : Combattants rapprochés avec des attaques puissantes mais une portée limitée

2. Ennemis Distance : Attaquants à distance avec des dégâts modérés mais une grande zone de danger

3. Boss : Ennemis élites apparaissant tous les 5 niveaux, combinant les deux styles de combat

L'implémentation utilise plusieurs classes clés :
nemy (Classe de base)
```csharp
// Gère le comportement fondamental de tous les ennemis
// - Détection et poursuite du joueur
// - Gestion des cooldowns d'attaque
// - Collisions et dégâts
```

EnemyDirector (Factory)
```csharp
// Crée des ennemis pré-configurés selon leur type et niveau
// - Applique des modificateurs de statistiques progressifs
// - Garantit l'équilibrage entre les vagues
```

EnemySystem (Manager)
```csharp
// Contrôle l'ensemble des ennemis actifs
// - Met à jour les positions et états
// - Gère la génération des vagues
// - Implémente un système de grille spatiale pour l'optimisation
```

LevelComposition
```csharp
// Calcule la composition de chaque vague
// - Détermine le nombre et types d'ennemis
// - Intègre des boss tous les 5 niveaux
// - Gère la progression exponentielle de la difficulté
```

StatsMultiplicator
```csharp
// Applique des formules de scaling pour :
// - Santé (+15% par niveau)
// - Dégâts (+8% par niveau)
// - Vitesse (+2% par niveau)
// - Défense (logarithmique)
```

### Global:
La classe `Global` contient des constantes et des paramètres globaux utilisés dans tout le jeu, tels que les dimensions de la fenêtre, les couleurs et les ressources partagées. Elle permet de centraliser la configuration du jeu et d'éviter la duplication de code.

## Utilisation du pattern

### Ennemies:
Le pattern Builder a été essentiel pour la création d'ennemis variés tout en évitant une hiérarchie de classes complexe. Voici comment nous l'avons appliqué :

Flexibilité : Permet de configurer facilement des dizaines de combinaisons de stats (dégâts, portée, vitesse) sans explosion combinatoire

Lisibilité : Le code client reste clair grâce au chaînage de méthodes :

```csharp
new Enemy.Builder(position, Vector2.Zero)
    .WithName("Boss")
    .WithHealth(100)
    .WithDamage(20)
    .Build();
```
Validation : Le Builder intègre des contrôles pour garantir que chaque ennemi a des stats valides :

```csharp
if (enemy.Damage < 0) enemy.Damage = 0; // Valeurs minimales garanties
```
Extensibilité : Simplifie l'ajout de nouveaux attributs (comme les capacités spéciales) sans modifier les classes existantes

Ce pattern nous a particulièrement aidés pour :

Créer des variants d'ennemis avec peu de code

Maintenir un équilibrage précis entre les niveaux

Isoler la logique de construction complexe du reste du jeu

## Conclusion
Notre projet de jeu vidéo a été une expérience enrichissante qui nous a permis de mettre en pratique nos compétences apprises en cours, notamment en programmation orientée objet et en design patterns. L'utilisation du pattern Builder a été particulièrement bénéfique pour structurer le code de manière modulaire et extensible, facilitant ainsi l'ajout de nouvelles fonctionnalités et la maintenance du projet.
Nous sommes assurement fiers du résultat final, qui offre une expérience de jeu fluide et engageante