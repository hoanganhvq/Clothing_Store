using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.Services
{
    public class DialogService : IDialogService
    {

        public async Task ShowMessageAsync(XamlRoot xamlRoot, string title, string content)
        {
            if (xamlRoot == null)
                throw new InvalidOperationException("DialogService not initialized with XamlRoot.");

            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "OK",
                XamlRoot = xamlRoot
            };

            await dialog.ShowAsync();
        }
    }

}