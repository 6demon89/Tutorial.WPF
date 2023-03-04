using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using MAUI.MVVM.Models;
using MAUI.MVVM.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MAUI.MVVM.ViewModels;

public class OverviewViewModel : ObservableObject
{
    readonly IDataService _dataSerivce;
    readonly IAlertService _alertService;

    public ObservableCollection<ISeries> Series { get; set; }
    public ObservableCollection<Finance> Data { get; set; }

    public decimal Balance { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Amount { get; set; }

    public AsyncRelayCommand UILoadedCommand { get; set; }

    public AsyncRelayCommand CommitCommand { get; set; }

    public OverviewViewModel(IDataService dataService, IAlertService alertService)
    {
        _dataSerivce = dataService;
        UILoadedCommand = new AsyncRelayCommand(InitializeDataAsync);
        CommitCommand = new AsyncRelayCommand(AddExpenseCommand);
        _alertService = alertService;
    }

    private async Task AddExpenseCommand()
    {
        if (ValidateInput())
        {
            if (decimal.TryParse(Amount, out decimal value))
            {
                var dataset = new Finance(Guid.NewGuid(), Name, Description, value, DateTime.Now);
                await _dataSerivce.AddFinance(dataset);
                Data.Add(dataset);
                return;
            }
        }
        await _alertService.ShowAlertAsync("Invalid Input", "Please make sure all inputs are correct");
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(Name)) return false;
        if (string.IsNullOrEmpty(Description)) return false;
        if (string.IsNullOrEmpty(Amount)) return false;
        if (!decimal.TryParse(Amount, out decimal value)) return false;
        return true;
    }

    private async Task InitializeDataAsync()
    {
        var startingDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;
        var data = await _dataSerivce.GetFinancesBasedOnDateRangeAsync(startingDate, endDate);
        Balance = await _dataSerivce.GetFinanceAsync();
        Data = new ObservableCollection<Finance>(await _dataSerivce.GetAllFinancesAsync());
        Series = new ObservableCollection<ISeries>();
        data.ForEach(x =>
        {
            Series.Add(
                new PieSeries<Finance>
                {
                    Values = new List<Finance> { x },
                    Name = $"{x.Name}:{x.Description}",
                    DataLabelsFormatter = (point) => $"({x.Amount.ToString("C2", CultureInfo.CurrentCulture)})",
                    Mapping = (finance, point) =>
                    {
                        // use the Population property in this series
                        point.PrimaryValue = (double)finance.Amount;
                        point.SecondaryValue = point.Context.Index;
                    }
                }
                );
        });

        OnPropertyChanged(nameof(Data));
        OnPropertyChanged(nameof(Series));
        OnPropertyChanged(nameof(Balance));

    }
}
