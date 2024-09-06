using System.ComponentModel.DataAnnotations;

namespace MessageClients.Models;

public class MessageToSendModel
{
    [Required, MaxLength(128)]
    public string Message { get; set; }

    public Guid SerialNumber { get; } = Guid.NewGuid();

    public override string ToString() =>
        $"Message: {Message}; (SerialNumber: {SerialNumber})";
}

