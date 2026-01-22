namespace Issue.Services;

public interface IAlarmService
{
    void ScheduleAlarm(DateTime scheduledTime, string title, string message, bool withAlarm);
    void CancelAlarm(int requestCode);
}
