using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class LibraryAnswer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }

    [ForeignKey("LibraryQuestionId")]
    public int? LibraryQuestionId { get; set; }
    public LibraryQuestion? LibraryQuestion { get; set; }

    [ForeignKey("LibraryConversationId")]
    public int? LibraryConversationId { get; set; }
    public LibraryConversation? LibraryConversation { get; set; }
}