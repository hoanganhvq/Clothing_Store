using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using vuapos.Presentation.Commands;
using vuapos.Presentation.Models;
using vuapos.Presentation.DAO.Interface;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows.Input;
using vuapos.Presentation.Services;
using System.Threading.Tasks;
using System;
using vuapos.Presentation.DTO.Login;
using vuapos.Presentation.Services.Interfaces;
using Windows.Web.UI;
using Microsoft.UI.Xaml;
using System.Net.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels;

namespace vuapos.Presentation.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _orders;
        private ObservableCollection<Order> _ordersTemp;
        private readonly OrderService _orderService;
        private Order _selectedOrder;
        private bool _isLoading = false;

        private XamlRoot _xamlRoot;

        public void UpdateXamlRoot(XamlRoot xamlRoot)
        {
            _xamlRoot = xamlRoot;
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

        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Order> OrdersTemp
        {
            get => _ordersTemp;
            set
            {
                _ordersTemp = value;
                OnPropertyChanged();
            }
        }

        public PaginationViewModel PaginationViewModel { get; private set; }

        public ICommand RemoveOrderCommand { get;  }
        public ICommand SendMailCommand { get; }
        public OrderViewModel(OrderService orderService)
        {
            _orderService = orderService;
            // Giả lập dữ liệu đơn hàng
            PaginationViewModel = new PaginationViewModel();
            PaginationViewModel.LoadItemsForCurrentPageCommand = new RelayCommand(async _ => await LoadOrdersForCurrentPage());

            _orders = new ObservableCollection<Order>();
            _ordersTemp = new ObservableCollection<Order>();
            RemoveOrderCommand = new RelayCommand<Order>(RemoveOrder);
            SendMailCommand = new RelayCommand<Order>(async p => SendMail(p));
            _ = LoadOrders();

        }

        private async Task LoadOrdersForCurrentPage()
        {
            var responseOrder = await _orderService.GetAllOrdersAsync(PaginationViewModel.CurrentPage);
            if (responseOrder != null)
            {
                Orders.Clear();
                // thêm đang xử lí;
                foreach (var order in OrdersTemp)
                {
                    Orders.Add(order);
                }

                //xử lí xong;
                foreach (var order in responseOrder.Data)
                {
                    Orders.Add(order);
                }
            }
            else
            {
                Orders.Clear();
            }
        }

        public async Task LoadOrdersByDate(string startDate, string endDate)
        {
            var responseOrder = await _orderService.GetOrderByDate(startDate, endDate);
            if (responseOrder == null || responseOrder.Data == null) return;
            Orders.Clear();
            // thêm đang xử lí;
            foreach (var order in OrdersTemp)
            {
                Orders.Add(order);
            }

            //xử lí xong;
            foreach (var order in responseOrder.Data)
            {
                Orders.Add(order);
            }
            PaginationViewModel.Initialize(responseOrder.TotalCount);
        }

        public async Task LoadOrders()
        {
            var responseOrder = await _orderService.GetAllOrdersAsync(1);
            if (responseOrder == null || responseOrder.Data == null) return;
            Orders.Clear();
            // thêm đang xử lí;
            foreach (var order in OrdersTemp)
            {
                Orders.Add(order);
            }
            
            //xử lí xong;
            foreach (var order in responseOrder.Data)
            {
               Orders.Add(order);
            }
            PaginationViewModel.Initialize(responseOrder.TotalCount);
        }

        private void RemoveOrder(Order order)
        {
            _ordersTemp.Remove(order);
            _ = LoadOrders();
        }

        private async Task SendMail(Order order)
        {
            // Gửi mail
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));
            var dialogService = App.Services!.GetRequiredService<IDialogService>();
            try
            {
                var result = await _orderService.SendMail(order.Order_Id);
                if (result == null)
                {
                    await dialogService.ShowMessageAsync(_xamlRoot, "Error", "Email sending failed");

                    return;
                }
                await dialogService.ShowMessageAsync(_xamlRoot, "Notification", "Email sent successfully");


            }
            catch (Exception ex)
            {
                await dialogService.ShowMessageAsync(_xamlRoot, "Error", "Failed to send email");

            }
            finally
            {
                IsLoading = false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
