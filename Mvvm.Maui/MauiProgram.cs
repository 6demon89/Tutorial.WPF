using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Services;
using Mvvm.Maui.ViewModels;
using Mvvm.Maui.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Mvvm.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .RegisterContainer()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }


    public static MauiAppBuilder RegisterContainer(this MauiAppBuilder app)
    {
        app.Services.AddSingleton<AppShell>();

        app.Services.AddTransient<OverviewPage>();
        app.Services.AddTransient<OverviewViewModel>();

        app.Services.AddTransient<FinanceDetailsPage>();
        app.Services.AddTransient<FinanceDetailsViewModel>();

        app.Services.AddTransient<StatisticPage>();
        app.Services.AddTransient<StatisticViewModel>();

        app.Services.AddSingleton<INavigationService, NavigationService>();
        app.Services.AddSingleton<IAlertService, AlertService>();
        app.Services.AddSingleton<IDataService, DataService>();
        return app;
    }
}