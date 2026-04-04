using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed record JobRunResult(string JobName, FfmpegProcessResult ProcessResult, DateTimeOffset StartedAtUtc, DateTimeOffset FinishedAtUtc);

public sealed class JobExecutionService
{
    private readonly IFfmpegCommandBuilder _commandBuilder;
    private readonly IFfmpegProcessRunner _processRunner;
    private readonly HostState _state;
    private readonly List<JobRunResult> _history = new();

    public IReadOnlyList<JobRunResult> History => _history;

    public JobExecutionService(IFfmpegCommandBuilder commandBuilder, IFfmpegProcessRunner processRunner, HostState state)
    {
        _commandBuilder = commandBuilder;
        _processRunner = processRunner;
        _state = state;
    }

    public async Task<JobRunResult> RunAsync(FfmpegJobDefinition job, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(job);

        _state.UpdateStatus(job.Name, JobStatus.Running);

        var startedAtUtc = DateTimeOffset.UtcNow;
        var command = _commandBuilder.Build(job.Request);
        var processResult = await _processRunner.StartAsync(command, cancellationToken);
        var finishedAtUtc = DateTimeOffset.UtcNow;

        _state.UpdateStatus(job.Name, processResult.ExitCode == 0 ? JobStatus.Succeeded : JobStatus.Failed);

        var result = new JobRunResult(job.Name, processResult, startedAtUtc, finishedAtUtc);
        _history.Insert(0, result);
        return result;
    }

    public async Task<JobRunResult> RunLatestAsync(CancellationToken cancellationToken = default)
    {
        var job = _state.Jobs.FirstOrDefault() ?? throw new InvalidOperationException("No jobs are available to run.");
        return await RunAsync(job, cancellationToken);
    }
}
