using AutoMapper;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>().ForMember(dest => dest.AnswerText, options => options.MapFrom(src => src.Text));
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserDto, User>().ForMember(dest => dest.FullName, options => options.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)));
        CreateMap<AddressDto, Address>();
        CreateMap<Address, AddressDto>();
        CreateMap<Conversation, ConversationDto>();


    }
}

