using Issue.ViewModels;
using Issue.Models;

namespace Issue.Views;

public partial class TaskListPage : ContentPage
{
    private readonly TaskListViewModel _viewModel;

    public TaskListPage(TaskListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button { BindingContext: TaskItem task })
        {
            await _viewModel.OpenTaskAsync(task);
        }
    }
}
