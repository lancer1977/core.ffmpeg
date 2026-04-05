namespace Polyhydra.Ffmpeg.Contracts;

public sealed record FfprobeSnapshot(
    string SourcePath,
    TimeSpan? Duration,
    int? Width,
    int? Height,
    string? VideoCodec,
    string? AudioCodec);