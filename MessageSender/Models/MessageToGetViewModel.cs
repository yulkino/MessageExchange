using System.ComponentModel.DataAnnotations;

namespace MessageSender.Models;

public class MessageToGetViewModel
{
    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public Guid SerialNumber { get; set; }
}
