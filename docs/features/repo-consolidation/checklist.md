# Consolidation Checklist

## Discovery
- [x] Inventory current TypeScript and .NET structure.
- [x] Identify duplicate app/library paths and stale references.
- [x] Confirm `v1` contains more complete `.NET` FFmpeg behavior.

## Documentation
- [x] Create feature folder for consolidation tracking.
- [x] Update immediate next-step documentation to reflect current state.
- [x] Update all legacy docs that still reference outdated paths.

## TypeScript
- [x] Fix `tsconfig` source path after move from `src` to `typescript`.
- [x] Keep existing TypeScript implementation untouched.
- [ ] Re-validate package publishing assumptions after path move.

## .NET Core
- [x] Port command-building behavior into `Polyhydra.Core.Ffmpeg` default command builder.
- [x] Port ffprobe execution/parsing into `Polyhydra.Core.Ffmpeg` default probe service.
- [x] Port process execution behavior into `Polyhydra.Core.Ffmpeg` default process runner.
- [x] Add deeper parity tests for HLS/RTMP edge cases and metadata handling.

## .NET Tooling
- [x] Fix solution project paths after directory move.
- [x] Repoint test project to `Polyhydra.Core.Ffmpeg`.
- [x] Update CI workflow paths and SDK versions.
- [x] Remove or archive stale `v1` references from automation and docs.

## Validation
- [x] Build `Polyhydra.Ffmpeg.Host` successfully after service port.
- [x] Run TypeScript test suite after `tsconfig` fix.
- [x] Run `.NET` consolidation tests against `Polyhydra.Core.Ffmpeg`.
- [x] Add CI status check artifacts/docs for release readiness.

## Follow-up
- [x] Delete or archive `dotnet/v1` once migration is complete.
- [ ] Add migration notes for downstream consumers.
