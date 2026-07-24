# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Shared project documentation (read this first)

This API is **one half of a two-repo project**. The frontend is a sibling repo at
`../vahedbargh-ui-vite`, and the shared, up-to-date documentation for BOTH repos lives there:

- **`../vahedbargh-ui-vite/docs/ai/00-START-HERE.md`** — start here (rules, read order, done-criteria)
- `../vahedbargh-ui-vite/docs/ai/02-ARCHITECTURE.md` — backend layers + the landing aggregate
- `../vahedbargh-ui-vite/docs/ai/04-RECIPES.md` — end-to-end feature recipe **and the EF migration procedure**
- `../vahedbargh-ui-vite/docs/ai/06-GOTCHAS.md` — traps (see below)

### Landing/CMS work happens here too

The public website + Administrator CMS are backed by the `LandingAgg` aggregate in this repo
(`Src/Domain/AggregatesModel/LandingAgg`). Landing branch: `feat/landing-endpoints`.

### Two traps that will bite you

1. **Every generated migration includes a spurious `AlterColumn` on `Clients.ApiKey`** (its default is
   randomly regenerated each build). Strip it from `Up()`, `Down()` **and** the generated `.sql`.
   Generate migrations from `Src/Persistence` with `--project . --startup-project .` so dotnet-ef
   does not rebuild and file-lock a running `Coreapi.Api.dll`.
2. **There is no global authorization fallback** — an action with no attribute is anonymous, and a
   class-level `[AllowAnonymous]` silently defeats action-level `[Authorize]`.

**After finishing a task, add a record** at
`../vahedbargh-ui-vite/docs/ai/tasks/YYYY-MM-DD-<title>.md`.

## Project Overview

**Vahedbargh** is a multi-tenant SaaS platform for managing electrical engineering projects (project lifecycle, engineer assignments, tariffs, client management, and payments). It targets the Iranian electricity sector and integrates with local payment gateways and cloud providers.

- **Language/Runtime**: C# / .NET 9.0
- **Framework**: ASP.NET Core Web API + SignalR
- **Solution file**: `ElectProject.sln`

## Common Commands

```bash
# Build
dotnet build

# Run the API (from repo root)
dotnet run --project Src/Api

# Add an EF Core migration (run from Src/Persistence)
dotnet ef migrations add <MigrationName> --startup-project ../Api

# Apply pending migrations
dotnet ef database update --startup-project ../Api

# Run tests (if a test project exists)
dotnet test
```

Deployment targets:
- **Docker**: `docker-compose up` (includes SQL Server, Traefik reverse proxy, API service)
- **Liara (Iranian cloud)**: configured via `liara.json`; the published DLL is `Coreapi.Api`

## Architecture

The solution follows **Clean Architecture** with **CQRS** via MediatR:

```
Src/
├── Api/             # Controllers, middleware, SignalR hubs, Program.cs
├── Application/     # MediatR commands/queries, FluentValidation, AutoMapper profiles, DTOs
├── Domain/          # Aggregates, entities, value objects, domain events, repository interfaces
├── Infrastructure/  # Identity/JWT, email/SMS, S3 file storage, auditing, background services
├── Persistence/     # EF Core DbContexts, repository implementations, EF configs, migrations
└── Common/          # Enums, shared models, utility classes
```

### Request Flow

1. **Controller** (inherits `BaseController`) → dispatches via `IMediator`
2. **MediatR pipeline** → `RequestValidationBehavior` (FluentValidation) → **Handler**
3. **Handler** calls repository interfaces from **Domain**, which are implemented in **Persistence**
4. **DbContext** (`CoreapiDbContext`) persists through EF Core; `IUnitOfWork` wraps the transaction

### Key Patterns

- **CQRS**: every feature under `Application/Features/<Aggregate>/` has `Commands/` and `Queries/` subdirectories with co-located handlers, validators, and DTOs.
- **Repository per aggregate**: interfaces in Domain, implementations in Persistence, registered as scoped in `Src/Persistence/DependencyInjection.cs`.
- **DI registration**: each layer exposes an extension method (`AddApplication()`, `AddInfrastructure()`, `AddPersistence()`), all called from `Program.cs`.

### Databases

| Database | Technology | Purpose |
|----------|-----------|---------|
| `vahedbargh_test` | SQL Server | Main application data (40+ tables) |
| `vahedbargh` audit | PostgreSQL | Audit trail via `AuditDbContext` |
| `notifications.db` | SQLite | SignalR notification cache |

**DbContext classes**: `CoreapiDbContext` (main), `IdentityAppDbContext` (ASP.NET Identity + custom `ApplicationUser`), `AuditDbContext` (PostgreSQL audit logs).

### API Conventions

- Routes: `/api/v{version:apiVersion}/[controller]/[action]`
- API version: `v1.0` (via `Asp.Versioning.Mvc`)
- Swagger/NSwag UI: `/api`
- SignalR hubs: `/notif` (backend) and `/frontnotif` (frontend)
- JWT tokens are accepted from both Authorization header and query string (`?access_token=`)

### Infrastructure Services

- **File storage**: AWS S3-compatible (Liara object storage) — configured under `ConfigS3` in `appsettings.json`
- **Payments**: Melli Bank gateway (`BMIMerchant` config) and Stripe
- **Auth**: JWT + refresh tokens; Google OAuth, Microsoft OAuth
- **SMS/Email**: external providers configured in `appsettings.json`
- **Caching**: Redis (`RedisConnection` config key)
- **PDF reports**: JsReport
- **Spatial data**: `NetTopologySuite` for geographic coordinates on areas/routes
- **HTML sanitization**: `HtmlSanitizer` used before persisting user-supplied HTML

## Configuration

Sensitive settings live in `Src/Api/appsettings.json` (never commit real secrets — use `dotnet user-secrets` for local development; the user secrets ID is `ae0763cb-c9e9-4adf-b4fc-767b16a2bef3`).

Key config sections: `tokenManagement`, `ConnectionString`, `AuditConnectionString`, `ConfigS3`, `BMIMerchant`, `RedisConnection`, `GoogleClientID`, `MicrosoftClientID`, `Metabase`.
