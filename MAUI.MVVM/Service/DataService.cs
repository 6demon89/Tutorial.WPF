using MAUI.MVVM.Interfaces;
using MAUI.MVVM.Models;
namespace MAUI.MVVM.Service;

internal class DataService : IDataService
{
    readonly List<Finance> _finances = new List<Finance>();

    public DataService()
    {
        _finances.Add(new Finance(Guid.NewGuid(),"Salary", "Software",6000,DateTime.Now.AddDays(-28)));
        _finances.Add(new Finance(Guid.NewGuid(),"Bill", "Electricity",-165.63m,DateTime.Now.AddDays(-5)));
        _finances.Add(new Finance(Guid.NewGuid(),"Mic", "Recording better audio", -200.65m, DateTime.Now.AddDays(-20)));
        _finances.Add(new Finance(Guid.NewGuid(),"Bill", "Apartment",-1500,DateTime.Now.AddDays(-18)));
        _finances.Add(new Finance(Guid.NewGuid(),"Bill", "Internet", -150,DateTime.Now.AddDays(-18)));
        _finances.Add(new Finance(Guid.NewGuid(), "Bill", "Phone", -50, DateTime.Now.AddDays(-18)));
    }

    public Task AddFinance(Finance finance)
    {
        _finances.Add(finance);
        return Task.CompletedTask;
    }

    public Task DeleteFinance(Finance finance)
    {
        _finances.Remove(finance);
        return Task.CompletedTask;
    }

    public Task<List<Finance>> GetAllFinancesAsync() => Task.FromResult(new List<Finance>(_finances));

    public async Task<decimal> GetFinanceAsync()
    {
        var current = await GetAllFinancesAsync();
        return current.Sum(x => x.Amount);
    }

    public Task<List<Finance>> GetFinancesBasedOnDateRangeAsync(DateTime start,DateTime end)
    {
        return Task.FromResult(_finances.Where(x=>x.TimeStamp >= start && x.TimeStamp <= end).ToList());
    }

    public Task UpdateFinance(Finance finance)
    {
        _finances.Remove(finance);
        return Task.CompletedTask;
    }
}
