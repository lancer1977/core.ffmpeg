# Next steps

## Immediate
- add migration notes for consumers of archived `.NET` `v1`
- validate archived snapshot retention policy (`dotnet/archive/v1-core-ffmpeg`)
- add/expand tests that validate the consolidated `.NET` core services

## Near-term
- port FFmpeg launch logic from `app-busey-box`
- port renderer/preset logic from `chop-it/v2`
- add documentation examples for RTMP, HLS, and file output usage

## After that
- integrate the core into the host apps
- reduce duplicate FFmpeg shell logic across repos
- define a stable V1 API surface
- keep TS and .NET streaming builders aligned

## Suggested host app pattern
- UI/API in a Blazor app
- media execution in a worker/service
- shared FFmpeg logic via `@polyhydra/core.ffmpeg`
