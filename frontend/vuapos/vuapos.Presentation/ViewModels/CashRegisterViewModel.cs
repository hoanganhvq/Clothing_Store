using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using vuapos.Presentation.Commands;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.CashRegister;

namespace vuapos.Presentation.ViewModels
{
    public class CashRegisterViewModel : INotifyPropertyChanged
    {
        private readonly ICashRegisterService _cashRegisterService;
        private CashRegister _activeRegister;
        private decimal _todayRevenue;
        private ObservableCollection<CashTransactionViewModel> _recentTransactions;
        private bool _isLoading;
        private XamlRoot _xaml;


        private decimal _actualBalance;
        private string _noteEndOfDay;


        public decimal Difference => _activeRegister.CurrentBalance - _actualBalance;

        public decimal ActualBalance
        {
            get => _actualBalance;
            set
            {
                SetProperty(ref _actualBalance, value);
                OnPropertyChanged(nameof(Difference));
            }
        }


        public string NoteEndOfDay
        {
            get => _noteEndOfDay;
            set => SetProperty(ref _noteEndOfDay, value);
        }

        public ObservableCollection<TransactionTypeItem> TransactionTypes { get; }

        private CashTransaction _cashTransaction;
        public CashTransaction CashTransaction
        {
            get => _cashTransaction;
            set => SetProperty(ref _cashTransaction, value);
        }

        public EndOfDayReport Report = new EndOfDayReport();


        // Command để thêm giao dịch mới
        public ICommand AddTransactionCommand { get; }

        // Command để kết số cuối ngày
        public ICommand EndOfDayCommand { get; }

        // Command để tải lại dữ liệu
        public ICommand RefreshDataCommand { get; }



        public ICommand FilterCommand { get; }

        public CashRegisterViewModel(ICashRegisterService cashRegisterService)
        {
            _cashRegisterService = cashRegisterService;
            RecentTransactions = new ObservableCollection<CashTransactionViewModel>();

            AddTransactionCommand = new RelayCommand(_ => ShowAddTransactionDialog());
            EndOfDayCommand = new RelayCommand(_ => ShowEndOfDayDialog());
            RefreshDataCommand = new RelayCommand(_ => LoadData());
            FilterCommand = new RelayCommand(_ => LoadDataFilter());

            // Tải dữ liệu ban đầu

            TransactionTypes = new ObservableCollection<TransactionTypeItem>
            {
                new TransactionTypeItem { Value1 = TransactionType.CashIn, Display = "Cash In" },
                new TransactionTypeItem { Value1 = TransactionType.CashOut, Display = "Cash Out" },
                new TransactionTypeItem { Value1 = TransactionType.Adjustment, Display = "Adjustment" },
            };

            LoadData();
        }

        private async void LoadDataFilter()
        {
            try
            {
                var fromDate = FromDate.DateTime;
                var toDate = ToDate.DateTime.AddDays(1).AddSeconds(-1); // Bao gồm cả ngày kết thúc
                IsLoading = true;
                var transactions = await _cashRegisterService.GetTransactionsByDateRangeAsync(fromDate, toDate);
                RecentTransactions.Clear();
                foreach (var transaction in transactions)
                {
                    RecentTransactions.Add(new CashTransactionViewModel(transaction));
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                Debug.WriteLine($"Lỗi khi tải dữ liệu két tiền: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void UpdateXamlRoot(XamlRoot xaml)
        {
            _xaml = xaml;
        }

        public CashRegister ActiveRegister
        {
            get => _activeRegister;
            set => SetProperty(ref _activeRegister, value);
        }

        public decimal TodayRevenue
        {
            get => _todayRevenue;
            set => SetProperty(ref _todayRevenue, value);
        }

        public string? ErrorMessage { get; set; } = string.Empty;

        public ObservableCollection<CashTransactionViewModel> RecentTransactions
        {
            get => _recentTransactions;
            set => SetProperty(ref _recentTransactions, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public DateTimeOffset FromDate { get; set; }

        public DateTimeOffset ToDate { get; set; }

        public async void LoadData()
        {
            try
            {
                IsLoading = true;

                // Tải thông tin két tiền đang hoạt động
                ActiveRegister = await _cashRegisterService.GetActiveCashRegisterAsync();

                // Tải doanh thu hôm nay
                TodayRevenue = await _cashRegisterService.GetTodayRevenueAsync();

                // Tải các giao dịch gần đây
                var transactions = await _cashRegisterService.GetRecentTransactionsAsync();
                RecentTransactions.Clear();
                foreach (var transaction in transactions)
                {
                    RecentTransactions.Add(new CashTransactionViewModel(transaction));
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                Debug.WriteLine($"Lỗi khi tải dữ liệu két tiền: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void ShowAddTransactionDialog()
        {
            _cashTransaction = new CashTransaction
            {
                Type = TransactionType.CashIn,
                Amount = 0,
                Description = "",
                ReferenceNumber = "",
                CreatedByEmployeeId = "",
                TransactionTime = DateTime.Now
            };
            // Hiển thị dialog thêm giao dịch mới
            var dialog = new ContentDialog
            {
                Title = "Add New Transaction",
                PrimaryButtonText = "Add",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = _xaml,
                Content = new AddTransactionDialog(this)
            };


            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var transaction = _cashTransaction;

                if (transaction.Type == TransactionType.Adjustment)
                {
                    if (App.Services!.GetRequiredService<IUserSession>().role != "MANAGER")
                    {
                        var dialogService = App.Services!.GetRequiredService<IDialogService>();
                        await dialogService.ShowMessageAsync(_xaml, "You do not have permission to perform this action", "Access Denied");
                        return;
                    }
                }
                var success = await _cashRegisterService.CreateCashTransactionAsync(transaction);

                if (success)
                {
                    // Tải lại dữ liệu sau khi thêm giao dịch thành công
                     LoadData();
                }
            }
        }

        private async void ShowEndOfDayDialog()
        {
            ActualBalance = ActiveRegister.CurrentBalance;
            if (App.Services!.GetRequiredService<IUserSession>().role != "MANAGER")
                {
                    var dialogService = App.Services!.GetRequiredService<IDialogService>();
                    await dialogService.ShowMessageAsync(_xaml, "You do not have permission to perform this action", "Access Denied");
                    return;
             }
            var dialog = new ContentDialog
            {
                Title = "End of Day Summary",
                PrimaryButtonText = "Confirm",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = _xaml,
                Content = new EndOfDayDialog(this)
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    Debug.WriteLine($"Kết số cuối ngày: {ActualBalance}");
                    var report = await _cashRegisterService.CloseRegisterForDayAsync(
                        ActualBalance,
                        NoteEndOfDay
                    );

                    Report = report;


                    var resultDialog = new ContentDialog
                    {
                        Title = "End of Day Result",
                        CloseButtonText = "Close",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = _xaml,
                        Content = new EndOfDayResultDialog(this)
                    };
                    await resultDialog.ShowAsync();

                    //// Tải lại dữ liệu sau khi kết số thành công
                    LoadData();
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    var errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Không thể kết số cuối ngày: {ex.Message}",
                        CloseButtonText = "Đóng",
                        XamlRoot = _xaml
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
