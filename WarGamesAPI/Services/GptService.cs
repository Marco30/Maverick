using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace WarGamesAPI.Services;

public class GptService : IGptService
{
    readonly ILogger<GptService> _logger;
    readonly IQuestionRepository _questionRepo;
    readonly string? _apiKey;

    public GptService(ILoggerFactory loggerFactory, IConfiguration configuration, IQuestionRepository questionRepo)
    {
        _logger = loggerFactory.CreateLogger<GptService>();
        _questionRepo = questionRepo;
        _apiKey = configuration["openAiKey"];
    }


    public async Task<AnswerDto?> AskQuestion(QuestionDto question, bool mockReply)
    {

        var request = await GenerateRequestAsync(question);

        var answer = "";

        if (mockReply)
        {
            answer = GetMockReply();
        }
        else
        {
            answer = await SendRequest(request);
        }


        return new AnswerDto
        {
            QuestionId = question.Id, 
            Text = answer, Date = DateTime.Now
        };

    }

    public string? Result { get; set; }


    private async Task<OpenAIRequest> GenerateRequestAsync(QuestionDto question)
    {

        var conversationQuestions = await _questionRepo.GetQuestionsFromConversationAsync(question.ConversationId);
        List<AnswerDto> conversationAnswers = await _questionRepo.GetAnswersFromConversationAsync(question.ConversationId);


        //var messages = new List<Message> { new() { Role = "system", Content = "You are a helpful but angry and rude assistant" } };

        var messages = new List<Message> { new() { Role = "system", Content = "You are a helpful assistant for software developers" } };

        //var messages = new List<Message> { new() { Role = "system", Content = "You are a helpful but shy assistant " } };




        foreach (var q in conversationQuestions)
        {
            var questionMessage = new Message { Role = "user", Content = q.Text };
            messages.Add(questionMessage);
            var answers = conversationAnswers.Where(a => a.QuestionId == q.Id);
            
            foreach (var answer in answers)
            {
                if (answer.ConversationId == q.ConversationId)
                {
                    var answerMessage = new Message { Role = "assistant", Content = answer.Text };
                    messages.Add(answerMessage);
                }
            }
        }
        return new OpenAIRequest { Model = "gpt-3.5-turbo", Messages = messages };

        
    }

    private async Task<string?> SendRequest(OpenAIRequest request)
    {
        try
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var json = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", stringContent);
            
            
            
            var jsonResult = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponse>(jsonResult);
                if (errorResponse != null)
                    throw new Exception(errorResponse.Error.Message);
                else
                    throw new Exception("Error generating answer");
            }

            var data = JsonSerializer.Deserialize<Root>(jsonResult);

            if (data != null)
            {
                if (data.usage != null)
                {
                    //_logger.LogInformation("GptServiceLog - completion tokens: {CompletionTokens}, prompt tokens: {PromptTokens}, total tokens: {TotalTokens}",
                    //    data.usage.completion_tokens, data.usage.prompt_tokens, data.usage.total_tokens);
                }



                if (data.choices is null ) throw new Exception("Error generating answer");


                /*
                 * REQUEST RESPONSE
                 */
                //string id = data.id;
                //string @object = data.@object;
                //int created = data.created;
                //string model = data.model;  // gpt-3.5-turbo-0301

                //int promptTokens = data.usage.prompt_tokens;    // tokens used generating the prompt
                //int completionTokens = data.usage.completion_tokens;    // tokens used generating answer
                //int totalTokens = data.usage.total_tokens;
                

                //Choice choice = data.choices[0];
                //string finishReason = choice.finish_reason;
                //int index = choice.index;
                //Message message = choice.Message;
                //string role = message.Role;         // 'assistant' or 'user', except for the first message in a conversation: 'system'
                //string content = message.Content;   // this is the answer text


                return data.choices[0].Message.Content;

            }

            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while making the OpenAI API request");
            Result = $"An error occurred: {e.Message}";
        }

        return null;
    }

    private string GetMockReply()
    {
        return "I am ChatGPT, a large language model developed by OpenAI using the GPT-3.5 architecture. " +
               "As a language model, my primary function is to generate natural language responses to text-based " +
               "inputs such as questions, prompts, and statements.\r\n\r\nI was trained on a massive dataset of " +
               "written text from the internet and other sources, which allows me to understand and generate " +
               "responses in a wide variety of topics and domains. My training data includes text in multiple " +
               "languages, making me capable of generating responses in several languages.\r\n\r\nI use machine " +
               "learning techniques such as deep neural networks to analyze and understand the structure and " +
               "context of the input text. Based on this analysis, I generate a response that is meant to be " +
               "natural-sounding and relevant to the input.\r\n\r\nOverall, my purpose is to assist users in " +
               "generating high-quality, natural language responses to a wide range of prompts and queries, " +
               "making communication and information retrieval more efficient and effective.";
    }
    
}


public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    [JsonPropertyName("messages")]
    public List<Message>? Messages { get; set; }

}

public class Message
{
    [JsonPropertyName("role")]
    public string? Role { get; set; }
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}



public class Choice
{
    
    [JsonPropertyName("message")]
    public Message? Message { get; set; }

    [JsonPropertyName("index")]
    public int index { get; set; }

    [JsonPropertyName("finish_reason")]
    public string? finish_reason { get; set; }
}
public class Root
{
    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("object")]
    public string? @object { get; set; }

    [JsonPropertyName("created")]
    public int created { get; set; }

    [JsonPropertyName("model")] 
    public string? model { get; set; }

    [JsonPropertyName("choices")]
    public List<Choice>? choices { get; set; }

    [JsonPropertyName("usage")]
    public Usage? usage { get; set; }
}

/*
 * $$$$$$$$$
 */
public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}



public class OpenAIErrorResponse
{
    [JsonPropertyName("error")]
    public OpenAIError? Error { get; set; }
}
public class OpenAIError
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("param")]
    public string? Param { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }
}