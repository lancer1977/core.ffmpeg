namespace Polyhydra.Ffmpeg.Contracts
{
    public sealed record TextOverlayOptions(
        string? Text = null,
        string? TextFilePath = null,
        string? FontPath = null,
        string? FontSize = null);
}