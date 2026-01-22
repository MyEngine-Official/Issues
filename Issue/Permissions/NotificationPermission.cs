using System.Threading.Tasks;

namespace Issue.Permissions;

public class NotificationPermission : Permissions.BasePlatformPermission
{
#if ANDROID
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (global::Android.Manifest.Permission.PostNotifications, true)
    ];
#endif
}
