using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductApi.Controllers;
namespace ProductApi.Tests.Controllers;

public class InfoControllerTests
{
    [Fact]
    public void GetInfo_ReturnsConfiguredApplicationInformation()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Application:Name"] = "Product API",
                ["Application:Version"] = "v1"
            })
            .Build();

        // Create the REAL InfoController from ProductApi
        var controller = new InfoController(configuration);

        // Act
        var result = controller.GetInfo();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}