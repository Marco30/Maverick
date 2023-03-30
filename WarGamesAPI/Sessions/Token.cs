using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WarGamesAPI.Sessions;

public sealed class Token
{
    public string? ActOnBehalfOfName { get; set; }
    public string? ActOnBehalfOfId { get; set; }
    public IEnumerable<string?> WarrantTypes { get; set; }
    //public RoleType Role { get; set; }
    public string? PersonalNumber { get; set; }
    public string? Surname { get; set; }
    public string? GivenName { get; set; }
    public string? DisplayName { get; set; }
    public IEnumerable<Claim>? Claims { get; set; }
    public string? AuthToken { get; set; }
    public JwtSecurityToken? InnerToken { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

}