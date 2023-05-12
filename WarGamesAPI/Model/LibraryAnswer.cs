using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class LibraryAnswer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }

    public int? ChatHistoryAnswerId { get; set; }
    public Answer? Answer { get; set; }

    [ForeignKey("LibraryQuestionId")]
    public int? LibraryQuestionId { get; set; }
    public LibraryQuestion? LibraryQuestion { get; set; }
    
}