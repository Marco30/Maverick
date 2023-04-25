using WarGamesAPI.DTO;

namespace WarGamesAPI.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetUserFromEmailAsync(string email);
    Task<UserDto?> GetUserFromIdAsync(int id);
    Task<bool> UpdateUserPassword(int userId, string password);
    Task<UserDto?> AddUser(RegisterUserDto register);
    Task<UserDto?> GetUserFromSocSecAsync(string socSecNr);
    Task UpdateUserAsync(int userId, UpdateUserDto updateUser);
    Task DeleteUserAsync(int userId);



}



