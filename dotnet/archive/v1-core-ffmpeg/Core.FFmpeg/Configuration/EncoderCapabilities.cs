namespace Core.FFmpeg.Configuration;

public sealed record EncoderCapabilities(
    bool SupportsNvenc,
    bool SupportsQsv,
    bool SupportsVaapi);
