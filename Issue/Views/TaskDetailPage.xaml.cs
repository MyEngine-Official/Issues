using Issue.Models;
using Issue.ViewModels;

namespace Issue.Views;

public partial class TaskDetailPage : ContentPage, IQueryAttributable
{
    public TaskDetailPage(TaskDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is TaskDetailViewModel viewModel && query.TryGetValue("Task", out var taskObj) && taskObj is TaskItem task)
        {
            viewModel.InitializeForEdit(task);
        }
    }
}
