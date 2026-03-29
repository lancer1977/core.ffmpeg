using Core.FFmpeg.Builders;
using Core.FFmpeg.Configuration;
using Core.FFmpeg.Overlay;

namespace Core.FFmpeg.Commanding;

public static class FfmpegCommandBuilder
{
    public static FfmpegCommand Build(FfmpegJobConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Input);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Output.Target);

        var args = new List<string> { "-y", "-i", config.Input };

        if (config.Overlays is not null)
        {
            foreach (var overlay in config.Overlays.Where(o => o.Enabled))
            {
                args.Add("-i");
                args.Add(overlay.Path);
            }
        }

        var filters = new List<string>();
        var overlayFilter = FfmpegOverlayBuilder.BuildOverlayFilter(config.Overlays);
        if (!string.IsNullOrWhiteSpace(overlayFilter)) filters.Add(overlayFilter);
        var drawTextFilter = FfmpegOverlayBuilder.BuildDrawTextFilter(config.RuntimeText);
        if (!string.IsNullOrWhiteSpace(drawTextFilter)) filters.Add(drawTextFilter);
        if (filters.Count > 0)
        {
            args.Add("-filter_complex");
            args.Add(string.Join(';', filters));
        }

        var encoding = config.Encoding;
        AddIfValue(args, "-c:v", encoding?.VideoCodec);
        AddIfValue(args, "-c:a", encoding?.AudioCodec);
        AddIfValue(args, "-b:v", encoding?.VideoBitrateKbps is int vbr ? $"{vbr}k" : null);
        AddIfValue(args, "-b:a", encoding?.AudioBitrateKbps is int abr ? $"{abr}k" : null);
        AddIfValue(args, "-r", encoding?.Fps);
        AddIfValue(args, "-g", encoding?.Gop);
        AddIfValue(args, "-preset", encoding?.Preset);
        AddIfValue(args, "-tune", encoding?.Tune);
        AddIfValue(args, "-crf", encoding?.Crf);

        if (encoding?.ExtraArgs is not null)
        {
            args.AddRange(encoding.ExtraArgs);
        }

        switch (config.Output.Transport)
        {
            case FfmpegTransport.Hls:
                args.AddRange(FfmpegHlsPresetBuilder.Build(config.Output.Target, $"{Path.GetFileNameWithoutExtension(config.Output.Target)}-%03d.ts"));
                break;
            case FfmpegTransport.Rtmp:
                args.AddRange(FfmpegRtmpPresetBuilder.Build(config.Output.Target));
                break;
            default:
                args.AddRange(FfmpegFilePresetBuilder.Build(config.Output.Target));
                break;
        }

        if (config.Output.ExtraArgs is not null)
        {
            args.AddRange(config.Output.ExtraArgs);
        }

        return new FfmpegCommand("ffmpeg", args);
    }

    private static void AddIfValue(List<string> args, string flag, object? value)
    {
        if (value is null) return;
        args.Add(flag);
        args.Add(value.ToString()!);
    }
}
