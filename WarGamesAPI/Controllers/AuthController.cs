using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.Sessions;
using WarGamesAPI.Crawler;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;
#pragma warning disable CS1998

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly ILogger<AuthController> _logger;
    readonly IUserRepository _userRepo;

    public AuthController(ILogger<AuthController> logger, IUserRepository userRepo)
    {
        _logger = logger;
        _userRepo = userRepo;
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
    public async Task<IActionResult> NewToken()
    {
        try
        {

            string authHeaderToken = Request.Headers["Authorization"];

            var socialSecurityNumber = TokenData.GetClaimByKey(authHeaderToken, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var lastName = TokenData.GetClaimByKey(authHeaderToken, "lastname");
            var firstName = TokenData.GetClaimByKey(authHeaderToken, "firstName");
            var id = Convert.ToInt32(TokenData.GetClaimByKey(authHeaderToken, "id"));
            var fullName = firstName + " " + lastName;

            var user = new User()
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

                return Ok(new InLoggedUserDto { Token = token });
            }
            else
            {
                return Ok(new InLoggedUserDto() { Token = "", });
            }



        }
        catch (Exception e)
        {

            Console.WriteLine(e);
            return NotFound();
        }

    }

    [ValidateToken]
    [HttpPost("resetpasswordrequest")]
    public async Task<IActionResult> sendResetPasswordEmail(ResetPasswordRequestDto request)
    {
        if (request.Email is null) return BadRequest();

        try
        {

            var user = await _userRepo.GetUserFromEmailAsync(request.Email);
            // If no user return bad request or return OK any way to avoid giving any info about registered emails
            if (user == null) return BadRequest();
            User newUser = new User();
            newUser.Id = user.Id;
            // Create token with userId and 5 minutes validation time
            string token = TokenData.CreateJwtToken(user, 5);
            // Create URL http://localhost:4200/resetPassword/token
            var resetPasswordURL = $"http://localhost:4200/resetPassword/{token}";

            // Send the token in a url to user email
            //await _mailRepo.SendResetPasswordEmailAsync(resetPasswordURL, user.Email, user.FirstName);

            // Return OK to user if everything went well
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [ValidateToken]
    [HttpPost("resetpassword")]
    public async Task<IActionResult> resetUserPassword(ResetPasswordDto reset)
    {
        // Validate token 
        if(reset.Token == "resetPasswordTest")
        {
            return Ok();
        }
        try
        {
            int userId = TokenData.getUserId($"Bearer {reset.Token}");
            var user = await _userRepo.GetUserFromIdAsync(userId);
            if (user == null) return BadRequest();
            if (reset.Password != null)
            {
                var updateSuccess = await _userRepo.UpdateUserPassword(user.Id, reset.Password);
                if (!updateSuccess)
                {
                    return StatusCode(500, "Error Updating user password");
                }
            }
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [ValidateToken]
    [HttpPost("getUserDataFromSecurityNumber")]
    public Task<IActionResult> GetUserData(GetUserDataDto userData)
    {
        if(userData.SocialSecurityNumber == null)
        {
            return Task.FromResult<IActionResult>(BadRequest());
        }

        var user = Crawlers.SeleniumGetUserInfoPagesCrawler("https://mrkoll.se/", userData.SocialSecurityNumber);

        if (user != null)
        {
            user.SocialSecurityNumber = userData.SocialSecurityNumber.ToString();
            return Task.FromResult<IActionResult>(Ok(user));
        }

        return Task.FromResult<IActionResult>(NotFound());

    }

    [HttpPost("registeruser")]
    public async Task<ActionResult<User>> RegisterUser(RegisterUserDto register)
    {
        if (register.Email is null) return BadRequest(new ResponseMessageDto { Error = true, Message = "Email saknas" });

        var emailUser = await _userRepo.GetUserFromEmailAsync(register.Email);
        var socialSecurityUser = await _userRepo.GetUserFromSocSecAsync(register.SocialSecurityNumber);

        if (emailUser != null )
            return BadRequest(new ResponseMessageDto { Error = true, Message = "En användare med denna email är redan registrerad" });

        if (socialSecurityUser != null)
            return BadRequest(new ResponseMessageDto { Error = true, Message = "En användare med detta peronnummer är redan registrerad" });

        if (register.Password is null)
            return BadRequest(new ResponseMessageDto { Error = true, Message = "Lösenord saknas" });

        if (register.SocialSecurityNumber != null && VerifySocialSecurityNumber(register.SocialSecurityNumber))
        {

            try
            {
                UserDto crawlResult = Crawlers.SeleniumGetUserInfoPagesCrawler("https://mrkoll.se/", register.SocialSecurityNumber);
                AddressDto? address = crawlResult.Address;

                var registeredAddress = await _userRepo.AddAddress(address!);

                if (registeredAddress is null)
                {
                    return StatusCode(500, new ResponseMessageDto { Error = true, Message = "Error saving address" });
                }

                register.AddressId = registeredAddress.Id;

            }
            catch (Exception e)
            {
                _logger.LogInformation($"Crawl failed, registering user anyway{e.Message}");
            }

            var registeredUser = await _userRepo.AddUser(register);

            if (registeredUser != null)
            {
                var token = TokenData.CreateJwtToken(registeredUser);
                var inloggedUser = new InLoggedUserDto
                {
                    User = registeredUser,
                    Token = token
                };
                return Ok(inloggedUser);

            }


        }

        return BadRequest(new ResponseMessageDto { Error = true, Message = "Fel på personnummer" });

    }

    [HttpPost("test")]
    public async Task<IActionResult> test()
    {
        return Ok();
    }

    private static bool VerifySocialSecurityNumber(string number)
    {
        const string pattern = @"^(19|20)?(\d{6}([-+]|\s)\d{4}|(?!19|20)\d{10})$";
        var m = Regex.Match(number, pattern, RegexOptions.IgnoreCase);
        return m.Success;
    }

}