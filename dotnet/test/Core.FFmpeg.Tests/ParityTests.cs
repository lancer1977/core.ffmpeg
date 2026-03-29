using Polyhydra.Core.Ffmpeg.Services;
using Polyhydra.Ffmpeg.Contracts;

namespace Core.FFmpeg.Tests;

public static class ParityTests
{
    public static void HlsPresetUsesDestinationFileStemForSegmentPattern()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "ignored.m3u8",
                Target: new StreamTarget(StreamTargetKind.Hls, "/tmp/out/live.m3u8")));

        var args = command.Arguments;
        Assert.Contains("-hls_segment_filename", args);
        Assert.Contains("live-%03d.ts", args);
        Assert.Equal("/tmp/out/live.m3u8", args[^1]);
    }

    public static void RtmpPresetWithoutStreamKeyUsesTrimmedDestination()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "ignored.flv",
                Target: new StreamTarget(StreamTargetKind.Rtmp, "rtmp://example/live/")));

        Assert.Contains("rtmp://example/live", command.Arguments);
    }

    public static void MetadataAddsRepeatedMetadataFlagsAndSkipsBlankKeys()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
                Metadata: new Dictionary<string, string>
                {
                    ["title"] = "demo",
                    ["service_name"] = "polyhydra",
                    [""] = "should-be-skipped"
                }));

        var args = command.Arguments;
        var metadataFlagCount = args.Count(a => a == "-metadata");
        Assert.Equal(2, metadataFlagCount);
        Assert.Contains("title=demo", args);
        Assert.Contains("service_name=polyhydra", args);
    }

    public static void OverlayNamedPositionAndOpacityAreAppliedWithClamp()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
                Overlay: new OverlayOptions(ImagePath: "logo.png", Position: "bottom-right", Opacity: 3.5)));

        var args = command.Arguments.ToList();
        var filterIndex = args.IndexOf("-filter_complex");
        Assert.True(filterIndex >= 0, "Missing filter_complex");
        var filterValue = args[filterIndex + 1];
        Assert.Contains("overlay=W-w:H-h", filterValue);
        Assert.Contains("alpha=1", filterValue);
    }

    public static void OverlayInvalidPositionFallsBackToOrigin()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
                Overlay: new OverlayOptions(ImagePath: "logo.png", Position: "not-a-real-position", Opacity: -10)));

        var args = command.Arguments.ToList();
        var filterIndex = args.IndexOf("-filter_complex");
        Assert.True(filterIndex >= 0, "Missing filter_complex");
        var filterValue = args[filterIndex + 1];
        Assert.Contains("overlay=0:0", filterValue);
        Assert.Contains("alpha=0", filterValue);
    }

    public static void DrawTextPrefersTextFileAndIncludesFontFileAndFallbackFontSize()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
                Text: new TextOverlayOptions(
                    Text: "inline value should be ignored",
                    TextFilePath: "/tmp/now.txt",
                    FontPath: "/tmp/font.ttf",
                    FontSize: "NaN")));

        var args = command.Arguments.ToList();
        var filterIndex = args.IndexOf("-filter_complex");
        Assert.True(filterIndex >= 0, "Missing filter_complex");
        var filterValue = args[filterIndex + 1];
        Assert.Contains("drawtext=textfile='/tmp/now.txt'", filterValue);
        Assert.Contains("reload=1", filterValue);
        Assert.Contains("fontfile='/tmp/font.ttf'", filterValue);
        Assert.Contains("fontsize=24", filterValue);
    }

    public static void FileOutputFallsBackToOutputPathWhenTargetDestinationIsBlank()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "fallback-output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, " ")));

        Assert.Equal("fallback-output.mp4", command.Arguments[^1]);
    }
}
