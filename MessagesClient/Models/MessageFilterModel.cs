namespace MessagesClient.Models;

public class MessageFilterModel
{
    public int? TimeRange { get; set; }
    public DateTime? DateTimeFrom { get; set; }
    public DateTime? DateTimeTo { get; set; }

    public override string ToString() => 
        $"TimeRange: {TimeRange}; DateTimeFrom: {DateTimeFrom}; DateTimeTo: {DateTimeTo}";
}
