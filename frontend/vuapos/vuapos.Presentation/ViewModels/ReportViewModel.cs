
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;

namespace vuapos.Presentation.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _reportService;
        private DateTimeOffset _startDate = DateTime.Now.AddDays(-30);
        private DateTimeOffset _endDate = DateTime.Now;
        private bool _isLoading;
        private string _errorMessage;
        private ObservableCollection<Report> _reports;
        private decimal _totalSales;
        private int _totalItems;

        public ReportViewModel(OrderService reportService)
        {
            _reportService = reportService;
            Reports = new ObservableCollection<Report>();
            LoadReportCommand = new RelayCommand(async _ => await LoadReportDataAsync());
        }

        public DateTimeOffset StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTimeOffset EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set
            {
                if (_reports != value)
                {
                    _reports = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalSales
        {
            get => _totalSales;
            set
            {
                if (_totalSales != value)
                {
                    _totalSales = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                if (_totalItems != value)
                {
                    _totalItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadReportCommand { get; }

        public async Task LoadReportDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                string startDate = StartDate.ToString("yyyy-MM-dd");
                string endDate = EndDate.ToString("yyyy-MM-dd");

                Response<Order> res = await _reportService.GetOrderByDate(startDate, endDate);
                Reports.Clear();
                for (var i = 0; i < res.TotalPages; i++)
                {
                    Response<Order> resItem = await _reportService.GetOrderByDate(startDate, endDate, i + 1);
                    foreach (var item in resItem.Data)
                    {
                        foreach (var order in item.OrderDetails)
                        {
                            var existingReport = Reports.FirstOrDefault(r => r.NameProduct == order.Product.Product_Name);
                            if (existingReport != null)
                            {
                                existingReport.quantity += order.Quantity;
                                existingReport.total += order.Price;

                            }
                            else
                            {
                                Reports.Add(new Report
                                {
                                    NameProduct = order.Product.Product_Name,
                                    quantity = order.Quantity,
                                    price = order.Product.Price,
                                    total = order.Price//order.Quantity * order.Price
                                });
                            }
                        }
                    }
                }

                // Update statistics
                TotalSales = Reports.Sum(r => r.total);
                TotalItems = Reports.Sum(r => r.quantity);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Lớp RelayCommand đơn giản hóa việc tạo command
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
