namespace WarGamesAPI.DTO;

public class AnswerDto
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Time { get; set; }
    public int QuestionId { get; set; }
}