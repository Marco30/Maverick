using AutoMapper;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;
#pragma warning disable CS8602

namespace WarGamesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<AskQuestionDto, QuestionDto>();
        CreateMap<Answer, AnswerDto>().ForMember(dest => dest.AnswerText, options => options.MapFrom(src => src.Text));
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserDto, User>().ForMember(dest => dest.FullName, options => options.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)));
        CreateMap<RegisterAddressDto, Address>();
        CreateMap<AddressDto, Address>();
        CreateMap<Address, AddressDto>();
        CreateMap<Conversation, ConversationDto>();
        CreateMap<UserDto, UserDataDto>();
        CreateMap<UserDto, UserDataDto>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => src.Address.Municipality));
    }
}


