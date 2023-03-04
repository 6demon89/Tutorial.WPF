using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using MAUI.MVVM.Models;
using MAUI.MVVM.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MAUI.MVVM.Views;

public partial class OverviewCodeBehind : ContentPage
{
    readonly IDataService _dataSerivce;
    readonly IAlertService _alertService;
    public ObservableCollection<ISeries> Series { get; set; }
    public ObservableCollection<Finance> Data { get; set; }

    public OverviewCodeBehind(IDataService dataService, IAlertService alertService)
	{
		InitializeComponent();
        _dataSerivce = dataService;
        _alertService = alertService;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            if (decimal.TryParse(ExpensesAmountEntry.Text, out decimal value))
            {
                var dataset = new Finance(Guid.NewGuid(), ExpenseNameEntry.Text, ExpensesDescriptionEntry.Text, value, DateTime.Now);
                await _dataSerivce.AddFinance(dataset);
                Data.Add(dataset);
                return;
            }
        }
        await _alertService.ShowAlertAsync("Invalid Input", "Please make sure all inputs are correct");
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(ExpenseNameEntry.Text)) return false;
        if (string.IsNullOrEmpty(ExpensesDescriptionEntry.Text)) return false;
        if (string.IsNullOrEmpty(ExpensesAmountEntry.Text)) return false;
        if (!decimal.TryParse(ExpensesAmountEntry.Text, out decimal value)) return false;
        return true;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
       
        var startingDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;
        var data = await _dataSerivce.GetFinancesBasedOnDateRangeAsync(startingDate, endDate);
        var t = await _dataSerivce.GetFinanceAsync();
        CurenctBalanceLabel.Text = t.ToString();
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
        DataCollectionView.ItemsSource = Data;
        pieChart.Series = Series;

    }
}