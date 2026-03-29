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
