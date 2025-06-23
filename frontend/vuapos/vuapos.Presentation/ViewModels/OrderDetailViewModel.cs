using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using vuapos.Presentation.Commands;
using vuapos.Presentation.DTO.Order;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.ViewModels
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly IDialogService _dialogService;
        private Window _window;

        private readonly OrderViewModel _orderViewModel;
        private Order _currentOrder = new Order();
        private string _searchQuery;
        private ObservableCollection<Product> _searchResults;

        private Product _selectedProduct;
        private int _productQuantity = 1;
        private bool _userCustomerPoints = false;

        public string PromotionCode { get; set; } = string.Empty;
        public string VisibilityUI { get; set; } = string.Empty;


        public decimal SubTotal => OrderDetails.Sum(od => od.Price);

        public decimal TotalDiscount { get; set; } = 0;

        public bool UseCustomerPoints
        {
            get => _userCustomerPoints;
            set
            {
               SetProperty(ref _userCustomerPoints, value);
                if (value)
                {
                    TotalDiscount += CustomerPointsValue;
                    OnPropertyChanged(nameof(TotalDiscount));
                    OnPropertyChanged(nameof(OrderTotal));
                }
                else
                {
                    TotalDiscount -= CustomerPointsValue;
                    OnPropertyChanged(nameof(TotalDiscount));
                    OnPropertyChanged(nameof(OrderTotal));
                }
            }
        }


        public decimal CustomerPointsValue { get; set; } = 0;
       


        public OrderDetailViewModel(OrderService orderService, ProductService productService, IDialogService dialogService , OrderViewModel orderViewModel)
        {
            _orderService = orderService;
            _productService = productService;
            _orderViewModel = orderViewModel;
            _dialogService = dialogService;

            // Initialize a new order
            SearchResults = new ObservableCollection<Product>();
            OrderDetails = new ObservableCollection<OrderDetail>();

            // Commands
            SearchProductCommand = new RelayCommand(async _ => await SearchProductsAsync());
            AddProductCommand = new RelayCommand(_ => AddProductToOrder(), _ => CanAddProduct());
            RemoveOrderDetailCommand = new RelayCommand(parameter => RemoveOrderDetail(parameter as OrderDetail));
            SaveOrderCommand = new RelayCommand(async _ => await SaveOrderAsync());
            ApplyPromotionCodeCommand = new RelayCommand (async _ => ApplyPromotionCode());
            CloseWindowCommand = new RelayCommand(_ => _window.Close(), _ => true);

            _ = LoadOrderDetail();
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private async Task LoadOrderDetail()
        {

            VisibilityUI = "Visible";

            if (_orderViewModel.SelectedOrder != null)
            {
                _currentOrder = _orderViewModel.SelectedOrder;
                CustomerName = _currentOrder.customer.Name;
                CustomerPhone = _currentOrder.customer.Phone;
                CustomerMail = _currentOrder.customer.Email;
                if (_currentOrder.Order_status == "Đã thanh toán")
                {

                    VisibilityUI = "Collapsed";
                }


                    // Load order details
                    foreach (var orderDetail in _currentOrder.OrderDetails)
                    {
                        OrderDetails.Add(orderDetail);
                    }
            }
            else
            {
                // Handle the case when no order is selected
                Debug.WriteLine("No order selected.");
            }
        }
        private async void ApplyPromotionCode()
        {
            var res = await _orderService.GetPromotionOrder(PromotionCode);

            if (res == null)
            {
                //thông báo giảm giá không thành công
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Error", "The discount code is invalid");
                return;
            }

            try
            {
                if (DateTime.TryParse(res.Data[0].Start_date, out var startDate) &&
                    DateTime.TryParse(_currentOrder.Order_Date, out var orderDate) &&
                    (startDate > DateTime.Now || orderDate > DateTime.Now))
                {
                    // thông báo mã giảm giá không hợp lệ
                    await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Error", "The discount code has expired.");
                    return;
                }
            }
            catch
            {
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Error", "The discount code is invalid.");
                return;
            }

                var discount = Convert.ToDecimal(res!.Data[0].Discount_percentage) / 100;

            await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, $"Discount code {res!.Data[0].Name}", $"Discount {res!.Data[0].Discount_percentage}%");


            TotalDiscount = UseCustomerPoints ? (CustomerPointsValue + OrderTotal * discount) : (OrderTotal * discount);

            // Update the UI
            OnPropertyChanged(nameof(TotalDiscount));
            OnPropertyChanged(nameof(OrderTotal));

        }
 
        public string CustomerPhone
        {
            get => _currentOrder.customer.Phone ?? string.Empty;
            set
            {
                if (_currentOrder.customer.Phone != value)
                {
                    _currentOrder.customer.Phone = value;
                    _ = LoadId();
                    OnPropertyChanged();
                }
            }
        }

        public string CustomerName
        {
            get => _currentOrder.customer.Name ?? string.Empty;
            set
            {
                if (_currentOrder.customer.Name != value)
                {
                    _currentOrder.customer.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CustomerMail
        {
            get => _currentOrder.customer.Email ?? string.Empty;
            set
            {
                if (_currentOrder.customer.Email != value)
                {
                    _currentOrder.customer.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public XamlRoot XamlRoot { get; set; }

        public bool IsCash = false;


        public decimal OrderTotal => SubTotal - TotalDiscount;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> SearchResults
        {
            get => _searchResults;
            set
            {
               SetProperty(ref _searchResults, value);
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                // When selected product changes, notify that CanAddProduct might have changed
                (AddProductCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public int ProductQuantity
        {
            get => _productQuantity;
            set
            {
                if (value < 1) value = 1;
                _productQuantity = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand SearchProductCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand RemoveOrderDetailCommand { get; }
        public ICommand SaveOrderCommand { get; }
        public ICommand ApplyPromotionCodeCommand { get; }

        private async Task SearchProductsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;
            // Search for products
            var products = await _productService.SearchProductsAsync(SearchQuery);
            SearchResults.Clear();

            foreach (var product in products.Data)
            {
                SearchResults.Add(product);
            }
        }

        private bool CanAddProduct()
        {
            return SelectedProduct != null && ProductQuantity > 0;
        }

        private void AddProductToOrder()
        {
            if (SelectedProduct == null)
                return;


            // Nếu đã có sản phẩm trong danh sách OrderDetails, chỉ cần cập nhật số lượng
            var existingDetail = OrderDetails.FirstOrDefault(od => od.Product_id == SelectedProduct.Product_Id);

            if (existingDetail != null)
            {
                // Cập nhật số lượng
                existingDetail.Quantity += ProductQuantity;
                // UI cập nhật tự động vì OrderDetail implements INotifyPropertyChanged
                RefreshOrderDetails();
            }
            else
            {
                // Thêm sản phẩm mới vào danh sách OrderDetails
                var orderDetail = new OrderDetail
                {
                    Product_id = SelectedProduct.Product_Id,
                    Product = SelectedProduct,
                    Quantity = ProductQuantity,
                };

                OrderDetails.Add(orderDetail);
            }

            // Reset
            ProductQuantity = 1;
            OnPropertyChanged(nameof(OrderTotal));
            OnPropertyChanged(nameof(SubTotal));
            (SaveOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void RemoveOrderDetail(OrderDetail orderDetail)
        {
            if (orderDetail != null)
            {
                OrderDetails.Remove(orderDetail);
                OnPropertyChanged(nameof(OrderTotal));
                OnPropertyChanged(nameof(SubTotal));
                (SaveOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void RefreshOrderDetails()
        {
            OnPropertyChanged(nameof(OrderDetails));
            OnPropertyChanged(nameof(OrderTotal));
        }


        private bool CanSaveOrder()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                   !string.IsNullOrWhiteSpace(CustomerPhone) &&
                   !string.IsNullOrWhiteSpace(CustomerMail) &&
                   OrderDetails.Count > 0;
        }

        private async Task LoadId()
        {
            if (CustomerPhone.Length >= 10)
            {
                var customerService = App.Services.GetRequiredService<CustomerService>();
                var customer = await customerService.SearchCustomersAsync(CustomerPhone);
                if (customer.Data.Count == 0)
                {
                    Debug.WriteLine($"Customer with phone {CustomerPhone} not found.");
                    return;
                }
                _currentOrder.Customer_Id = customer.Data[0].Customer_Id;
                Debug.WriteLine(_currentOrder.Customer_Id);
                _currentOrder.customer.Name = customer.Data[0].Name;
                _currentOrder.customer.Email = customer.Data[0].Email;
                CustomerPointsValue = customer.Data[0].Point;
                OnPropertyChanged(nameof(CustomerName));
                OnPropertyChanged(nameof(CustomerPointsValue));
                OnPropertyChanged(nameof(CustomerMail));
            }

        }

        private async Task SaveOrderAsync()

        {
       
            if (!CanSaveOrder())
            {
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Error", "Please fill in all customer and product information.");

                return;
            }

            // get customer by name and staff id
            try
            {
                await LoadId();
                if (_currentOrder.Customer_Id == string.Empty)
                {
                    await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Error", "Customer does not exist. Please create a new customer.");

                    return;
                }
            }
            catch
            {
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Error", "The customer does not exist. Please create a new customer.");

                return;
            }

                var order = new Order
                {
                    Order_Id = Guid.NewGuid().ToString(),
                    Customer_Id = _currentOrder.Customer_Id,
                    Staff_Id = App.Services!.GetRequiredService<IUserSession>().UserId,
                    Order_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    customer = new Customer
                    {
                        Name = CustomerName,
                        Phone = CustomerPhone,
                        Email = CustomerMail,
                    },
                    Total_Amount = OrderTotal,
                    OrderDetails = OrderDetails,
                    Order_status = "Đang xử lí"
                };
      

            if (IsCash)
            {
                OrderCreateDTO orderCreate = new OrderCreateDTO
                {
                    customer_id = order.Customer_Id,
                    staff_id = order.Staff_Id,
                    total_amount = order.Total_Amount,
                };
                var response = await _orderService.CreateOrder(orderCreate);
                try
                {
                    List<OrderDetailCreateDTO> items = new List<OrderDetailCreateDTO>();

                    foreach (var orderDetail in OrderDetails)
                    {
                        OrderDetailCreateDTO item = new OrderDetailCreateDTO
                        {
                            order_id = response.Order_id,
                            product_id = orderDetail.Product_id,
                            quantity = orderDetail.Quantity,
                            price = orderDetail.Price,
                        };
                        items.Add(item);
                    }
                  
                    var res = await _orderService.CreateOrderDetail(new OrderDetailCreateDTOList { items = items });
                    if (res.Count > 0)
                    {
                        if (_orderViewModel.SelectedOrder != null)
                        {
                            _orderViewModel.OrdersTemp.Remove(_orderViewModel.SelectedOrder);
                            await _orderViewModel.LoadOrders();
                        }
                        var cashTransaction = new CashTransaction
                        {
                            Amount = order.Total_Amount,
                            TransactionTime = DateTime.Now,
                            Type = TransactionType.CashIn,
                            Description = $"Paid order by customer {CustomerName}",
                            CreatedByEmployeeId = order.Staff_Id,
                            ReferenceNumber = response.Order_id,
                            Notes = $"Order of customer {CustomerName}"
                        };
                        var cashSVervice = App.Services!.GetRequiredService<ICashRegisterService>();
                        await cashSVervice.CreateCashTransactionAsync(cashTransaction);
                        await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Notification", "The paid order has been successfully added.");
                        _window.Close();
                    }
                    else await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Notification", "Failed to add the order.");

                    return;
                }
                catch
                {
                    await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Notification", "Failed to add the order.");
                    return;
                }
            }

            if (_orderViewModel.SelectedOrder != null)
            {
                var index = _orderViewModel.OrdersTemp.IndexOf(_orderViewModel.SelectedOrder);
                if (index >= 0)
                {
                    _orderViewModel.OrdersTemp[index] = order;
                    await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Notification", "Order updated successfully.");

                    _window.Close();
                }
            }
            else
            {
             
                _orderViewModel.OrdersTemp.Add(order);
                await _orderViewModel.LoadOrders();
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Notification", "Order added successfully.");
                _window.Close();
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
