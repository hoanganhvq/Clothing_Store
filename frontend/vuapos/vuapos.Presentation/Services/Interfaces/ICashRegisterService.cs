using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.Services.Interfaces
{
    public interface ICashRegisterService
    {
        Task<CashRegister> GetActiveCashRegisterAsync();
        Task<bool> CreateCashTransactionAsync(CashTransaction transaction);
        Task<List<CashTransaction>> GetRecentTransactionsAsync(int count = 10);
        Task<decimal> GetTodayRevenueAsync();
        Task<EndOfDayReport> CloseRegisterForDayAsync(decimal actualBalance, string notes);
        Task<List<CashTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }


}
