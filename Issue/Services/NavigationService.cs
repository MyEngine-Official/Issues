namespace Issue.Services;

public interface INavigationService
{
    Task GoToAsync(string route, IDictionary<string, object>? parameters = null);
    Task GoBackAsync();
}

public class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        return Shell.Current?.GoToAsync(route, parameters) ?? Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        return Shell.Current?.GoToAsync("..") ?? Task.CompletedTask;
    }
}
