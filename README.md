# Backend Grenishop

API REST pour **Grenishop**, une application e-commerce de vente de téléphones (neufs et d'occasion). Développée avec **ASP.NET Core 8** dans le cadre d'un projet d'étude.

## Table des matières

- [Stack technique](#stack-technique)
- [Architecture](#architecture)
- [Fonctionnalités](#fonctionnalités)
- [Démarrage rapide](#démarrage-rapide)
- [Prérequis](#prérequis)
- [Installation](#installation)
- [Configuration](#configuration)
- [Lancement](#lancement)
- [Endpoints API](#endpoints-api)
- [Auteur](#auteur)

## Stack technique

| Technologie           | Version | Rôle                         |
| --------------------- | ------- | ---------------------------- |
| ASP.NET Core          | 8.0     | Framework web                |
| Entity Framework Core | 9.0     | ORM                          |
| SQL Server            | —       | Base de données (production) |
| EF Core InMemory      | 9.0     | Base de données (mode démo)  |
| ASP.NET Identity      | 8.0     | Gestion des utilisateurs     |
| JWT Bearer            | 8.0     | Authentification             |
| FluentValidation      | 11.3    | Validation des données       |
| Swashbuckle           | 6.6     | Documentation Swagger        |

## Architecture

Le projet suit une architecture en couches avec le pattern **Repository + Service** :

```
BackendGrenishop/
├── Controllers/          # Points d'entrée API (7 controllers)
├── Services/
│   ├── Interfaces/       # Contrats des services
│   └── Implementations/  # Logique métier
├── Repositories/
│   ├── Interfaces/       # Contrats d'accès aux données
│   └── Implementations/  # Requêtes EF Core
├── Models/               # Entités de la base de données
├── DTOs/
│   ├── Request/          # Données entrantes (register, login, etc.)
│   └── Response/         # Données sortantes (profil, commandes, etc.)
├── Data/                 # Seeder de données de démo
├── DbContext/            # Configuration Entity Framework
├── Common/
│   ├── Middleware/        # Gestion globale des erreurs
│   ├── Helpers/          # Utilitaires (génération JWT)
│   ├── Exceptions/       # Exceptions personnalisées
│   └── Validators/       # Validateurs FluentValidation
├── Migrations/           # Migrations EF Core
└── Program.cs            # Configuration et pipeline
```

## Fonctionnalités

### Authentification

- Inscription / Connexion avec JWT
- Consultation du profil utilisateur
- Hachage des mots de passe avec ASP.NET Identity

### Gestion des produits

- CRUD complet sur les produits (téléphones)
- Filtrage des produits disponibles (non commandés)
- Pagination

### Catalogue

- Gestion des **marques** (CRUD + pagination)
- Gestion des **modèles** par marque (CRUD + filtrage par marque)
- Relation hiérarchique : Marque → Modèle → Produit

### Commandes

- Création de commandes (endpoint protégé par JWT)
- Suivi des statuts : `En attente` → `En cours` → `Livrée` / `Annulée`
- Consultation des commandes par utilisateur (`my-orders`)

### Liste de souhaits

- Ajout / suppression de modèles en favoris

### Sécurité

- Authentification JWT Bearer
- Rate limiting (100 requêtes/minute)
- Middleware de gestion centralisée des erreurs
- CORS configuré

## Démarrage rapide

L'API fonctionne **sans aucune configuration** grâce au mode InMemory. Il suffit de :

```bash
git clone https://github.com/votre-username/Backend-Grenishop.git
cd Backend-Grenishop
dotnet run
```

L'API démarre avec une base de données en mémoire pré-remplie :

| Donnée        | Contenu                                         |
| ------------- | ----------------------------------------------- |
| 4 marques     | Apple, Samsung, Xiaomi, Google                  |
| 8 modèles     | iPhone 15 Pro, Galaxy S24 Ultra, Pixel 8 Pro... |
| 12 produits   | Mix neufs/occasion                              |
| 1 compte test | `test@grenishop.com` / `Test123!`               |
| 2 commandes   | 1× "En attente" + 1× "Livrée"                   |

> Swagger UI : [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (optionnel — l'API fonctionne en mode InMemory sans base de données)
- Un IDE : Visual Studio, Rider ou VS Code

## Installation

1. **Cloner le dépôt**

```bash
git clone https://github.com/votre-username/Backend-Grenishop.git
cd Backend-Grenishop
```

2. **Restaurer les dépendances**

```bash
dotnet restore
```

3. **Configurer les secrets** (voir section suivante)

4. **Appliquer les migrations**

```bash
dotnet ef database update
```

> **Note** : Les migrations sont aussi appliquées automatiquement au démarrage de l'application.

## Configuration

L'application nécessite un fichier `appsettings.json` à la racine du projet. Un fichier d'exemple est fourni :

```bash
cp appsettings.example.json appsettings.json
```

Puis remplissez les valeurs :

| Variable                              | Description                          | Exemple                                   |
| ------------------------------------- | ------------------------------------ | ----------------------------------------- |
| `ConnectionStrings:DefaultConnection` | Chaîne de connexion SQL Server       | `Server=localhost;Database=Grenishop;...` |
| `Jwt:SecretKey`                       | Clé secrète JWT (min. 32 caractères) | Générez-la avec `openssl rand -base64 48` |
| `Jwt:Issuer`                          | Émetteur du token                    | `GrenishopAPI`                            |
| `Jwt:Audience`                        | Audience du token                    | `GrenishopClient`                         |
| `Urls:BaseUrl`                        | URL de l'application                 | `http://localhost:5000`                   |

**Alternative recommandée** — Utiliser les User Secrets de .NET :

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:SecretKey" "VotreCléSecrèteGénéréeIci_MinimumTrenteDeux"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=Grenishop;..."
```

## Lancement

```bash
dotnet run
```

L'API sera accessible sur `http://localhost:5000`. La documentation Swagger est disponible sur [`/swagger`](http://localhost:5000/swagger).

> **Mode InMemory** : Si aucune connexion SQL Server n'est configurée, l'API utilise automatiquement une base de données en mémoire avec des données de démo pré-chargées. Idéal pour tester sans installation supplémentaire.

## Endpoints API

### Authentification — `/api/Auth`

| Méthode | Route                | Description        |
| ------- | -------------------- | ------------------ |
| `POST`  | `/api/Auth/register` | Inscription        |
| `POST`  | `/api/Auth/login`    | Connexion          |
| `GET`   | `/api/Auth/profile`  | Profil utilisateur |

### Produits — `/api/Produits`

| Méthode  | Route                              | Description          |
| -------- | ---------------------------------- | -------------------- |
| `GET`    | `/api/Produits?page=1&pageSize=10` | Liste paginée        |
| `GET`    | `/api/Produits/available`          | Produits disponibles |
| `GET`    | `/api/Produits/{id}`               | Détail d'un produit  |
| `POST`   | `/api/Produits`                    | Créer un produit     |
| `DELETE` | `/api/Produits/{id}`               | Supprimer un produit |

### Marques — `/api/Marques`

| Méthode  | Route                             | Description          |
| -------- | --------------------------------- | -------------------- |
| `GET`    | `/api/Marques?page=1&pageSize=10` | Liste paginée        |
| `GET`    | `/api/Marques/{id}`               | Détail d'une marque  |
| `POST`   | `/api/Marques`                    | Créer une marque     |
| `PUT`    | `/api/Marques/{id}`               | Modifier une marque  |
| `DELETE` | `/api/Marques/{id}`               | Supprimer une marque |

### Modèles — `/api/Modeles`

| Méthode  | Route                               | Description         |
| -------- | ----------------------------------- | ------------------- |
| `GET`    | `/api/Modeles?page=1&pageSize=10`   | Liste paginée       |
| `GET`    | `/api/Modeles/by-marque/{marqueId}` | Modèles par marque  |
| `GET`    | `/api/Modeles/{id}`                 | Détail d'un modèle  |
| `POST`   | `/api/Modeles`                      | Créer un modèle     |
| `PUT`    | `/api/Modeles/{id}`                 | Modifier un modèle  |
| `DELETE` | `/api/Modeles/{id}`                 | Supprimer un modèle |

### Commandes — `/api/Commandes`

| Méthode  | Route                        | Description            |
| -------- | ---------------------------- | ---------------------- |
| `GET`    | `/api/Commandes`             | Toutes les commandes   |
| `GET`    | `/api/Commandes/{id}`        | Détail d'une commande  |
| `GET`    | `/api/Commandes/my-orders`   | Mes commandes          |
| `POST`   | `/api/Commandes`             | Créer une commande     |
| `PATCH`  | `/api/Commandes/{id}/status` | Modifier le statut     |
| `DELETE` | `/api/Commandes/{id}`        | Supprimer une commande |

### Liste de souhaits — `/api/ListeDeSouhaits`

| Méthode  | Route                       | Description          |
| -------- | --------------------------- | -------------------- |
| `GET`    | `/api/ListeDeSouhaits`      | Toute la liste       |
| `GET`    | `/api/ListeDeSouhaits/{id}` | Détail               |
| `POST`   | `/api/ListeDeSouhaits`      | Ajouter un souhait   |
| `DELETE` | `/api/ListeDeSouhaits/{id}` | Supprimer un souhait |

### Utilitaires

| Méthode | Route     | Description          |
| ------- | --------- | -------------------- |
| `GET`   | `/`       | Message de bienvenue |
| `GET`   | `/health` | Health check         |

## Auteur

**Kossa Keliane** — Développeur Backend

Projet réalisé dans le cadre d'un projet d'étude.

---

_Grenishop — Achetez vos téléphones neufs et d'occasion_ 📱
