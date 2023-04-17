using WarGamesAPI.DTO;

namespace WarGamesAPI.Interfaces;

public interface IGptService
{
    Task<AnswerDto?> AskQuestion(QuestionDto question, bool mockReply);
}