using AutoMapper;
using MessageExchange.DAO;
using MessageExchange.DTOs;

namespace MessageExchange.MappingProfiles;

public class MessageDaoMappingProfile : Profile
{
    public MessageDaoMappingProfile()
    {
        CreateMap<MessageDao, MessageToGetDto>();
    }
}
