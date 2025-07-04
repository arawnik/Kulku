# Kulku.Web.Admin

Blazor WebAssembly admin panel for Kulku’s content management.


## 🚀 Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- PostgreSQL (local or container)  
- (Optional) Docker & Docker Compose  

- 
### 1. Clone & Configure

```bash
git clone git@github.com:arawnik/Kulku.git
cd src/Kulku.Web/Kulku.Web.Admin
```

Copy `.env.template` to `.env` and fill in secrets.


### 2. Apply Migrations

Follow instructions in the [Database Changes](../../../readme.md#-database-changes) section of main level readme.


### 3. Run Locally
```bash
cd src/Kulku.Web/Kulku.Web.Admin
dotnet run
```
- The app listens on `https://localhost:7215` by default.


## 🐳 Docker (Optional)

0. Env vars are loaded from the top-level `.env.docker`.
1. From repo root:
   ```bash
   docker-compose up -d --build admin
   ```
