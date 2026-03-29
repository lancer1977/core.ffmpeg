# Hosting recommendation for a homelab-exposed video app

## Recommended stack
- **ASP.NET Core** backend
- **Blazor Web App** or **Blazor Server** for the operator UI
- **MudBlazor** for the component library
- **Docker** for deployment
- **Caddy** or **Traefik** as the reverse proxy
- **SQLite** for local state in V1
- **PostgreSQL** later if the app grows beyond local single-node usage

## Why this fits
- Fast to build for internal tooling and manual controls
- Good UI ergonomics for operator workflows
- Container-friendly deployment
- Reverse proxy handles TLS and exposure cleanly
- SQLite keeps early operations simple while you validate the workflow

## Recommended exposure pattern
### Preferred
- run the app privately in the homelab
- expose it through VPN/Tailscale when possible
- use a reverse proxy for any public route

### If public exposure is required
- terminate TLS at the proxy
- put auth in front of privileged routes
- separate read-only endpoints from control endpoints
- consider signed links or token-gated actions for external integrations

## Good split for the application
### UI
- manual tweaks
- review queues
- asset/browser controls
- channel-cheevos operator actions

### API
- status
- control actions
- URL generation
- broadcast/stream orchestration
- integration endpoints for other apps

### Runtime
- ffmpeg container or worker
- state store
- media scanning/indexing jobs

## Practical default
If you want the least-friction path that still scales well enough for a homelab:

**ASP.NET Core + Blazor Web App + MudBlazor + Docker + Caddy + SQLite**

That gives you a clean operator UI, a stable backend, and a simple deployment story.
