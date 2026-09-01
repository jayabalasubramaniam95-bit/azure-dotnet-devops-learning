using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
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
}