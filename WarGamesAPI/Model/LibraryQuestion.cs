using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class LibraryQuestion
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("LibraryConversationId")]
    public int LibraryConversationId { get; set; }
    public LibraryConversation? LibraryConversation { get; set; }

    public ICollection<LibraryAnswer> LibraryAnswers { get; set; } = new List<LibraryAnswer>();

    


}