using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ProductApi.Security;

public class ApiKeyAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeaderName = "X-API-Key";

    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check for X-API-Key header
        if (!Request.Headers.TryGetValue(
                ApiKeyHeaderName,
                out var apiKeyHeader))
        {
            return Task.FromResult(
                AuthenticateResult.NoResult());
        }

        var providedApiKey = apiKeyHeader.ToString();

        // Read expected API key from configuration
        var configuredApiKey =
            _configuration["ApiKey:Value"];

        if (string.IsNullOrWhiteSpace(configuredApiKey))
        {
            return Task.FromResult(
                AuthenticateResult.Fail(
                    "API key is not configured."));
        }

        // Compare provided key with configured key
        if (!string.Equals(
                providedApiKey,
                configuredApiKey,
                StringComparison.Ordinal))
        {
            return Task.FromResult(
                AuthenticateResult.Fail(
                    "Invalid API key."));
        }

        // Create identity for the authenticated API client
        var claims = new[]
        {
            new Claim(
                ClaimTypes.Name,
                "ApiKeyClient"),

            new Claim(
                ClaimTypes.AuthenticationMethod,
                "ApiKey")
        };

        var identity = new ClaimsIdentity(
            claims,
            Scheme.Name);

        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(
            principal,
            Scheme.Name);

        return Task.FromResult(
            AuthenticateResult.Success(ticket));
    }
}