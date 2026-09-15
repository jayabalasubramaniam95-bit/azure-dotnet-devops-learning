using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers
{
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
      [Authorize(AuthenticationSchemes = "ApiKey")]
    public IActionResult GetProducts()
    {
        var products = new[]
        {
            new { Id = 1, Name = "Laptop", Price = 1200 },
            new { Id = 2, Name = "Keyboard", Price = 100 },
            new { Id = 3, Name = "Mouse", Price = 50 }
        };

        return Ok(products);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    // Learning example
    return Ok(new
    {
        message = $"Product {id} deleted successfully."
    });
}

}
}