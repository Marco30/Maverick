using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;
using WarGamesAPIAPI.JsonCRUD;

#pragma warning disable CS1998

namespace Courses.Api.Repositories;

public class MessageRepository : IMessageRepository
{
   
    public async Task<Message?> SaveMessage(Message message)
    {
        var random = new Random();
        message.Id = random.Next();
        Json.CheckAndAddDataToJson("Message", message);
        List<Message> allMessages = Json.GetJsonData<Message>("Message");
        var confirmedMessage = allMessages.FirstOrDefault(m => m.Id == message.Id);
        return confirmedMessage ?? null;
    }


}