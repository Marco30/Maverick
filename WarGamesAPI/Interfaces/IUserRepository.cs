using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserFromEmailAsync(string email);
    Task<User?> GetUserFromIdAsync(int id);
    Task<bool> UpdateUserPassword(int userId, string password);
    Task<Address?> AddAddress(AddressDto address);
    Task<User?> AddUser(RegisterUserDto register);
    Task<User?> GetUserFromSocSecAsync(string socSecNr);



}



