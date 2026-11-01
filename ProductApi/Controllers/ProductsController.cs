using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers
{
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
        private readonly ILogger<ProductsController> _logger;
    
        public ProductsController(
        ILogger<ProductsController> logger)
    {
        _logger = logger;
    }
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
    
    [HttpGet("{id:int}")]
public async Task<IActionResult> GetProduct(int id)
{
    _logger.LogInformation(
        "Retrieving product. ProductId={ProductId}, RequestId={RequestId}",
        id,
        HttpContext.TraceIdentifier);

    var products = new[]
    {
        new { Id = 1, Name = "Laptop", Price = 1200m },
        new { Id = 2, Name = "Monitor", Price = 450m },
        new { Id = 3, Name = "Keyboard", Price = 100m }
    };

    var product =
        products.FirstOrDefault(p => p.Id == id);

    if (product is null)
    {
        _logger.LogWarning(
            "Product not found. ProductId={ProductId}, RequestId={RequestId}",
            id,
            HttpContext.TraceIdentifier);

        return NotFound();
    }

    _logger.LogInformation(
        "Product retrieved successfully. ProductId={ProductId}, RequestId={RequestId}",
        id,
        HttpContext.TraceIdentifier);

    return Ok(product);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProduct(
       CreateProductRequest request)
        {
            var product = new ProductResponse(
                4,
                request.Name!,
                request.Price);

            return Ok(product);
        }
    
    [HttpGet("test-error")]
public IActionResult TestError()
{
    throw new InvalidOperationException(
        "This is a test exception.");
}

    }
}