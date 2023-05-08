using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Data;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;

namespace Courses.Api.Repositories;

public class UserRepository : IUserRepository
{
    readonly WarGamesContext _context;
    readonly IMapper _mapper;
    readonly IChatHistoryRepository _questionRepo;

    public UserRepository(WarGamesContext context, IMapper mapper, IChatHistoryRepository questionRepo)
    {
        _context = context;
        _mapper = mapper;
        _questionRepo = questionRepo;
    }
   
    public async Task<UserDto?> GetUserFromEmailAsync(string email)
    {
        return await _context.User.Where(u => u.Email == email)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public async Task<UserDto?> GetUserFromSocSecAsync(string socSecNr)
    {
        return await _context.User.Where(u => u.SocialSecurityNumber == socSecNr)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public async Task<UserDto?> GetUserFromIdAsync(int id)
    {
        return await _context.User.Where(u => u.Id == id)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.User.Where(u => u.Id == userId).SingleOrDefaultAsync();
        
        if (user is null)
        {
            throw new Exception("User not found");
        }

        var userConversations = await _context.Conversation.Where(c => c.UserId == userId).ToListAsync();

        foreach (var c in userConversations)
        {
            await _questionRepo.DeleteConversationAsync(c.Id);
        }

        _context.User.Remove(user);
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(int userId, UpdateUserDto updateUser)
    {
        var user = await _context.User.Where(u => u.Id == userId).SingleOrDefaultAsync();
        
        if (user is null)
        {
            throw new Exception("User not found");
        }

        _mapper.Map(updateUser, user);
        var address = await _context.Address.SingleOrDefaultAsync(a => a.UserId == userId);
        _mapper.Map(updateUser.Address, address);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> UpdateUserPassword(int userId, string password)
    {
        var user = await _context.User.SingleOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return false;
        }

        try
        {
            user.Password = password;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception("Error updating user password", e);
        }
    }

    public async Task<UserDto?> AddUser(RegisterUserDto register)
    {
        var user = _mapper.Map<User>(register);

        if (register.Address != null)
        {
            var addressToAdd = _mapper.Map<Address>(register.Address);
            user.Address = addressToAdd;
        }

        try
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception e)
        {
            throw new Exception("Error saving User", e);
        }
        
    }

   









}