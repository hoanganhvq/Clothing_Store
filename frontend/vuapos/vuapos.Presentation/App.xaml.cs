using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.DAO.MockData;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using Windows.Devices.SerialCommunication;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.ViewModels;
using vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels;

namespace vuapos.Presentation
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }
        public static Window? m_window { get; private set; }

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            //httpclient
            services.AddHttpClient<ApiService>();
            services.AddHttpClient<CustomerService>();
            services.AddHttpClient<CategoryService>();
            services.AddHttpClient<StaffService>();
            services.AddHttpClient<ProductService>();
            services.AddHttpClient<OrderService>();
            services.AddSingleton<CloudinaryService>();
            services.AddSingleton<PromotionService>();
            services.AddSingleton<FrequentlyBoughtTogetherService>();


            //services
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IUserSession, UserSession>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ICashRegisterService, CashRegisterFileService>();

            //main
            services.AddSingleton<MainWindow>();


            // viewmodel
            services.AddTransient<ProductSearchViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddTransient<StaffViewModel>();
            services.AddTransient<CashRegisterViewModel>();
            services.AddTransient<PaginationViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<PromotionViewModel>();
            services.AddTransient<ReportViewModel>();


            services.AddTransient<Func<OrderViewModel, OrderDetailViewModel>>(provider => (orderViewModel) => {
                var orderService = provider.GetRequiredService<OrderService>();
                var productService = provider.GetRequiredService<ProductService>();
                var dialogService = provider.GetRequiredService<IDialogService>();
                // Create and return the OrderDetailViewModel instance
                return new OrderDetailViewModel(orderService, productService, dialogService, orderViewModel);
            });


            Services = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
