using MudBlazor.Services;
using Polyhydra.Core.Ffmpeg;
using Polyhydra.Ffmpeg.Host.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreFfmpeg();
builder.Services.AddMudServices();
builder.Services.AddSingleton<Polyhydra.Ffmpeg.Host.Services.HostStorage>();
builder.Services.AddSingleton<Polyhydra.Ffmpeg.Host.Services.HostState>();
builder.Services.AddSingleton<Polyhydra.Ffmpeg.Host.Services.HostHealthService>();
builder.Services.AddScoped<Polyhydra.Ffmpeg.Host.Services.HostWorkflowService>();
builder.Services.AddScoped<Polyhydra.Ffmpeg.Host.Services.JobExecutionService>();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var disableHttpsRedirect = builder.Configuration.GetValue("DISABLE_HTTPS_REDIRECT", false);
if (!disableHttpsRedirect)
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapGet("/api/status", (Polyhydra.Ffmpeg.Host.Services.HostWorkflowService workflow) => Results.Ok(new
{
    Service = "Polyhydra.Ffmpeg.Host",
    Status = "Ready",
    Jobs = workflow.Jobs.Count,
    Presets = workflow.Presets.Count
}));

app.MapGet("/api/health", (Polyhydra.Ffmpeg.Host.Services.HostHealthService health) =>
{
    var snapshot = health.GetSnapshot();
    return snapshot.Ready ? Results.Ok(snapshot) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
});

app.Run();
