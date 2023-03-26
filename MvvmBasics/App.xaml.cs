using Microsoft.Extensions.DependencyInjection;
using MvvmBasics.Service;
using MvvmBasics.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmBasics;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider? Container { get; protected set; }

    protected override void OnStartup(StartupEventArgs e)
    {



        InitContainer();
        if (Container is null)
            throw new Exception("something went wrong during initializing DI container");
        MainWindowViewModel? MainVM = Container.GetService<MainWindowViewModel>() as MainWindowViewModel;
        var window = Container.GetService(typeof(MainWindow)) as MainWindow;
        if(window is null)
            throw new Exception("something went wrong during initializing DI container. MainWindow is missing");
        window.DataContext= MainVM;
        window.Show();
        base.OnStartup(e);
    }

    private static void InitContainer()
    {
        ServiceCollection services = new();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<EvenetsToCommandViewModel>();
        services.AddSingleton<LogInViewModel>();
        services.AddSingleton<NavigationViewModel>();
        services.AddSingleton<ProjectsViewModel>();
        services.AddSingleton<GreetingViewModel>();
        services.AddScoped<GenerateDataService>();
        Container = services.BuildServiceProvider();
    }



}
