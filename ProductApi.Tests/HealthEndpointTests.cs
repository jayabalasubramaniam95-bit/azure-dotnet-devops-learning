using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ProductApi.Tests
{
   public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ShouldReturnHealthy()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body =
            await response.Content.ReadFromJsonAsync<HealthResponse>();

        Assert.NotNull(body);
        Assert.Equal("Healthy", body.Status);
        Assert.Equal("1.0.0", body.Version);
    }

    [Fact]
    public async Task InvalidRoute_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/invalid-route");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private record HealthResponse(
        string Status,
        string Version);
}
}