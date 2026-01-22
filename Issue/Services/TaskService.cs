using System.Collections.ObjectModel;
using System.Text.Json;
using Issue.Models;

namespace Issue.Services;

public class TaskService
{
    private const string TasksKey = "issue_tasks";
    private readonly ObservableCollection<TaskItem> _tasks = new();

    public TaskService()
    {
        var stored = Preferences.Default.Get(TasksKey, string.Empty);
        if (!string.IsNullOrWhiteSpace(stored))
        {
            try
            {
                var tasks = JsonSerializer.Deserialize<List<TaskItem>>(stored);
                if (tasks is not null)
                {
                    foreach (var task in tasks)
                    {
                        _tasks.Add(task);
                    }
                }
            }
            catch (JsonException)
            {
                Preferences.Default.Remove(TasksKey);
            }
        }

        if (_tasks.Count == 0)
        {
            _tasks.Add(new TaskItem
            {
                Title = "Configurar notificación",
                Description = "Crear la primera tarea en Issue",
                DueDateTime = DateTime.Today.AddHours(18),
                AlarmEnabled = false,
                PrimaryNotification = new NotificationOffset
                {
                    Offset = TimeSpan.Zero,
                    Label = "Notificación principal"
                }
            });
        }
    }

    public ObservableCollection<TaskItem> GetTasks() => _tasks;

    public void AddTask(TaskItem task)
    {
        _tasks.Add(task);
        SaveTasks();
    }

    public void UpdateTask(TaskItem task)
    {
        var existing = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existing is null)
        {
            return;
        }

        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.DueDateTime = task.DueDateTime;
        existing.AlarmEnabled = task.AlarmEnabled;
        existing.PrimaryNotification = task.PrimaryNotification;
        existing.SecondaryNotification = task.SecondaryNotification;
        existing.TertiaryNotification = task.TertiaryNotification;
        SaveTasks();
    }

    public void DeleteTask(TaskItem task)
    {
        var existing = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existing is null)
        {
            return;
        }

        _tasks.Remove(existing);
        SaveTasks();
    }

    private void SaveTasks()
    {
        var payload = JsonSerializer.Serialize(_tasks);
        Preferences.Default.Set(TasksKey, payload);
    }
}
