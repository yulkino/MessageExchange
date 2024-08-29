using System.ComponentModel.DataAnnotations;

namespace MessageClients.Models;

public class MessageToSendViewModel
{
    [Required]
    [StringLength(128, ErrorMessage = "The message must not exceed 128 characters.")]
    public string Content { get; set; }

    [Required]
    public Guid SerialNumber { get; set; }
}

