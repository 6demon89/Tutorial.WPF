using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MAUI.MVVM.Interfaces;

namespace MAUI.MVVM;

public partial class App : Application
{
    public App(INavigationService navigationService)
    {
        InitializeComponent();
        MainPage = new AppShell(navigationService);

        LiveCharts.Configure(config =>
            config
                .AddSkiaSharp()
                .AddDefaultMappers()
                .AddLightTheme());
    }
}