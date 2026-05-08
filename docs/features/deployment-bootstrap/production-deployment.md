# Production Deployment Notes

## Purpose
Document the repeatable production path for `Polyhydra.Ffmpeg.Host` when it runs behind a reverse proxy.

## Recommended topology

- `polyhydra-ffmpeg-host` runs the ASP.NET Core host
- `caddy` or `traefik` terminates TLS
- persistent storage backs `FFMPEG_HOST_DATA_PATH`
- operator access is protected by basic auth on the host routes

## Production environment variables

- `ASPNETCORE_URLS=http://+:8080`
- `DISABLE_HTTPS_REDIRECT=true`
- `FFMPEG_HOST_DATA_PATH=/var/lib/polyhydra-ffmpeg`
- `FFMPEG_HOST_OPERATOR_USERNAME=<operator-user>`
- `FFMPEG_HOST_OPERATOR_PASSWORD=<secret>`
- `FFMPEG_HOSTNAME=<public-hostname>` when using the Caddy profile

## Deployment steps

1. Build the host image.
2. Provision the persistent data directory or volume.
3. Set the operator credentials and host name in the environment or secret store.
4. Start `docker compose -f docker-compose.yml -f docker-compose.proxy.yml up -d --build`.
5. Verify:
   - `GET /api/health`
   - `GET /api/status` with operator credentials
   - browser access through the reverse proxy

## Rollback procedure

### Application rollback

1. Stop the current deployment stack.
2. Re-deploy the previous known-good image tag.
3. Keep the same `FFMPEG_HOST_DATA_PATH` volume so jobs, presets, and state remain intact.
4. Re-run `GET /api/health` and `GET /api/status`.

### Config rollback

1. Revert the last environment or secret change.
2. Restart the host container.
3. Confirm the proxy still forwards to the host and the auth gate still challenges anonymous requests.

### Emergency rollback

If the operator auth change blocks access, temporarily unset the auth variables and restart the host to restore anonymous access while the secret/config issue is fixed.

## Validation checklist

- [ ] `docker compose -f docker-compose.yml -f docker-compose.proxy.yml up --build` succeeds
- [ ] `/api/health` returns `200`
- [ ] `/api/status` returns `401` without credentials
- [ ] `/api/status` returns `200` with operator credentials
- [ ] Persistent data remains intact after restart
