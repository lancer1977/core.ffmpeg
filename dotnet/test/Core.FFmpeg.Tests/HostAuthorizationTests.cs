using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Core.FFmpeg.Tests;

public static class HostAuthorizationTests
{
    public static async Task HealthEndpoint_RemainsAnonymous()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        var response = await client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public static async Task ProtectedRoutes_RejectAnonymousRequests()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        var rootResponse = await client.GetAsync("/");
        var statusResponse = await client.GetAsync("/api/status");

        Assert.Equal(HttpStatusCode.Unauthorized, rootResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, statusResponse.StatusCode);
        Assert.True(statusResponse.Headers.WwwAuthenticate.Any(header => header.Scheme == "Basic"));
    }

    public static async Task ProtectedRoutes_AllowConfiguredOperatorCredentials()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        client.DefaultRequestHeaders.Authorization = BuildBasicAuthorizationHeader();

        var response = await client.GetAsync("/api/status");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadAsStringAsync();
        Assert.Contains("Polyhydra.Ffmpeg.Host", payload);
    }

    private static WebApplicationFactory<global::Program> CreateFactory()
    {
        var dataPath = Path.Combine(Path.GetTempPath(), $"core-ffmpeg-auth-{Guid.NewGuid():N}");
        return new FfmpegHostFactory(dataPath);
    }

    private static AuthenticationHeaderValue BuildBasicAuthorizationHeader()
    {
        var raw = "operator:secret";
        var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(raw));
        return new AuthenticationHeaderValue("Basic", token);
    }

    private sealed class FfmpegHostFactory : WebApplicationFactory<global::Program>
    {
        private readonly string _dataPath;

        public FfmpegHostFactory(string dataPath)
        {
            _dataPath = dataPath;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting(WebHostDefaults.EnvironmentKey, Environments.Development);
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["DISABLE_HTTPS_REDIRECT"] = "true",
                    ["FFMPEG_HOST_DATA_PATH"] = _dataPath,
                    ["FFMPEG_HOST_OPERATOR_USERNAME"] = "operator",
                    ["FFMPEG_HOST_OPERATOR_PASSWORD"] = "secret",
                });
            });
        }
    }
}
