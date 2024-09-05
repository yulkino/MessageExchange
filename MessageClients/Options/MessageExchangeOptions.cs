using SimpleOptions;

namespace MessageClients.Options;

public class MessageExchangeOptions : ConfigurationOptions<MessageExchangeOptions>
{
    public string BaseAddress { get; set; } = string.Empty;
}
