namespace Issue.Services;

public interface INotificationManager
{
    void ScheduleNotification(int notificationId, DateTime scheduledTime, string title, string message, bool withAlarm);
    void CancelNotification(int notificationId);
}
