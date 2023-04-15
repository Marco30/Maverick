namespace WarGamesAPI.DTO;

public class ConversationDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();

    public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();




}