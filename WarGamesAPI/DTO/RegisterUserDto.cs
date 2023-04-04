namespace WarGamesAPI.DTO;

public class RegisterUserDto
{
    public string? SocialSecurityNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? AddressId { get; set; }
    public AddressDto? Address { get; set; }




}