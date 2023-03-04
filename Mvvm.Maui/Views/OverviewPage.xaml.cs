namespace Mvvm.Maui.Views;

public partial class OverviewPage : ContentPage
{
    public OverviewPage(ViewModels.OverviewViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}