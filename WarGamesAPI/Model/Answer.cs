using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class Answer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Time { get; set; }

    [ForeignKey("QuestionId")]
    public int QuestionId { get; set; }
    public Question? Question { get; set; }

    [ForeignKey("ConversationId")]
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
}