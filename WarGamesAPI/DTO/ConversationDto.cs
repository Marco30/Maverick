namespace WarGamesAPI.DTO;

public class ConversationDto
{
    public int UserId { get; set; }
    public List<QAItemDto> Conversation { get; set; } = new();
}