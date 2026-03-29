using MudBlazor.Services;
using Polyhydra.Core.Ffmpeg;
using Polyhydra.Ffmpeg.Host.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreFfmpeg();
builder.Services.AddMudServices();
builder.Services.AddSingleton<Polyhydra.Ffmpeg.Host.Services.HostState>();
builder.Services.AddScoped<Polyhydra.Ffmpeg.Host.Services.HostWorkflowService>();
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

app.Run();
