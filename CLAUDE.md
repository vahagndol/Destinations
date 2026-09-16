# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build, run, test

```powershell
dotnet build Destinations.sln
dotnet test Destinations.sln

# One test project / one test
dotnet test tests/Locations.API.Tests
dotnet test tests/Places.API.Tests --filter FullyQualifiedName~GetPlacesTest
```

Run all three hosts (separate terminals; the Application needs both APIs up):

```powershell
dotnet run --project src/Locations.API/Locations.API.csproj   # http://localhost:5001, /swagger
dotnet run --project src/Places.API/Places.API.csproj         # http://localhost:4001, /swagger
dotnet run --project src/Application/Application.csproj       # https://localhost:65136
```

CI (`.github/workflows/ci.yml`) is restore -> build -> test on the solution, Release, .NET 10 SDK.
The `README.md` run instructions are stale (.NET Core 2.1, port 56986) — trust the above.

## Architecture

Three ASP.NET Core hosts plus two class libraries, wired as an onion:

- `src/Domain` — `EntityBase` (Id/Name/Summary/ImageUri) and its subclasses `Location`, `Place`.
- `src/Infrastructure` — the generic data pipeline, all constrained `where T : EntityBase`.
- `src/Locations.API`, `src/Places.API` — one microservice per entity type.
- `src/Application` — a BFF that proxies to both APIs and hosts the Angular SPA.

### The generic pipeline (the thing to understand first)

Every microservice is the same four-link chain, closed over one entity type in `Program.cs`:

```
IContextReader<T>            per-service, reads a JSON file from the output dir
  -> ApplicationDbContext<T> holds IList<T> in memory; ctor calls reader.ReadContext()
  -> Repository<T>           GetAll / FindById / Insert / Delete / Update over that list
  -> IEntityService<T>       business layer the controller depends on
```

There is no database. `locations.json` / `places.json` are `Content` items copied to the build
output and read by filename from the working directory. Adding a microservice means: an entity in
`Domain`, an `IContextReader<T>` implementation, a JSON file with `CopyToOutputDirectory`, and the
four `AddScoped` lines in a new `Program.cs`.

### Where the two services deliberately differ

`Places.API` is the customized instance and shows the extension points:

- `PlaceContextReader` flattens `places.json`'s `{ cityId, places[] }` shape (`PlacesDto`) into a
  flat `IList<Place>`, stamping `CityId` onto each child.
- `PlaceService : EntityService<Place>` overrides `GetItems(string)` to filter by `CityId` instead
  of the base's filter-by-`Name`. That override is what makes `GET /api/place/{cityId}` work.
- `IApplicationDbContext<Place>` is registered **singleton** in `Places.API/Program.cs`, while
  `Locations.API` registers it **scoped** (re-reading the JSON per request). Intentional divergence
  — don't "unify" it without a reason.

### Endpoints

| Host | Route | Notes |
| --- | --- | --- |
| Locations.API | `GET /api/location`, `GET /api/location/{id}` | 404 + log on unknown id |
| Places.API | `GET /api/place`, `GET /api/place/{cityId}` | id route param is the **city** id |
| Application | `GET /api/locations/GetAll`, `GET /api/places/GetAll/{locationId}` | passthrough |

`Application/Program.cs` registers named `HttpClient`s `"locations"` and `"places"`; their base
addresses come from the `LocationsUrl` / `PlacesUrl` config keys. `appsettings.Development.json`
points them at localhost:5001/4001; `appsettings.json` points at the Azure deployment. The
Application controllers return the downstream response body as a raw string — they do not
deserialize into domain types.

## Gotchas

- **Case-insensitive JSON is load-bearing.** Both context readers pass
  `new JsonSerializerOptions { PropertyNameCaseInsensitive = true }`. The seed files use camelCase;
  without this the entities deserialize to all-default values and the APIs silently serve empty
  data. Don't drop it.
- **Test DI is a hand-maintained copy of `Program.cs`.** `tests/*/Setup/TestContext.cs` builds its
  own `WebHostBuilder` and repeats every registration, plus
  `.AddApplicationPart(typeof(...Controller).Assembly)` so the controllers are discovered at all.
  Change a registration in `Program.cs` -> change it there too.
- **Tests assert exact seed-data counts** (3 locations, 9 places, and specific ids). Editing
  `locations.json` / `places.json` breaks them.
- Production code serializes with `System.Text.Json`; the tests deserialize with `Newtonsoft.Json`,
  which arrives transitively via the MSTest packages rather than a direct `PackageReference`.
- The context readers end with `return (IList<T>) locations;` — that cast only holds because `T` is
  exactly `Location` / `Place`. A more specific closure would throw at runtime.
- `Nullable` is enabled solution-wide but the `Domain` entities predate it, so builds carry
  nullability warnings. Don't treat them as new breakage.

## ClientApp (Angular) — currently broken

`src/Application/ClientApp` is an Angular 5-era app (CLI 1.7, TypeScript 2.5) with a stray
Dependabot bump of `@angular/core` to `^11.0.5`, so `npm install` / `ng build` do not work. The .NET
build no longer invokes it (the SPA targets were removed from `Application.csproj`), and
`src/Application/wwwroot` contains only `favicon.ico` — so the SPA is **not served**, and
`GET /` (which redirects to `/index.html`) 404s. Components live under `ClientApp/src/app/`
(`home`, `locations`, `places`, `account`, `nav-menu`) and call the Application's own
`api/Locations/GetAll` and `api/places/GetAll/{locationId}` routes.

A full rebuild on a current Angular version is planned but not started; check with the user before
investing in this directory.
