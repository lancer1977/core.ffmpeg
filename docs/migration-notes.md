# Migration notes

## From app-busey-box
Move into the shared core:
- ffmpeg launch argument construction
- overlay image handling
- drawtext reload pattern
- atomic write helper for runtime text
- encoder selection and fallback behavior
- ffprobe duration lookup

Keep in the host app:
- playlist logic
- app-specific UI
- state machine for which video to play next
- environment-specific config management

## From chop-it/v2
Move into the shared core:
- filter graph building
- HLS output argument construction
- NVENC availability check
- render lifecycle helpers
- reusable process execution patterns

Keep in the host app:
- clip selection logic
- UI/API endpoints
- app-specific presets
- collection browsing and rendering workflow

## Shared core boundary
The shared package should not depend on any one app’s domain model.

It should be able to support:
- broadcaster-style streaming
- clip rendering
- overlayed playback
- future Svengoolie video workflows

## Porting order
1. Recreate command builders in the core package
2. Add probing and runtime helpers
3. Migrate host app call sites
4. Replace duplicated shell logic
5. Add tests for parity
