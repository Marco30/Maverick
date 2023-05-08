using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface ILibraryRepository
{
    Task<LibraryConversation?> CreateLibraryConversationAsync(int userId, string conversationName);


}



