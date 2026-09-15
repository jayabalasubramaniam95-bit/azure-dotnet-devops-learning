using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;


namespace ProductApi.Controllers
{

       [ApiController]
       [Route("auth")]
    public class AuthController: ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    // [HttpPost("login")]
    // public IActionResult Login(LoginRequest request)
    // {
    //     if (request.Username != "jayashree" ||
    //         request.Password != "Password123!")
    //     {
    //         return Unauthorized(new
    //         {
    //             message = "Invalid username or password"
    //         });
    //     }

    //     var token =
    //         _jwtTokenService.GenerateToken(request.Username);

    //     return Ok(new LoginResponse
    //     {
    //         AccessToken = token,
    //         TokenType = "Bearer",
    //         ExpiresIn = 3600
    //     });
    // }
     [HttpPost("user-token")]
    public IActionResult GenerateUserToken()
    {
        var token = _jwtTokenService.GenerateToken(
            "Jayashree",
            "User");

        return Ok(new
        {
            username = "Jayashree",
            role = "User",
            token
        });
    }
    [HttpPost("admin-token")]
    public IActionResult GenerateAdminToken()
    {
        var token = _jwtTokenService.GenerateToken(
            "Jayashree",
            "Admin");

        return Ok(new
        {
            username = "Jayashree",
            role = "Admin",
            token
        });
    }

    }
}