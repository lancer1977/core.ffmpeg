using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Core.Ffmpeg.Services;

namespace Core.FFmpeg.Tests;

public static class IntegrationTests
{
    public static async Task CanHandleMissingFfprobeGracefully()
    {
        var service = new DefaultFfprobeService();
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ProbeAsync("does-not-exist.mp4"));
    }

    public static async Task ProcessRunnerReturnsFailureWhenCommandCannotStart()
    {
        var runner = new DefaultFfmpegProcessRunner();
        var result = await runner.StartAsync(new FfmpegCommand("ffmpeg-definitely-missing", ["-version"]));

        Assert.Equal(false, result.WasStarted);
        Assert.Equal(null, result.ExitCode);
        Assert.NotNull(result.StandardError);
    }

    public static async Task ProcessRunnerCanExecuteDotnetVersion()
    {
        var runner = new DefaultFfmpegProcessRunner();
        var result = await runner.StartAsync(new FfmpegCommand("dotnet", ["--version"]));

        Assert.Equal(true, result.WasStarted);
        Assert.Equal(0, result.ExitCode);
        Assert.True(!string.IsNullOrWhiteSpace(result.StandardOutput));
    }
}
