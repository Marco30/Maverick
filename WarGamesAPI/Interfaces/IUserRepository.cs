using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetUserFromEmailAsync(string email);
    Task<UserDto?> GetUserFromIdAsync(int id);
    Task<bool> UpdateUserPassword(int userId, string password);
    Task<Address?> AddAddress(AddressDto address);
    Task<UserDto?> AddUser(RegisterUserDto register);
    Task<UserDto?> GetUserFromSocSecAsync(string socSecNr);



}



