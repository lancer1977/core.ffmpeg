# Deployment Environment Variables

## Purpose
Document the configuration surface for the `Polyhydra.Ffmpeg.Host` deployment baseline.

## Matrix

| Variable | Required | Purpose | Notes |
| --- | --- | --- | --- |
| `ASPNETCORE_URLS` | No | Binds Kestrel inside the container or local process. | Defaults to the repo's container-friendly HTTP binding. |
| `DISABLE_HTTPS_REDIRECT` | No | Disables HTTPS redirection in local and reverse-proxy deployments. | Keep `false` when the app terminates TLS directly. |
| `FFMPEG_HOST_DATA_PATH` | No | Overrides the host state root used for jobs and presets. | Use a persistent volume in production. |
| `FFMPEG_HOST_OPERATOR_USERNAME` | Yes for operator access | Basic-auth username for protected operator routes. | Store in a secret store or compose secret, not source control. |
| `FFMPEG_HOST_OPERATOR_PASSWORD` | Yes for operator access | Basic-auth password for protected operator routes. | Treat as a secret. Rotate it with the deployment. |
| `FFMPEG_HOSTNAME` | No | Hostname used by the Caddy reverse-proxy profile. | Defaults to `localhost` for local testing. |

## Secrets guidance

- Keep `FFMPEG_HOST_OPERATOR_PASSWORD` out of source control.
- Prefer Docker secrets, a local secret manager, or an environment file that is not committed.
- Use a persistent volume for `FFMPEG_HOST_DATA_PATH` so jobs and presets survive container rebuilds.
- For local development, set `FFMPEG_HOST_OPERATOR_USERNAME` and `FFMPEG_HOST_OPERATOR_PASSWORD` explicitly before opening the UI.

## Example

```bash
export FFMPEG_HOST_OPERATOR_USERNAME=operator
export FFMPEG_HOST_OPERATOR_PASSWORD='change-me'
export FFMPEG_HOST_DATA_PATH=/var/lib/polyhydra-ffmpeg
export DISABLE_HTTPS_REDIRECT=true
export ASPNETCORE_URLS=http://+:8080
```
