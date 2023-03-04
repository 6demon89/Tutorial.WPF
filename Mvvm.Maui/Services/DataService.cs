using Mvvm.Maui.Interfaces;
using Mvvm.Maui.Models;

namespace Mvvm.Maui.Services
{
    internal class DataService : IDataService
    {
        readonly List<Finance> _finances = new List<Finance>();

        public DataService()
        {
            _finances.Add(new(Guid.NewGuid(), "Loan", "Income", 234.78m, DateTime.Now.AddDays(-35)));
            _finances.Add(new(Guid.NewGuid(), "Work", "Income", 3000, DateTime.Now.AddDays(-3)));
            _finances.Add(new(Guid.NewGuid(), "Bill", "Internet", -50, DateTime.Now.AddDays(-4)));
            _finances.Add(new(Guid.NewGuid(), "Bill", "Phone", -100, DateTime.Now.AddDays(-35)));
            _finances.Add(new(Guid.NewGuid(), "Bill", "Electricity", -103.90m, DateTime.Now.AddDays(-2)));
            _finances.Add(new(Guid.NewGuid(), "Bill", "Water/Heat", -480, DateTime.Now.AddDays(-1)));
            _finances.Add(new(Guid.NewGuid(), "Loan", "Income", 234.78m, DateTime.Now));
            _finances.Add(new(Guid.NewGuid(), "Bill", "Apartment", -52.28m, DateTime.Now.AddDays(-15)));
            _finances.Add(new(Guid.NewGuid(), "Misc", "Groceries", -13.33m, DateTime.Now.AddDays(-11)));
            _finances.Add(new(Guid.NewGuid(), "Misc", "Groceries", -75.39m, DateTime.Now.AddDays(-13)));
        }
        public Task<Finance> GetFinanceAsync(Guid id) => Task.FromResult(_finances.Single(x => x.id == id));

        public async Task AddFinance(Finance finance)
        {
            await Task.Delay(10);
            _finances.Add(finance);
        }

        public Task UpdateFinance(Finance finance)
        {
            var temp = _finances.Single(x=>x.id == finance.id);
            _finances.Remove(temp);
            _finances.Add(finance);
            return Task.CompletedTask;
        }

        public async Task DeleteFinance(Finance finance)
        {
            await Task.Delay(10);
            _finances.Remove(finance);
        }

        public async Task<List<Finance>> GetAllFinancesAsync()
        {
            await Task.Delay(50);
            return _finances;
        }

        public async Task<List<Finance>> GetFinancesBasecOnDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            await Task.Delay(50);
            return _finances.Where(x => x.TimeStamp >= startDate && x.TimeStamp <= endDate).ToList();
        }

        public Task<decimal> GetFinancialStateAsync() => Task.FromResult(_finances.Sum(x => x.Amount));
    }
}
