# Hjort Restaurant Backend API (.NET 8)

This ASP.NET Core 8 API replaces the legacy Node/Express service and powers both the public site and the internal CMS. It preserves the same DB schema and public API surface. For endpoints and data shapes, use the Swagger UI (`/swagger`) or the legacy README: [https://github.com/RobinHawiz/hjort-backend](https://github.com/RobinHawiz/hjort-backend). This document focuses on .NET-specific setup and local development.

## Features

- JWT-based admin authentication (1‑hour tokens)
- Manage drink and course menus
- Manage table reservations
- Consistent error responses with typed domain exceptions
- SQLite persistence via Dapper
- Built-in Swagger/OpenAPI (see `/swagger`)

## Tech Stack

- **ASP.NET Core 8** — MVC controllers for primary API endpoints, Minimal APIs for health/auth
- **Dapper** + **SQLite**
- **JWT** via `Microsoft.AspNetCore.Authentication.JwtBearer`
- **Password hashing** via `BCrypt.Net-Next`
- **Swagger/OpenAPI** via `Swashbuckle.AspNetCore`
- **Structured logs** via `Microsoft.Extensions.Logging`

## Solution Structure

```
# Repo root
HjortBackend.sln
├── HjortApi/                 # Web API
│   ├── Program.cs            # App startup & middleware
│   ├── Controllers/          # Admin, Reservation, Course*, Drink* controllers
│   ├── Endpoints/            # Utility endpoints (Minimal APIs: health/auth)
│   ├── Models/               # Request DTOs + ErrorResponse
│   └── Setups/               # DI, Auth, CORS, JSON, API behavior config
├── ServiceLibrary/           # Domain/application services
│   ├── *Service.cs           # Business rules & validation
│   └── Exceptions/           # Domain exceptions thrown by services; mapped to HTTP status codes by controllers
├── DataAccessLibrary/        # Data access (Dapper + SQLite)
│   ├── *Data.cs              # Dapper DAOs (per persistence entity: Reservation, CourseMenu, Course, DrinkMenu, Drink)
│   └── Models/               # Persistence models
├── Tools/
│   └── AdminCreator/         # Console tool to seed a local admin (dev-only)
│       └── Program.cs
└── db/
    └── hjort.db              # SQLite database (preconfigured)
```

## Running Locally

### Prerequisites

- .NET 8 SDK
- Git

### 1) Clone

```bash
git clone https://github.com/RobinHawiz/hjort-backend-dotnet
cd hjort-backend-dotnet
```

### 2) Configure secrets

This project uses .NET **User Secrets** for JWT settings (Issuer, Audience, SecretKey).

From the `HjortApi/` directory:

```bash
cd HjortApi

# JWT settings
dotnet user-secrets set "Authentication:SecretKey" "<a-long-random-string>"
dotnet user-secrets set "Authentication:Issuer" "http://localhost:4000"
dotnet user-secrets set "Authentication:Audience" "HjortApi"
```

> SQLite connection and CORS are already configured for local dev/testing, you only need to set JWT secrets.

### 3) Run the API

```bash
# run the API (pick one)
dotnet run --project HjortApi     # from repo root
# or
dotnet run                        # from HjortApi folder
```

> The console will print the local URL. Swagger is available at `/swagger`.

### 4) Quick checks

- `GET /api/health` → 200 `"healthy"` (no auth)
- `GET /api/auth` → 200 only **with** a valid Bearer token

## Admin user & passwords

The SQLite file under `db/hjort.db` is preconfigured. If you need to add another admin, use the dev seeder:

### Create a local admin (dev seeder)

A small console tool lives in the repo to create a local admin.

```bash
# run the tool and follow the prompts (pick one)
dotnet run --project Tools/AdminCreator   # from repo root
# or
dotnet run                                # from Tools/AdminCreator folder
```

> The tool hashes the password with **BCrypt** (work factor 10).
> It auto-targets `db/hjort.db` by resolving the solution root, no User Secrets required.

## Relevant Projects

- CMS: https://github.com/RobinHawiz/hjort-cms
- Public frontend: https://github.com/RobinHawiz/hjort-frontend
- Legacy backend (Node): https://github.com/RobinHawiz/hjort-backend
