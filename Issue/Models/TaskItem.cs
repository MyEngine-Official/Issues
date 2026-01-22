namespace Issue.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDateTime { get; set; } = DateTime.Today;
    public bool AlarmEnabled { get; set; }
    public NotificationOffset PrimaryNotification { get; set; } = new();
    public NotificationOffset? SecondaryNotification { get; set; }
    public NotificationOffset? TertiaryNotification { get; set; }

    public IEnumerable<NotificationOffset> GetNotifications()
    {
        yield return PrimaryNotification;

        if (SecondaryNotification is not null)
        {
            yield return SecondaryNotification;
        }

        if (TertiaryNotification is not null)
        {
            yield return TertiaryNotification;
        }
    }
}
