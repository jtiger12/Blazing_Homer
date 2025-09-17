# Use the official .NET 8 runtime as base image
FROM #chainguard/dotnet-sdk AS base
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Install curl for healthchecks
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Use the official .NET 8 SDK for building
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
FROM chainguard/dotnet-sdk AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["src/HomerBlazor.Web/HomerBlazor.Web.csproj", "src/HomerBlazor.Web/"]
COPY ["src/HomerBlazor.Core/HomerBlazor.Core.csproj", "src/HomerBlazor.Core/"]
COPY ["src/HomerBlazor.ServiceCards/HomerBlazor.ServiceCards.csproj", "src/HomerBlazor.ServiceCards/"]
COPY ["src/HomerBlazor.Shared/HomerBlazor.Shared.csproj", "src/HomerBlazor.Shared/"]

RUN dotnet restore "src/HomerBlazor.Web/HomerBlazor.Web.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/src/HomerBlazor.Web"
RUN dotnet build "HomerBlazor.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HomerBlazor.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage - runtime image
FROM base AS final
WORKDIR /app

# Create necessary directories
RUN mkdir -p /app/config /app/logs /app/data /app/wwwroot/assets

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV Homer__ConfigPath=/app/config/config.yml

# Create a non-root user for security
RUN adduser --disabled-password --gecos '' --uid 1000 appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "HomerBlazor.Web.dll"]
