namespace Issue.Models;

public class NotificationOffset
{
    public TimeSpan Offset { get; set; }
    public string Label { get; set; } = "Notificación";

    public DateTime GetScheduledTime(DateTime dueDateTime)
    {
        return dueDateTime - Offset;
    }
}
