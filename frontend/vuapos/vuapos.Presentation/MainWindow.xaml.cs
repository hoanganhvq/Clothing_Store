using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.ViewModels;
using vuapos.Presentation.Views.CashRegister;
using vuapos.Presentation.Views.Category;
using vuapos.Presentation.Views.Customer;
using vuapos.Presentation.Views.FrequentlyBoughtTogether;
using vuapos.Presentation.Views.Login;
using vuapos.Presentation.Views.Module1;
using vuapos.Presentation.Views.Order;
using vuapos.Presentation.Views.Product;
using vuapos.Presentation.Views.Promotion;
using vuapos.Presentation.Views.Report;
using vuapos.Presentation.Views.Staff;
using Windows.Devices.PointOfService;
using Windows.Media.Core;

namespace vuapos.Presentation
{
    public sealed partial class MainWindow : Window
    {
        private readonly IUserSession _userSession;
        private readonly LoginViewModel _loginViewModel;

        private CustomerPage customerPage;
        private CategoryPage categoryPage;
        private ProductPage productPage;
        private PromotionPage promotionPage;
        private FrequentlyBoughtTogether frequentlyBoughtTogetherPage;
        private StaffPage staffPage;
        private OrderPage orderPage;
        private CashRegisterPage cashRegisterPage;
        private ReportPage reportPage;


        //login
        private Grid rootGrid;
        private LoginUserControl loginControl;


        public MainWindow()
        {
            this.InitializeComponent();

            _userSession = App.Services!.GetRequiredService<IUserSession>();
            _loginViewModel = App.Services!.GetRequiredService<LoginViewModel>();
            _loginViewModel.LoginSuccessful += LoginViewModel_LoginSuccessful;


            // Tạo trang đăng nhập
            rootGrid = this.Content as Grid;

            // Tạo và thêm control đăng nhập
            loginControl = new LoginUserControl();

            // Kiểm tra trạng thái đăng nhập
            if (_userSession.Token == null)
            {
                // Hiển thị trang đăng nhập
                ShowLoginScreen();
            }
            else
            {
                Debug.WriteLine("MainWindow constructor called");
                // Người dùng đã đăng nhập, hiển thị giao diện chính
                InitializeApp();

            }
        }

        private void ShowLoginScreen()
        {
            // Ẩn NavigationView
            MainNavigationView.Visibility = Visibility.Collapsed;

            // Thêm LoginUserControl vào Grid
            if (!rootGrid.Children.Contains(loginControl))
            {
                rootGrid.Children.Add(loginControl);
            }
            loginControl.Visibility = Visibility.Visible;
        }

        private void HideLoginScreen()
        {
            // Ẩn LoginUserControl
            if (loginControl != null)
            {
                loginControl.Visibility = Visibility.Collapsed;
            }

            // Hiển thị NavigationView
            MainNavigationView.Visibility = Visibility.Visible;
        }

        private void LoginViewModel_LoginSuccessful(object sender, System.EventArgs e)
        {
            // Ẩn màn hình đăng nhập
            HideLoginScreen();

            // Hiển thị giao diện chính
            InitializeApp();
        }

        private void InitializeApp()
        {
            //// Ẩn trang đăng nhập
            MainNavigationView.Visibility = Visibility.Visible;

            // Khởi tạo các trang
            customerPage = new CustomerPage();
            categoryPage = new CategoryPage();
            productPage = new ProductPage();
            promotionPage = new PromotionPage();
            frequentlyBoughtTogetherPage = new FrequentlyBoughtTogether();
            // Set default selected item
            staffPage = new StaffPage();
            orderPage = new OrderPage();
            cashRegisterPage = new CashRegisterPage();
            reportPage = new ReportPage();

            // Kiểm tra quyền và hiển thị các mục phù hợp
            ConfigureNavigationItemsByRole();
            LoadUserInfo();

            // Chọn trang đầu tiên
            MainNavigationView.SelectedItem = MainNavigationView.MenuItems[0];
        }

        private void ConfigureNavigationItemsByRole()
        {
            // Ẩn/hiện các mục menu dựa trên vai trò người dùng
            if (_userSession.role != "MANAGER")
            {
                staffsTag.Visibility = Visibility.Collapsed;
            }

        }


        private void LoadUserInfo()
        {
            // Lấy thông tin người dùng hiện tại từ IUserSession
            UserNameTextBlock.Text = _userSession.Username;
            UserRoleTextBlock.Text = _userSession.role;
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem selectedItem)
            {
                string tag = selectedItem.Tag.ToString();

                if (tag == "logout")
                {
                    HandleLogout();
                    return;
                }

                switch (tag)
                {
                    case "customers":
                        MainLayout.Title = "Customers";
                        MainLayout.PageContent = customerPage;
                        break;

                    case "categories":
                        MainLayout.Title = "Categories";
                        MainLayout.PageContent = categoryPage;
                        break;

                    case "products":
                        MainLayout.Title = "Products";
                        MainLayout.PageContent = productPage;
                        break;

                    case "promotions":
                        MainLayout.Title = "Promotions";
                        MainLayout.PageContent = promotionPage;
                        break;
                    case "frequentlyboughttogether":
                        MainLayout.Title = "Frequently Bought Together Products";
                        MainLayout.PageContent = frequentlyBoughtTogetherPage;
                        break;
                    case "staffs":
                        MainLayout.Title = "Staffs";
                        MainLayout.PageContent = staffPage;
                        break;

                    case "orders":
                        MainLayout.Title = "Orders";
                        MainLayout.PageContent = orderPage;
                        break;

                    case "cash":
                        MainLayout.Title = "Cash Register";
                        MainLayout.PageContent = cashRegisterPage;
                        break;

                    case "reports":
                        MainLayout.Title = "Reports";
                        MainLayout.PageContent = reportPage;
                        break;
                }
            }
        }

        private void HandleLogout()
        {
            var authService = App.Services!.GetRequiredService<IAuthService>();
            authService.Logout();
            ShowLoginScreen();
        }


    }
}
