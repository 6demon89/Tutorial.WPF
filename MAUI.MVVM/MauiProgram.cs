using CommunityToolkit.Maui;
using MAUI.MVVM.Interfaces;
using MAUI.MVVM.Service;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MAUI.MVVM;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .RegisterAppViews()
            .RegisterAppViewModels()
            .RegisterAppServices()
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
    public static MauiAppBuilder RegisterAppViews(this MauiAppBuilder app)
    {
        app.Services.AddSingleton<Views.OverviewPage>();
        app.Services.AddSingleton<Views.OverviewCodeBehind>();
        return app;
    }

    public static MauiAppBuilder RegisterAppViewModels(this MauiAppBuilder app)
    {
        app.Services.AddSingleton<ViewModels.OverviewViewModel>();
        return app;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder app)
    {
        app.Services.AddSingleton<INavigationService, NavigationService>();
        app.Services.AddSingleton<IDataService, DataService>(); 
        app.Services.AddScoped<IAlertService,AlertService>();
        return app;
    }
}