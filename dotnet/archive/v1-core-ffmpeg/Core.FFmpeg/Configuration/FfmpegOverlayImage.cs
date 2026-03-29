namespace Core.FFmpeg.Configuration;

public sealed record FfmpegOverlayImage(
    string Path,
    int? X = null,
    int? Y = null,
    bool Enabled = true);
