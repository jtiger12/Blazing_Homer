# Blazing_Homer
Blazing Homer is a Blazor (.NET) port of the popular Homer dashboard — a lightweight, configurable homepage used to organize and access self‑hosted services. This project aims for feature parity with the original Vue.js Homer while embracing .NET strengths: type safety, powerful tooling, and first‑class hosting options (Docker, Windows/IIS, Linux, cloud).

---

## Key Goals

- Feature parity with Homer v24.x (configuration, theming, search, and “smart cards”)  
- Seamless migration from existing Homer `config.yml`  
- Extensible plugin architecture for smart/service cards  
- First‑class Docker support, easy local development

---

## Features (Current Status)

- Configuration via YAML with hot‑reload foundations (`config/config.yml`)
- Responsive dashboard layout with groups and service cards
- Real‑time search filter and basic keyboard interactions
- Theme support (light/dark; foundations in place for multiple themes)
- JavaScript interop hardened for Blazor lifecycle correctness
- Early smart-card architecture for service integrations (e.g., Pi‑hole) with registry/DI scaffolding

For a detailed, living breakdown of what’s done and what’s next, see:
- `docs/implementation-status.md` — current implementation checklist
- `src/Homer-Blazor-PRD.md` — product requirements and roadmap

---

## Repository Structure

```
Blazing_Homer/
├── assets/                    # Static assets mounted into the app in Docker
├── config/
│   └── config.yml             # Main dashboard configuration (YAML)
├── docs/
│   ├── implementation-status.md
├── src/
│   ├── HomerBlazor.Core/      # Core models and services (config, registry, etc.)
│   ├── HomerBlazor.ServiceCards/ # Smart card implementations (planned/in progress)
│   ├── HomerBlazor.Shared/    # Shared components/contracts
│   └── HomerBlazor.Web/       # Blazor Server app (entrypoint)
├── tests/
│   └── HomerBlazor.Tests/     # Unit and integration tests (planned)
├── Dockerfile                 # Multi-stage build & runtime image
├── docker-compose.yml         # Local development / deployment compose
└── README.md
```

---

## Quick Start (Local)

Prerequisites:
- .NET 8 SDK or later

Steps:
1. Restore and build
   - `dotnet restore`
   - `dotnet build`
2. Run the web app
   - `dotnet run --project src/HomerBlazor.Web/HomerBlazor.Web.csproj`
3. Open your browser to `http://localhost:8080` (if configured) or the port printed in the console.

Note: The app reads configuration from `config/config.yml`. You can customize the dashboard title, links, groups, and services there.

---

## Quick Start (Docker)

Build and run using Docker Compose:

```
docker compose up --build
```

This will:
- Build the Blazor app into a runtime image
- Expose the app on port `8080`
- Mount local `config/`, `assets/`, `logs/`, and `data/` into the container

Health checks are configured in `docker-compose.yml` and `Dockerfile` exposes `ASPNETCORE_URLS=http://+:8080`.

---

## Configuration

- Location: `config/config.yml`
- Mirrors Homer’s YAML structure (title, subtitle, links, services, colors, etc.)
- Example entries include groups with `items` for each service, logos/icons, tags, and optional `type` to enable a smart card when available.

Snippet from `config/config.yml`:

```yaml
title: "Homer Dashboard"
subtitle: "Welcome to your new dashboard!"
logo: "assets/logo.png"
theme: default
links:
  - name: "GitHub"
    icon: "fab fa-github"
    url: "https://github.com/bastienwirtz/homer"
services:
  - name: "Applications"
    icon: "fas fa-cloud"
    items:
      - name: "Awesome app"
        logo: "assets/tools/sample.png"
        url: "https://www.reddit.com/r/selfhosted/"
  - name: "Other group"
    icon: "fas fa-heartbeat"
    items:
      - name: "Pi-hole"
        url: "http://192.168.0.151/admin"
        type: "PiHole"   # enables smart card when implemented
```

Hot Reload: The foundation exists for reloading configuration without restarting the app; additional work is tracked in `docs/implementation-status.md`.

---

## Smart Cards (Service Integrations)

The architecture supports “smart” cards that fetch and display live metrics (e.g., Pi‑hole, Portainer, Proxmox, Plex, Traefik, Uptime Kuma, Home Assistant, Nextcloud).  

Status:
- Base interfaces and registry are in place in `HomerBlazor.Core`  
- Initial Pi‑hole card stub exists; more cards planned  
- Background polling and richer error/loading states are on the roadmap

See the PRD: `src/Homer-Blazor-PRD.md` for the full list and phases.

---

## Development

- Tech stack: .NET 8, Blazor Server (WebAssembly option considered), DI/HttpClient, YamlDotNet, Serilog (planned), SignalR (planned), FluentValidation (planned).
- Run locally: `dotnet run --project src/HomerBlazor.Web/HomerBlazor.Web.csproj`
- Linting/formatting/test strategy and CI are outlined in the PRD; contributions to tests are welcome.

Project highlights:
- Robust `IJS` interop service to avoid premature calls and lifecycle issues
- Responsive CSS and theme toggle foundations
- Clear separation of concerns: Core, Web, ServiceCards, Shared

---

## Roadmap

High‑level milestones (see `docs/implementation-status.md` and `src/Homer-Blazor-PRD.md` for details):
- Phase 1: Core dashboard, themes, search, YAML config
- Phase 2: Smart cards for key services (Pi‑hole, Portainer, Proxmox, Plex, Traefik, Uptime Kuma, Home Assistant, Nextcloud)
- Phase 3: PWA, multi‑page, advanced theming, keyboard shortcuts, config validation
- Phase 4: Extended smart cards, plugin model, performance
- Phase 5: Deployment & docs polish

---

## Contributing

Issues and PRs are welcome! Please open an issue describing the feature, bug, or proposal. For significant changes, consider discussing in an issue first to align on direction.

---

## License

This project is licensed under the terms of the MIT License. See `LICENSE` for details.
