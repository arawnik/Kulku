Welcome to Kulku, the code repository that powers my personal CV site and its administrative backend.
This project is designed to showcase my professional profile, publicly shareable solo projects, and work as a code sample.

---


## 🌟 Key Features

### Site
- Dynamic CV & Portfolio: Easily update work experience, education, skills, and project showcases without code changes.
- Contact & Inquiries: Built-in form with spam protection and email forwarding.
- Theming & Localization: Light/dark mode toggle and multilingual support (English & Finnish).
- Responsive & Accessible: Optimized for mobile, tablet, and desktop. Also SEO optimized for search engines.

### Backend
- Clean architecture: Separation of concerns with a focus on maintainability and testability.
- CQRS & Vertical Slices: Command handlers, query handlers, and validators organized in feature slices for maintainability.


## 🧱 Architecture

### 1. System Context
A high-level list of all major components:
- **Public Site** (Next.js / React)
- **API** (ASP.NET 9 + MinimalAPI)
- **Admin site** (ASP.NET 9 + Blazor)
- **Data Store** (PostgreSQL)

### 2. Core Patterns & Principles
- **Clean Architecture**
  - Core (domain) -> Application -> Infrastructure (+Persistence) -> Presentation
  - Dependency Inversion keeps your domain pure.
- **CQRS**
  - Commands mutate state; Queries read models.
  - Handlers live alongside their feature slice.
- **Code reuse**
  - Admin and API are separate presentation layers for the core

### 3. Data Access & Persistence
- **EF Core + Repository Pattern**
  - Abstracts DbContext behind repositories.
  - Unit of Work baked into each request scope.
- **Migrations & Seeding**
  - Automatic schema updates via EF CLI.
  - Initial seed data.

### 4. Scalability & Resilience
- **Containerization**
  - Docker Compose for local dev; each service in its own container.
- **Stateless API**
  - Horizontal scaling friendly.

### 5. Observability & Monitoring
- **Logging**
  - Structured logs (Serilog) written to console/elastic.
- **Health Checks**
  - ASP.NET Core Health Checks endpoint for readiness/liveness.

### 6. CI/CD & Infrastructure as Code
- **GitHub Actions Pipelines**
  - Build → Test → Publish Docker images.
- **Deployment**
  - Docker images to GitHub Container Registry.
  - (Optional) Helm charts / Terraform modules for Kubernetes.

## 🚀 Getting Started

Here's how to set up the project either locally or using Docker.

### 🛠 Installing locally

Here's how to set up the project locally

#### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker + Docker Compose](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Node (react + next.js)](https://nodejs.org/en)

#### Installing
1. Clone the repository:
   ```bash
   git clone https://dev.azure.com/jerejunttila/_git/Kulku
   cd Kulku
   ```
2. Follow instructions of each project to get them running:
   - For [**Frontend** (React + Next.js)](src/Kulku.Web/kulku.web.client/readme.md)
   - For [**API** (.NET Api)](src/Kulku.Web/Kulku.Web.Api/readme.md)
   - For [**Admin** (Blazor)](src/Kulku.Web/Kulku.Web.Admin/readme.md)
3. Apply migrations to the database:
   Follow the instructions in the [Database Changes](#🧪-database-changes) section below to apply migrations.

### 🐳 Using through Docker

1. Clone the repository:
   ```bash
   git clone https://github.com/arawnik/Kulku.git
   cd Kulku
   ```
2. Configure environment
   Copy `.env.template` to `.env` and fill in secrets.
3. Build and run the Docker containers:
   ```bash
   docker-compose up -d --build
   ```


## 🧪 Database Changes

Entity Framework Core is used for database migrations. Use the following commands to manage schema:

```bash  
# Add new migration
dotnet ef migrations add MigrationName -p Kulku.Persistence.Pgsql -s Kulku.Web.Admin -context AppDbContext

# Apply migrations
dotnet ef database update -p Kulku.Persistence.Pgsql -s Kulku.Web.Admin -context AppDbContext
dotnet ef database update -p Kulku.Persistence.Pgsql -s Kulku.Web.Admin -context UserDbContext
```


## ✅ Running the Tests

Tests use **xUnit** and cover all major features.  
Run tests with:
```bash
dotnet test
```


## 🚀 Deployment

Continuous integration is managed via GitHub actions:

- Build validation on pull requests
- Publish builds for main branch. Generates a Docker image into GitHub Container Registry.


## 🧰 Built With

- Backend
  - [.NET 9](https://dotnet.microsoft.com/)
  - [Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor)
  - [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
  - [PostgreSQL](https://www.postgresql.org/)
- Frontend:
  - [React](https://react.dev/)
  - [Next.js](https://nextjs.org/)
  - [Bootstrap 5.3](https://getbootstrap.com/)
  - [TanStack Query](https://tanstack.com/query/latest)
- Admin:
  - [Blazor WebApp](https://learn.microsoft.com/en-us/aspnet/core/blazor)


## 📜 License

The project is licensed under [MIT License](LICENSE)


## 👤 Authors

- [**Jere Junttila**](https://jerejunttila.fi/)

See also the list of members who contributed on [GitHub](https://github.com/arawnik/Kulku/graphs/contributors).
