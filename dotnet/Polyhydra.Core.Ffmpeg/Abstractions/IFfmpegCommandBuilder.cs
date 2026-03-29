using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Core.Ffmpeg.Abstractions;

public interface IFfmpegCommandBuilder
{
    FfmpegCommand Build(FfmpegJobRequest request);
}

public sealed record FfmpegCommand(
    string FileName,
    IReadOnlyList<string> Arguments);
