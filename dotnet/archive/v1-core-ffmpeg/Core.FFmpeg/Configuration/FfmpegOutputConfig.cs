namespace Core.FFmpeg.Configuration;

public sealed record FfmpegOutputConfig(
    FfmpegTransport Transport,
    string Target,
    IReadOnlyList<string>? ExtraArgs = null);
