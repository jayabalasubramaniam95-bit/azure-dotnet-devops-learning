using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public InfoController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetInfo()
    {
        var applicationName =
            _configuration["Application:Name"];

        var version =
            _configuration["Application:Version"];

        return Ok(new
        {
            applicationName,
            version
        });
    }
}
