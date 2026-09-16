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

Build the SPA before running the Application (Node 24; see ClientApp below):

```powershell
cd src/Application/ClientApp; npm ci; npm run build   # type-check + build into ../wwwroot
```

CI (`.github/workflows/ci.yml`) has two independent jobs: `build-and-test` (restore -> build ->
test on the solution, Release, .NET 10 SDK) and `build-clientapp` (`npm ci` -> `npm run build`).
The tooling list at the bottom of `README.md` is stale (.NET Core 2.1) — trust the above.

## Architecture

Three ASP.NET Core hosts plus two class libraries, wired as an onion:

- `src/Domain` — `EntityBase` (Id/Name/Summary/ImageUri) and its subclasses `Location`, `Place`.
- `src/Infrastructure` — the generic data pipeline, all constrained `where T : EntityBase`.
- `src/Locations.API`, `src/Places.API` — one microservice per entity type.
- `src/Application` — a BFF that proxies to both APIs and hosts the Vue SPA.

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

## ClientApp (Vue 3 + Vite)

`src/Application/ClientApp` is a Vue 3 + TypeScript + Vue Router app scaffolded with create-vue.
It replaced a broken Angular 5 app. Bootstrap 5 and bootstrap-icons come from npm; Bootstrap's
JavaScript is deliberately not loaded (the nav menu collapses via Vue state).

- **Build output goes straight into `src/Application/wwwroot`** (`vite.config.ts` `build.outDir`,
  with `emptyOutDir`), which is git-ignored. Put static files like `favicon.ico` in
  `ClientApp/public/`, never in `wwwroot` — the build wipes it. The .NET build does not run npm,
  so a fresh clone serves 404s until `npm run build` has run.
- **Dev loop:** run the Application host, then `npm run dev` (http://localhost:5173); Vite proxies
  `/api` to https://localhost:65136.
- **Host routing** (`Application/Program.cs`): `UseStaticFiles` serves the build, unknown `api/**`
  routes return 404, and everything else falls back to `index.html` for history-mode routing.
  Keep `UseStaticFiles` — `MapStaticAssets` serves only files known at `dotnet build` time.
- Views (`src/views/`) load data with `useJson` (`src/composables/useJson.ts`), which re-fetches
  when a reactive URL changes. `src/assets/page.css` is shared via `<style scoped src>` by the
  Locations, Places and Account views.
- The seed data's `imageUri`s point at external sites that no longer serve those images, so the
  tables show broken images; that's the data, not the client.
- Node `^22.18.0 || >=24.12.0` (the `engines` range in `package.json`); CI uses Node 24. The
  Node 20.18 that nvm-windows has installed here is too old for Vite 8.
