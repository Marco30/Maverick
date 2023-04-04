namespace WarGamesAPI.DTO;

public class AnswerDto
{
    public int Id { get; set; }
    public string? AnswerText { get; set; }
    public DateTime Time { get; set; }
    public int QuestionId { get; set; }
    public int ConversationId { get; set; }
}