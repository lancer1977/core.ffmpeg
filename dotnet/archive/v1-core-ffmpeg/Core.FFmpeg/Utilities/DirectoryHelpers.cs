namespace Core.FFmpeg.Utilities;

public static class DirectoryHelpers
{
    public static void EnsureDirectory(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        Directory.CreateDirectory(path);
    }
}
