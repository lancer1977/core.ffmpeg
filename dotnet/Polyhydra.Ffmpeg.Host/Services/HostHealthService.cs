namespace Polyhydra.Ffmpeg.Host.Services;

public sealed record HostHealthSnapshot(
    string Service,
    string Status,
    bool Ready,
    string DataPath,
    int JobCount,
    int PresetCount,
    DateTimeOffset CheckedAtUtc);

public sealed class HostHealthService
{
    private readonly HostState _state;
    private readonly HostStorage _storage;

    public HostHealthService(HostState state, HostStorage storage)
    {
        _state = state;
        _storage = storage;
    }

    public HostHealthSnapshot GetSnapshot()
    {
        var ready = Directory.Exists(_storage.RootPath) && _state.Presets.Count > 0 && _state.Jobs.Count > 0;
        var status = ready ? "Healthy" : "Degraded";

        return new HostHealthSnapshot(
            Service: "Polyhydra.Ffmpeg.Host",
            Status: status,
            Ready: ready,
            DataPath: _storage.RootPath,
            JobCount: _state.Jobs.Count,
            PresetCount: _state.Presets.Count,
            CheckedAtUtc: DateTimeOffset.UtcNow);
    }
}
