using WarGamesAPI.DTO;

namespace WarGamesAPI.Interfaces;

public interface IQuestionRepository
{
    Task<QuestionDto?> SaveQuestionAsync(AskQuestionDto userQuestion);
    Task<AnswerDto?> SaveAnswerAsync(AnswerDto answer);
    Task<List<QuestionDto>> GetUserQuestionsAsync(int userId);
    Task<QuestionDto?> GetQuestionAsync(int questionId);
    Task<AnswerDto?> GetAnswerAsync(int answerId);
    Task<List<AnswerDto>> GetAnswersAsync(int questionId);

    Task DeleteQuestionAsync(int questionId);
    Task DeleteAnswerAsync(int answerId);
    Task DeleteConversationAsync(int conversationId);


}



