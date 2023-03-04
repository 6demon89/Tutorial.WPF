using Mvvm.Maui.ViewModels;

namespace Mvvm.Maui.Views;

public partial class FinanceDetailsPage : ContentPage
{
	public FinanceDetailsPage(FinanceDetailsViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}

}