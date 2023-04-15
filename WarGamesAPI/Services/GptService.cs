using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;

#pragma warning disable CS1998

namespace WarGamesAPI.Services;

public class GptService : IGptService
{
    readonly ILogger<GptService> _logger;

    public GptService(ILogger<GptService> logger)
    {
        _logger = logger;
    }

    public async Task<AnswerDto?> AskQuestion(QuestionDto question)
    {
        var answer = await GenerateAnswer();

        return new AnswerDto
        {
            QuestionId = question.Id, 
            Text = answer, Date = DateTime.Now
        };

    }


    private async Task<string> GenerateAnswer()
    {
        return "I am ChatGPT, a large language model developed by OpenAI using the GPT-3.5 architecture. As a language model, my primary function is to generate natural language responses to text-based inputs such as questions, prompts, and statements.\r\n\r\nI was trained on a massive dataset of written text from the internet and other sources, which allows me to understand and generate responses in a wide variety of topics and domains. My training data includes text in multiple languages, making me capable of generating responses in several languages.\r\n\r\nI use machine learning techniques such as deep neural networks to analyze and understand the structure and context of the input text. Based on this analysis, I generate a response that is meant to be natural-sounding and relevant to the input.\r\n\r\nOverall, my purpose is to assist users in generating high-quality, natural language responses to a wide range of prompts and queries, making communication and information retrieval more efficient and effective.";
    }
}