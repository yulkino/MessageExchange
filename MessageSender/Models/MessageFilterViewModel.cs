namespace MessageSender.Models;

public class MessageFilterViewModel
{
    public string TimeRange { get; set; }
    public DateTime? DateTimeFrom { get; set; }
    public DateTime? DateTimeTo { get; set; }

    public List<MessageToGetViewModel> Messages { get; set; }
}
