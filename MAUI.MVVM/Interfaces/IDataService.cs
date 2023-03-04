using MAUI.MVVM.Models;
namespace MAUI.MVVM.Interfaces;

public interface IDataService
{
    public Task<decimal> GetFinanceAsync();
    public Task<List<Finance>> GetAllFinancesAsync();
    public Task<List<Finance>> GetFinancesBasedOnDateRangeAsync(DateTime start, DateTime end);
    public Task AddFinance(Finance finance);
    public Task UpdateFinance(Finance finance);
    public Task DeleteFinance(Finance finance);

}
