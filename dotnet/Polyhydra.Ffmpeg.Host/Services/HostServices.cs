using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed record ProbeViewModel(string SourcePath, FfprobeSnapshot Snapshot, DateTimeOffset ProbedAtUtc);
public sealed record JobPreview(string Name, FfmpegJobRequest Request, FfmpegCommand Command);

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

    public JobPreview CreatePreview(string inputPath, string outputPath, EncoderProfile? preset)
    {
        var request = new FfmpegJobRequest(inputPath, outputPath, new StreamTarget(StreamTargetKind.File, outputPath), preset);
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
