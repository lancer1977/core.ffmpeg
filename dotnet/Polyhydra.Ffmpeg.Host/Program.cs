using Microsoft.AspNetCore.Authentication;
using MudBlazor.Services;
using Polyhydra.Core.Ffmpeg;
using Polyhydra.Ffmpeg.Host.Components;
using Polyhydra.Ffmpeg.Host.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreFfmpeg();
builder.Services.AddMudServices();
builder.Services.AddAuthentication(OperatorBasicAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, OperatorBasicAuthenticationHandler>(
        OperatorBasicAuthenticationHandler.SchemeName, _ => { });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(OperatorBasicAuthenticationHandler.PolicyName, policy =>
    {
        policy.AddAuthenticationSchemes(OperatorBasicAuthenticationHandler.SchemeName);
        policy.RequireAuthenticatedUser();
    });
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

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization(OperatorBasicAuthenticationHandler.PolicyName);

app.MapGet("/api/status", (Polyhydra.Ffmpeg.Host.Services.HostWorkflowService workflow) => Results.Ok(new
{
    Service = "Polyhydra.Ffmpeg.Host",
    Status = "Ready",
    Jobs = workflow.Jobs.Count,
    Presets = workflow.Presets.Count
})).RequireAuthorization(OperatorBasicAuthenticationHandler.PolicyName);

app.MapGet("/api/health", (Polyhydra.Ffmpeg.Host.Services.HostHealthService health) =>
{
    var snapshot = health.GetSnapshot();
    return snapshot.Ready ? Results.Ok(snapshot) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
}).AllowAnonymous();

app.Run();

public partial class Program { }
