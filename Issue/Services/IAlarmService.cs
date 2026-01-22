namespace Issue.Services;

public interface IAlarmService
{
    void ScheduleAlarm(int requestCode, DateTime scheduledTime, string title, string message, bool withAlarm);
    void CancelAlarm(int requestCode);
}
