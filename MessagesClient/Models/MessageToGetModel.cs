using System.ComponentModel.DataAnnotations;

namespace MessagesClient.Models;

public class MessageToGetModel
{
    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public Guid SerialNumber { get; set; }

    public override string ToString() =>
        $"Message: {Message}; Timestamp: {Timestamp}; (SerialNumber: {SerialNumber})";
}
