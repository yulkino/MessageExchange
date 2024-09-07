using MessagesServer.DAOs;
using MessagesServer.DTOs;
using Riok.Mapperly.Abstractions;

namespace MessagesServer.Mapping;

[Mapper]
public static partial class MapperConfiguration
{
    [MapProperty(nameof(MessageDao.Content), nameof(MessageToGetDto.Message))]
    public static partial MessageToGetDto ToDto(this MessageDao dao);
    public static partial List<MessageToGetDto> ToDtos(this List<MessageDao> daos);

    [MapProperty(nameof(MessageToSendDto.Message), nameof(MessageDao.Content))]
    public static partial MessageDao ToDao(this MessageToSendDto dto);
}
