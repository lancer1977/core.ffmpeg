using System.Diagnostics;
using Core.FFmpeg.Commanding;

namespace Core.FFmpeg.Process;

public sealed class FfmpegProcessRunner
{
    public FfmpegProcessHandle Start(FfmpegCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var psi = new ProcessStartInfo
        {
            FileName = command.FileName,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in command.Arguments)
        {
            psi.ArgumentList.Add(arg);
        }

        var process = System.Diagnostics.Process.Start(psi) ?? throw new InvalidOperationException("Failed to start FFmpeg.");
        return new FfmpegProcessHandle(process);
    }
}
