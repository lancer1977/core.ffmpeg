namespace Polyhydra.Ffmpeg.Contracts
{
    public sealed record EncoderProfile(
        string Name,
        string? VideoCodec = null,
        string? AudioCodec = null,
        string? VideoBitrate = null,
        string? AudioBitrate = null,
        string? Preset = null);
}