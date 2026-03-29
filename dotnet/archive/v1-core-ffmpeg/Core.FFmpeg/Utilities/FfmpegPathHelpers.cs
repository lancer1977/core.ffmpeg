namespace Core.FFmpeg.Utilities;

public static class FfmpegPathHelpers
{
    public static string EscapePath(string value) => value.Replace("\\", "\\\\").Replace("'", "\\'");
}
