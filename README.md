## 🌟 Key Features

### Site
- 

### Admin
- 


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
- .NET 9 SDK
- Docker + Docker Compose
- PostgreSQL (default)
- EF Core CLI tools (for migrations)
- npm (react + next.js)


### 🛠 Installing


## 🐳 Docker Usage

### Build and run


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

Deployment is managed via Azure DevOps Pipelines. You can view the current status via the build badges at the top.

CI/CD pipeline includes:
- Build validation
- Publish builds for main branch. Generates a Docker image into GitLab


## 📦 Additional Project Information

### Versioning

This project uses **date-based versioning**.  
Format: `YYYYMMDD` (e.g. `20250406`)  
See [tags on this repository](https://dev.azure.com/jerejunttila/_git/Kulku/tags) for previous versions.


## 🧰 Built With

- [.NET 9](https://dotnet.microsoft.com/)
- [Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL](https://www.postgresql.org/)
- [Bootstrap 5.3](https://getbootstrap.com/)


## 🤝 Contributing

This project is currently under private development. Public contributions may be accepted in the future. Please stay tuned for updates and contribution guidelines.


## 📜 License

The project is licensed under [TBD] – See `LICENSE` file once available.


## 🙏 Acknowledgments

Special thanks to:

- .NET open source contributors
- PostgreSQL developers
- Azure DevOps pipelines and deployment tooling


## 👤 Authors

- [**Jere Junttila**](https://jerejunttila.fi/)

See also the list of members who contributed on [Azure DevOps](https://dev.azure.com/jerejunttila/).
