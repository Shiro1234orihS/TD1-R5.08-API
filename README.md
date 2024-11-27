# Projet : Application ASP.NET API et Frontend Blazor

Ce projet est une application basée sur une API ASP.NET Core et une interface utilisateur développée avec Blazor. L'API expose des services avec une base de données PostgreSQL, et l'interface utilise Playwright pour des tests end-to-end (E2E). 

---

## Structure du Projet

1. **API (ASP.NET Core)**  
    - Développement d'une API RESTful.  
    - Gestion de la base de données avec Entity Framework Core et PostgreSQL.  
    - Documentation de l'API avec Swagger.

2. **Frontend (Blazor)**  
    - Interface utilisateur dynamique utilisant Blazor.  
    - Tests automatisés avec Playwright pour valider les fonctionnalités.

---

## Frameworks et Bibliothèques

### Backend (API)

| Package                                          | Version  |
|--------------------------------------------------|----------|
| `AutoMapper`                                     | 12.0.1   |
| `AutoMapper.Extensions.Microsoft.DependencyInjection` | 12.0.1   |
| `Microsoft.EntityFrameworkCore.Tools`           | 8.0.4    |
| `Npgsql.EntityFrameworkCore.PostgreSQL`         | 8.0.4    |
| `Npgsql.EntityFrameworkCore.PostgreSQL.Design`  | 1.1.0    |
| `Swashbuckle.AspNetCore`                        | Latest   |
| `Moq` (pour les tests unitaires)                | 4.20.7   |

### Frontend (Blazor)

| Outil                     | Usage                    |
|---------------------------|--------------------------|
| `PlaywrightTests`         | Tests E2E pour Blazor.   |



