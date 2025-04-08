# wt-desktop - Application de Gestion

## Aperçu du Projet

wt-desktop est une application de bureau conçue pour gérer des utilisateurs, des locations et des ressources. L'application est structurée en modules, notamment un module de comptabilité et un module d'administration.

## Technologies Utilisées

- **Langage de programmation**: C#
- **Framework UI**: Avalonia UI & SukiUI
- **Architecture**: MVVM (Model-View-ViewModel)
- **ORM**: Entity Framework Core
- **Tests**: xUnit

## Prérequis

- .NET 9.0 SDK ou plus récent
- Un IDE compatible:
    - Visual Studio 2022 (Windows)
    - JetBrains Rider (Windows, macOS, Linux)
    - Visual Studio Code avec extensions C# (Tous OS)

## Installation

### Cloner le Projet

```bash
git clone https://github.com/Aiizon/wt-desktop.git && cd wt-desktop
```

### Configuration de la Base de Données

Dans le projet wt-desktop.ef, copiez le fichier `.env.dist` et renommez-le en `.env`.
Modifiez les paramètres de connexion à la base de données selon vos besoins.

## Compilation et Exécution

### Avec Visual Studio (Windows)

1. Ouvrez le fichier solution `wt-desktop.sln`
2. Cliquez avec le bouton droit sur la solution dans l'explorateur de solutions
3. Sélectionnez "Restaurer les packages NuGet"
4. Appuyez sur F5 pour compiler et exécuter l'application

### Avec JetBrains Rider (Windows, macOS, Linux)

1. Ouvrez le dossier du projet
2. Rider détectera automatiquement la solution
3. Cliquez sur le bouton "Exécuter" ou appuyez sur Shift+F10

### Avec la Ligne de Commande (Tous OS)

```bash
# Restaurer les packages
dotnet restore

# Compiler le projet
dotnet build

# Exécuter l'application
dotnet run --project wt-desktop.app
```

## Structure du Projet

- **wt-desktop.app**: Application principale Avalonia UI
    - **Module**: Contient les modules de l'application (Accounting, Admin, etc.)
    - **Controls**: Contrôles UI personnalisés
    - **Core**: Fonctionnalités de base de l'application
    - **Admin**: Module d'administration
    - **Accounting**: Module de comptabilité
    - **Assets**: Ressources statiques (images, styles, etc.)

- **wt-desktop.ef**: Couche d'accès aux données avec Entity Framework
    - **Entity**: Entités de base de données (User, etc.)

- **wt-desktop.test**: Tests unitaires pour l'application

## Fonctionnalités

- Gestion des utilisateurs avec différents rôles (Admin, Comptable, Utilisateur)
- Module d'administration pour gérer les baies, unités, offres et interventions
- Module de comptabilité
- Tests unitaires pour assurer la qualité du code

## Roadmap

- [ ] filtre sur boards (en cours)
- [ ] email grisé à l'édition
- [ ] saisie password à la création d'un user / édition de son propre user
- [ ] BI
- [ ] quantité unités à la création d'une baie
- [ ] lien user <-> intervention autocomplété avec l'utilisateur connecté
- [ ] quantité d'unités d'une location
