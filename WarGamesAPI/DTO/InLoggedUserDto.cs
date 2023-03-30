using WarGamesAPI.Model;

namespace WarGamesAPI.DTO;

public class InLoggedUserDto
{
    public User? User { get; set; }
    public string? Token { get; set; }
}