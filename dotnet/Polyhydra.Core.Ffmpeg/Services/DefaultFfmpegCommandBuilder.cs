using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Core.Ffmpeg.Services;

public sealed class DefaultFfmpegCommandBuilder : IFfmpegCommandBuilder
{
    public FfmpegCommand Build(FfmpegJobRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.InputPath);

        var arguments = new List<string>
        {
            "-hide_banner",
            "-y",
            "-i", request.InputPath,
        };

        if (request.Overlay is { ImagePath.Length: > 0 } overlay)
        {
            arguments.Add("-i");
            arguments.Add(overlay.ImagePath);
        }

        var filters = BuildFilters(request);
        if (filters.Count > 0)
        {
            arguments.Add("-filter_complex");
            arguments.Add(string.Join(';', filters));
        }

        AddIfValue(arguments, "-c:v", request.Encoder?.VideoCodec);
        AddIfValue(arguments, "-c:a", request.Encoder?.AudioCodec);
        AddIfValue(arguments, "-b:v", request.Encoder?.VideoBitrate);
        AddIfValue(arguments, "-b:a", request.Encoder?.AudioBitrate);
        AddIfValue(arguments, "-preset", request.Encoder?.Preset);

        if (request.Metadata is not null)
        {
            foreach (var pair in request.Metadata)
            {
                if (string.IsNullOrWhiteSpace(pair.Key)) continue;
                arguments.Add("-metadata");
                arguments.Add($"{pair.Key}={pair.Value}");
            }
        }

        var destination = ResolveDestination(request);
        AddOutputPreset(arguments, request.Target, destination, request.HlsOutput);

        return new FfmpegCommand("ffmpeg", arguments);
    }

    private static List<string> BuildFilters(FfmpegJobRequest request)
    {
        var filters = new List<string>();

        var overlayFilter = BuildOverlayFilter(request.Overlay);
        if (!string.IsNullOrWhiteSpace(overlayFilter))
        {
            filters.Add(overlayFilter);
        }

        var drawTextFilter = BuildDrawTextFilter(request.Text);
        if (!string.IsNullOrWhiteSpace(drawTextFilter))
        {
            filters.Add(drawTextFilter);
        }

        return filters;
    }

    private static string? BuildOverlayFilter(OverlayOptions? overlay)
    {
        if (overlay is null || string.IsNullOrWhiteSpace(overlay.ImagePath))
        {
            return null;
        }

        var (x, y) = ParseOverlayPosition(overlay.Position);
        var baseFilter = $"[0:v][1:v]overlay={x}:{y}";

        if (overlay.Opacity is not null)
        {
            var alpha = Math.Clamp(overlay.Opacity.Value, 0d, 1d);
            return $"{baseFilter}:alpha={alpha:0.###}";
        }

        return baseFilter;
    }

    private static string? BuildDrawTextFilter(TextOverlayOptions? text)
    {
        if (text is null)
        {
            return null;
        }

        string? input = null;
        if (!string.IsNullOrWhiteSpace(text.TextFilePath))
        {
            input = $"textfile='{EscapeForFfmpeg(text.TextFilePath)}'";
        }
        else if (!string.IsNullOrWhiteSpace(text.Text))
        {
            input = $"text='{EscapeForFfmpeg(text.Text)}'";
        }

        if (input is null)
        {
            return null;
        }

        var parts = new List<string>
        {
            $"drawtext={input}",
            "x=(w-text_w)/2",
            "y=h-(text_h*2)",
            "fontcolor=white",
            $"fontsize={NormalizeFontSize(text.FontSize)}"
        };

        if (!string.IsNullOrWhiteSpace(text.TextFilePath))
        {
            parts.Add("reload=1");
        }

        if (!string.IsNullOrWhiteSpace(text.FontPath))
        {
            parts.Add($"fontfile='{EscapeForFfmpeg(text.FontPath)}'");
        }

        return string.Join(':', parts);
    }

    private static int NormalizeFontSize(string? value)
    {
        return int.TryParse(value, out var parsed) && parsed > 0 ? parsed : 24;
    }

    private static (string X, string Y) ParseOverlayPosition(string? position)
    {
        if (string.IsNullOrWhiteSpace(position))
        {
            return ("0", "0");
        }

        var normalized = position.Trim().ToLowerInvariant();
        return normalized switch
        {
            "top-left" => ("0", "0"),
            "top-right" => ("W-w", "0"),
            "bottom-left" => ("0", "H-h"),
            "bottom-right" => ("W-w", "H-h"),
            "center" => ("(W-w)/2", "(H-h)/2"),
            _ => ParsePositionCoordinatesOrDefault(position)
        };
    }

    private static (string X, string Y) ParsePositionCoordinatesOrDefault(string value)
    {
        var parts = value.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length == 2 && parts.All(p => p.Length > 0))
        {
            return (parts[0], parts[1]);
        }

        return ("0", "0");
    }

    private static void AddOutputPreset(List<string> arguments, StreamTarget target, string destination, HlsOutputOptions? hlsOutput)
    {
        switch (target.Kind)
        {
            case StreamTargetKind.Hls:
            {
                var hls = hlsOutput ?? new HlsOutputOptions();
                var segmentPattern = hls.SegmentFilename ?? $"{Path.GetFileNameWithoutExtension(destination)}-%03d.ts";
                arguments.AddRange(
                [
                    "-f", "hls",
                    "-hls_time", hls.SegmentTimeSeconds.ToString(),
                    "-hls_playlist_type", hls.PlaylistType,
                    "-hls_segment_filename", segmentPattern,
                    destination
                ]);
                break;
            }

            case StreamTargetKind.Rtmp:
                arguments.AddRange(["-f", "flv", BuildRtmpDestination(target)]);
                break;

            default:
                arguments.Add(destination);
                break;
        }
    }

    private static string ResolveDestination(FfmpegJobRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Target.Destination))
        {
            return request.Target.Destination;
        }

        if (!string.IsNullOrWhiteSpace(request.OutputPath))
        {
            return request.OutputPath;
        }

        throw new ArgumentException("A target destination or output path is required.");
    }

    private static string BuildRtmpDestination(StreamTarget target)
    {
        var baseDestination = target.Destination.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(target.StreamKey))
        {
            return baseDestination;
        }

        return $"{baseDestination}/{target.StreamKey.TrimStart('/')}";
    }

    private static void AddIfValue(List<string> arguments, string flag, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        arguments.Add(flag);
        arguments.Add(value);
    }

    private static string EscapeForFfmpeg(string value)
    {
        return value.Replace("\\", "\\\\").Replace("'", "\\'");
    }
}
