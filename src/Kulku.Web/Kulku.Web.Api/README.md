# Kulku.Web.Api

.NET 9 Minimal API for Kulku’s backend.


## 🚀 Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- PostgreSQL (local or container)
- (Optional) Docker & Docker Compose


### 1. Clone & Configure

1. Clone the repository:
   ```bash
   git clone git@github.com:arawnik/Kulku.git
   cd Kulku/src/Kulku.Web/Kulku.Web.Api
   ```
2. Set up env:
   Copy `.env.template` to `.env` and fill in secrets.


### 2. Apply Migrations

Follow instructions in the [Database Changes](../../../README.md#-database-changes) section of main level readme.


### 3. Run Locally

- From repo root:
   ```bash
   cd src/Kulku.Web/Kulku.Web.Api
   dotnet run
   ```

- API runs on `https://localhost:7219` by default.
- Sample queries in: [Kulku.Web.Api.http](Kulku.Web.Api.http)


## 🐳 Docker (Optional)

0. Env vars are loaded from the top-level `.env.docker` (by default no SSL, api port 5144).
1. From repo root:
   ```bash
   docker-compose up -d --build api
   ```
