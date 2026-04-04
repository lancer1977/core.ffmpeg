using Polyhydra.Core.Ffmpeg.Services;
using Polyhydra.Ffmpeg.Contracts;

namespace Core.FFmpeg.Tests;

public static class HlsOutputTests
{
    public static void UsesDefaultHlsSettingsWhenNotSpecified()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "playlist.m3u8",
                Target: new StreamTarget(StreamTargetKind.Hls, "playlist.m3u8")));

        Assert.Contains("-hls_time", command.Arguments);
        Assert.Contains("4", command.Arguments);
        Assert.Contains("-hls_playlist_type", command.Arguments);
        Assert.Contains("event", command.Arguments);
    }

    public static void RespectsCustomHlsSettings()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "playlist.m3u8",
                Target: new StreamTarget(StreamTargetKind.Hls, "playlist.m3u8"),
                HlsOutput: new HlsOutputOptions(SegmentTimeSeconds: 8, PlaylistType: "vod", SegmentFilename: "custom-%03d.ts")));

        Assert.Contains("8", command.Arguments);
        Assert.Contains("vod", command.Arguments);
        Assert.Contains("custom-%03d.ts", command.Arguments);
    }
}
