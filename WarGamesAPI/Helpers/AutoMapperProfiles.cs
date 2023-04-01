using AutoMapper;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>();
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserDto, User>();

    }
}

