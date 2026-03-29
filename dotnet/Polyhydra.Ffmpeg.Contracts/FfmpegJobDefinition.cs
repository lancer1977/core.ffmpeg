namespace Polyhydra.Ffmpeg.Contracts
{
    public sealed record FfmpegJobDefinition(
        string Name,
        FfmpegJobRequest Request,
        DateTimeOffset CreatedAtUtc);
}