namespace ProductApi.Tests;

public class ProductTests
{
    [Fact]
    public void Product_Id_Should_Be_Positive()
    {
        var productId = 1;

        Assert.True(productId > 0);
    }
}
