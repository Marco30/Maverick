using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WarGamesAPI.Model;
using WarGamesAPI.Sessions;

namespace MiracleMileAPI.Sessions;

public class TokenData
{
    

    public static bool ValidateToken(string authToken)
    {
        var appSettings = GetAppSettings();

        authToken = CleanToken(authToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters(appSettings.JwtAudience, appSettings.JwtIssuer, appSettings.JWtSecretKey);

        SecurityToken validatedToken;
        IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
        return true;
    }

    public static int getUserId(string authorizaiton)
    {
        var id = Convert.ToInt32(TokenData.GetClaimByKey(authorizaiton, "id"));
        return id;
    }

    private static TokenValidationParameters GetValidationParameters(string audience, string issuer, string secretKey)
    {
        return new TokenValidationParameters()
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)) // The same key as the one that generate the token
        };
    }

    public static string CleanToken(string authToken)
    {
        var stream = authToken;
        if (authToken.Contains("Bearer "))
        {
            stream = authToken.Substring("Bearer ".Length).Trim();
        }


        return stream;

    }

    public static string GetClaimByKey(string authToken, string key)
    {
        var stream = "";

        var data = "";
        // authToken != null && authToken.StartsWith("Bearer")
        if (authToken != null && authToken.StartsWith("Bearer"))
        {
            //Extract credentials
            stream = CleanToken(authToken);

            var handler = new JwtSecurityTokenHandler();
            //var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

            var collection = tokenS.Claims;
            foreach (var item in collection)
            {
                if (item.Type == key)
                {
                    data = item.Value;
                }

                // do your stuff   
            }

        }
        else
        {
            //Handle what happens if that isn't the case
            throw new Exception("The authorization header is either empty or isn't Basic.");
        }

        return data;
    }

    public Token DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        //var jsonToken = handler.ReadToken(stream);
        var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        return new Token {
            ActOnBehalfOfName = tokenS.Actor,
            ActOnBehalfOfId = tokenS.Id,
            WarrantTypes = tokenS.Audiences,
            // Role = GetClaimByKey(token, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
            Claims = tokenS.Claims,

        };
    }

    public static AppSettings GetAppSettings()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();

        var appSettings = config.Providers.FirstOrDefault();

        string audience = "";
        string issuer = "";
        string secretKey = "";
        string tokenValidMinutes = "";
        string timeLeftReminderMinutes = "";

        appSettings.TryGet("AppSettings:JwtAudience", out audience);
        appSettings.TryGet("AppSettings:JwtIssuer", out issuer);
        appSettings.TryGet("AppSettings:JwtSecret", out secretKey);
        appSettings.TryGet("AppSettings:JWtTokenValidMinutes", out tokenValidMinutes);
        appSettings.TryGet("AppSettings:JWtTimeLeftReminderMinutes", out timeLeftReminderMinutes);
            
        int tokenValidMinutesInt = 0;
        int timeLeftReminderMinutesInt = 0;

        if (Int32.TryParse(tokenValidMinutes, out tokenValidMinutesInt) && Int32.TryParse(timeLeftReminderMinutes, out timeLeftReminderMinutesInt))
        {
            return new AppSettings
            {
                JWtTokenValidMinutes = tokenValidMinutesInt,
                JWtTimeLeftReminderMinutes = timeLeftReminderMinutesInt,
                JwtAudience = audience,
                JwtIssuer = issuer,
                JWtSecretKey = secretKey

            };
        }
        else
        {

            return new AppSettings
            {
                JWtTokenValidMinutes = 15,
                JWtTimeLeftReminderMinutes = 5,
                JwtAudience = audience,
                JwtIssuer = issuer,
                JWtSecretKey = secretKey

            };

        }

    }

    public static string CreateJwtToken(User user, int timeValidity = 0 )
    {
      

        var appSettings = GetAppSettings();


        var now = DateTime.Now;



        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.SocialSecurityNumber ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimTypes.Name, user.SocialSecurityNumber ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.SocialSecurityNumber ?? string.Empty),

            new Claim("issuer", appSettings.JwtIssuer ?? string.Empty),
            new Claim("audience", appSettings.JwtAudience ?? string.Empty),
            new Claim("fullname", user.FullName ?? string.Empty),
            new Claim("lastname", user.LastName ?? string.Empty),
            new Claim("firstname", user.FirstName ?? string.Empty),
            new Claim("id", user.Id.ToString() ?? string.Empty),
        };


        // Add act on behalf of self claim.
        claims.Add(new Claim(ClaimTypes.Role, "privatperson"));
        claims.Add(new Claim("http://schemas.danica.se/identity/claims/actonbehalfof", user.SocialSecurityNumber));

        int tokenTimeValidity = appSettings.JWtTokenValidMinutes;
        if (timeValidity > 0) tokenTimeValidity = timeValidity;


        var jwt = new JwtSecurityToken(
            issuer: appSettings.JwtIssuer,
            audience: appSettings.JwtAudience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(tokenTimeValidity),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JWtSecretKey)),
                SecurityAlgorithms.HmacSha256)
        );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }

}