namespace Polyhydra.Ffmpeg.Contracts;

public sealed record FfmpegJobRequest(
    string InputPath,
    string OutputPath,
    StreamTarget Target,
    EncoderProfile? Encoder = null,
    OverlayOptions? Overlay = null,
    TextOverlayOptions? Text = null,
    IReadOnlyDictionary<string, string>? Metadata = null);