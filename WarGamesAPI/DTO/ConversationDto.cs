namespace WarGamesAPI.DTO;

public class ConversationDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
}