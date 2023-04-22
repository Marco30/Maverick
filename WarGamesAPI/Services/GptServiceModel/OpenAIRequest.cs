using System.Text.Json.Serialization;

/*
* REQUEST RESPONSE
*/

//string id = data.id;
//string @object = data.@object;
//int created = data.created;
//string model = data.model;  // gpt-3.5-turbo-0301

//int promptTokens = data.tokenUsage.prompt_tokens;    // tokens used generating the prompt
//int completionTokens = data.tokenUsage.completion_tokens;    // tokens used generating answer
//int totalTokens = data.tokenUsage.total_tokens;


//Choice choice = data.choices[0];
//string finishReason = choice.finish_reason;
//int index = choice.index;
//Message message = choice.Message;
//string role = message.Role;         // 'assistant' or 'user', except for the first message in a conversation: 'system'
//string content = message.Content;   // this is the answer text


public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
 
    [JsonPropertyName("messages")]
    public List<Message>? Messages { get; set; }

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

}