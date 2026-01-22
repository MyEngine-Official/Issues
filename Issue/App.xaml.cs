using Microsoft.Maui.ApplicationModel;
using NotificationPermission = Issue.Permissions.NotificationPermission;

namespace Issue;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override async void OnStart()
	{
		base.OnStart();
#if ANDROID
		var status = await Permissions.CheckStatusAsync<NotificationPermission>();
		if (status != PermissionStatus.Granted)
		{
			await Permissions.RequestAsync<NotificationPermission>();
		}
#endif
	}
}
