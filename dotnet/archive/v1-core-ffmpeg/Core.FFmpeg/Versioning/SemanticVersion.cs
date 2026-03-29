namespace Core.FFmpeg.Versioning;

public sealed record SemanticVersion(int Major, int Minor, int Patch, string? PreRelease = null, string? BuildMetadata = null)
{
    public static SemanticVersion Parse(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var parts = value.Split('+', 2);
        var preParts = parts[0].Split('-', 2);
        var numbers = preParts[0].Split('.');
        if (numbers.Length != 3) throw new FormatException($"Invalid semantic version: {value}");
        return new SemanticVersion(
            int.Parse(numbers[0]),
            int.Parse(numbers[1]),
            int.Parse(numbers[2]),
            preParts.Length > 1 ? preParts[1] : null,
            parts.Length > 1 ? parts[1] : null);
    }

    public override string ToString()
    {
        var version = $"{Major}.{Minor}.{Patch}";
        if (!string.IsNullOrWhiteSpace(PreRelease)) version += $"-{PreRelease}";
        if (!string.IsNullOrWhiteSpace(BuildMetadata)) version += $"+{BuildMetadata}";
        return version;
    }
}
