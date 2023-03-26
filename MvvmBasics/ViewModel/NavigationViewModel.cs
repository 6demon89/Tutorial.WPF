using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmBasics.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace MvvmBasics.ViewModel;

public class NavigationViewModel : ObservableObject
{
    public List<NavigationModel> NavigationOptions { get; set; } = new();

    public ICommand SelectionChangedCommand { get; set; } = new RelayCommand<object>((o) => 
    { 
        if(o is SelectionChangedEventArgs selectionChanged)
        {
            if (selectionChanged.AddedItems.Count == 0)
                return;
            if (selectionChanged.AddedItems[0] is NavigationModel navModel)
            {
                var message = new NavigationChangedRequestedMessage(navModel);
                WeakReferenceMessenger.Default.Send(message);
            }
        }
    });

    public NavigationViewModel(
        LogInViewModel loginView,
        ProjectsViewModel projectView,
        EvenetsToCommandViewModel evenetsToCommandViewModel)
    {
        projectView.OnProjectSelectionChanged += ProjectView_OnProjectSelectionChanged;
        
        NavigationOptions.Add(new() { Name = "Login", Description = "User can login with this page. the demo shows how to request data from other ViewModels", DestinationVM = loginView });
        NavigationOptions.Add(new() { Name = "Events", Description = "This shows how to redirect event into commands", DestinationVM = evenetsToCommandViewModel });
        NavigationOptions.Add(new() { Name = "Project", Description = "This shows how to redirect event into commands", DestinationVM = projectView });
        
        var message = new NavigationChangedRequestedMessage(NavigationOptions[0]);
        WeakReferenceMessenger.Default.Send(message);
    }

    private async void ProjectView_OnProjectSelectionChanged(TempProject obj)
    {
        await Task.Delay(20000);
        MessageBox.Show($"{obj.id}");
    }
}
