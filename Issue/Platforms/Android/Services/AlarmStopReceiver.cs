using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace Issue.Platforms.Android.Services;

[BroadcastReceiver(Enabled = true, Exported = false)]
public class AlarmStopReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        var manager = NotificationManagerCompat.From(context);
        manager.CancelAll();
    }
}
