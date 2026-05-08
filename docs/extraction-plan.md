# Extraction Plan

## Source projects

- `~/code/chop-it/v2`
- `~/code/app-busey-box`

## Shared concerns already implemented here

- command construction
- overlay composition
- drawtext runtime-text pattern
- atomic runtime text file writing
- probing and metadata helpers
- process lifecycle wrapper
- filesystem helpers

## Next extraction targets

### From `chop-it/v2`
- NVENC detection
- output preset logic
- richer media probing
- any logging helpers tied to media jobs
- active preset resolution and preset filter merge helpers
- preset-aware pipeline resolution for host handoff

### From `app-busey-box`
- RTMP streaming command invocation variants
- overlay image handling edge cases
- atomic write helper for `now_playing.txt`
- FFmpeg startup/shutdown refinements
- broadcaster command adaptation and preset passthrough

## Recommended next implementation step

1. Add tests for the current command builder.
2. Port the next missing shared helper only if a consumer proves it is needed.
3. Keep app-specific policy in the apps rather than inside `core.ffmpeg`.

## Migration strategy

1. Build the shared package first.
2. Replace duplicated pieces in `app-busey-box`.
3. Replace duplicated pieces in `chop-it/v2`.
4. Add one shared test fixture for command generation.
5. Wire any Svengoolie-specific apps into it after the shared behaviors are stable.
