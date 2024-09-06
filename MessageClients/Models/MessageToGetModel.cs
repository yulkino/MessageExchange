using System.ComponentModel.DataAnnotations;

namespace MessageClients.Models;

public class MessageToGetModel
{
    [Required]
    public string Message { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public Guid SerialNumber { get; set; }

    public override string ToString() =>
        $"Message: {Message}; Timestamp: {Timestamp}; (SerialNumber: {SerialNumber})";
}
