namespace Polyhydra.Ffmpeg.Contracts;

public enum JobStatus
{
    Draft = 0,
    Queued = 1,
    Running = 2,
    Succeeded = 3,
    Failed = 4,
}
