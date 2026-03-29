using Polyhydra.Core.Ffmpeg.Services;
using Polyhydra.Ffmpeg.Contracts;

namespace Core.FFmpeg.Tests;

public static class CommandBuilderTests
{
    public static void BuildsBasicFileCommand()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4")));

        Assert.Equal("ffmpeg", command.FileName);
        Assert.Contains("-hide_banner", command.Arguments);
        Assert.Contains("-i", command.Arguments);
        Assert.Contains("input.mp4", command.Arguments);
        Assert.Contains("output.mp4", command.Arguments);
        Assert.DoesNotContain("-f", command.Arguments);
    }

    public static void BuildsRtmpCommandWithCodecAndBitrates()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "ignored-for-rtmp.flv",
                Target: new StreamTarget(StreamTargetKind.Rtmp, "rtmp://example/live", "stream"),
                Encoder: new EncoderProfile(
                    Name: "Main",
                    VideoCodec: "libx264",
                    AudioCodec: "aac",
                    VideoBitrate: "6000k",
                    AudioBitrate: "160k",
                    Preset: "veryfast")));

        Assert.Contains("-f", command.Arguments);
        Assert.Contains("flv", command.Arguments);
        Assert.Contains("-c:v", command.Arguments);
        Assert.Contains("libx264", command.Arguments);
        Assert.Contains("-b:v", command.Arguments);
        Assert.Contains("6000k", command.Arguments);
        Assert.Contains("rtmp://example/live/stream", command.Arguments);
    }

    public static void BuildsOverlayAndRuntimeTextFilters()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
                Overlay: new OverlayOptions(ImagePath: "logo.png", Position: "20:30"),
                Text: new TextOverlayOptions(TextFilePath: "now_playing.txt")));

        var args = command.Arguments.ToList();
        var filterIndex = args.IndexOf("-filter_complex");
        Assert.True(filterIndex >= 0, "Missing filter_complex");
        var filterValue = args[filterIndex + 1];
        Assert.Contains("overlay=20:30", filterValue);
        Assert.Contains("drawtext", filterValue);
        Assert.Contains("textfile=", filterValue);
        Assert.Contains("now_playing.txt", filterValue);
    }
}
