using AutoMapper;
using MessageExchange.DAOs;
using MessageExchange.DTOs;

namespace MessageExchange.MappingProfiles;

public class MessageDaoMappingProfile : Profile
{
    public MessageDaoMappingProfile()
    {
        CreateMap<MessageDao, MessageToGetDto>()
            .ForMember(x => x.Message, o => o.MapFrom(x => x.Content));
    }
}
