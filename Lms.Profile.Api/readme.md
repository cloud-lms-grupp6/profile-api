# LMS Profile API

Profile API är en microservice i LMS-projektet som ansvarar för användarprofiler.

## Funktioner

- Skapa användarprofil
- Hämta alla profiler
- Hämta profil via ID
- Uppdatera profil
- Ta bort profil
- DTO-validering
- Swagger-dokumentation
- Service layer
- EF Core + SQL Server

## Arkitektur

Projektet är uppdelat enligt Clean Architecture:

- `Lms.Profile.Domain` - entiteter
- `Lms.Profile.Application` - DTOs och interfaces
- `Lms.Profile.Infrastructure` - DbContext och service implementation
- `Lms.Profile.Api` - controllers och API-konfiguration
- `Lms.Profile.Tests` - tester

## API Endpoints

| Method | Endpoint | Beskrivning |
|---|---|---|
| GET | `/api/profiles` | Hämtar alla profiler |
| GET | `/api/profiles/{id}` | Hämtar profil via ID |
| POST | `/api/profiles` | Skapar ny profil |
| PUT | `/api/profiles/{id}` | Uppdaterar profil |
| DELETE | `/api/profiles/{id}` | Tar bort profil |

## Teknik

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger
- Clean Architecture
- Dependency Injection