using System.ComponentModel.DataAnnotations;

namespace MessagesClient.Models;

public class MessageToSendModel
{
    [Required, MaxLength(128)]
    public string Message { get; set; } = string.Empty;

    public Guid SerialNumber { get; } = Guid.NewGuid();

    public override string ToString() =>
        $"Message: {Message}; (SerialNumber: {SerialNumber})";
}
