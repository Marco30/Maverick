using Microsoft.EntityFrameworkCore;

namespace WarGamesAPI.Interfaces;

public interface IValidationRepository
{
    Task<bool> UserOwnsQuestion(int userId, int questionId);
    Task<bool> UserOwnsAnswer(int userId, int answerId);
    Task<bool> UserOwnsConversation(int userId, int conversationId);
    Task<bool> UserOwnsLibraryConversation(int userId, int conversationId);
    Task<bool> UserOwnsLibraryAnswer(int userId, int answerId);
    Task<bool> UserOwnsLibraryQuestion(int userId, int questionId);



}



