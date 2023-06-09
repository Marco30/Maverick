using System.Text.Json.Serialization;

namespace WarGamesAPI.Services.TypeSenseModel;

public class QuestionAnswerIndex
{
    [JsonPropertyName("id")] 
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string? Text { get; set; } = string.Empty;

    [JsonPropertyName("userid")]
    public int UserId { get; set; }

    [JsonPropertyName("conversationid")]
    public int ConversationId { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public int Timestamp { get; set; }
}