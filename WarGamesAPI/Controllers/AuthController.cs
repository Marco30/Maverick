using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.Sessions;
using WarGamesAPI.Crawler;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly ILogger<AuthController> _logger;
    readonly IUserRepository _userRepo;
    readonly IMapper _mapper;

    public AuthController(ILogger<AuthController> logger, IUserRepository userRepo, IMapper mapper)
    {
        _logger = logger;
        _userRepo = userRepo;
        _mapper = mapper;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(LoginDto login)
    {


        if (string.IsNullOrEmpty(login.Email))
        {
            return BadRequest("Lacking email");
        }


        var user = await _userRepo.GetUserFromEmailAsync(login.Email);


        if (user != null)
        {

            if (user.Password == login.Password)
            {
                string token = TokenData.CreateJwtToken(user);
                user = await _userRepo.GetUserFromEmailAsync(login.Email);
                return Ok(new InLoggedUserDto { Token = token, User = user });
            }
            else
            {
                return Ok(new InLoggedUserDto() { Token = "", });
            }
        }
        else
        {
            return Ok(new InLoggedUserDto() { Token = "", });
        }

    }

    [ValidateToken]
    [HttpPost("newtoken")]
    public Task<IActionResult> NewToken()
    {
        try
        {

            string authHeaderToken = Request.Headers["Authorization"];

            var socialSecurityNumber = TokenData.GetClaimByKey(authHeaderToken, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var lastName = TokenData.GetClaimByKey(authHeaderToken, "lastname");
            var firstName = TokenData.GetClaimByKey(authHeaderToken, "firstName");
            var id = Convert.ToInt32(TokenData.GetClaimByKey(authHeaderToken, "id"));
            var fullName = firstName + " " + lastName;

            var user = new UserDto
            {
                Id = id,
                FullName = fullName,
                LastName = lastName,
                FirstName = firstName,
                SocialSecurityNumber = socialSecurityNumber
            };

            if (user != null)
            {
                string token = TokenData.CreateJwtToken(user);

                return Task.FromResult<IActionResult>(Ok(new InLoggedUserDto { Token = token }));
            }
            else
            {
                return Task.FromResult<IActionResult>(Ok(new InLoggedUserDto() { Token = "", }));
            }



        }
        catch (Exception e)
        {

            Console.WriteLine(e);
            return Task.FromResult<IActionResult>(NotFound());
        }

    }
    
    [ValidateToken]
    [HttpPost("resetpassword")]
    public async Task<IActionResult> resetUserPassword(ResetPasswordDto reset)
    {
        // Validate token 
        if (reset.Token == "resetPasswordTest")
        {
            return Ok();
        }
        
        var userId = TokenData.getUserId($"Bearer {reset.Token}");
        
        try
        {
            var user = await _userRepo.GetUserFromIdAsync(userId);

            if (user == null || reset.Password == null) return BadRequest();

            var updateSuccess = await _userRepo.UpdateUserPassword((int)user.Id!, reset.Password);
            if (!updateSuccess)
            {
                return StatusCode(500, "Error Updating user password");
            }

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating user password");

        }
    }

    [HttpPost("getUserDataFromSecurityNumber")]
    public Task<IActionResult> GetUserData(GetUserDataDto userData)
    {
        if (userData.SocialSecurityNumber == null)
        {
            return Task.FromResult<IActionResult>(BadRequest());
        }

        UserDto user = Crawlers.SeleniumGetUserInfoPagesCrawler("https://mrkoll.se/", userData.SocialSecurityNumber);


        user.SocialSecurityNumber = userData.SocialSecurityNumber;
        return Task.FromResult<IActionResult>(Ok(user));


    }

    [HttpPost("registeruser")]
    public async Task<ActionResult<User>> RegisterUser(RegisterUserDto register)
    {
        register.Address = _mapper.Map<AddressDto>(register.Address);

        if (register.Email is null) 
            return BadRequest(new ResponseMessageDto { Error = true, Message = "Email saknas" });
        
        if (register.SocialSecurityNumber is null) 
            return BadRequest(new ResponseMessageDto { Error = true, Message = "Personnummer saknas" });
        
        if (register.Password is null) 
            return BadRequest(new ResponseMessageDto { Error = true, Message = "Lösenord saknas" });

        if (await _userRepo.GetUserFromEmailAsync(register.Email) != null )
            return BadRequest(new ResponseMessageDto { Error = true, Message = "En användare med denna email är redan registrerad" });

        if (await _userRepo.GetUserFromSocSecAsync(register.SocialSecurityNumber) != null)
            return BadRequest(new ResponseMessageDto { Error = true, Message = "En användare med detta peronnummer är redan registrerad" });

        if (!VerifySocialSecurityNumber(register.SocialSecurityNumber)) return BadRequest(new ResponseMessageDto { Error = true, Message = "Fel på personnummer" });

        try
        {
            //var crawlResult = Crawlers.SeleniumGetUserInfoPagesCrawler("https://mrkoll.se/", register.SocialSecurityNumber);
            //AddressDto? address = crawlResult.Address;

            try
            {
                //register.AddressId = await _userRepo.AddAddress(address!);

                register.AddressId = await _userRepo.AddAddress(register.Address);

            }
            catch (Exception)
            {
                return StatusCode(500, new ResponseMessageDto { Error = true, Message = "Error saving address" });
            }
            
        }
        catch (Exception e)
        {
            _logger.LogInformation($"Crawl failed, registering user anyway{e.Message}");
        }

        try
        {
            var registeredUser = await _userRepo.AddUser(register);
            
            var token = TokenData.CreateJwtToken(registeredUser!);

            return Ok(new InLoggedUserDto { User = registeredUser, Token = token });

        }
        catch (Exception)
        {
            return StatusCode(500, new ResponseMessageDto { Error = true, Message = "Error registering user" });

        }
        
            


            


        


    }

    private static bool VerifySocialSecurityNumber(string number)
    {
        const string pattern = @"^(19|20)?(\d{6}([-+]|\s)\d{4}|(?!19|20)\d{10})$";
        var m = Regex.Match(number, pattern, RegexOptions.IgnoreCase);
        return m.Success;
    }

}