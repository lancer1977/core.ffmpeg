using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Polyhydra.Ffmpeg.Host.Services;

public sealed class OperatorBasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "OperatorBasic";
    public const string PolicyName = "OperatorOnly";

    private readonly IConfiguration _configuration;

    public OperatorBasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var username = _configuration.GetValue<string>("FFMPEG_HOST_OPERATOR_USERNAME");
        var password = _configuration.GetValue<string>("FFMPEG_HOST_OPERATOR_PASSWORD");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!Request.Headers.TryGetValue("Authorization", out var authorizationValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authorization = authorizationValues.ToString();
        if (!authorization.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var encoded = authorization["Basic ".Length..].Trim();
        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }
        catch (FormatException ex)
        {
            return Task.FromResult(AuthenticateResult.Fail(ex));
        }

        var separatorIndex = decoded.IndexOf(':');
        if (separatorIndex <= 0)
        {
            return Task.FromResult(AuthenticateResult.Fail("The basic authorization header is malformed."));
        }

        var suppliedUsername = decoded[..separatorIndex];
        var suppliedPassword = decoded[(separatorIndex + 1)..];

        if (!string.Equals(suppliedUsername, username, StringComparison.Ordinal) ||
            !string.Equals(suppliedPassword, password, StringComparison.Ordinal))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid operator credentials."));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Operator"),
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Polyhydra Ffmpeg Host\", charset=\"UTF-8\"");
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }
}
