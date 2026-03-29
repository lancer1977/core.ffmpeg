namespace Core.FFmpeg.Probe;

public sealed record FfmpegProbeResult(
    double? DurationSeconds,
    string? VideoCodec,
    string? AudioCodec);
