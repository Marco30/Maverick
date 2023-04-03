using WarGamesAPIAPI.JsonCRUD;

namespace WarGamesAPI.Model;

public class User : Json.IGenericIdInterface<User>
{
    public int Id { get; set; }
    public string? SocialSecurityNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobilePhoneNumber { get; set; }
    public bool AgreeMarketing { get; set; }
    public bool SubscribeToEmailNotification { get; set; }
    public string? ProfileImage { get; set; }
    public int? AddressId { get; set; }
    public string? Gender { get; set; }
}



