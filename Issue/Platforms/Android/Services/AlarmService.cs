using Android.App;
using Android.Content;
using Issue.Services;

namespace Issue.Platforms.Android.Services;

public class AlarmService : IAlarmService
{
    public void ScheduleAlarm(int requestCode, DateTime scheduledTime, string title, string message, bool withAlarm)
    {
        var context = Application.Context;
        var intent = new Intent(context, typeof(AlarmReceiver));
        intent.PutExtra(AlarmReceiver.TitleKey, title);
        intent.PutExtra(AlarmReceiver.MessageKey, message);
        intent.PutExtra(AlarmReceiver.WithAlarmKey, withAlarm);

        var pendingIntent = PendingIntent.GetBroadcast(context, requestCode, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
        var triggerAtMillis = new DateTimeOffset(scheduledTime).ToUnixTimeMilliseconds();

        var alarmManager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        alarmManager?.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
    }

    public void CancelAlarm(int requestCode)
    {
        var context = Application.Context;
        var intent = new Intent(context, typeof(AlarmReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(context, requestCode, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        var alarmManager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        alarmManager?.Cancel(pendingIntent);
    }
}
