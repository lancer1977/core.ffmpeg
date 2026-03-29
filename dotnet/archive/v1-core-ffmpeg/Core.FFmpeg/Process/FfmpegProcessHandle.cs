using System.Diagnostics;

namespace Core.FFmpeg.Process;

public sealed class FfmpegProcessHandle : IAsyncDisposable
{
    private readonly System.Diagnostics.Process _process;

    internal FfmpegProcessHandle(System.Diagnostics.Process process)
    {
        _process = process;
        Pid = process.Id;
    }

    public int Pid { get; }

    public Task StopAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        var wait = timeout ?? TimeSpan.FromSeconds(3);

        if (_process.HasExited)
        {
            return Task.CompletedTask;
        }

        _process.Kill(entireProcessTree: true);
        return _process.WaitForExitAsync(cancellationToken).WaitAsync(wait, cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        if (!_process.HasExited)
        {
            _process.Dispose();
        }

        return ValueTask.CompletedTask;
    }
}
