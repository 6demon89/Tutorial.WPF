using Mvvm.Maui.Models;
namespace Mvvm.Maui.Interfaces;

public interface IDataService 
{
    public Task<Finance> GetFinanceAsync(Guid id);
    public Task<decimal> GetFinancialStateAsync();
    public Task<List<Finance>> GetAllFinancesAsync();
    public Task<List<Finance>> GetFinancesBasecOnDateRangeAsync(DateTime startDate, DateTime endDate);
    public Task AddFinance(Finance finance);
    public Task UpdateFinance(Finance finance);
    public Task DeleteFinance(Finance finance);

}
