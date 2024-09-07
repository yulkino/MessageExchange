using SimpleOptions;

namespace MessagesClient.Options;

public sealed class MessageExchangeOptions : ConfigurationOptions<MessageExchangeOptions>
{
    public string BaseAddress { get; set; } = string.Empty;
}
