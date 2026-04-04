using Polyhydra.Core.Ffmpeg.Services;

namespace Core.FFmpeg.Tests;

public static class FfprobeServiceTests
{
    public static async Task ThrowsHelpfulErrorWhenFfprobeIsMissing()
    {
        var service = new DefaultFfprobeService();
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ProbeAsync("does-not-exist.mp4"));
    }
}
