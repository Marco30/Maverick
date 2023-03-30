using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;

#pragma warning disable CS1998

namespace WarGamesAPI.Services;

public class QuestionService : IQuestionService
{
    readonly IQuestionRepository _messageRepo;

    public QuestionService(IQuestionRepository messageRepo)
    {
        _messageRepo = messageRepo;
    }

    public async Task<Answer?> AskQuestion(Question question)
    {
        var answer = await GenerateAnswer();
        var answerToSave = new Answer
        {
            QuestionId = question.Id, 
            Text = answer, Time = DateTime.Now
        };

        return await _messageRepo.SaveAnswer(answerToSave);
    }


    private async Task<string> GenerateAnswer()
    {
        return "42";
    }
}