using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmBasics.Model;
using System.Windows.Input;

namespace MvvmBasics.ViewModel;

public class GreetingViewModel : ObservableObject
{
    public string UserName { get; set; }
    public ICommand LogOutCommand { get; set; }

    public GreetingViewModel(string username)
    {
        UserName = username;
        LogOutCommand = new RelayCommand(() =>
        {
            var navModel = new NavigationChangedRequestedMessage(new() { DestinationVM = new LogInViewModel() { UserName=UserName } });
            WeakReferenceMessenger.Default.Send(navModel);
        });
        WeakReferenceMessenger.Default.Register<GreetingViewModel, LoggedInUserRequestMessage>(this, (r, m) =>
        {
            m.Reply(new UserModel() { Name= UserName });
        });
    }
}
