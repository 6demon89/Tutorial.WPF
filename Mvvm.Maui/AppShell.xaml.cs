using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Views;

namespace Mvvm.Maui;

public partial class AppShell : Shell
{
    readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;
        InitilizeRouting();
        InitializeComponent();
    }

    private static void InitilizeRouting()
    {
        Routing.RegisterRoute("Overview", typeof(OverviewPage));
        Routing.RegisterRoute("Transaction", typeof(FinanceDetailsPage));
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (Handler is not null)
            await _navigationService.InitilizeAsync();
    }
}