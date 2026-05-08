# Roadmap

## V0.1 - Foundation

Status: mostly complete.

Completed:
- package/module structure
- config types
- command builder types
- FFmpeg probing helper
- process lifecycle wrapper
- documentation skeleton

Still missing:
- command-generation tests
- error handling polish around probe/process helpers
- integration examples

## V0.2 - Overlay and text support

Planned:
- overlay image composition
- drawtext helpers
- atomic text file writer
- reloadable runtime text input
- watermark positioning helpers

## V0.3 - Streaming targets

Planned:
- RTMP output builder
- HLS output builder
- local file output builder
- encoder selection and fallback logic

## V0.4 - Integration examples

Planned:
- port logic from `app-busey-box`
- port logic from `chop-it/v2`
- add sample usage in each app
- document migration strategy
- add preset-resolution and broadcaster-command examples

## V1.0 - Stable shared core

Planned:
- lock API surface
- finalize docs
- add tests around command building and config mapping
- publish as reusable internal dependency
