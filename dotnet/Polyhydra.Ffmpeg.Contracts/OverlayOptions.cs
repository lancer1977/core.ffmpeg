namespace Polyhydra.Ffmpeg.Contracts
{
    public sealed record OverlayOptions(
        string ImagePath,
        string? Position = null,
        double? Opacity = null);
}