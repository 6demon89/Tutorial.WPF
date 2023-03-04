using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmBasics.Model;
using MvvmBasics.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace MvvmBasics.ViewModel;

public class EvenetsToCommandViewModel : ObservableObject
{
    readonly GenerateDataService _generateDataService;
    private List<DataModel> _data = new();

    private string? userName;

    public string? UserName
    {
        get => userName; 
        set => SetProperty(ref userName , value);
    }

    public ICollectionView DataView { get; set; }
    public ICommand GetDataCommand { get; set; }
    public ICommand SearchTextChangeCommand { get; set; }

    public EvenetsToCommandViewModel(GenerateDataService generateDataService)
    {
        _generateDataService= generateDataService;
        GetDataCommand = new AsyncRelayCommand(GetDataFromService);
        SearchTextChangeCommand = new RelayCommand<object>(FilterData);
        DataView = CollectionViewSource.GetDefaultView(_data);
    }

    private void FilterData(object? obj)
    {
        if (obj is TextChangedEventArgs textarg)
            if (textarg.Source is TextBox textBox)
                DataView.Filter = f =>
                {
                    if(f is DataModel data)
                        return data.Name.Contains(textBox.Text.ToUpper());
                    return false;
                };
    }

    private async Task GetDataFromService()
    {
        try
        {
            UserModel user = WeakReferenceMessenger.Default.Send<LoggedInUserRequestMessage>();
            UserName = user.Name;
        }
        catch
        {
            UserName = null;
        }
        if(_data.Count > 0)
        {
            DataView.Refresh();
            return;
        }
        try
        { 
            _data.AddRange(await _generateDataService.GetDataAsync());
        }
        catch
        {
            //Handle the exception here! Since it will not be rerouted and will be ignored by the App
        }
        DataView.Refresh();
    }
}
