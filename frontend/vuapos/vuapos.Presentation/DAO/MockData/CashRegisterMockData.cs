using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.MockData
{
    public static class CashRegisterMockData
    {
        public static List<CashRegister> GetMockCashRegisters()
        {
            return new List<CashRegister>
            {
                new CashRegister
                {
                    Id = "1",
                    Name = "Main Register",
                    CurrentBalance = 1000000m, // 1,000,000 VND
                    IsActive = true,
                    LastClosedAt = DateTime.Now.AddDays(-30),
                    CreatedBy =  "Admin",
                },
                new CashRegister
                {
                    Id = "2",
                    Name = "Secondary Register",
                    CurrentBalance = 500000m, // 500,000 VND
                    IsActive = false,
                    LastClosedAt = DateTime.Now.AddDays(-60),
                    CreatedBy = "Admin1"
                }
            };
        }

        public static List<CashTransaction> GetMockCashTransactions()
        {
            return new List<CashTransaction>
            {
                // Initial cash
                new CashTransaction
                {
                    Id = "1",
                    CashRegisterId = "1",
                    Amount = 1000000m,
                    TransactionTime = DateTime.Now.Date.AddHours(8), // 8:00 AM today
                    Type = TransactionType.InitialCash,
                    Description = "Initial cash for the day",
                    CreatedByEmployeeId = "2",
                    Notes = "Opening balance"
                },
                
                // Cash in transactions
                new CashTransaction
                {
                    Id = "2",
                    CashRegisterId = "2",
                    Amount = 150000m,
                    TransactionTime = DateTime.Now.Date.AddHours(9).AddMinutes(15), // 9:15 AM today
                    Type = TransactionType.CashIn,
                    Description = "Cash payment for order #1001",
                    CreatedByEmployeeId = "2",
                    Notes = "Customer paid in cash"
                },
                new CashTransaction
                {
                    Id = "3",
                    CashRegisterId = "1",
                    Amount = 75000m,
                    TransactionTime = DateTime.Now.Date.AddHours(10).AddMinutes(30), // 10:30 AM today
                    Type = TransactionType.CashIn,
                    Description = "Cash payment for order #1002",
                    CreatedByEmployeeId = "2",
                    Notes = ""
                },
                new CashTransaction
                {
                    Id = "4",
                    CashRegisterId = "1",
                    Amount = 225000m,
                    TransactionTime = DateTime.Now.Date.AddHours(11).AddMinutes(45), // 11:45 AM today
                    Type = TransactionType.CashIn,
                    Description = "Cash payment for order #1003",
                    CreatedByEmployeeId = "3",
                    Notes = ""
                },
                
                // Cash out transactions
                new CashTransaction
                {
                    Id = "5",
                    CashRegisterId = "1",
                    Amount = 50000m,
                    TransactionTime = DateTime.Now.Date.AddHours(12).AddMinutes(10), // 12:10 PM today
                    Type = TransactionType.CashOut,
                    Description = "Supplier payment",
                    CreatedByEmployeeId = "1",
                    Notes = "Paid for urgent delivery"
                },
                new CashTransaction
                {
                    Id = "6",
                    CashRegisterId = "1",
                    Amount = 30000m,
                    TransactionTime = DateTime.Now.Date.AddHours(14).AddMinutes(25), // 2:25 PM today
                    Type = TransactionType.CashOut,
                    Description = "Staff lunch",
                    CreatedByEmployeeId = "1",
                    Notes = ""
                },
                
                // Adjustment transaction
                new CashTransaction
                {
                    Id = "7",
                    CashRegisterId = "1",
                    Amount = 1370000m, // Adjusted total balance
                    TransactionTime = DateTime.Now.Date.AddHours(16), // 4:00 PM today
                    Type = TransactionType.Adjustment,
                    Description = "Mid-day count adjustment",
                    CreatedByEmployeeId = "1",
                    Notes = "Balance adjusted after cash count"
                },
                
                // Previous day transactions
                new CashTransaction
                {
                    Id = "8",
                    CashRegisterId = "1",
                    Amount = 100000m,
                    TransactionTime = DateTime.Now.AddDays(-1).Date.AddHours(9), // 9:00 AM yesterday
                    Type = TransactionType.CashIn,
                    Description = "Cash payment for order #995",
                    CreatedByEmployeeId = "2",
                    Notes = ""
                },
                new CashTransaction
                {
                    Id = "9",
                    CashRegisterId = "1",
                    Amount = 1200000m, // Final balance for yesterday
                    TransactionTime = DateTime.Now.AddDays(-1).Date.AddHours(20), // 8:00 PM yesterday
                    Type = TransactionType.EndOfDay,
                    Description = "End of day closing",
                    CreatedByEmployeeId = "2",
                    Notes = "System balance: 1190000, Actual balance: 1200000, Difference: 10000"
                }
            };
        }

        public static List<EndOfDayReport> GetMockEndOfDayReports()
        {
            return new List<EndOfDayReport>
            {
                new EndOfDayReport
                {
                    Id = "1",
                    CashRegisterId = "1",
                    ReportDate = DateTime.Now.AddDays(-1).Date.AddHours(20), // 8:00 PM yesterday
                    SystemBalance = 1190000m,
                    ActualBalance = 1200000m,
                    Difference = 10000m,
                    Notes = "Small overage found during counting",
                    EmployeeId = "1",
                    IsApproved = true,
                    ApprovedByEmployeeId = "4", // Manager ID
                },
                new EndOfDayReport
                {
                    Id = "2",
                    CashRegisterId = "2",
                    ReportDate = DateTime.Now.AddDays(-2).Date.AddHours(20), // 8:00 PM two days ago
                    SystemBalance = 480000m,
                    ActualBalance = 475000m,
                    Difference = -5000m,
                    Notes = "Small shortage found",
                    EmployeeId = "3",
                    IsApproved = true,
                    ApprovedByEmployeeId = "4", // Manager ID
                }
            };
        }

        // Sample method to create a new cash transaction for testing
        public static CashTransaction CreateMockCashInTransaction()
        {
            return new CashTransaction
            {
                CashRegisterId = "1",
                Amount = 125000m,
                Type = TransactionType.CashIn,
                Description = "Cash payment for order #1005",
                Notes = "Test transaction"
            };
        }

        // Sample method to create a new end-of-day scenario for testing
        public static EndOfDayTestData CreateMockEndOfDayData()
        {
            return new EndOfDayTestData
            {
                ActualBalance = 1450000m,
                Notes = "End of day test closing"
            };
        }
    }

    // Helper class for end-of-day testing
    public class EndOfDayTestData
    {
        public decimal ActualBalance { get; set; }
        public string Notes { get; set; }
    }
}
