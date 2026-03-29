using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Core.Ffmpeg.Abstractions;

public interface IFfprobeService
{
    Task<FfprobeSnapshot> ProbeAsync(string sourcePath, CancellationToken cancellationToken = default);
}
