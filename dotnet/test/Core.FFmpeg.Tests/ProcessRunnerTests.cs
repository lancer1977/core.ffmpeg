using Polyhydra.Core.Ffmpeg.Services;

namespace Core.FFmpeg.Tests;

public static class ProcessRunnerTests
{
    public static async Task ReturnsNotStartedWhenExecutableIsMissing()
    {
        var runner = new DefaultFfmpegProcessRunner();
        var result = await runner.StartAsync(new Polyhydra.Core.Ffmpeg.Abstractions.FfmpegCommand("definitely-missing-binary", ["--version"]));

        Assert.Equal(false, result.WasStarted);
        Assert.Equal(null, result.ExitCode);
        Assert.NotNull(result.StandardError);
    }
}
