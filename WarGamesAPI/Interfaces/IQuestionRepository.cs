using WarGamesAPI.DTO;

namespace WarGamesAPI.Interfaces;

public interface IQuestionRepository
{
    Task<QuestionDto?> SaveQuestionAsync(QuestionDto userQuestion);
    Task<AnswerDto?> SaveAnswerAsync(AnswerDto answer);
    Task<List<QuestionDto>> GetUserQuestionsAsync(int userId);
    Task<QuestionDto?> GetQuestionAsync(int questionId);
    Task<AnswerDto?> GetAnswerAsync(int answerId);
    Task<ConversationDto?> GetConversationAsync(int conversationId);
    Task<List<AnswerDto>> GetAnswersAsync(int questionId);
    Task<bool> ConversationExists(int conversationId);
    Task<int> GetConversationUserId(int conversationId);
    Task<List<ConversationDto?>> GetConversationsAsync(int userId);
    

    Task DeleteQuestionAsync(int questionId);
    Task DeleteAnswerAsync(int answerId);
    Task DeleteConversationAsync(int conversationId);
    Task<List<QuestionDto>> GetQuestionsFromConversationAsync(int conversationId);
    Task<List<AnswerDto>> GetAnswersFromConversationAsync(int conversationId);


}



