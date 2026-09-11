using Microsoft.AspNetCore.Mvc.Testing;

namespace ProductApi.Tests
{
    public class InfoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public InfoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetInfo_ReturnsExpectedApiVersion()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/config");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("\"apiVersion\":\"1.0\"", content);
    }
}
}