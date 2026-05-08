# Next steps

## Immediate
- add migration notes for consumers of archived `.NET` `v1`
- validate archived snapshot retention policy (`dotnet/archive/v1-core-ffmpeg`)
- add/expand tests that validate the consolidated `.NET` core services

## Near-term
- port FFmpeg launch logic from `app-busey-box` into the shared broadcaster adapter
- port renderer/preset logic from `chop-it/v2` into the shared preset application layer
- port preset selection from `chop-it/v2` into the shared preset resolver
- port encoder fallback resolution from `chop-it/v2` into the shared encoder helper
- port the preset-aware pipeline resolver into host apps
- add documentation examples for RTMP, HLS, and file output usage
- keep a Twitch/RTMP destination helper aligned with Busey-Box's broadcaster command shape
- keep the Busey-Box-compatible broadcaster adapter aligned with the launcher profile shape

## After that
- integrate the core into the host apps
- reduce duplicate FFmpeg shell logic across repos
- define a stable V1 API surface
- keep TS and .NET streaming builders aligned

## Suggested host app pattern
- UI/API in a Blazor app
- media execution in a worker/service
- shared FFmpeg logic via `@polyhydra/core.ffmpeg`
