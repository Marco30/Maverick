using WarGamesAPIAPI.JsonCRUD;

namespace WarGamesAPI.Model;

public class Question : Json.IGenericIdInterface<Question>
{
    public int Id { get; set; }
    public string? Text { get; set; }

    public int? UserId { get; set; }

}