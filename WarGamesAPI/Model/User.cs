using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public string? Gender { get; set; }

    public Address? Address { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    public ICollection<LibraryConversation> LibraryConversations { get; set; } = new List<LibraryConversation>();


}



