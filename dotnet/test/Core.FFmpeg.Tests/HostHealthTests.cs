using Polyhydra.Ffmpeg.Host.Services;

namespace Core.FFmpeg.Tests;

public static class HostHealthTests
{
    public static void ReportsHealthySnapshotForSeededState()
    {
        var root = Path.Combine(Path.GetTempPath(), $"core-ffmpeg-health-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);

        var storage = new HostStorage(root);
        var state = new HostState(storage);
        var health = new HostHealthService(state, storage);

        var snapshot = health.GetSnapshot();

        Assert.True(snapshot.Ready);
        Assert.Equal("Healthy", snapshot.Status);
        Assert.Equal(root, snapshot.DataPath);
        Assert.True(snapshot.JobCount > 0);
        Assert.True(snapshot.PresetCount > 0);
    }
}
