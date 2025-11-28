# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app
#EXPOSE 7215
EXPOSE 5090

USER $APP_UID

#ENV ASPNETCORE_HTTPS_PORTS=7215
ENV ASPNETCORE_HTTP_PORTS=5090

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY [".editorconfig", "."]

COPY ["src/Kulku.Web/Kulku.Web.Admin/Kulku.Web.Admin.csproj", "src/Kulku.Web/Kulku.Web.Admin/"]
COPY ["src/Kulku.Infrastructure/Kulku.Infrastructure.csproj", "src/Kulku.Infrastructure/"]
COPY ["src/Kulku.Application/Kulku.Application.csproj", "src/Kulku.Application/"]
COPY ["src/Kulku.Domain/Kulku.Domain.csproj", "src/Kulku.Domain/"]
COPY ["src/Kulku.Contract/Kulku.Contract.csproj", "src/Kulku.Contract/"]
COPY ["src/Kulku.Persistence/Kulku.Persistence.Pgsql/Kulku.Persistence.Pgsql.csproj", "src/Kulku.Persistence/Kulku.Persistence.Pgsql/"]
COPY ["src/Kulku.Persistence/Kulku.Persistence/Kulku.Persistence.csproj", "src/Kulku.Persistence/Kulku.Persistence/"]

RUN dotnet restore "./src/Kulku.Web/Kulku.Web.Admin/Kulku.Web.Admin.csproj"

COPY . .
WORKDIR "/src/src/Kulku.Web/Kulku.Web.Admin"

ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./Kulku.Web.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kulku.Web.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Copy the published output from the publish stage
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Kulku.Web.Admin.dll"]