using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;
#pragma warning disable CS1998

namespace WarGamesAPI.Services;

public class QuestionService : IQuestionService
{
    readonly IMessageRepository _messageRepo;

    public QuestionService(IMessageRepository messageRepo)
    {
        _messageRepo = messageRepo;
    }

    public async Task<Message?> AskQuestion(QuestionDto question)
    {
        var answer = await GenerateAnswer();
        var messageToSave = new Message { Question = question.Question, Answer = answer };
        return await _messageRepo.SaveMessage(messageToSave);
    }


    private async Task<string> GenerateAnswer()
    {
        return "42";
    }
}