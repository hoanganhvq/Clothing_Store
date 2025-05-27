using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.Services;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace vuapos.Presentation.Views.OrderDetail
{
 
    public sealed partial class OrderDetailPage : Window
    {
       public OrderDetailViewModel ViewModel { get; set; }  

        public OrderDetailPage(OrderViewModel orderViewModel)
        {
            this.InitializeComponent();
            var factory = App.Services!.GetRequiredService<Func<OrderViewModel, OrderDetailViewModel>>();
            ViewModel = factory(orderViewModel);
            ViewModel.SetWindow(this);
        }

    }
}
