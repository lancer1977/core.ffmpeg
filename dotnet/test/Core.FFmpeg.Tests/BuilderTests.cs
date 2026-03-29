using Polyhydra.Core.Ffmpeg.Services;
using Polyhydra.Ffmpeg.Contracts;

namespace Core.FFmpeg.Tests;

public static class BuilderTests
{
    public static void BuildsHlsPreset()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "playlist.m3u8",
                Target: new StreamTarget(StreamTargetKind.Hls, "playlist.m3u8")));

        var args = command.Arguments;
        Assert.Contains("-f", args);
        Assert.Contains("hls", args);
        Assert.Contains("-hls_time", args);
        Assert.Contains("-hls_segment_filename", args);
        Assert.Contains("playlist-%03d.ts", args);
    }

    public static void BuildsRtmpPreset()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "unused.flv",
                Target: new StreamTarget(StreamTargetKind.Rtmp, "rtmp://example/live", "stream")));

        var args = command.Arguments;
        Assert.Contains("flv", args);
        Assert.Contains("rtmp://example/live/stream", args);
    }
}
