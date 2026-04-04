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
- Restores use the repo-local `nuget.config` to avoid inheriting private feeds.

## Suggested next steps
1. Add MudBlazor package references and theme/layout wiring.
2. Expand `FfmpegJobRequest` into a richer job/preset model.
3. Add real FFprobe/process execution services.
4. Add pages for dashboard, jobs, probe, and presets.

## Integration examples

### Build a file-output job

```csharp
var request = new FfmpegJobRequest(
    InputPath: "input.mp4",
    OutputPath: "output.mp4",
    Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
    Encoder: new EncoderProfile("Fast H.264", VideoCodec: "libx264", AudioCodec: "aac"));

var command = commandBuilder.Build(request);
```

### Build an HLS job

```csharp
var request = new FfmpegJobRequest(
    InputPath: "input.mp4",
    OutputPath: "playlist.m3u8",
    Target: new StreamTarget(StreamTargetKind.Hls, "playlist.m3u8"),
    Encoder: new EncoderProfile("HLS 1080p", VideoCodec: "libx264", AudioCodec: "aac"),
    HlsOutput: new HlsOutputOptions(SegmentTimeSeconds: 6, PlaylistType: "event", SegmentFilename: "playlist-%03d.ts"));

var command = commandBuilder.Build(request);
```

### Probe media and inspect the snapshot

```csharp
var snapshot = await ffprobeService.ProbeAsync("input.mp4");
Console.WriteLine($"{snapshot.Duration} {snapshot.Width}x{snapshot.Height} {snapshot.VideoCodec}");
```

### Queue a host job

```csharp
var job = workflow.Enqueue("input.mp4", "output.mp4", preset);
```
