namespace Issue;

using Issue.Services;
using Issue.ViewModels;
using Issue.Views;
#if ANDROID
using Issue.Platforms.Android.Services;
#endif

namespace Issue;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<TaskService>();
		builder.Services.AddSingleton<NotificationService>();
		builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
		builder.Services.AddSingleton<TaskListViewModel>();
		builder.Services.AddTransient<TaskDetailViewModel>();

#if ANDROID
		builder.Services.AddSingleton<IAlarmService, AlarmService>();
		builder.Services.AddSingleton<INotificationManager, AndroidNotificationManager>();
#else
		builder.Services.AddSingleton<INotificationManager, PlatformNotificationManager>();
#endif

		builder.Services.AddTransient<TaskListPage>();
		builder.Services.AddTransient<TaskDetailPage>();
		builder.Services.AddTransient<NotificationSettingsPage>();

		return builder.Build();
	}
}
