using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Models;

namespace Mvvm.Maui.ViewModels;

public class StatisticViewModel : ObservableObject
{
    readonly IDataService _dataService;
    readonly INavigationService _navigationService;

    public AsyncRelayCommand UILoadedCommand { get; set; }
    public AsyncRelayCommand<SelectionChangedEventArgs> ItemSelectionCommand { get; set; }
    public AsyncRelayCommand RefreshCommand { get; set; }

    public IEnumerable<Finance> Data { get; set; }
    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set => SetProperty(ref isRefreshing, value);
    }

    public StatisticViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        UILoadedCommand = new AsyncRelayCommand(LoadData);
        ItemSelectionCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(ItemSelectionChanged);
        RefreshCommand = new AsyncRelayCommand(LoadData);
    }

    private async Task ItemSelectionChanged(SelectionChangedEventArgs arg)
    {
        if (arg.CurrentSelection.Count < 1)
            return;
        if (arg.CurrentSelection[0] is Finance f)
            await _navigationService.NavigateToAsync("Transaction", new Dictionary<string, object> { { nameof(f.id), f.id } });
        return;
    }

    private async Task LoadData()
    {
        Data = new List<Finance>(await _dataService.GetAllFinancesAsync());
        IsRefreshing = false;
        OnPropertyChanged(nameof(Data));
    }
}

