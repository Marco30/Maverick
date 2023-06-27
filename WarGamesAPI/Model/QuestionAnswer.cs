﻿using System.Text.Json.Serialization;

namespace WarGamesAPI.Model;

public class QuestionAnswer
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

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
