using Microsoft.Extensions.DependencyInjection;
using Polyhydra.Core.Ffmpeg.Abstractions;
using Polyhydra.Core.Ffmpeg.Services;

namespace Polyhydra.Core.Ffmpeg;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreFfmpeg(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IFfmpegCommandBuilder, DefaultFfmpegCommandBuilder>();
        services.AddSingleton<IFfprobeService, DefaultFfprobeService>();
        services.AddSingleton<IFfmpegProcessRunner, DefaultFfmpegProcessRunner>();

        return services;
    }
}
