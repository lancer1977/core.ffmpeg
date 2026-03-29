using Polyhydra.Core.Ffmpeg.Services;
using Polyhydra.Ffmpeg.Contracts;

namespace Core.FFmpeg.Tests;

public static class SmokeTests
{
    public static void CommandBuilds()
    {
        var builder = new DefaultFfmpegCommandBuilder();
        var cmd = builder.Build(new FfmpegJobRequest(
            "input.mp4",
            "output.mp4",
            new StreamTarget(StreamTargetKind.File, "output.mp4")));

        if (cmd.FileName != "ffmpeg") throw new Exception("Unexpected executable.");
        if (!cmd.Arguments.Contains("input.mp4")) throw new Exception("Missing input.");
    }
}
