using Issue.Services;

namespace Issue.Services;

public class AndroidNotificationManager : INotificationManager
{
    private readonly IAlarmService _alarmService;

    public AndroidNotificationManager(IAlarmService alarmService)
    {
        _alarmService = alarmService;
    }

    public void ScheduleNotification(DateTime scheduledTime, string title, string message, bool withAlarm)
    {
        _alarmService.ScheduleAlarm(scheduledTime, title, message, withAlarm);
    }
}
