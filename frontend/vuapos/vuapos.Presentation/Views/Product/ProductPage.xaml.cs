using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;
using vuapos.Presentation.DTO.Product;
using System.Net.Http;
using WinRT.Interop;
using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using vuapos.Presentation.Services;
using System.Threading.Tasks;
using OfficeOpenXml;
using vuapos.Presentation.ViewModels;
using vuapos.Presentation.Models;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Product
{

    public sealed partial class ProductPage : UserControl
    {
        private StorageFile? selectedImageFile;
        public ProductViewModel ViewModel { get; }
        private readonly ICategoryService _categoryService;
        private readonly CategoryViewModel _categoryViewModel;
        private readonly ProductService _productService;
        public ProductPage()
        {
            this.InitializeComponent();
            _categoryService = new CategoryService(new HttpClient());
            _categoryViewModel = new CategoryViewModel();
            _productService = new ProductService(new HttpClient());
            ViewModel = new ProductViewModel();
            LoadInitialData();

        }
        private async void ProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            await _categoryViewModel.LoadCategoriesAsync();
            await ViewModel.LoadProductsAsync();
            //ProductListView.ItemsSource = ViewModel.Products;
        }
        private async void LoadInitialData()
        {
            await _categoryViewModel.LoadCategoriesAsync();
            await ViewModel.LoadProductsAsync();
            CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addProductDialog = new AddProductDialog(ViewModel, _categoryViewModel, _productService)
                {
                    XamlRoot = this.XamlRoot
                };
                await addProductDialog.ShowAsync();
                await ViewModel.LoadProductsAsync();
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to open dialog: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
        private async void ImportProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var importErrors = new List<ImportError>();

                var importProductsDialog = new ImportExcelProduct(ViewModel, _productService, this.XamlRoot)
                {
                    XamlRoot = this.XamlRoot
                };

                await importProductsDialog.ShowAsync();
                await ViewModel.LoadProductsAsync();
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";


            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"Failed to open import dialog: {ex.Message}");
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            await new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            }.ShowAsync();
        }

        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Models.Product;
            if (product != null)
            {
                var categories = _categoryService.GetAllCategoriesAsync();

                var editProductDialog = new EditProductDialog(ViewModel, _categoryViewModel, product);
                editProductDialog.XamlRoot = this.XamlRoot;
                var result = await editProductDialog.ShowAsync();
                await ViewModel.LoadProductsAsync();
            }
        }

        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Models.Product;

            var confirmDialog = new ContentDialog
            {
                Title = "Confirm Delete",
                Content = $"Are you sure you want to delete '{product?.Product_Name}'?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                XamlRoot = this.XamlRoot,
                DefaultButton = ContentDialogButton.Secondary
            };

            var result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var success = await ViewModel.DeleteProductAsync(product.Product_Id);
                if (success)
                {
                    await ViewModel.LoadProductsAsync();
                    CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                }
            }

        }

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.currentPage > 1)
            {
                ViewModel.currentPage--;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";
                await ViewModel.LoadProductsAsync();
            }
        }

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.currentPage < ViewModel.totalPages)
            {
                ViewModel.currentPage++;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                await ViewModel.LoadProductsAsync();
            }
        }
    }
}