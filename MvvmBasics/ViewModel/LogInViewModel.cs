using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmBasics.Model;
using System.Windows.Data;
using System.Windows.Input;

namespace MvvmBasics.ViewModel
{
    public class LogInViewModel : ObservableObject
    {
		private string? userName;

		public string? UserName
		{
			get => userName;
			set => SetProperty(ref userName , value);
		}

		public ICommand	LoginCommand{ get; set; }


		public LogInViewModel()
		{
			LoginCommand = new RelayCommand(() =>
			{
				var navModel = new NavigationChangedRequestedMessage(new() { DestinationVM = new GreetingViewModel(UserName) });
				WeakReferenceMessenger.Default.Send(navModel);
			});
		
		}
	}
}
