namespace Core.FFmpeg.Commanding;

public sealed record FfmpegCommand(string FileName, IReadOnlyList<string> Arguments);
