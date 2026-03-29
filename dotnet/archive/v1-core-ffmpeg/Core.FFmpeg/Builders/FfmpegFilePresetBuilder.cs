namespace Core.FFmpeg.Builders;

public static class FfmpegFilePresetBuilder
{
    public static IReadOnlyList<string> Build(string outputPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        return [outputPath];
    }
}
