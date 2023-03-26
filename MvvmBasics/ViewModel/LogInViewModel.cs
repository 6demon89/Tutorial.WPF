using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmBasics.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace MvvmBasics.ViewModel;

public record TempProject(int id,string name);


public class ProjectsViewModel : ObservableObject
{

	public event Action<TempProject> OnProjectSelectionChanged;
    public ObservableCollection<TempProject> temp { get; set; }

    public ICommand SelectionCommand { get; set; } 
    public ProjectsViewModel()
    {
        SelectionCommand = new RelayCommand<object>(PassSelectedProjectToMain);
        temp= new ObservableCollection<TempProject>();
		for (int i = 0; i < 20; i++)
		{
			temp.Add(new(i, i.ToString()));
		}
    }

    private void PassSelectedProjectToMain(object? obj)
    {
		if(obj is System.Windows.Controls.SelectionChangedEventArgs project)
		{
			OnProjectSelectionChanged(project.AddedItems[0] as TempProject);
		}
    }
}






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
