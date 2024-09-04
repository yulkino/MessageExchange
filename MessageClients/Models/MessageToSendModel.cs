using System.ComponentModel.DataAnnotations;

namespace MessageClients.Models;

public class MessageToSendModel
{
    [Required]
    public string Message { get; set; }

    public Guid SerialNumber { get; } = Guid.NewGuid();
}

