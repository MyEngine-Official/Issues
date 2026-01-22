using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;

namespace Issue.Platforms.Android.Services;

[BroadcastReceiver(Enabled = true, Exported = false)]
public class AlarmReceiver : BroadcastReceiver
{
    public const string ChannelId = "issue_notifications";
    public const string TitleKey = "title";
    public const string MessageKey = "message";
    public const string WithAlarmKey = "with_alarm";
    public const string StopAction = "ISSUE_STOP_ALARM";
    private const int NotificationId = 1001;

    public override void OnReceive(Context context, Intent intent)
    {
        var title = intent.GetStringExtra(TitleKey) ?? "Tarea";
        var message = intent.GetStringExtra(MessageKey) ?? "Recordatorio";
        var withAlarm = intent.GetBooleanExtra(WithAlarmKey, false);

        CreateNotificationChannel(context);

        var stopIntent = new Intent(context, typeof(AlarmStopReceiver));
        stopIntent.SetAction(StopAction);

        var stopPendingIntent = PendingIntent.GetBroadcast(context, 0, stopIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        var notificationBuilder = new NotificationCompat.Builder(context, ChannelId)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetAutoCancel(true)
            .AddAction(Resource.Mipmap.appicon, "Detener alarma", stopPendingIntent);

        if (withAlarm)
        {
            notificationBuilder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
        }

        var notification = notificationBuilder.Build();

        var manager = NotificationManagerCompat.From(context);
        manager.Notify(NotificationId, notification);
    }

    private static void CreateNotificationChannel(Context context)
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        {
            return;
        }

        var channel = new NotificationChannel(ChannelId, "Issue", NotificationImportance.High)
        {
            Description = "Notificaciones de tareas"
        };

        var manager = (NotificationManager?)context.GetSystemService(Context.NotificationService);
        manager?.CreateNotificationChannel(channel);
    }
}
