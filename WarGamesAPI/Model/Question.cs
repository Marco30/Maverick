using System.ComponentModel.DataAnnotations.Schema;
using WarGamesAPIAPI.JsonCRUD;

namespace WarGamesAPI.Model;

public class Question : Json.IGenericIdInterface<Question>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("ConversationId")]
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();


}