using Polyhydra.Core.Ffmpeg.Services;
using Polyhydra.Ffmpeg.Contracts;

namespace Core.FFmpeg.Tests;

public static class VersioningTests
{
    public static void SupportsInlineTextOverlay()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var command = builder.Build(
            new FfmpegJobRequest(
                InputPath: "input.mp4",
                OutputPath: "output.mp4",
                Target: new StreamTarget(StreamTargetKind.File, "output.mp4"),
                Text: new TextOverlayOptions(Text: "Now Playing")));

        var args = command.Arguments.ToList();
        var filterIndex = args.IndexOf("-filter_complex");
        Assert.True(filterIndex >= 0, "Missing filter_complex");
        var filterValue = args[filterIndex + 1];
        Assert.Contains("text='Now Playing'", filterValue);
    }
}
