namespace Core.FFmpeg.Configuration;

public sealed record FfmpegRuntimeText(
    string Path,
    bool Enabled = true,
    int ReloadSeconds = 1,
    string FontColor = "white",
    int FontSize = 24,
    string X = "(w-text_w)/2",
    string Y = "h-(text_h*2)");
