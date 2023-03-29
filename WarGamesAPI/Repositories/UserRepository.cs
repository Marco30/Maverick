using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;
using WarGamesAPIAPI.JsonCRUD;

#pragma warning disable CS1998

namespace Courses.Api.Repositories;

public class UserRepository : IUserRepository
{
   
    public async Task<User?> GetUserFromEmailAsync(string email)
    {
        var users = Json.GetJsonData<User>("User");
        return users.FirstOrDefault(u => u.Email?.ToLower() == email.ToLower());
    }

    public async Task<User?> GetUserFromIdAsync(int id)
    {
        var users = Json.GetJsonData<User>("User");
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<bool> UpdateUserPassword(int userId, string password)
    {
        try
        {
            var users = Json.GetJsonData<User>("User");
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Password = password;
                Json.EditAndAddDataToJson("User", user);

            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<User?> AddUser(RegisterUserDto register)
    {

        var user = new User
        {
            Id = new Random().Next(),
            Email = register.Email,
            Password = register.Password,
            FullName = register.FirstName + " " + register.LastName,
            SocialSecurityNumber = register.SocialSecurityNumber,
            FirstName = register.FirstName,
            Gender = register.Gender,
            LastName = register.LastName,
            AgreeMarketing = register.AgreeMarketing,
            SubscribeToEmailNotification = register.SubscribeToEmailNotification,
            AddressId = register.AddressId
        };

        Json.CheckAndAddDataToJson("User", user);
        List<User> allUsers = Json.GetJsonData<User>("User");
        var confirmedUser = allUsers.FirstOrDefault(u => u.Id == user.Id);
        return confirmedUser ?? null;
    }

    public async Task<Address?> AddAddress(AddressDto address)
    {
        Json.CheckAndAddDataToJson("Address", address);
        List<Address> allAddresses = Json.GetJsonData<Address>("Address");
        var confirmedAddress = allAddresses.FirstOrDefault(a => a.Id == address.Id);
        return confirmedAddress ?? null;
    }










}