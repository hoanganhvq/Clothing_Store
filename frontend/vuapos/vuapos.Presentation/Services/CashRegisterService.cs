using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using vuapos.Presentation.DAO.MockData;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.Services
{
    public class CashRegisterService : ICashRegisterService
    {
        private readonly string _services;
        public CashRegisterService()
        {
            
        }

        public async Task<CashRegister> GetActiveCashRegisterAsync()
        {
            return CashRegisterMockData.GetMockCashRegisters().FirstOrDefault(r => r.IsActive);
        }

        public async Task<bool> CreateCashTransactionAsync(CashTransaction transaction)
        {
            // Thêm thông tin người tạo giao dịch
            transaction.CreatedByEmployeeId = "1";
            transaction.TransactionTime = DateTime.Now;

            // Cập nhật số dư két tiền
            var register = await GetActiveCashRegisterAsync();
            if (register == null)
                throw new Exception("Không tìm thấy két tiền đang hoạt động");

            // Cập nhật số dư
            if (transaction.Type == TransactionType.CashIn || transaction.Type == TransactionType.InitialCash)
                register.CurrentBalance += transaction.Amount;
            else if (transaction.Type == TransactionType.CashOut)
                register.CurrentBalance -= transaction.Amount;
            else if (transaction.Type == TransactionType.Adjustment)
                register.CurrentBalance = transaction.Amount; // Điều chỉnh trực tiếp số dư

            // Lưu giao dịch và cập nhật két
            //update
            return true;
        }

        public async Task<List<CashTransaction>> GetRecentTransactionsAsync(int count = 10)
        {
            var register = await GetActiveCashRegisterAsync();
            if (register == null)
                return new List<CashTransaction>();

            return CashRegisterMockData.GetMockCashTransactions().Where(ct => ct.CashRegisterId == register.Id)
                .OrderByDescending(ct => ct.TransactionTime)
                .Take(count)
                .ToList();
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.Today;
            var register = await GetActiveCashRegisterAsync();

            if (register == null)
                return 0;

            var cashIn = CashRegisterMockData.GetMockCashTransactions()
                .Where(ct => ct.CashRegisterId == register.Id &&
                      ct.TransactionTime.Date == today &&
                      ct.Type == TransactionType.CashIn)
                .Sum(ct => ct.Amount);

            var cashOut = CashRegisterMockData.GetMockCashTransactions()
                .Where(ct => ct.CashRegisterId == register.Id &&
                      ct.TransactionTime.Date == today &&
                      ct.Type == TransactionType.CashOut)
                .Sum(ct => ct.Amount);

            return cashIn - cashOut;
        }

        public async Task<EndOfDayReport> CloseRegisterForDayAsync(decimal actualBalance, string notes)
        {
            var register = await GetActiveCashRegisterAsync();
            if (register == null)
                throw new Exception("Không tìm thấy két tiền đang hoạt động");

            /// Tạo ID nhân viên thực hiện giao dịch (có thể lấy từ thông tin đăng nhập của người dùng)
            var employeeId = "1";

            // Tạo báo cáo kết số
            var endOfDayReport = new EndOfDayReport
            {
                CashRegisterId = register.Id,
                ReportDate = DateTime.Now,
                SystemBalance = register.CurrentBalance,
                ActualBalance = actualBalance,
                Difference = actualBalance - register.CurrentBalance,
                Notes = notes,
                EmployeeId = employeeId,
                IsApproved = false
            };

            // Tạo giao dịch kết số
            var transaction = new CashTransaction
            {
                CashRegisterId = register.Id,
                Amount = actualBalance,
                TransactionTime = DateTime.Now,
                Type = TransactionType.EndOfDay,
                Description = "Kết số cuối ngày",
                CreatedByEmployeeId = employeeId,
                Notes = $"Số dư hệ thống: {register.CurrentBalance}, Số dư thực tế: {actualBalance}, Chênh lệch: {actualBalance - register.CurrentBalance}"
            };

            // Cập nhật két tiền
            register.LastClosedAt = DateTime.Now;
            register.CurrentBalance = actualBalance; // Cập nhật số dư thực tế

            //using (var dbTransaction = await _dbContext.Database.BeginTransactionAsync())
            //{
            //    try
            //    {
            //        _dbContext.EndOfDayReports.Add(endOfDayReport);
            //        _dbContext.CashTransactions.Add(transaction);
            //        _dbContext.CashRegisters.Update(register);

            //        await _dbContext.SaveChangesAsync();
            //        await dbTransaction.CommitAsync();

            //        return endOfDayReport;
            //    }
            //    catch (Exception)
            //    {
            //        await dbTransaction.RollbackAsync();
            //        throw;
            //    }
            //}
            return endOfDayReport;
        }

        public async Task<List<CashTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var register = await GetActiveCashRegisterAsync();
            if (register == null)
                return new List<CashTransaction>();

            return CashRegisterMockData.GetMockCashTransactions()
                .Where(ct => ct.CashRegisterId == register.Id &&
                      ct.TransactionTime >= startDate &&
                      ct.TransactionTime <= endDate)
                .OrderByDescending(ct => ct.TransactionTime).ToList();
        }
    }
}
