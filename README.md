# core.ffmpeg

Reusable FFmpeg core library for OpenClaw media apps.

This package is a shared TypeScript library for the FFmpeg plumbing used by:

- `chop-it`
- `app-busey-box`
- future Svengoolie/media tooling

## What is implemented

The repo currently contains a working foundation for:

- typed pipeline config objects
- deterministic FFmpeg command construction
- overlay filter-chain assembly
- reloadable `drawtext` runtime-text helpers
- basic `ffprobe` media probing
- process start/stop lifecycle helpers with graceful shutdown
- atomic runtime text utilities
- encoder discovery and NVENC fallback helpers

## Public surface

The package currently exports:

- `buildFfmpegCommand(config)`
- `buildOverlayFilterChain(overlays)`
- `buildDrawTextFilter(runtimeText)`
- `buildRtmpOutput(targetUrl)`
- `buildTwitchRtmpOutput(ingestUrl, streamKey)`
- `composeTwitchRtmpUrl(ingestUrl, streamKey)`
- `buildHlsOutput(playlistPath, segmentPattern, segmentTimeSeconds)`
- `buildBuseyBoxBroadcastCommand(options)`
- `applyRenderPreset(config, preset)`
- `resolveActiveRenderPreset(presets, activePresetId)`
- `buildResolvedPipelineConfig(options)`
- `probeMedia(filePath)`
- `parseProbeOutput(stdout)`
- `tryParseProbeOutput(stdout)`
- `startFfmpegProcess(command, args)`
- `atomicWriteText(path, value)`
- `writeRuntimeText(path, value)`
- `ensureDirectory(path)`
- `quoteForFfmpegPath(path)`
- `resolveEncoderConfig(config, nvencAvailable)`
- encoder helpers for NVENC discovery/fallback
- the typed config interfaces from `typescript/config`

## Cross-repo role

`core.ffmpeg` is the reusable media-engine boundary in the related media platform:

- `Core.Models` owns shared media data contracts.
- `core.nas` owns scanning, indexing, storage availability, and NAS media records.
- `SvengoolieDB` owns catalog, episode, schedule, voting, and stream-state semantics.
- `chop-it` owns operator playlist/control workflow.
- `core.ffmpeg` owns FFmpeg command construction, RTMP/HLS/file output mechanics, overlays, runtime text, probing, and process lifecycle.
- `app-busey-box` owns concrete Dockerized Twitch/RTMP playout.

Central coordination docs:

- [SvengoolieDB media metadata transition](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/README.md>)
- [One-swoop execution map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/execution-map.md>)
- [Cross-project link map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/link-map.md>)

## Current status

- Build: passing (`typescript` sources)
- Typecheck: passing (`typescript` sources)
- Tests: passing (`node:test` suite in `test/`)
- Repository state: scaffold-plus-core-helpers, not a finished migration
- .NET restores are pinned locally via `nuget.config` to avoid inheriting private feeds

## Planned structure

- `typescript/config` - shared config and option types
- `typescript/command` - FFmpeg command builders
- `typescript/overlay` - overlay and filter-chain helpers
- `typescript/process` - process start/stop lifecycle helpers
- `typescript/probe` - media probing helpers
- `typescript/runtime` - runtime text file helpers and reload patterns
- `typescript/utils` - filesystem and generic utility helpers

## Next steps

1. Validate Docker deployment on the target environment
2. Port the remaining shared logic from `chop-it` and `app-busey-box`
3. Replace duplicated helper code in the consuming apps
4. Add app-level integration samples for TS and .NET consumers
5. Add a Twitch/RTMP playout example that matches Busey-Box's current `run_ffmpeg.sh` command shape.

### Build a Twitch RTMP destination

```ts
import { buildTwitchRtmpOutput } from '@polyhydra/core.ffmpeg';

const args = buildTwitchRtmpOutput('rtmp://live.twitch.tv/app/', process.env.TWITCH_STREAM_KEY ?? '');
```

### Build a Busey-Box compatible broadcaster command

```ts
import { buildBuseyBoxBroadcastCommand } from '@polyhydra/core.ffmpeg';

const command = buildBuseyBoxBroadcastCommand({
  inputPath: '/media/input.mp4',
  ingestUrl: 'rtmp://live.twitch.tv/app/',
  streamKey: process.env.TWITCH_STREAM_KEY ?? '',
});
```

### Apply a render preset

```ts
import { applyRenderPreset, buildFfmpegCommand } from '@polyhydra/core.ffmpeg';

const config = applyRenderPreset(
  {
    input: '/media/input.mp4',
    output: { transport: 'rtmp', target: 'rtmp://example/live/base' },
  },
  {
    id: 'soft',
    name: 'Soft',
    category: 'basic',
    enabled: true,
    filters: [{ name: 'tone', vf: 'eq=contrast=1.2' }],
  },
);

const command = buildFfmpegCommand(config);
```

### Resolve an active preset

```ts
import { resolveActiveRenderPreset } from '@polyhydra/core.ffmpeg';

const preset = resolveActiveRenderPreset(presets, config.activePresetId);
```

### Resolve an encoder config

```ts
import { resolveEncoderConfig } from '@polyhydra/core.ffmpeg';

const encoder = resolveEncoderConfig({ videoCodec: 'h264_nvenc', audioCodec: 'aac' }, false);
```

### Resolve a full pipeline config

```ts
import { buildResolvedPipelineConfig } from '@polyhydra/core.ffmpeg';

const pipeline = buildResolvedPipelineConfig({
  config: {
    input: '/media/input.mp4',
    encoding: { videoCodec: 'h264_nvenc', audioCodec: 'aac' },
    output: { transport: 'rtmp', target: 'rtmp://example/live/base' },
  },
  presets,
  activePresetId: config.activePresetId,
  nvencAvailable: false,
});
```

### Parse ffprobe output

```ts
import { parseProbeOutput, tryParseProbeOutput } from '@polyhydra/core.ffmpeg';

const parsed = tryParseProbeOutput(stdout);
const probe = parseProbeOutput(stdout);
```

## Build and Deploy

### Local build and test

```bash
npm test
dotnet build dotnet/core.ffmpeg.sln -c Release
dotnet build dotnet/test/Core.FFmpeg.Tests/Core.FFmpeg.Tests.csproj -c Release
dotnet ./dotnet/test/Core.FFmpeg.Tests/bin/Release/net10.0/Core.FFmpeg.Tests.dll
```

### Run host locally without Docker

```bash
FFMPEG_HOST_OPERATOR_USERNAME=operator \
FFMPEG_HOST_OPERATOR_PASSWORD='change-me' \
DISABLE_HTTPS_REDIRECT=true ASPNETCORE_URLS=http://localhost:8080 \
  dotnet run --project dotnet/Polyhydra.Ffmpeg.Host/Polyhydra.Ffmpeg.Host.csproj
```

### Run host with Docker Compose

```bash
docker compose up --build
```

Host URL: `http://localhost:8080`  
Status endpoint: `http://localhost:8080/api/status`

See [deployment environment variables](docs/features/deployment-bootstrap/environment-variables.md) for the operator auth and storage settings.
