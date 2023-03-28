using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.JsonCRUD;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;
#pragma warning disable CS1998

namespace WarGamesAPI.Controllers;

[Route("wargames")] [ApiController]
public class AuthController : ControllerBase
{
    readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<Message>> Login(LoginDto login)
    {
        var users = Json.GetJsonData<User>("User");
        var user = users.FirstOrDefault(u => u.Password == login.Password);
        if (user is null) return Unauthorized("Wrong Password");
        _logger.LogInformation($" User logged in. userId: {user.Id}");

        return Ok(user);

    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(RegisterUserDto user)
    {
        var newUser = new User { Id = new Random().Next(), Email = user.Email, Password = user.Password };
        Json.CheckAndAddDataToJson("User", newUser);
        return StatusCode(201, newUser);
    }
}