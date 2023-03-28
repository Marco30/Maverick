using Microsoft.AspNetCore.Mvc;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;
#pragma warning disable CS1998

namespace WarGamesAPI.Controllers;

[Route("wargames")] [ApiController]
public class AuthController : ControllerBase
{

    [HttpPost("login")]
    public async Task<ActionResult<Message>> Login(LoginDto login)
    {
        var user = new User { Id = new Random().Next(), Email = login.Email, Password = login.Password };
        return Ok(user);

    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(RegisterUserDto user)
    {
        var newUser = new User { Id = new Random().Next(), Email = user.Email, Password = user.Password };
        return StatusCode(201, newUser);
    }
}