namespace Issue.Services;

public class AndroidNotificationManager : INotificationManager
{
    private readonly IAlarmService _alarmService;

    public AndroidNotificationManager(IAlarmService alarmService)
    {
        _alarmService = alarmService;
    }

    public void ScheduleNotification(int notificationId, DateTime scheduledTime, string title, string message, bool withAlarm)
    {
        _alarmService.ScheduleAlarm(notificationId, scheduledTime, title, message, withAlarm);
    }

    public void CancelNotification(int notificationId)
    {
        _alarmService.CancelAlarm(notificationId);
    }
}
