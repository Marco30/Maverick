
namespace WarGamesAPI.DTO;

public class RegisterUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? SocialSecurityNumber { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool AgreeMarketing { get; set; }
    public bool SubscribeToEmailNotification { get; set; }
    public Genders Gender { get; set; }
    public AddressDto? Address { get; set; }
    public int? PhoneNumber { get; set; }
    public int? MobilePhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public int? AddressId { get; set; }



}