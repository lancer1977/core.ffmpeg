namespace Polyhydra.Ffmpeg.Contracts;

public sealed record HlsOutputOptions(
    int SegmentTimeSeconds = 4,
    string PlaylistType = "event",
    string? SegmentFilename = null);
