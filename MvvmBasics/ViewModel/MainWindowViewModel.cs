using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MvvmBasics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmBasics.ViewModel
{
    public class MainWindowViewModel: ObservableObject
    {
        public ObservableObject? NavigationVM { get; set; }

        private ObservableObject? currentVM;

        public ObservableObject? CurrentVM
        {
            get => currentVM; 
            set => SetProperty(ref currentVM ,value);
        }

        public MainWindowViewModel(NavigationViewModel navVM)
        {
            NavigationVM = navVM;
            WeakReferenceMessenger.Default.Register<NavigationChangedRequestedMessage>(this, NavigateTo);
        }

        private void NavigateTo(object recipient, NavigationChangedRequestedMessage message)
        {
            if (message.Value is NavigationModel navModel)
                CurrentVM = navModel.DestinationVM;
        }
    }
}
