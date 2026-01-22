using System.Windows.Input;
using Issue.Models;
using Issue.Services;

namespace Issue.ViewModels;

public class TaskDetailViewModel : ViewModelBase
{
    private readonly TaskService _taskService;
    private readonly NotificationService _notificationService;
    private readonly INotificationManager _notificationManager;
    private readonly INavigationService _navigationService;

    private TaskItem _task;
    private string _title;
    private string? _description;
    private DateTime _date;
    private TimeSpan _time;
    private bool _alarmEnabled;
    private bool _secondaryEnabled;
    private bool _tertiaryEnabled;
    private const int DefaultSecondaryValue = 10;
    private const int DefaultTertiaryValue = 1;
    private int _secondaryValue = DefaultSecondaryValue;
    private int _tertiaryValue = DefaultTertiaryValue;
    private int _secondaryUnitIndex;
    private int _tertiaryUnitIndex = 1;

    public TaskDetailViewModel(TaskService taskService, NotificationService notificationService, INotificationManager notificationManager, INavigationService navigationService)
    {
        _taskService = taskService;
        _notificationService = notificationService;
        _notificationManager = notificationManager;
        _navigationService = navigationService;
        _task = new TaskItem();

        _title = "";
        _description = "";
        _date = DateTime.Today;
        _time = TimeSpan.FromHours(9);

        SaveCommand = new Command(OnSave);
        DeleteCommand = new Command(OnDelete);
    }

    public TaskItem Task
    {
        get => _task;
        set
        {
            if (SetProperty(ref _task, value))
            {
                LoadTask(value);
            }
        }
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public TimeSpan Time
    {
        get => _time;
        set => SetProperty(ref _time, value);
    }

    public bool AlarmEnabled
    {
        get => _alarmEnabled;
        set => SetProperty(ref _alarmEnabled, value);
    }

    public bool SecondaryEnabled
    {
        get => _secondaryEnabled;
        set => SetProperty(ref _secondaryEnabled, value);
    }

    public bool TertiaryEnabled
    {
        get => _tertiaryEnabled;
        set => SetProperty(ref _tertiaryEnabled, value);
    }

    public int SecondaryValue
    {
        get => _secondaryValue;
        set => SetProperty(ref _secondaryValue, Math.Max(1, value));
    }

    public int TertiaryValue
    {
        get => _tertiaryValue;
        set => SetProperty(ref _tertiaryValue, Math.Max(1, value));
    }

    public int SecondaryUnitIndex
    {
        get => _secondaryUnitIndex;
        set => SetProperty(ref _secondaryUnitIndex, value);
    }

    public int TertiaryUnitIndex
    {
        get => _tertiaryUnitIndex;
        set => SetProperty(ref _tertiaryUnitIndex, value);
    }

    public IList<string> Units { get; } = new List<string> { "Minutos", "Horas", "Días" };

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public void InitializeForEdit(TaskItem task)
    {
        Task = task;
    }

    private void LoadTask(TaskItem task)
    {
        _title = task.Title;
        _description = task.Description;
        _date = task.DueDateTime.Date;
        _time = task.DueDateTime.TimeOfDay;
        _alarmEnabled = task.AlarmEnabled;
        _secondaryEnabled = false;
        _tertiaryEnabled = false;
        _secondaryValue = DefaultSecondaryValue;
        _tertiaryValue = DefaultTertiaryValue;
        _secondaryUnitIndex = 0;
        _tertiaryUnitIndex = 1;

        if (task.SecondaryNotification is not null)
        {
            _secondaryEnabled = true;
            _secondaryValue = Math.Max(1, (int)GetValueFromOffset(task.SecondaryNotification.Offset, out var unitIndex));
            _secondaryUnitIndex = unitIndex;
        }

        if (task.TertiaryNotification is not null)
        {
            _tertiaryEnabled = true;
            _tertiaryValue = Math.Max(1, (int)GetValueFromOffset(task.TertiaryNotification.Offset, out var unitIndex));
            _tertiaryUnitIndex = unitIndex;
        }

        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Description));
        OnPropertyChanged(nameof(Date));
        OnPropertyChanged(nameof(Time));
        OnPropertyChanged(nameof(AlarmEnabled));
        OnPropertyChanged(nameof(SecondaryEnabled));
        OnPropertyChanged(nameof(TertiaryEnabled));
        OnPropertyChanged(nameof(SecondaryValue));
        OnPropertyChanged(nameof(TertiaryValue));
        OnPropertyChanged(nameof(SecondaryUnitIndex));
        OnPropertyChanged(nameof(TertiaryUnitIndex));
    }

    private async void OnSave()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            return;
        }

        Task.Title = Title;
        Task.Description = Description;
        Task.DueDateTime = Date.Date + Time;
        Task.AlarmEnabled = AlarmEnabled;
        Task.PrimaryNotification = new NotificationOffset
        {
            Offset = TimeSpan.Zero,
            Label = "Notificación principal"
        };

        Task.SecondaryNotification = SecondaryEnabled
            ? new NotificationOffset { Offset = BuildOffset(Math.Max(1, SecondaryValue), SecondaryUnitIndex), Label = "Notificación secundaria" }
            : null;
        Task.TertiaryNotification = TertiaryEnabled
            ? new NotificationOffset { Offset = BuildOffset(Math.Max(1, TertiaryValue), TertiaryUnitIndex), Label = "Notificación terciaria" }
            : null;

        if (_taskService.GetTasks().Any(t => t.Id == Task.Id))
        {
            _taskService.UpdateTask(Task);
        }
        else
        {
            _taskService.AddTask(Task);
        }

        foreach (var notification in _notificationService.BuildNotifications(Task))
        {
            _notificationManager.ScheduleNotification(notification.scheduleTime, notification.title, notification.body, notification.withAlarm);
        }

        await _navigationService.GoBackAsync();
    }

    private async void OnDelete()
    {
        _taskService.DeleteTask(Task);
        await _navigationService.GoBackAsync();
    }

    private static TimeSpan BuildOffset(int value, int unitIndex)
    {
        return unitIndex switch
        {
            0 => TimeSpan.FromMinutes(value),
            1 => TimeSpan.FromHours(value),
            _ => TimeSpan.FromDays(value)
        };
    }

    private static double GetValueFromOffset(TimeSpan offset, out int unitIndex)
    {
        if (offset.TotalDays >= 1)
        {
            unitIndex = 2;
            return offset.TotalDays;
        }

        if (offset.TotalHours >= 1)
        {
            unitIndex = 1;
            return offset.TotalHours;
        }

        unitIndex = 0;
        return offset.TotalMinutes;
    }
}
