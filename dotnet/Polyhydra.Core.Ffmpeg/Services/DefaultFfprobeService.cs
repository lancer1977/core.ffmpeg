using System.Diagnostics;
using System.Text.Json;
using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Core.Ffmpeg.Services;

public sealed class DefaultFfprobeService : IFfprobeService
{
    public async Task<FfprobeSnapshot> ProbeAsync(string sourcePath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);

        var psi = new ProcessStartInfo
        {
            FileName = "ffprobe",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        psi.ArgumentList.Add("-v");
        psi.ArgumentList.Add("error");
        psi.ArgumentList.Add("-show_entries");
        psi.ArgumentList.Add("format=duration:stream=codec_type,codec_name,width,height");
        psi.ArgumentList.Add("-of");
        psi.ArgumentList.Add("json");
        psi.ArgumentList.Add(sourcePath);

        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start ffprobe.");
        var stdoutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);
        var stdout = await stdoutTask;
        var stderr = await stderrTask;

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"ffprobe exited with code {process.ExitCode}: {stderr}".Trim());
        }

        using var document = JsonDocument.Parse(stdout);
        var root = document.RootElement;
        TimeSpan? duration = null;
        int? width = null;
        int? height = null;
        string? videoCodec = null;
        string? audioCodec = null;

        if (root.TryGetProperty("format", out var format) &&
            format.TryGetProperty("duration", out var durationProp) &&
            double.TryParse(durationProp.GetString(), out var parsedDuration) &&
            parsedDuration >= 0)
        {
            duration = TimeSpan.FromSeconds(parsedDuration);
        }

        if (root.TryGetProperty("streams", out var streams) && streams.ValueKind == JsonValueKind.Array)
        {
            foreach (var stream in streams.EnumerateArray())
            {
                var codecType = stream.TryGetProperty("codec_type", out var ct) ? ct.GetString() : null;
                var codecName = stream.TryGetProperty("codec_name", out var cn) ? cn.GetString() : null;

                if (codecType == "video")
                {
                    videoCodec ??= codecName;

                    if (width is null && stream.TryGetProperty("width", out var w) && w.TryGetInt32(out var parsedWidth))
                    {
                        width = parsedWidth;
                    }

                    if (height is null && stream.TryGetProperty("height", out var h) && h.TryGetInt32(out var parsedHeight))
                    {
                        height = parsedHeight;
                    }
                }

                if (codecType == "audio")
                {
                    audioCodec ??= codecName;
                }
            }
        }

        return new FfprobeSnapshot(
            SourcePath: sourcePath,
            Duration: duration,
            Width: width,
            Height: height,
            VideoCodec: videoCodec,
            AudioCodec: audioCodec);
    }
}
