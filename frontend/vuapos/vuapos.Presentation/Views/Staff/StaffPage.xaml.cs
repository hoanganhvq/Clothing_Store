using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.Models;
using vuapos.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Staff

{
    public sealed partial class StaffPage : UserControl
    {
        public StaffViewModel ViewModel { get; }
        public StaffPage()
            {
                this.InitializeComponent();
                ViewModel = App.Services!.GetRequiredService<StaffViewModel>();
                this.DataContext = ViewModel;
                // Do usercontrol vs page thường load ui sau khi khởi tạo xong và gắn vào visual tree.
                this.Loaded += StaffPage_Loaded;
            }

            private void StaffPage_Loaded(object sender, RoutedEventArgs e)
            {
              
                if (this.XamlRoot != null)
                {
                    ViewModel.UpdateXamlRoot(this.XamlRoot);
                }
            }
    }

}
