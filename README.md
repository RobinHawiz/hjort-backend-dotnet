# Hjort Restaurant Backend API (.NET 8)

This ASP.NET Core 8 API replaces the legacy Node/Express service and powers both the public site and the internal CMS. It preserves the same DB schema and public API surface.

For endpoints, see [Swagger UI](https://hjort-backend.azurewebsites.net/swagger).

## Features

- JWT-based admin authentication (1‑hour tokens)
- Manage drink and course menus
- Manage table reservations
- Consistent error responses
- SQLite persistence
- Built-in Swagger/OpenAPI

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

Go to the `HjortApi/` directory:

```bash
cd HjortApi
```

Set JWT settings:

```bash
dotnet user-secrets set "Authentication:SecretKey" "<a-long-random-string>"
dotnet user-secrets set "Authentication:Issuer" "http://localhost:4000"
dotnet user-secrets set "Authentication:Audience" "HjortApi"
```

> SQLite connection and CORS are already configured for local dev/testing, you only need to set JWT secrets.

### 3) Run the API

```bash
dotnet run --project HjortApi     # from hjort-backend-dotnet/
```

or

```bash
dotnet run                        # from HjortApi/ folder
```

> The console will print the local URL. Swagger is available at `/swagger`.

### 4) Quick checks

- `GET /api/health` → 200 `"healthy"` (no auth)
- `GET /api/auth` → 200 only **with** a valid Bearer token

## Admin user & passwords

The SQLite file under `db/hjort.db` is preconfigured. If you need to add another admin, use the admin creator tool:

### Create a local admin

A small console tool lives in the repo to create a local admin. Run the tool and follow the prompts:

```bash
dotnet run --project Tools/AdminCreator   # from hjort-backend-dotnet/
```

or

```bash
dotnet run                                # from Tools/AdminCreator/ folder
```

> The tool hashes the password with **BCrypt** (work factor 10).
> It auto-targets `db/hjort.db` by resolving the solution root, no User Secrets required.

## Data Structures

The database is SQLite. The schema mirrors the legacy Node/Express backend. Below, “Validation” reflects **request model** rules from `HjortApi/Models/*ReqModel.cs` (DataAnnotations) that apply when creating/updating data via the API.

### `reservation`

| Field              | SQLite Type | Constraints               | Description                  | Validation (request model)                         |
| ------------------ | ----------- | ------------------------- | ---------------------------- | -------------------------------------------------- |
| `id`               | INTEGER     | PRIMARY KEY AUTOINCREMENT | Primary key.                 | —                                                  |
| `first_name`       | TEXT        | NOT NULL                  | Guest’s first name.          | Required; min 1; max 50                            |
| `last_name`        | TEXT        | NOT NULL                  | Guest’s last name.           | Required; min 1; max 50                            |
| `phone_number`     | TEXT        | NOT NULL                  | Contact number.              | Required; min 1; max 20                            |
| `email`            | TEXT        | NOT NULL                  | Email address.               | Required; min 1; max 128                           |
| `message`          | TEXT        | NOT NULL                  | Message from the guest.      | Required; max 1000                                 |
| `guest_amount`     | INTEGER     | NOT NULL                  | Number of guests.            | Integer 1–6                                        |
| `reservation_date` | TEXT        | NOT NULL                  | Reservation date/time (UTC). | ISO‑8601 string (e.g., `2025-08-12T16:30:00.000Z`) |

### `admin_user`

| Field          | SQLite Type | Constraints               | Description           | Validation (request model)                |
| -------------- | ----------- | ------------------------- | --------------------- | ----------------------------------------- |
| `id`           | INTEGER     | PRIMARY KEY AUTOINCREMENT | Primary key.          | —                                         |
| `username`     | TEXT        | NOT NULL, UNIQUE          | Admin login username. | (Created via tool; not set by public API) |
| `passwordHash` | TEXT        | NOT NULL                  | BCrypt password hash. | (Created via tool; not set by public API) |
| `email`        | TEXT        | NOT NULL                  | Admin email.          | (Created via tool; not set by public API) |
| `first_name`   | TEXT        | NOT NULL                  | Admin first name.     | (Created via tool; not set by public API) |
| `last_name`    | TEXT        | NOT NULL                  | Admin last name.      | (Created via tool; not set by public API) |

### `course_menu`

| Field       | SQLite Type | Constraints               | Description               | Validation (request model) |
| ----------- | ----------- | ------------------------- | ------------------------- | -------------------------- |
| `id`        | INTEGER     | PRIMARY KEY AUTOINCREMENT | Primary key.              | —                          |
| `title`     | TEXT        | NOT NULL                  | Menu title.               | Required; min 1; max 50    |
| `price_tot` | INTEGER     | NOT NULL                  | Total price for the menu. | Integer ≥ 1                |

### `course`

| Field            | SQLite Type | Constraints                      | Description                 | Validation (request model)              |
| ---------------- | ----------- | -------------------------------- | --------------------------- | --------------------------------------- |
| `id`             | INTEGER     | PRIMARY KEY AUTOINCREMENT        | Primary key.                | —                                       |
| `course_menu_id` | INTEGER     | NOT NULL, FK → `course_menu(id)` | Foreign key to course menu. | Integer ≥ 1                             |
| `name`           | TEXT        | NOT NULL                         | Course name.                | Required; min 1; max 200                |
| `type`           | TEXT        | NOT NULL                         | Course type.                | One of `starter` \| `main` \| `dessert` |

### `drink_menu`

| Field       | SQLite Type | Constraints               | Description                     | Validation (request model) |
| ----------- | ----------- | ------------------------- | ------------------------------- | -------------------------- |
| `id`        | INTEGER     | PRIMARY KEY AUTOINCREMENT | Primary key.                    | —                          |
| `title`     | TEXT        | NOT NULL                  | Menu title.                     | max 50                     |
| `subtitle`  | TEXT        | NOT NULL                  | Optional subtitle.              | max 50                     |
| `price_tot` | INTEGER     | NOT NULL                  | Total price for the drink menu. | Integer ≥ 1                |

### `drink`

| Field           | SQLite Type | Constraints                     | Description                | Validation (request model) |
| --------------- | ----------- | ------------------------------- | -------------------------- | -------------------------- |
| `id`            | INTEGER     | PRIMARY KEY AUTOINCREMENT       | Primary key.               | —                          |
| `drink_menu_id` | INTEGER     | NOT NULL, FK → `drink_menu(id)` | Foreign key to drink menu. | Integer ≥ 1                |
| `name`          | TEXT        | NOT NULL                        | Drink name.                | Required; min 1; max 200   |

**Foreign keys:**

- `course.course_menu_id` → `course_menu(id)` (ON DELETE CASCADE).
- `drink.drink_menu_id` → `drink_menu(id)` (ON DELETE CASCADE).

> Validation timing: with `[ApiController]`, ASP.NET Core runs DataAnnotations **after model binding and before controller action executes**. Invalid requests return **400 bad request** with one or more `ErrorResponses`.

## Relevant Projects

- CMS: https://github.com/RobinHawiz/hjort-cms
- Public frontend: https://github.com/RobinHawiz/hjort-frontend
- Legacy backend (Node/Express): https://github.com/RobinHawiz/hjort-backend
