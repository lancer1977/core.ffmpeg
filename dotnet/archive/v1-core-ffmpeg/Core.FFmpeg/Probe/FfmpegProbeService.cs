using System.Diagnostics;
using System.Text.Json;

namespace Core.FFmpeg.Probe;

public sealed class FfmpegProbeService
{
    public async Task<FfmpegProbeResult> ProbeAsync(string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

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
        psi.ArgumentList.Add("format=duration:stream=codec_type,codec_name");
        psi.ArgumentList.Add("-of");
        psi.ArgumentList.Add("json");
        psi.ArgumentList.Add(filePath);

        using var process = System.Diagnostics.Process.Start(psi) ?? throw new InvalidOperationException("Failed to start ffprobe.");
        var stdout = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        using var document = JsonDocument.Parse(stdout);
        var root = document.RootElement;
        double? duration = null;
        string? videoCodec = null;
        string? audioCodec = null;

        if (root.TryGetProperty("format", out var format) && format.TryGetProperty("duration", out var durationProp) && double.TryParse(durationProp.GetString(), out var parsedDuration))
        {
            duration = parsedDuration;
        }

        if (root.TryGetProperty("streams", out var streams))
        {
            foreach (var stream in streams.EnumerateArray())
            {
                var codecType = stream.TryGetProperty("codec_type", out var ct) ? ct.GetString() : null;
                var codecName = stream.TryGetProperty("codec_name", out var cn) ? cn.GetString() : null;

                if (codecType == "video" && videoCodec is null) videoCodec = codecName;
                if (codecType == "audio" && audioCodec is null) audioCodec = codecName;
            }
        }

        return new FfmpegProbeResult(duration, videoCodec, audioCodec);
    }
}
