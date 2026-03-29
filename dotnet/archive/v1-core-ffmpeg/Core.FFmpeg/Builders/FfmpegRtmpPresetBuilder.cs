namespace Core.FFmpeg.Builders;

public static class FfmpegRtmpPresetBuilder
{
    public static IReadOnlyList<string> Build(string rtmpUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rtmpUrl);
        return ["-f", "flv", rtmpUrl];
    }
}
