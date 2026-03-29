using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Core.Ffmpeg.Abstractions;

public interface IFfmpegProcessRunner
{
    Task<FfmpegProcessResult> StartAsync(FfmpegCommand command, CancellationToken cancellationToken = default);
}

public sealed record FfmpegProcessResult(
    int? ExitCode,
    string? StandardOutput,
    string? StandardError,
    bool WasStarted);
