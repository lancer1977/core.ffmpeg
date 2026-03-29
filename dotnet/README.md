# Polyhydra FFmpeg .NET scaffold

This folder contains the starter Blazor/MudBlazor host layout for the FFmpeg plan.

## Projects
- `Polyhydra.Ffmpeg.Contracts` — shared job, probe, preset, and target contracts.
- `Polyhydra.Core.Ffmpeg` — core abstractions and placeholder services.
- `Polyhydra.Ffmpeg.Host` — ASP.NET Core Blazor host shell.

## Notes
- The current code is intentionally minimal and build-oriented.
- Command building, probing, and process execution are placeholder implementations.
- MudBlazor has not been wired in yet; the host is ready for that next step.
- The root repo remains the TypeScript package; this scaffold is additive.

## Suggested next steps
1. Add MudBlazor package references and theme/layout wiring.
2. Expand `FfmpegJobRequest` into a richer job/preset model.
3. Add real FFprobe/process execution services.
4. Add pages for dashboard, jobs, probe, and presets.
