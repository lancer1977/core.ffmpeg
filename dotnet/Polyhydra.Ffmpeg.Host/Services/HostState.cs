using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed class HostState
{
    private readonly List<FfmpegJobDefinition> _jobs = new();
    private readonly List<EncoderProfile> _presets = new();
    private readonly HostStorage _storage;

    public HostState(HostStorage storage)
    {
        _storage = storage;
        InitializeDefaults();
    }

    public IReadOnlyList<FfmpegJobDefinition> Jobs => _jobs;
    public IReadOnlyList<EncoderProfile> Presets => _presets;

    public FfmpegJobDefinition Enqueue(FfmpegJobRequest request, string name)
    {
        var job = new FfmpegJobDefinition(name, request, DateTimeOffset.UtcNow, JobStatus.Queued);
        _jobs.Insert(0, job);
        _ = PersistAsync();
        return job;
    }

    public FfmpegJobDefinition UpdateStatus(string name, JobStatus status)
    {
        var index = _jobs.FindIndex(job => string.Equals(job.Name, name, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            throw new InvalidOperationException($"Job '{name}' was not found.");
        }

        var updated = _jobs[index] with { Status = status };
        _jobs[index] = updated;
        _ = PersistAsync();
        return updated;
    }

    private void InitializeDefaults()
    {
        var loadedPresets = _storage.LoadPresetsAsync().GetAwaiter().GetResult();
        var loadedJobs = _storage.LoadJobsAsync().GetAwaiter().GetResult();

        if (loadedPresets is { Count: > 0 })
        {
            _presets.AddRange(loadedPresets);
        }
        else
        {
            _presets.AddRange([
                new EncoderProfile("Fast H.264", VideoCodec: "libx264", AudioCodec: "aac", VideoBitrate: "3500k", AudioBitrate: "160k", Preset: "veryfast"),
                new EncoderProfile("Archive Copy", VideoCodec: "copy", AudioCodec: "copy"),
                new EncoderProfile("HLS 1080p", VideoCodec: "libx264", AudioCodec: "aac", VideoBitrate: "5000k", AudioBitrate: "192k", Preset: "medium")
            ]);
        }

        if (loadedJobs is { Count: > 0 })
        {
            _jobs.AddRange(loadedJobs);
        }
        else
        {
            _jobs.AddRange([
                new FfmpegJobDefinition("Transcode-001", new FfmpegJobRequest("sample.mp4", "sample-out.mp4", new StreamTarget(StreamTargetKind.File, "sample-out.mp4"), _presets[0]), DateTimeOffset.UtcNow.AddMinutes(-40), JobStatus.Succeeded),
                new FfmpegJobDefinition("Transcode-002", new FfmpegJobRequest("archive.mov", "archive-copy.mov", new StreamTarget(StreamTargetKind.File, "archive-copy.mov"), _presets[1]), DateTimeOffset.UtcNow.AddMinutes(-18), JobStatus.Queued)
            ]);
        }

        _ = PersistAsync();
    }

    private async Task PersistAsync()
    {
        try
        {
            await _storage.SavePresetsAsync(_presets);
            await _storage.SaveJobsAsync(_jobs);
        }
        catch
        {
            // Persistence is best-effort for now.
        }
    }
}
