using AutoMapper;
using MessageExchange.DAOs;
using MessageExchange.DTOs;

namespace MessageExchange.MappingProfiles;

public class MessageDtoMappingProfile : Profile
{
    public MessageDtoMappingProfile()
    {
        CreateMap<MessageToSendDto, MessageDao>()
            .ForMember(x => x.Content, o => o.MapFrom(x => x.Message));
    }
}
