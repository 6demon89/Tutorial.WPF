using System.ComponentModel;
using System.Windows.Input;

namespace Tutorial.WPF.ViewModel
{
    /*
     * Goals for upcomming Stream:
     * - TextBox changed to combobox
     * - Add interactive command for windows loaded to initialize the list of known crypto names
     * - Add OnLoad for UI to loadup async data
     * - Modify Base View Model
     * - Add new Views and implement Navigation
     * 
     * - Explane Template Selector
     * 
     * - Maybe section:
     * - Add Application Manager
     * - Add Messaging System
     * - ViewModelLocator
     * 
     * Last Goals:
     * - Explain windows chrome
     * - Explain Triggers and StoryBoards
     * - Custom Command, based on the Command Pattern
     * - Add Blend Interactive *Install-Package Microsoft.Xaml.Behaviors.Wpf
     * - Custom Dialog Result before executing command
     * 
     */

    public class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand ViewLoadedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChanges(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyname)));
        }

    }
}
