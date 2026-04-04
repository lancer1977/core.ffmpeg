using System.Text.Json;
using Polyhydra.Ffmpeg.Contracts;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed class HostStorage
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly string _rootPath;

    public HostStorage(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _rootPath = configuration.GetValue<string>("FFMPEG_HOST_DATA_PATH") ?? Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(_rootPath);
    }

    private string JobsPath => Path.Combine(_rootPath, "jobs.json");
    private string PresetsPath => Path.Combine(_rootPath, "presets.json");

    public async Task<List<FfmpegJobDefinition>?> LoadJobsAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(JobsPath)) return null;
        await using var stream = File.OpenRead(JobsPath);
        return await JsonSerializer.DeserializeAsync<List<FfmpegJobDefinition>>(stream, JsonOptions, cancellationToken);
    }

    public async Task<List<EncoderProfile>?> LoadPresetsAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(PresetsPath)) return null;
        await using var stream = File.OpenRead(PresetsPath);
        return await JsonSerializer.DeserializeAsync<List<EncoderProfile>>(stream, JsonOptions, cancellationToken);
    }

    public async Task SaveJobsAsync(IEnumerable<FfmpegJobDefinition> jobs, CancellationToken cancellationToken = default)
    {
        await using var stream = File.Create(JobsPath);
        await JsonSerializer.SerializeAsync(stream, jobs, JsonOptions, cancellationToken);
    }

    public async Task SavePresetsAsync(IEnumerable<EncoderProfile> presets, CancellationToken cancellationToken = default)
    {
        await using var stream = File.Create(PresetsPath);
        await JsonSerializer.SerializeAsync(stream, presets, JsonOptions, cancellationToken);
    }
}
