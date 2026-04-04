using System.Diagnostics;
using System.Text;
using Polyhydra.Core.Ffmpeg.Abstractions;

namespace Polyhydra.Core.Ffmpeg.Services;

public sealed class DefaultFfmpegProcessRunner : IFfmpegProcessRunner
{
    public async Task<FfmpegProcessResult> StartAsync(FfmpegCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.FileName);

        var psi = new ProcessStartInfo
        {
            FileName = command.FileName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in command.Arguments)
        {
            psi.ArgumentList.Add(argument);
        }

        try
        {
            using var process = Process.Start(psi);
            if (process is null)
            {
                return new FfmpegProcessResult(
                    ExitCode: null,
                    StandardOutput: null,
                    StandardError: "Failed to start FFmpeg process.",
                    WasStarted: false);
            }

            using var registration = cancellationToken.Register(() =>
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill(entireProcessTree: true);
                    }
                }
                catch
                {
                    // Cancellation cleanup should not throw into the callback path.
                }
            });

            var stdoutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (!process.HasExited)
                {
                    try { process.Kill(entireProcessTree: true); } catch { }
                }

                throw;
            }

            var stdout = await stdoutTask;
            var stderr = await stderrTask;

            return new FfmpegProcessResult(
                ExitCode: process.ExitCode,
                StandardOutput: stdout,
                StandardError: stderr,
                WasStarted: true);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var message = new StringBuilder();
            message.Append("Failed to execute FFmpeg process.");
            message.Append(' ');
            message.Append(ex.Message);

            return new FfmpegProcessResult(
                ExitCode: null,
                StandardOutput: null,
                StandardError: message.ToString(),
                WasStarted: false);
        }
    }
}
