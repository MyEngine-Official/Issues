namespace Issue.Services;

public class PlatformNotificationManager : INotificationManager
{
    public void ScheduleNotification(int notificationId, DateTime scheduledTime, string title, string message, bool withAlarm)
    {
        // Placeholder for other platforms.
    }

    public void CancelNotification(int notificationId)
    {
        // Placeholder for other platforms.
    }
}
