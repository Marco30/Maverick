namespace WarGamesAPI.Interfaces;

public interface IValidationRepository
{
    Task<bool> UserOwnsQuestion(int userId, int questionId);
    Task<bool> UserOwnsAnswer(int userId, int answerId);
    Task<bool> UserOwnsConversation(int userId, int conversationId);


}



