namespace Core.FFmpeg.Configuration;

public sealed record FfmpegEncodingConfig(
    string? VideoCodec = null,
    string? AudioCodec = null,
    int? VideoBitrateKbps = null,
    int? AudioBitrateKbps = null,
    int? Fps = null,
    int? Gop = null,
    string? Preset = null,
    string? Tune = null,
    int? Crf = null,
    IReadOnlyList<string>? ExtraArgs = null);
