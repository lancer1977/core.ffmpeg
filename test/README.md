# Tests

This folder is reserved for command-generation and lifecycle coverage.

## Intended coverage

- FFmpeg command shapes for file / HLS / RTMP outputs
- overlay chain generation
- drawtext/runtime text filtering
- probe result parsing
- process start/stop behavior

## Current status

Automated tests exist for:

- command assembly
- overlay and drawtext filter generation
- ffprobe output parsing
- process lifecycle helpers
- runtime text writing
- encoder parsing/fallback behavior
