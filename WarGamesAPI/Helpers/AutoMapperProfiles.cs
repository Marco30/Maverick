using AutoMapper;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;
#pragma warning disable CS8602

namespace WarGamesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Question, QuestionDto>().ForMember(dest => dest.QuestionText, options => options.MapFrom(src => src.Text));
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
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.Municipality))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));
        CreateMap<Conversation, ConversationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));
            
       



    }
}


