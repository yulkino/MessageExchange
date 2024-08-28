using AutoMapper;
using MessageExchange.DAO;
using MessageExchange.DTO;

namespace MessageExchange.MappingProfiles;

public class MessageDtoMappingProfile : Profile
{
    public MessageDtoMappingProfile()
    {
        CreateMap<MessageToSendDto, MessageDao>();
    }
}
