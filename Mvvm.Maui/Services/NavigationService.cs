using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Models;

namespace Mvvm.Maui.Services;


internal class NavigationService : INavigationService
{
    public Task InitilizeAsync() => NavigateToAsync("//Overview");

    public Task NavigateToAsync(string route, IDictionary<string, object> parameters = null)
        => parameters is not null
        ? Shell.Current.GoToAsync(new ShellNavigationState(route), parameters)
        : Shell.Current.GoToAsync(new ShellNavigationState(route));

    public Task PopAsync() => Shell.Current.GoToAsync("..");
}
