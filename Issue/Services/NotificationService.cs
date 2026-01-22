using Issue.Models;

namespace Issue.Services;

public class NotificationService
{
    public IEnumerable<(DateTime scheduleTime, string title, string body, bool withAlarm)> BuildNotifications(TaskItem task)
    {
        var now = DateTimeOffset.Now.LocalDateTime;
        foreach (var notification in task.GetNotifications())
        {
            var scheduled = notification.GetScheduledTime(task.DueDateTime);
            if (scheduled <= now)
            {
                continue;
            }

            yield return (scheduled, task.Title, notification.Label, task.AlarmEnabled);
        }
    }
}
