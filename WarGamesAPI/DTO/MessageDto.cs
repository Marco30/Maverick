namespace WarGamesAPI.DTO;

public class MessageDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ConversationId { get; set; }
    public string? Text { get; set; }

}