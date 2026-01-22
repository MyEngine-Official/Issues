namespace Issue.Services;

public interface INotificationManager
{
    void ScheduleNotification(DateTime scheduledTime, string title, string message, bool withAlarm);
}
