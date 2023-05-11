using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class Question
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime? Date { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("ConversationId")]
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
} 