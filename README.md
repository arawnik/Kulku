Welcome to Kulku, the code repository that powers my personal CV site and its administrative backend.
This project is designed to showcase my professional profile, publicly shareable solo projects, and work as a code sample.

## 🌟 Key Features

### Site
- Dynamic CV & Portfolio: Easily update work experience, education, skills, and project showcases without code changes.
- Contact & Inquiries: Built-in form with spam protection and email forwarding.
- Theming & Localization: Light/dark mode toggle and multilingual support (English & Finnish).
- Responsive & Accessible: Optimized for mobile, tablet, and desktop. Also SEO optimized for search engines.

### Backend
- Clean architecture: Separation of concerns with a focus on maintainability and testability.
- CQRS & Vertical Slices: Command handlers, query handlers, and validators organized in feature slices for maintainability.


## 🔐 Important Notices

Legal content is still being created for:
- [Privacy Policy](https://jerejunttila.fi/privacy)
- [Terms of Service](https://jerejunttila.fi/tos)


## 🧱 Architecture

For details on the project architecture, see [Architecture.md](./documents/Architecture.md)

- Combination of CQRS separation with commands, commands handlers, queries, and query handlers. 
	- But also creating them similar to vertical slices to keep related code together.
	- Repository pattern separates data access from business logic and reduces direct dependencies on Entity Framework Core.

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker + Docker Compose](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)
- EF Core CLI tools (for migrations)
- [Node (react + next.js)](https://nodejs.org/en)


### 🛠 Installing


## 🐳 Docker Usage


## 🧪 Database Changes

Entity Framework Core is used for database migrations. Use the following commands to manage schema:

```bash  
# Add new migration
dotnet ef migrations add MigrationName -p Kulku.Persistence.Pgsql -s Kulku.Web.Admin

# Apply migrations
dotnet ef database update -p Kulku.Persistence.Pgsql -s Kulku.Web.Admin
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

The project is licensed under [TBD] – See `LICENSE` file once available.


## 👤 Authors

- [**Jere Junttila**](https://jerejunttila.fi/)

See also the list of members who contributed on [GitHub](https://github.com/arawnik/Kulku/graphs/contributors).
