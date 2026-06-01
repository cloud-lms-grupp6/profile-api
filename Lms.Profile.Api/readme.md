# LMS Profile API

## Översikt

Profile API är en mikrotjänst i Learning Management System (LMS) som ansvarar för hantering av användarprofiler. Tjänsten gör det möjligt att skapa, hämta, uppdatera och ta bort användarprofiler samt tillhandahålla profilinformation till andra delar av systemet.

Projektet är utvecklat i ASP.NET Core Web API och följer principerna för Clean Architecture för att skapa en tydlig, skalbar och underhållbar lösning.

---

# Funktionalitet

API:t stödjer följande funktioner:

* Skapa användarprofiler
* Hämta alla profiler
* Hämta profil via ID
* Uppdatera befintlig profil
* Ta bort profil
* Validering av inkommande data via DTO:er
* JWT-autentisering
* Swagger-dokumentation
* Enhetstester

---

# Projektstruktur

Lösningen är uppdelad enligt Clean Architecture.

## Lms.Profile.Domain

Innehåller domänmodeller och affärsregler.

## Lms.Profile.Application

Innehåller DTO:er, interfaces och applikationslogik.

## Lms.Profile.Infrastructure

Innehåller databasåtkomst, Entity Framework Core och implementationer av tjänster.

## Lms.Profile.Api

Innehåller controllers, konfiguration, autentisering och Swagger.

## Lms.Profile.Tests

Innehåller enhetstester för ProfileService.

---

# Använd teknik

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger/OpenAPI
* xUnit
* Dependency Injection
* Clean Architecture

---

# API Endpoints

## Hämta alla profiler

```http
GET /api/profiles
```

## Hämta profil via ID

```http
GET /api/profiles/{id}
```

## Skapa profil

```http
POST /api/profiles
```

Exempel:

```json
{
  "userId": "user-1",
  "firstName": "Sahand",
  "lastName": "Bagheri",
  "email": "sahand@test.se"
}
```

## Uppdatera profil

```http
PUT /api/profiles/{id}
```

## Ta bort profil

```http
DELETE /api/profiles/{id}
```

---

# Säkerhet

API:t är konfigurerat för JWT-autentisering.

Skyddade endpoints kräver en giltig JWT-token:

```http
Authorization: Bearer <token>
```

Behörighetskontroll hanteras med attributet:

```csharp
[Authorize]
```

---

# Testning

Projektet innehåller enhetstester för ProfileService.

Följande funktioner testas:

* Skapa profil
* Hämta profil via ID
* Uppdatera profil
* Ta bort profil

Kör tester:

```bash
dotnet test
```

---

# Starta projektet

Återställ paket:

```bash
dotnet restore
```

Bygg projektet:

```bash
dotnet build
```

Starta API:t:

```bash
dotnet run --project Lms.Profile.Api
```

Öppna Swagger:

```text
http://localhost:5244/swagger
```

---

# Utvecklare

Utvecklad som en del av EC Utbildnings projektarbete för Learning Management System (LMS).
