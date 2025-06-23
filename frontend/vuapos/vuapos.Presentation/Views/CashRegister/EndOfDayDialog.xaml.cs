using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.CashRegister
{
    public sealed partial class EndOfDayDialog : UserControl
    {

        private CashRegisterViewModel ViewModel;
        public EndOfDayDialog(CashRegisterViewModel viewModel)
        {
            this.InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = viewModel;
        }

        private void ActualBalanceBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var difference = ViewModel.Difference;
            // Hiển thị màu khác nhau dựa trên chênh lệch
            if (difference < 0)
                differenceText.Foreground = new SolidColorBrush(Colors.Red);
            else if (difference > 0)
                differenceText.Foreground = new SolidColorBrush(Colors.Green);
            else
                differenceText.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
