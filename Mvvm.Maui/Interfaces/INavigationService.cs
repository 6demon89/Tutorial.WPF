namespace Mvvm.Maui.Interfaces;
public interface INavigationService
{
    Task InitilizeAsync();

    Task NavigateToAsync(string route, IDictionary<string, object> parameters = null);

    Task PopAsync();
}
