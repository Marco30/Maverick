using System.Net.Http.Headers;
using System.Text;
using AI.Dev.OpenAI.GPT;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace WarGamesAPI.Services;

public class GptService : IGptService
{
    readonly ILogger<GptService> _logger;
    readonly IQuestionRepository _questionRepo;
    readonly string? _apiKey;
    private static string _apiEndpoint => "https://api.openai.com/v1/chat/completions";
    static double _tokenPriceInUSDcents => 0.0002;
    public string? Result { get; set; }

    public GptService(ILoggerFactory loggerFactory, IConfiguration configuration, IQuestionRepository questionRepo)
    {
        _logger = loggerFactory.CreateLogger<GptService>();
        _questionRepo = questionRepo;
        _apiKey = configuration["openAiKey"];
    }

    public async Task<AnswerDto?> AskQuestion(QuestionDto question, bool mockReply)
    {

        var request = await GenerateRequestAsync(question);

        string? answer;

        if (mockReply)
            answer = GetMockReply();
        else
            answer = await SendRequest(request);


        return new AnswerDto
        {
            QuestionId = question.Id,
            Text = answer,
            Date = DateTime.Now
        };

    }

    private async Task<OpenAIRequest> GenerateRequestAsync(QuestionDto question)
    {

        var conversationQuestions = await _questionRepo.GetQuestionsFromConversationAsync(question.ConversationId);
        List<AnswerDto> conversationAnswers = await _questionRepo.GetAnswersFromConversationAsync(question.ConversationId);


        //var messages = new List<Message> { new() { Role = "system", Content = "You are a helpful but angry and rude assistant" } };

        var messages = new List<Message> { new() { Role = "system", Content = "You are a helpful assistant for software developers" } };

        //var messages = new List<Message> { new() { Role = "system", Content = "You are a helpful but shy assistant " } };




        foreach (var q in conversationQuestions)
        {
            var messageText = "";

            if (q.Text != null) messageText = CleanMessageText(q.Text);
            var questionMessage = new Message { Role = "user", Content = messageText };

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

    private int EstimateTokenCount(List<Message> messages)
    {
        var tokenCount = 0;

        foreach (var message in messages)
        {
            if (message.Content != null)
            {
                List<int> tokens = GPT3Tokenizer.Encode(message.Content);
                tokenCount += tokens.Count;
            }
        }

        return tokenCount;
    }

    private string CleanMessageText(string messageText)
    {
        return string.Join(" ", messageText.Trim().Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries));
    }

    private void LogTokenUsage(Root data, List<Message> messages)
    {
            var estimatedTokenCount = EstimateTokenCount(messages);
            var numberOfMessages = messages.Count;
            var cost = _tokenPriceInUSDcents * data.TokenUsage!.total_tokens;

            _logger.LogInformation("Token usage: {@EstimatedTokenCount} {@NumberOfMessages} {@PromptTokens} {@CompletionTokens} {@TotalTokens} {@Cost}",
            estimatedTokenCount,
            numberOfMessages,
            data.TokenUsage!.prompt_tokens,
            data.TokenUsage.completion_tokens,
            data.TokenUsage.total_tokens,
            cost);

    }

    private async Task<string?> SendRequest(OpenAIRequest request)
    {
        if (request.Messages is null)
            throw new ArgumentException("Messages is null");

        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var jsonRequest = JsonSerializer.Serialize(request);
                var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_apiEndpoint, stringContent);
                var jsonResult = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponse>(jsonResult);
                    if (errorResponse != null && errorResponse.Error != null)
                        throw new GptApiException(errorResponse.Error.Message);
                    throw new GptApiException("Error generating answer");
                }

                Root? data = JsonSerializer.Deserialize<Root>(jsonResult);


                if (data is null) throw new InvalidOperationException("Error Deserializing Json");
                if (data.TokenUsage is null) throw new InvalidOperationException("TokenUsage is null");
                if (data.Choices is null) throw new InvalidOperationException("Error generating answer");

                LogTokenUsage(data, request.Messages);

                return data.Choices[0].Message?.Content ?? throw new InvalidOperationException("Error generating answer");
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















