namespace Core.FFmpeg.Configuration;

public sealed record FfmpegJobConfig(
    string Input,
    FfmpegOutputConfig Output,
    FfmpegEncodingConfig? Encoding = null,
    IReadOnlyList<FfmpegOverlayImage>? Overlays = null,
    FfmpegRuntimeText? RuntimeText = null);
