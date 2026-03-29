using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed class HostState
{
    private readonly List<FfmpegJobDefinition> _jobs = new();
    private readonly List<EncoderProfile> _presets = new();

    public HostState()
    {
        _presets.AddRange(
        [
            new EncoderProfile("Fast H.264", VideoCodec: "libx264", AudioCodec: "aac", VideoBitrate: "3500k", AudioBitrate: "160k", Preset: "veryfast"),
            new EncoderProfile("Archive Copy", VideoCodec: "copy", AudioCodec: "copy"),
            new EncoderProfile("HLS 1080p", VideoCodec: "libx264", AudioCodec: "aac", VideoBitrate: "5000k", AudioBitrate: "192k", Preset: "medium")
        ]);

        _jobs.AddRange([
            new FfmpegJobDefinition("Transcode-001", new FfmpegJobRequest("sample.mp4", "sample-out.mp4", new StreamTarget(StreamTargetKind.File, "sample-out.mp4"), _presets[0]), DateTimeOffset.UtcNow.AddMinutes(-40)),
            new FfmpegJobDefinition("Transcode-002", new FfmpegJobRequest("archive.mov", "archive-copy.mov", new StreamTarget(StreamTargetKind.File, "archive-copy.mov"), _presets[1]), DateTimeOffset.UtcNow.AddMinutes(-18))
        ]);
    }

    public IReadOnlyList<FfmpegJobDefinition> Jobs => _jobs;
    public IReadOnlyList<EncoderProfile> Presets => _presets;

    public FfmpegJobDefinition Enqueue(FfmpegJobRequest request, string name)
    {
        var job = new FfmpegJobDefinition(name, request, DateTimeOffset.UtcNow);
        _jobs.Insert(0, job);
        return job;
    }
}
