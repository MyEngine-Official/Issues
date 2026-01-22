using Issue.Models;

namespace Issue.Services;

public class NotificationService
{
    public IEnumerable<(int notificationId, DateTime scheduleTime, string title, string body, bool withAlarm)> BuildNotifications(TaskItem task)
    {
        var now = DateTimeOffset.Now.LocalDateTime;
        var index = 0;
        foreach (var notification in task.GetNotifications())
        {
            var scheduled = notification.GetScheduledTime(task.DueDateTime);
            if (scheduled <= now)
            {
                index++;
                continue;
            }

            var notificationId = BuildNotificationId(task, index);
            yield return (notificationId, scheduled, task.Title, notification.Label, task.AlarmEnabled);
            index++;
        }
    }

    public IEnumerable<int> BuildNotificationIds(TaskItem task)
    {
        var index = 0;
        foreach (var _ in task.GetNotifications())
        {
            yield return BuildNotificationId(task, index);
            index++;
        }
    }

    private static int BuildNotificationId(TaskItem task, int index)
    {
        unchecked
        {
            var hash = task.Id.GetHashCode();
            return (hash * 397) ^ index;
        }
    }
}
