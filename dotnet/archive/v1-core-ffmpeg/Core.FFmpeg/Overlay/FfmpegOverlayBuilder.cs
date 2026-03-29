using System.Text;
using Core.FFmpeg.Configuration;

namespace Core.FFmpeg.Overlay;

public static class FfmpegOverlayBuilder
{
    public static string? BuildOverlayFilter(IReadOnlyList<FfmpegOverlayImage>? overlays)
    {
        if (overlays is null || overlays.Count == 0)
        {
            return null;
        }

        var enabled = overlays.Where(o => o.Enabled).ToList();
        if (enabled.Count == 0)
        {
            return null;
        }

        var parts = new List<string>(enabled.Count);
        for (var i = 0; i < enabled.Count; i++)
        {
            var overlay = enabled[i];
            var x = overlay.X ?? 0;
            var y = overlay.Y ?? 0;
            parts.Add($"[0:v][{i + 1}:v]overlay={x}:{y}");
        }

        return string.Join(';', parts);
    }

    public static string? BuildDrawTextFilter(FfmpegRuntimeText? runtimeText)
    {
        if (runtimeText is null || !runtimeText.Enabled)
        {
            return null;
        }

        return string.Join(':', new[]
        {
            $"drawtext=textfile='{Escape(runtimeText.Path)}'",
            $"reload={runtimeText.ReloadSeconds}",
            $"fontcolor={runtimeText.FontColor}",
            $"fontsize={runtimeText.FontSize}",
            $"x={runtimeText.X}",
            $"y={runtimeText.Y}"
        });
    }

    private static string Escape(string value) => value.Replace("\\", "\\\\").Replace("'", "\\'");
}
