namespace WarGamesAPI.DTO;

public class QuestionDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? AnswerId { get; set; }
    public string? Text { get; set; }
}