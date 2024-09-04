namespace MessageClients.Models;

public class MessageFilterModel
{
    public int? TimeRange { get; set; }
    public DateTime? DateTimeFrom { get; set; }
    public DateTime? DateTimeTo { get; set; }
}
