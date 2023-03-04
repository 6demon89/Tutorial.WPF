using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Models;
using System.Windows.Input;

namespace Mvvm.Maui.ViewModels;

[QueryProperty(nameof(ID), nameof(Finance.id))]
public class FinanceDetailsViewModel : ObservableObject 
{ 
    readonly IDataService _dataService;
    readonly INavigationService _navigationService;
    readonly IAlertService _alertService;

    public Guid ID { get; set; }
    public string AmounString { get; set; }
    private Finance finance;
    public Finance FinanceDetail
    {
        get => finance;
        set => SetProperty(ref finance, value);
    }

    public ICommand UILoadedCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand DeleteCommand { get; set; }

    public FinanceDetailsViewModel
        (
        IDataService dataService,
        INavigationService navigationService,
        IAlertService alertService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _alertService = alertService;
        UILoadedCommand = new AsyncRelayCommand(LoadData);
        SaveCommand = new AsyncRelayCommand(SaveChanges);
        CancelCommand = new AsyncRelayCommand(async () => await _navigationService.PopAsync());
        DeleteCommand = new AsyncRelayCommand(DeleteTransaction);
    }

    private async Task DeleteTransaction()
    {
        await _dataService.DeleteFinance(finance);
        if (ID == Guid.Empty)
            await _navigationService.InitilizeAsync();
        else
            await _navigationService.PopAsync();
    }

    private async Task SaveChanges()
    {
        if (decimal.TryParse(AmounString, out var amount))
            finance.Amount = amount;
        else 
        { 
            await _alertService.ShowAlertAsync("Incorrect input","Please ensure that the Amount value is decimal");
            return;
        }
        if (ID != Guid.Empty)
        {
            await _dataService.UpdateFinance(finance);
            await _navigationService.PopAsync();
        }
        else
        {
            finance.id = Guid.NewGuid();
            await _dataService.AddFinance(finance);
            FinanceDetail = new Finance();
            await _navigationService.InitilizeAsync();
        }
    }

    private async Task LoadData()
    {
        if (ID == Guid.Empty)
            FinanceDetail = new Finance();
        else
            FinanceDetail = await _dataService.GetFinanceAsync(ID);
        AmounString = FinanceDetail.Amount.ToString();
        OnPropertyChanged(nameof(ID));
        OnPropertyChanged(nameof(AmounString));
    }
}

