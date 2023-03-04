using MAUI.MVVM.Interfaces;

namespace MAUI.MVVM.Service;

public class NavigationService : INavigationService
{
    public Task InitializeAsync() =>
        NavigateToAsync("//Overview");

    public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }

    public Task PopAsync() =>
        Shell.Current.GoToAsync("..");
}
