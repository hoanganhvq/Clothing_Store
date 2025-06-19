﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using vuapos.Presentation.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Login
{
    public sealed partial class LoginUserControl : UserControl
    {
        public LoginViewModel ViewModel { get; }

        public LoginUserControl()
        {
            this.InitializeComponent();
            ViewModel = App.Services!.GetRequiredService<LoginViewModel>();
            Debug.WriteLine("Clicked from code-behind");
        }

        private void SignIn_Click(object sender, RoutedEventArgs eventArgs) 
        {
           
            if (ViewModel != null && ViewModel.LoginCommand.CanExecute(null))
            {
                ViewModel.LoginCommand.Execute(null);
            }
            else
            {
                Debug.WriteLine("ViewModel is null or command cannot execute.");
            }
        }

    }
}
