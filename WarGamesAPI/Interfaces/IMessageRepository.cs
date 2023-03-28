using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IMessageRepository
{
    Task<Message?> SaveMessage(Message message);

}



