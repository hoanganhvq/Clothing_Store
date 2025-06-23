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
using WinRT.Interop;
using vuapos.Presentation.Services;
using System.Diagnostics;
using vuapos.Presentation.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Product
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddProductDialog : ContentDialog
    {
        private readonly ProductViewModel _productViewModel;
        private readonly CategoryViewModel _categoryViewModel;
        private readonly ProductService _productService;

        private StorageFile _selectedImageFile;

        public AddProductDialog(ProductViewModel productViewModel, CategoryViewModel categoryViewModel, ProductService productService)
        {
            InitializeComponent();
            _productViewModel = productViewModel;
            _categoryViewModel = categoryViewModel;
            _productService = productService;
            CategoryComboBox.ItemsSource = _categoryViewModel.Categories;

            if (_categoryViewModel.Categories.Count == 0)
            {
                _ = _categoryViewModel.LoadCategoriesAsync();
            }
        }

        private async void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var hwnd = WindowNative.GetWindowHandle(App.m_window as MainWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            _selectedImageFile = await picker.PickSingleFileAsync();
            if (_selectedImageFile != null)
            {
                ImageFilePathTextBlock.Text = _selectedImageFile.Path;
                errorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private async void PrimaryButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();

            try
            {
                errorTextBlock.Visibility = Visibility.Collapsed;
                Debug.WriteLine($"{CategoryComboBox.SelectedValue.ToString()} called");
                if (string.IsNullOrWhiteSpace(ProductCodeTextBox.Text))
                    throw new Exception("Product code is required");
                if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
                    throw new Exception("Product name is required");
                if (CategoryComboBox.SelectedValue == null)
                    throw new Exception("Please select a category");
                if (string.IsNullOrWhiteSpace(PriceTextBox.Text) || !decimal.TryParse(PriceTextBox.Text, out var price))
                    throw new Exception("Price must be a valid number");
                if (string.IsNullOrWhiteSpace(CostPriceTextBox.Text) || !decimal.TryParse(CostPriceTextBox.Text, out var costPrice))
                    throw new Exception("Cost price must be a valid number");
                if (string.IsNullOrWhiteSpace(StockQuantityTextBox.Text) || !int.TryParse(StockQuantityTextBox.Text, out var stockQuantity))
                    throw new Exception("Stock quantity must be a valid integer");
                if (_selectedImageFile == null)
                    throw new Exception("Image is required");

                var pageProductResponse = await _productService.GetAllProductsAsync();

                //Debug.WriteLine($"Existing products: {existingProducts}");
                //if (pageProductResponse != null)
                //{
                //    var existingProducts = pageProductResponse.Data;
                //    var product_code = ProductCodeTextBox.Text.Trim();
                //    var product_name = ProductNameTextBox.Text.Trim();
                //    Debug.WriteLine($"Existing products: {existingProducts}");

                //    if (existingProducts.Any(p => p.Product_Code.Equals(product_code, StringComparison.OrdinalIgnoreCase)))
                //    {
                //        Debug.WriteLine($"Product code already exists: {product_code}");
                //        throw new Exception("Product code already exists");

                //    }
                //    //if (existingProducts.Any(p => p.Product_Name.Equals(product_name, StringComparison.OrdinalIgnoreCase)))
                //    //    throw new Exception("Product name already exists");
                //}
                var product_code = ProductCodeTextBox.Text.Trim();
                Debug.WriteLine($"Product code: {product_code}");
                if (await _productViewModel.SearchProduct(product_code))
                {
                    Debug.WriteLine($"Product code already exists: {product_code}");
                    throw new Exception("Product code already exists");
                }

                var productCode = ProductCodeTextBox.Text;
                var productName = ProductNameTextBox.Text;
                var categoryId = CategoryComboBox.SelectedValue.ToString();

                await _productViewModel.AddProductAsync(productCode, productName, categoryId, price, costPrice, stockQuantity, _selectedImageFile);

                Hide();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("test ----------------------");
                Debug.WriteLine(ex.Message);
                errorTextBlock.Text = $"Error: {ex.Message}";
                errorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}