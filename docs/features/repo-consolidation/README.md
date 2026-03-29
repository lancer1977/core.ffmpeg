# Repository Consolidation

## Summary
Consolidate duplicate implementation paths so the repo has one canonical TypeScript library path and one canonical .NET core path.

## Scope
- Keep TypeScript library implementation under `typescript/`.
- Consolidate .NET runtime behavior into `dotnet/Polyhydra.*`.
- Keep historical `.NET` implementation archived under `dotnet/archive/v1-core-ffmpeg`.

## Current status
- Consolidated.
- Command/probe/process logic has been ported into `Polyhydra.Core.Ffmpeg` default services.
- Legacy `dotnet/v1` has been archived to `dotnet/archive/v1-core-ffmpeg`.

## Decisions
- Canonical TypeScript source path: `typescript/`.
- Canonical .NET core path: `dotnet/Polyhydra.Core.Ffmpeg`.
- Canonical host path: `dotnet/Polyhydra.Ffmpeg.Host`.

## Next steps
1. Keep expanding test coverage around command/probe/process edge cases.
2. Add migration notes for downstream consumers.
3. Decide long-term retention policy for the archived `v1` snapshot.
