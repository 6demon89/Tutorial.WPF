using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Models;
namespace Mvvm.Maui.ViewModels;
public class OverviewViewModel : ObservableObject
{
    readonly IDataService _dataService;
    readonly INavigationService _navigationService;

    public decimal Balance { get; set; }

    private bool isRefreshing;
    public bool IsRefreshing 
    { 
        get => isRefreshing; 
        set => SetProperty(ref isRefreshing, value);
    }
    
    public AsyncRelayCommand UILoadedCommand { get; set; }
    public AsyncRelayCommand<SelectionChangedEventArgs> ItemSelectionCommand { get; set; }

    public AsyncRelayCommand RefreshCommand { get; set; }
    
    public List<ISeries> Series { get; set; } = new List<ISeries>();

    public IEnumerable<Finance> Data { get; set; }

    public OverviewViewModel(IDataService dataService, INavigationService navigationService)
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
        var dataset = await _dataService.GetFinancesBasecOnDateRangeAsync(DateTime.Now.AddDays(-30), DateTime.Now);
        Data = dataset;
        Series.Clear();
        dataset.ForEach(x =>
        {
            Series.Add(new PieSeries<Finance>
            {
                Values = new List<Finance> { x },
                Name = $"{x.Name} {x.Description}",
                DataLabelsFormatter = (point) => x.Amount.ToString("{0:C}"),
                Mapping = (finance, point) =>
                {
                    point.PrimaryValue = (double)finance.Amount;
                }
            });
        });
        Balance = await _dataService.GetFinancialStateAsync();
        IsRefreshing = false;
        OnPropertyChanged(nameof(Balance));
        OnPropertyChanged(nameof(Data));
        OnPropertyChanged(nameof(Series));
    }
}
