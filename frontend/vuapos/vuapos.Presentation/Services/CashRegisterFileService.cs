using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.Services
{
    public class CashRegisterFileService : ICashRegisterService
    {
        private readonly string _baseDataPath;
        private readonly string _cashRegistersFile;
        private readonly string _cashTransactionsFile;
        private readonly string _endOfDayReportsFile;

        // Semaphore để kiểm soát truy cập vào từng file
        private readonly SemaphoreSlim _cashRegisterSemaphore = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _cashTransactionSemaphore = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _endOfDayReportSemaphore = new SemaphoreSlim(1, 1);

        public CashRegisterFileService()
        {
            // Xác định đường dẫn lưu trữ trong thư mục AppData
            _baseDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "VuaPOS",
                "Data");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(_baseDataPath))
            {
                Directory.CreateDirectory(_baseDataPath);
            }

            _cashRegistersFile = Path.Combine(_baseDataPath, "cashregisters.json");
            _cashTransactionsFile = Path.Combine(_baseDataPath, "cashtransactions.json");
            _endOfDayReportsFile = Path.Combine(_baseDataPath, "endofdayreports.json");

            // Khởi tạo file với dữ liệu mẫu nếu chưa tồn tại
            InitializeFilesIfNeeded();
        }

        #region Private Methods

        private void InitializeFilesIfNeeded()
        {
            try
            {
                // Khởi tạo file CashRegisters
                if (!File.Exists(_cashRegistersFile))
                {
                    var initialCashRegisters = new List<CashRegister>
                    {
                        new CashRegister
                        {
                            Id = "1",
                            Name = "Main Register",
                            CurrentBalance = 1000000m, // 1,000,000 VND
                            IsActive = true,
                            LastClosedAt = DateTime.Now.AddDays(-1),
                            CreatedBy = "Admin",
                        }
                    };
                    File.WriteAllText(_cashRegistersFile, JsonSerializer.Serialize(initialCashRegisters, new JsonSerializerOptions { WriteIndented = true }));
                }

                // Khởi tạo file CashTransactions
                if (!File.Exists(_cashTransactionsFile))
                {
                    var initialCashTransactions = new List<CashTransaction>
                    {
                        new CashTransaction
                        {
                            Id = "1",
                            CashRegisterId = "1",
                            Amount = 1000000m,
                            TransactionTime = DateTime.Now.Date.AddHours(8), // 8:00 AM today
                            Type = TransactionType.InitialCash,
                            Description = "Initial cash for the day",
                            CreatedByEmployeeId = "1",
                            Notes = "Opening balance"
                        }
                    };
                    File.WriteAllText(_cashTransactionsFile, JsonSerializer.Serialize(initialCashTransactions, new JsonSerializerOptions { WriteIndented = true }));
                }

                // Khởi tạo file EndOfDayReports
                if (!File.Exists(_endOfDayReportsFile))
                {
                    var initialEndOfDayReports = new List<EndOfDayReport>();
                    File.WriteAllText(_endOfDayReportsFile, JsonSerializer.Serialize(initialEndOfDayReports, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing files: {ex.Message}");
                // Xử lý ngoại lệ nếu cần
            }
        }

        private async Task<List<T>> ReadDataAsync<T>(string filePath, SemaphoreSlim semaphore)
        {
            // Sử dụng semaphore để đảm bảo chỉ một luồng truy cập file tại một thời điểm
            await semaphore.WaitAsync();
            try
            {
                if (!File.Exists(filePath))
                    return new List<T>();

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return await JsonSerializer.DeserializeAsync<List<T>>(fs) ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data from {filePath}: {ex.Message}");
                return new List<T>();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task WriteDataAsync<T>(string filePath, List<T> data, SemaphoreSlim semaphore)
        {
            // Sử dụng semaphore để đảm bảo chỉ một luồng truy cập file tại một thời điểm
            await semaphore.WaitAsync();
            try
            {
                // Sử dụng FileShare.None để đảm bảo không có tiến trình nào khác truy cập file
                // trong khi đang ghi
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await JsonSerializer.SerializeAsync(fs, data, new JsonSerializerOptions { WriteIndented = true });
                    await fs.FlushAsync(); // Đảm bảo dữ liệu được ghi vào đĩa
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing data to {filePath}: {ex.Message}");
                throw; // Ném lại ngoại lệ để gọi phương thức xử lý
            }
            finally
            {
                semaphore.Release();
            }
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        #endregion

        #region Public Methods

        public async Task<CashRegister> GetActiveCashRegisterAsync()
        {
            var cashRegisters = await ReadDataAsync<CashRegister>(_cashRegistersFile, _cashRegisterSemaphore);
            var activeRegister = cashRegisters.FirstOrDefault(r => r.IsActive);

            if (activeRegister == null)
            {
                throw new InvalidOperationException("No active cash register found");
            }

            return activeRegister;
        }

        public async Task<bool> CreateCashTransactionAsync(CashTransaction transaction)
        {
            try
            {
                // Cập nhật CashRegisterId nếu chưa có
                if (string.IsNullOrEmpty(transaction.CashRegisterId))
                {
                    var activeRegister = await GetActiveCashRegisterAsync();
                    transaction.CashRegisterId = activeRegister.Id;
                }

                // Đọc các giao dịch hiện tại
                var transactions = await ReadDataAsync<CashTransaction>(_cashTransactionsFile, _cashTransactionSemaphore);

                // Cập nhật thông tin và ID nếu chưa có
                if (string.IsNullOrEmpty(transaction.Id))
                {
                    transaction.Id = GenerateId();
                }

                if (transaction.TransactionTime == default)
                {
                    transaction.TransactionTime = DateTime.Now;
                }

                transactions.Add(transaction);
                await WriteDataAsync(_cashTransactionsFile, transactions, _cashTransactionSemaphore);

                // Cập nhật số dư hiện tại của máy tính tiền
                var registers = await ReadDataAsync<CashRegister>(_cashRegistersFile, _cashRegisterSemaphore);
                var register = registers.FirstOrDefault(r => r.IsActive);

                if (register != null)
                {
                    // Cập nhật số dư dựa trên loại giao dịch
                    switch (transaction.Type)
                    {
                        case TransactionType.CashIn:
                        case TransactionType.InitialCash:
                            register.CurrentBalance += transaction.Amount;
                            break;
                        case TransactionType.CashOut:
                            register.CurrentBalance -= transaction.Amount;
                            break;
                        case TransactionType.Adjustment:
                        case TransactionType.EndOfDay:
                            register.CurrentBalance = transaction.Amount; // Đặt lại số dư
                            break;
                    }

                    await WriteDataAsync(_cashRegistersFile, registers, _cashRegisterSemaphore);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating cash transaction: {ex.Message}");
                return false;
            }
        }

        public async Task<List<CashTransaction>> GetRecentTransactionsAsync(int count = 10)
        {
            try
            {
                var transactions = await ReadDataAsync<CashTransaction>(_cashTransactionsFile, _cashTransactionSemaphore);
                return transactions
                    .OrderByDescending(t => t.TransactionTime)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting recent transactions: {ex.Message}");
                return new List<CashTransaction>();
            }
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            try
            {
                var today = DateTime.Now.Date;
                var tomorrow = today.AddDays(1);

                var transactions = await ReadDataAsync<CashTransaction>(_cashTransactionsFile, _cashTransactionSemaphore);

                return ((transactions
                    .Where(t => t.TransactionTime >= today && t.TransactionTime < tomorrow)
                    .Where(t => t.Type == TransactionType.CashIn)
                    .Sum(t => t.Amount)) - (transactions.Where(t => t.TransactionTime >= today && t.TransactionTime < tomorrow)
                    .Where(t => t.Type == TransactionType.CashOut)
                    .Sum(t => t.Amount)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting today's revenue: {ex.Message}");
                return 0;
            }
        }

        public async Task<EndOfDayReport> CloseRegisterForDayAsync(decimal actualBalance, string notes)
        {
            try
            {
                // Lấy máy tính tiền đang hoạt động
                var activeRegister = await GetActiveCashRegisterAsync();

                // Tạo báo cáo kết thúc ngày
                var endOfDayReport = new EndOfDayReport
                {
                    Id = GenerateId(),
                    CashRegisterId = activeRegister.Id,
                    ReportDate = DateTime.Now,
                    SystemBalance = activeRegister.CurrentBalance,
                    ActualBalance = actualBalance,
                    Difference = actualBalance - activeRegister.CurrentBalance,
                    Notes = notes,
                    EmployeeId = "CurrentUser", // Cần thay thế bằng ID của người dùng hiện tại
                    IsApproved = false
                };

                // Lưu báo cáo
                var reports = await ReadDataAsync<EndOfDayReport>(_endOfDayReportsFile, _endOfDayReportSemaphore);
                reports.Add(endOfDayReport);
                await WriteDataAsync(_endOfDayReportsFile, reports, _endOfDayReportSemaphore);

                // Tạo giao dịch kết thúc ngày
                var endOfDayTransaction = new CashTransaction
                {
                    Id = GenerateId(),
                    CashRegisterId = activeRegister.Id,
                    Amount = actualBalance,
                    TransactionTime = DateTime.Now,
                    Type = TransactionType.EndOfDay,
                    Description = "End of day closing",
                    CreatedByEmployeeId = "CurrentUser", // Cần thay thế
                    Notes = $"System balance: {activeRegister.CurrentBalance}, Actual balance: {actualBalance}, Difference: {endOfDayReport.Difference}"
                };

                await CreateCashTransactionAsync(endOfDayTransaction);

                // Cập nhật thông tin máy tính tiền
                var registers = await ReadDataAsync<CashRegister>(_cashRegistersFile, _cashRegisterSemaphore);
                var register = registers.FirstOrDefault(r => r.Id == activeRegister.Id);

                if (register != null)
                {
                    register.CurrentBalance = actualBalance;
                    register.LastClosedAt = DateTime.Now;
                    await WriteDataAsync(_cashRegistersFile, registers, _cashRegisterSemaphore);
                }

                return endOfDayReport;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing register for day: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CashTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await ReadDataAsync<CashTransaction>(_cashTransactionsFile, _cashTransactionSemaphore);
                return transactions
                    .Where(t => t.TransactionTime >= startDate && t.TransactionTime < endDate.AddDays(1))
                    .OrderBy(t => t.TransactionTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transactions by date range: {ex.Message}");
                return new List<CashTransaction>();
            }
        }

        #endregion

        #region IDisposable implementation

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cashRegisterSemaphore.Dispose();
                    _cashTransactionSemaphore.Dispose();
                    _endOfDayReportSemaphore.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
