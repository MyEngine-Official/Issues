using System.Collections.ObjectModel;
using System.Windows.Input;
using Issue.Models;
using Issue.Services;
using Issue.Views;

namespace Issue.ViewModels;

public class TaskListViewModel : ViewModelBase
{
    private readonly TaskService _taskService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<TaskItem> Tasks { get; }

    public ICommand AddTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }

    public TaskListViewModel(TaskService taskService, INavigationService navigationService)
    {
        _taskService = taskService;
        _navigationService = navigationService;

        Tasks = _taskService.GetTasks();
        AddTaskCommand = new Command(OnAddTask);
        DeleteTaskCommand = new Command<TaskItem>(OnDeleteTask);
    }

    public Task OpenTaskTemplateAsync()
    {
        var task = new TaskItem
        {
            Title = "Nueva tarea",
            Description = "Descripción opcional",
            DueDateTime = DateTime.Now.AddHours(2),
            AlarmEnabled = true,
            PrimaryNotification = new NotificationOffset
            {
                Offset = TimeSpan.Zero,
                Label = "Notificación principal"
            },
            SecondaryNotification = new NotificationOffset
            {
                Offset = TimeSpan.FromMinutes(30),
                Label = "30 min antes"
            }
        };

        return OpenTaskAsync(task);
    }

    public Task OpenTaskAsync(TaskItem task)
    {
        return _navigationService.GoToAsync(nameof(TaskDetailPage), new Dictionary<string, object>
        {
            { "Task", task }
        });
    }

    private async void OnAddTask()
    {
        await OpenTaskTemplateAsync();
    }

    private void OnDeleteTask(TaskItem? task)
    {
        if (task is null)
        {
            return;
        }

        _taskService.DeleteTask(task);
    }
}
