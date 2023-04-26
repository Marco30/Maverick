using AutoMapper;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;
#pragma warning disable CS8602

namespace WarGamesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Question, QuestionDto>().ForMember(dest => dest.Text, options => options.MapFrom(src => src.Text));
        CreateMap<AskQuestionDto, QuestionDto>();
        CreateMap<QuestionDto, MessageDto>();
        CreateMap<AnswerDto, MessageDto>();

        CreateMap<Answer, AnswerDto>().ForMember(dest => dest.Text, options => options.MapFrom(src => src.Text));
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserDto, User>()
            .ForMember(dest => dest.FullName, options => options.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)))
            .ForMember(dest => dest.Address, options => options.Ignore());
        CreateMap<RegisterAddressDto, Address>();
        CreateMap<AddressDto, Address>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));;
        CreateMap<Address, AddressDto>();
        CreateMap<Conversation, ConversationDto>();
        CreateMap<Conversation, ConversationInfoDto>();
        CreateMap<UserDto, UserDataDto>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.Municipality))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));




        
    }
}


