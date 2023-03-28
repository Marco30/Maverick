using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IQuestionService
{
    Task<Message?> AskQuestion(QuestionDto question);
}