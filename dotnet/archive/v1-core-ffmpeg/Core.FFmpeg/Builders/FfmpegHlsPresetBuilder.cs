using Core.FFmpeg.Configuration;

namespace Core.FFmpeg.Builders;

public static class FfmpegHlsPresetBuilder
{
    public static IReadOnlyList<string> Build(string playlistPath, string segmentPattern, int segmentTimeSeconds = 4)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playlistPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(segmentPattern);

        return [
            "-f", "hls",
            "-hls_time", segmentTimeSeconds.ToString(),
            "-hls_playlist_type", "event",
            "-hls_segment_filename", segmentPattern,
            playlistPath
        ];
    }
}
