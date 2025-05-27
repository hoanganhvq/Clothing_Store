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
using System.Diagnostics;
using vuapos.Presentation.DTO.Product;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

using CategoryNS = vuapos.Presentation.Views.Category;
using vuapos.Presentation.ViewModels;

namespace vuapos.Presentation.Views.Product
{
    public sealed partial class EditProductDialog : ContentDialog
    {
        private readonly ProductViewModel _productViewModel;
        private readonly CategoryViewModel _categoryViewModel;
        private readonly Models.Product _product;
        private StorageFile _selectedImageFile;
        public Models.Category SelectedCategory { get; set; }
        private string _tempImagePath;

        public EditProductDialog(ProductViewModel productViewModel, CategoryViewModel categoryViewModel, Models.Product product)
        {
            this.InitializeComponent();
            _productViewModel = productViewModel;
            _categoryViewModel = categoryViewModel;
            _product = product;
            Debug.WriteLine("categories: ", _categoryViewModel.Categories);
            ProductCodeTextBox.Text = _product.Product_Code ?? string.Empty;
            ProductNameTextBox.Text = _product.Product_Name ?? string.Empty;
            CategoryComboBox.ItemsSource = _categoryViewModel.Categories;
            CategoryComboBox.SelectedItem = _categoryViewModel.Categories.FirstOrDefault(c => c.Category_Id == _product.Category_Id);
            PriceTextBox.Text = _product.Price.ToString();
            CostPriceTextBox.Text = _product.Cost_Price.ToString();
            StockQuantityTextBox.Text = _product.Stock_Quantity.ToString();
            DiscountTextBox.Text = _product.Discount.ToString();

            _tempImagePath = _product.Image_Path;
            ImagePathTextBox.Text = _tempImagePath;
            if (_categoryViewModel.Categories.Count == 0)
            {
                _ = _categoryViewModel.LoadCategoriesAsync();
            }
        }

        private async void PrimaryButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                errorTextBlock.Text = null;
                errorTextBlock.Visibility = Visibility.Collapsed;
                if (string.IsNullOrWhiteSpace(ProductCodeTextBox.Text))
                    throw new Exception("Product code is required");
                if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
                    throw new Exception("Product name is required");
                Models.Category selectedCategory = CategoryComboBox.SelectedItem as Models.Category;
                string categoryId = selectedCategory?.Category_Id;
                Debug.WriteLine($"Selected category: {selectedCategory?.Name}, Category_Id: {categoryId}");
                if (selectedCategory == null || string.IsNullOrWhiteSpace(categoryId))
                    throw new Exception("Please select a category");
                if (string.IsNullOrWhiteSpace(PriceTextBox.Text) || !decimal.TryParse(PriceTextBox.Text, out var price))
                    throw new Exception("Price must be a valid number");
                if (string.IsNullOrWhiteSpace(CostPriceTextBox.Text) || !decimal.TryParse(CostPriceTextBox.Text, out var costPrice))
                    throw new Exception("Cost price must be a valid number");
                if (string.IsNullOrWhiteSpace(StockQuantityTextBox.Text) || !int.TryParse(StockQuantityTextBox.Text, out var stockQuantity))
                    throw new Exception("Stock quantity must be a valid integer");
                if (ImagePathTextBox == null)
                    throw new Exception("Image is required");
                int discount = 0;
                if (!string.IsNullOrWhiteSpace(DiscountTextBox.Text) && !int.TryParse(DiscountTextBox.Text, out discount))
                    throw new Exception("Discount must be a valid integer");

                var updateDto = new ProductUpdateDTO
                {
                    product_code = ProductCodeTextBox.Text,
                    product_name = ProductNameTextBox.Text,
                    category_id = categoryId,
                    price = price,
                    cost_price = costPrice,
                    stock_quantity = stockQuantity,
                    discount = discount,
                    image_path = ImagePathTextBox.Text,
                };

                Debug.WriteLine($"Updating product with Price: {price}");
                await _productViewModel.UpdateProductAsync(_product, updateDto, _selectedImageFile);
                this.Hide();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in PrimaryButton_Click: {ex.Message}");
                errorTextBlock.Text = $"Error: {ex.Message}";
                errorTextBlock.Visibility = Visibility.Visible;

                args.Cancel = true;
            }
        }

        private async void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
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
                Debug.WriteLine($"Selected image: {_selectedImageFile.Path}");
                _tempImagePath = _selectedImageFile.Path;
                ImagePathTextBox.Text = _tempImagePath;
            }
        }
    }
}