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

    public UserRepository(WarGamesContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

        try
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception e)
        {
            throw new Exception("Error saving User", e);
        }
        
    }

    public async Task<int> AddAddress(RegisterAddressDto address)
    {
        try
        {
            var addressToAdd = _mapper.Map<Address>(address);
            await _context.Address.AddAsync(addressToAdd);
            await _context.SaveChangesAsync();
            return addressToAdd.Id;
        }
        catch (Exception e)
        {
            throw new Exception("Error saving Address", e);
        }
    }










}