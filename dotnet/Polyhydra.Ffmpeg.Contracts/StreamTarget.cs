namespace Polyhydra.Ffmpeg.Contracts
{
    public sealed record StreamTarget(
        StreamTargetKind Kind,
        string Destination,
        string? StreamKey = null);
}