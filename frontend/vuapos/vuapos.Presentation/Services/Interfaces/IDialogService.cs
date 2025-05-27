using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace vuapos.Presentation.Services.Interfaces
{
    public interface IDialogService
    {
        Task ShowMessageAsync(XamlRoot xamlRoot,string title, string message);
    }
}