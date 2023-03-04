using Mvvm.Maui.Interfaces;

namespace Mvvm.Maui;

public partial class App : Application
{
    public App(AppShell shell)
    {
        InitializeComponent();
        MainPage = shell;
    }
}