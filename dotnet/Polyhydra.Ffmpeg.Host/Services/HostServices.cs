using System.Linq;
using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed record ProbeViewModel(string SourcePath, FfprobeSnapshot Snapshot, DateTimeOffset ProbedAtUtc);
public sealed record JobPreview(string Name, FfmpegJobRequest Request, FfmpegCommand Command);
public sealed record HostOverview(int JobCount, int PresetCount, int FileTargets, int HlsTargets, int RtmpTargets, int FailedJobs);
public sealed record JobDraft(string InputPath, string OutputPath, EncoderProfile? Preset, bool UseHls, HlsOutputOptions? HlsOutput);

public sealed class HostWorkflowService
{
    private readonly IFfprobeService _probeService;
    private readonly IFfmpegCommandBuilder _commandBuilder;
    private readonly HostState _state;

    public HostWorkflowService(IFfprobeService probeService, IFfmpegCommandBuilder commandBuilder, HostState state)
    {
        _probeService = probeService;
        _commandBuilder = commandBuilder;
        _state = state;
    }

    public IReadOnlyList<FfmpegJobDefinition> Jobs => _state.Jobs;
    public IReadOnlyList<EncoderProfile> Presets => _state.Presets;
    public HostOverview Overview => new(
        JobCount: _state.Jobs.Count,
        PresetCount: _state.Presets.Count,
        FileTargets: _state.Jobs.Count(job => job.Request.Target.Kind == StreamTargetKind.File),
        HlsTargets: _state.Jobs.Count(job => job.Request.Target.Kind == StreamTargetKind.Hls),
        RtmpTargets: _state.Jobs.Count(job => job.Request.Target.Kind == StreamTargetKind.Rtmp),
        FailedJobs: _state.Jobs.Count(job => job.Status == JobStatus.Failed));

    public JobPreview CreatePreview(string inputPath, string outputPath, EncoderProfile? preset, bool useHls = false, string? hlsSegmentTime = null, string? hlsPlaylistType = null, string? hlsSegmentPattern = null)
    {
        var draft = new JobDraft(
            inputPath,
            outputPath,
            preset,
            useHls,
            useHls ? new HlsOutputOptions(
                SegmentTimeSeconds: int.TryParse(hlsSegmentTime, out var parsedTime) && parsedTime > 0 ? parsedTime : 4,
                PlaylistType: string.IsNullOrWhiteSpace(hlsPlaylistType) ? "event" : hlsPlaylistType,
                SegmentFilename: string.IsNullOrWhiteSpace(hlsSegmentPattern) ? null : hlsSegmentPattern) : null);

        return CreatePreview(draft);
    }

    public JobPreview CreatePreview(JobDraft draft)
    {
        ArgumentNullException.ThrowIfNull(draft);

        var target = draft.UseHls ? new StreamTarget(StreamTargetKind.Hls, draft.OutputPath) : new StreamTarget(StreamTargetKind.File, draft.OutputPath);
        var request = new FfmpegJobRequest(draft.InputPath, draft.OutputPath, target, draft.Preset, HlsOutput: draft.HlsOutput);
        return new JobPreview($"Job-{DateTimeOffset.UtcNow:HHmmss}", request, _commandBuilder.Build(request));
    }

    public FfmpegJobDefinition Enqueue(string inputPath, string outputPath, EncoderProfile? preset)
    {
        var request = new FfmpegJobRequest(inputPath, outputPath, new StreamTarget(StreamTargetKind.File, outputPath), preset);
        return _state.Enqueue(request, $"Job-{DateTimeOffset.UtcNow:HHmmss}");
    }

    public async Task<ProbeViewModel> ProbeAsync(string sourcePath, CancellationToken cancellationToken = default)
    {
        var snapshot = await _probeService.ProbeAsync(sourcePath, cancellationToken);
        return new ProbeViewModel(sourcePath, snapshot, DateTimeOffset.UtcNow);
    }
}
