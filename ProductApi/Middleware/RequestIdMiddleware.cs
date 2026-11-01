namespace ProductApi.Middleware;

public class RequestIdMiddleware
{
    private const string RequestIdHeader = "X-Request-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestIdMiddleware> _logger;

    public RequestIdMiddleware(
        RequestDelegate next,
        ILogger<RequestIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = context.Request.Headers[RequestIdHeader].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(requestId))
        {
            requestId = Guid.NewGuid().ToString();
        }

        context.TraceIdentifier = requestId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[RequestIdHeader] = requestId;

            return Task.CompletedTask;
        });

        using (_logger.BeginScope(
            new Dictionary<string, object>
            {
                ["RequestId"] = requestId
            }))
        {
            _logger.LogInformation(
                "Request started: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            _logger.LogInformation(
                "Request completed: {StatusCode}",
                context.Response.StatusCode);
        }
    }
}