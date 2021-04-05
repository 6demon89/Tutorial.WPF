using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace Tutorial.WPF.ViewModel
{
    /*
     * Todays Goals:
     * - Explain windows chrome
     * - Explain Triggers and StoryBoards
     * - Add Blend Interactive
     * - Custom Command, based on the Command Pattern
     * - Custom Dialog Result before executing command
     * - Maybe : Add new Views and implement Navigation
     */
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChanges(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyname)));
        }

    }
}
