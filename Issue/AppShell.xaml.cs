using Issue.Views;

namespace Issue;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(TaskDetailPage), typeof(TaskDetailPage));
    }
}
