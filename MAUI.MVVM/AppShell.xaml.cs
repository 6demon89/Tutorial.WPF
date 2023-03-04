using MAUI.MVVM.Interfaces;
using MAUI.MVVM.ViewModels;
using MAUI.MVVM.Views;

namespace MAUI.MVVM;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        AppShell.InitializeRouting();
        InitializeComponent();
    }

    private static void InitializeRouting()
    {
        Routing.RegisterRoute("Overview", typeof(OverviewPage));
        Routing.RegisterRoute("Overview CodeBehind", typeof(OverviewCodeBehind));
    }


    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is not null)
        {
            await _navigationService.InitializeAsync();
        }
    }
}