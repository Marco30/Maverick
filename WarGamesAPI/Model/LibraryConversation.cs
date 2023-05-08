using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class LibraryConversation
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime Updated { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User? User { get; set; }

    public ICollection<LibraryQuestion> LibraryQuestions { get; set; } = new List<LibraryQuestion>();
    public ICollection<LibraryAnswer> LibraryAnswers { get; set; } = new List<LibraryAnswer>();


}