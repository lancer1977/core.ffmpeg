# Core architecture sketch

## Goal
Provide a reusable FFmpeg core that can be embedded by multiple video-oriented applications.

## Layers

### 1. Configuration layer
Defines encoder choice, bitrates, frame rate, output mode, overlay settings, and runtime text settings.

### 2. Command builder layer
Produces FFmpeg argument arrays and filter graphs for:
- overlays
- drawtext
- scaling and fps normalization
- RTMP output
- HLS output
- local file output
- Twitch/RTMP destination composition for playout apps
- Busey-Box-compatible broadcaster command assembly
- preset application for encoder/output/filter defaults

### 3. Probe and utility layer
Provides helpers for:
- ffprobe duration lookup
- encoder capability detection
- directory/file helpers
- atomic text writes

### 4. Runtime layer
Owns:
- process startup
- stderr/stdout capture
- graceful shutdown
- lifecycle state
- restart/health reporting

### 5. Host application layer
The host app decides:
- what media to stream
- what overlay or text to use
- which URLs or controls to expose
- how operators interact with the stream

## Integration model
The shared core should stay opinionated about FFmpeg mechanics, but not about app-specific behavior.

That means:
- no UI logic in the core
- no playlist or CMS logic in the core
- no channel-specific rules in the core
- no storage schema assumptions in the core

## Cross-repo media-platform boundary

`core.ffmpeg` sits after catalog/operator selection and before concrete output execution:

- `Core.Models` provides shared media contracts.
- `core.nas` discovers and indexes media.
- `SvengoolieDB` adds product/catalog meaning.
- `chop-it` lets an operator select or shape playlist/control intent.
- `core.ffmpeg` builds and runs FFmpeg commands for RTMP, HLS, file output, overlays, runtime text, and probing.
- `core.ffmpeg` also provides a small Twitch/RTMP destination helper so playout apps can compose ingest URLs without storing secrets in the library.
- `core.ffmpeg` also exposes a Busey-Box-compatible broadcaster adapter for the current Docker playout command shape.
- `core.ffmpeg` also lets host apps apply named render presets onto the shared pipeline config before command generation.
- `core.ffmpeg` also resolves the active preset from a catalog so host apps can keep selection logic outside the engine.
- `core.ffmpeg` also offers a resolved-pipeline helper that combines preset selection, preset merge, and codec fallback.
- `core.ffmpeg` also exposes a safe ffprobe parse helper so bad output can be handled without crashing the caller.
- `Busey-Box` remains the concrete Twitch/RTMP playout app until the shared playout contract is stable.

The immediate integration target is to prove that `core.ffmpeg` can represent the FFmpeg command currently hand-built by `app-busey-box/app/run_ffmpeg.sh`.

## Deployment pattern
A common pattern for homelab use:

- UI/API in a Blazor host container
- FFmpeg execution in a worker/container
- proxy in front for TLS and external exposure
- SQLite for early state, Postgres later if needed

## Primary architectural boundary
The core library should only know how to:
- translate settings into FFmpeg commands
- run FFmpeg safely
- report state back to the caller

Everything else belongs to the application.

## Related docs

- [SvengoolieDB media metadata transition](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/README.md>)
- [One-swoop execution map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/execution-map.md>)
- [Cross-project link map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/link-map.md>)
