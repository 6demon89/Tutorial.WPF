using Mvvm.Maui.ViewModels;

namespace Mvvm.Maui.Views;

public partial class StatisticPage : ContentPage
{
	public StatisticPage(StatisticViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}