using System.Text;

namespace Core.FFmpeg.Runtime;

public static class FfmpegRuntimeTextWriter
{
    public static async Task WriteAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, content, Encoding.UTF8, cancellationToken);
    }
}
