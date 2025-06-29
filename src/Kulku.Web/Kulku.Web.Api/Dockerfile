# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=5678
WORKDIR /app
#EXPOSE 7219
EXPOSE 5144

#ENV ASPNETCORE_HTTPS_PORTS=7219
ENV ASPNETCORE_HTTP_PORTS=5144


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution-related files FIRST (important for dependency resolution)
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY [".editorconfig", "."]

COPY ["src/Kulku.Web/Kulku.Web.Api/Kulku.Web.Api.csproj", "src/Kulku.Web/Kulku.Web.Api/"]

# Restore dependencies
RUN dotnet restore "./src/Kulku.Web/Kulku.Web.Api/Kulku.Web.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Kulku.Web/Kulku.Web.Api"
RUN dotnet build "./Kulku.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kulku.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos "" --uid $APP_UID --shell /bin/false appuser

COPY --from=publish --chown=appuser:appuser /app/publish .

# Switch to non-root user
USER appuser

ENTRYPOINT ["dotnet", "Kulku.Web.Api.dll"]