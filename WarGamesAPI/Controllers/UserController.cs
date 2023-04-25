using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiracleMileAPI.Sessions;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class UserController : ControllerBase
{
    readonly ILogger<UserController> _logger;
    readonly IMapper _mapper;
    readonly IUserRepository _userRepo;
    readonly IValidationRepository _validationRepo;

    public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository userRepo, 
        IValidationRepository validationRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _userRepo = userRepo;
        _validationRepo = validationRepo;
    }

    
    [ValidateToken]
    [HttpGet("getuser")]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        
        return Ok(await _userRepo.GetUserFromIdAsync(userId));
    }

    [ValidateToken]
    [HttpPost("updateuser")]
    public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto updateUser)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        
        try
        {
            await _userRepo.UpdateUserAsync(userId, updateUser);
            return NoContent();
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }



    [ValidateToken]
    [HttpDelete("deleteuser")]
    public async Task<ActionResult> DeleteUser()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        try
        {
            await _userRepo.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }
        
    }
    
}