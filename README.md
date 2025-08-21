## Applicant Web API

### Overview

This project is a .NET 8 Web API for managing applicants, built with clean architecture principles.

- **Application layer**: MediatR for CQRS, FluentValidation, pipeline behaviours for logging and validation
- **Domain**: `Applicant` entity
- **Infrastructure**: EF Core (SQL Server), Polly-enabled HTTP client for country validation
- **Web API**: Controllers + global exception handling, Swagger UI

### Key Features

- **Create applicant** with validation:
  - Name/FamilyName min length 5
  - Address min length 10
  - Email required, valid format, and unique
  - Age between 20 and 60
  - Country validated through an external service
- **Update applicant** with same validations (including email uniqueness)
- **List applicants** with optional paging; returns `Address` and `Country`
- **Get applicant by id** returning full details
- **Hire applicant** `POST /api/applicants/{id}/hire` (400 if already hired)
- **DTO mappings**: `CountryOfOrigin` maps to `Country` in response DTOs

### Run the API

From the repository root:

```powershell
dotnet run --project src/WebApi --launch-profile WebApi
```

Swagger UI will be available at `https://localhost:7292/swagger` or `http://localhost:5066/swagger`.

If you need to trust HTTPS dev cert (Windows):

```powershell
dotnet dev-certs https --trust
```

### Configuration

Set the SQL Server connection string in `src/WebApi/appsettings.json` under `Persistence:SqlServerConnectionString`.

### Database Migrations

Create a migration (if needed):

```powershell
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/WebApi --context Infrastructure.Persistence.ApplicationDbContext
```

Apply migrations:

```powershell
dotnet ef database update --project src/Infrastructure --startup-project src/WebApi --context Infrastructure.Persistence.ApplicationDbContext
```

Note: A unique index on `Applicant.EmailAdress` is configured in the model to enforce unique emails at the database level.

### API Endpoints

- `GET /api/applicants` — list applicants (query: `page`, `pageSize`)
- `GET /api/applicants/{id}` — get by id
- `POST /api/applicants` — create
- `PUT /api/applicants/{id}` — update
- `DELETE /api/applicants/{id}` — delete
- `POST /api/applicants/{id}/hire` — mark as hired

### Error Handling

Validation errors return HTTP 400 with `ValidationProblemDetails`.
Missing resources return HTTP 404 with `ProblemDetails`.

### Notes on Husky

Husky (git hooks) is not used in this repository. Any previous template references were removed from this README.
