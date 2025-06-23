using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OfficeOpenXml;
using vuapos.Presentation.DTO.Product;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Product
{
    public sealed partial class ImportExcelProduct : ContentDialog
    {
        private readonly ProductViewModel _productViewModel;
        private readonly ProductService _productService;
        private StorageFile _selectedExcelFile;
        private StorageFolder _selectedImageFolder;
        private readonly CloudinaryService _cloudinaryService;
        private readonly XamlRoot _parentXamlRoot;
        private readonly CategoryViewModel _categoryViewModel;
        public ImportExcelProduct(ProductViewModel productViewModel, ProductService productService, XamlRoot parentXamlRoot)
        {
            InitializeComponent();
            _productViewModel = productViewModel;
            _productService = productService;
            _cloudinaryService = App.Services.GetRequiredService<CloudinaryService>();
            _parentXamlRoot = parentXamlRoot;
            _categoryViewModel = new CategoryViewModel();

        }

        private async void ChooseExcelFile_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            var window = App.m_window as MainWindow;
            var hwnd = WindowNative.GetWindowHandle(window);
            InitializeWithWindow.Initialize(filePicker, hwnd);

            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            filePicker.FileTypeFilter.Add(".xlsx");

            _selectedExcelFile = await filePicker.PickSingleFileAsync();
            if (_selectedExcelFile != null)
            {
                ExcelFilePathTextBlock.Text = _selectedExcelFile.Path;
                errorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private async void ChooseImageFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker();
            var window = App.m_window as MainWindow;
            var hwnd = WindowNative.GetWindowHandle(window);
            InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            folderPicker.FileTypeFilter.Add("*");

            _selectedImageFolder = await folderPicker.PickSingleFolderAsync();
            if (_selectedImageFolder != null)
            {
                ImageFolderPathTextBlock.Text = _selectedImageFolder.Path;
                errorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private async void PrimaryButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            try
            {
                errorTextBlock.Visibility = Visibility.Collapsed;

                if (_selectedExcelFile == null)
                    throw new Exception("Please select an Excel file.");
                if (_selectedImageFolder == null)
                    throw new Exception("Please select an image folder.");

                progressRing.IsActive = true;
                progressRing.Visibility = Visibility.Visible;
                Debug.WriteLine("Showing progress dialog...");
                var (products, importErrors) = await ReadProductsFromExcelAsync(_selectedExcelFile, _selectedImageFolder);
                if (products == null || !products.Any())
                {
                    throw new Exception("No valid products found in the Excel file.");
                }
                
                Debug.WriteLine("----------------------------------");
                Debug.WriteLine(products);
                //var successCount = 
                ImportProductsAsync(products);
                //Debug.WriteLine("successCount");
                //Debug.WriteLine(successCount);
                await _productViewModel.LoadProductsAsync();


                progressRing.IsActive = false;
                progressRing.Visibility = Visibility.Collapsed;

                Debug.WriteLine("deactive ring");

                string resultMessage = string.Empty;// = $"Successfully imported {successCount} product(s).";
                if (importErrors.Any())
                {
                    resultMessage += "\n\nThe following rows could not be imported:\n";
                    foreach (var error in importErrors)
                    {
                        resultMessage += $"Row {error.Row}: {error.Reason}\n";
                    }
                }
                else resultMessage = "Successfully imported products.";
                //Hide();
                await new ContentDialog
                {
                    Title = "Import Result",
                    Content = resultMessage,
                    CloseButtonText = "OK",
                    XamlRoot = _parentXamlRoot

                }.ShowAsync();
                //Hide();
            }
            catch (Exception ex)
            {
                progressRing.IsActive = false;
                progressRing.Visibility = Visibility.Collapsed;

                errorTextBlock.Text = $"Error: {ex.Message}";
                errorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
            };
        }

        private async Task<(List<ProductCreateDTO> Products, List<ImportError> Errors)> ReadProductsFromExcelAsync(StorageFile excelFile, StorageFolder imageFolder)
        {
            var categories = await _categoryViewModel.LoadAllCategoriesAsync();
            var importErrors = new List<ImportError>();
            var products = new List<ProductCreateDTO>();
            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            using (var stream = await excelFile.OpenStreamForReadAsync())
            using (var package = new ExcelPackage(stream))
            {
                Debug.WriteLine("Reading Excel file...");
                var worksheet = package.Workbook.Worksheets[0];
                if (worksheet == null)
                    return (products, importErrors);
                Debug.WriteLine("excel...");
                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var categoryName = worksheet.Cells[row, 3].Text;
                        string categoryId = string.Empty;
                        Debug.WriteLine(categoryName);
                        Debug.WriteLine(categoryName);
                        if (!string.IsNullOrWhiteSpace(categoryName))
                        {
                            var category = categories?.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                            Debug.WriteLine(category);
                            if (category != null)
                            {
                                categoryId = category.Category_Id;
                                Debug.WriteLine("category id: ");
                                Debug.WriteLine(categoryId);
                            }
                            
                            else
                            {
                                importErrors.Add(new ImportError(row, $"Category '{categoryName}' not found."));
                                continue;
                            }
                        }
                        else
                        {
                            importErrors.Add(new ImportError(row, "Category name is missing."));
                            continue;
                        }
                        var product = new ProductCreateDTO
                        {
                            product_code = worksheet.Cells[row, 1].Text,
                            product_name = worksheet.Cells[row, 2].Text,
                            category_id = categoryId,
                            price = decimal.TryParse(worksheet.Cells[row, 4].Text, out var price) ? price : 0,
                            cost_price = decimal.TryParse(worksheet.Cells[row, 5].Text, out var costPrice) ? costPrice : 0,
                            stock_quantity = int.TryParse(worksheet.Cells[row, 6].Text, out var stock) ? stock : 0,
                            discount = int.TryParse(worksheet.Cells[row, 7].Text, out var discount) ? discount : 0,
                            image_path = worksheet.Cells[row, 8].Text ?? string.Empty
                        };
                        Debug.WriteLine("check code: ");
                        Debug.WriteLine(product.product_code);
                        if (string.IsNullOrWhiteSpace(product.product_code))
                        {
                            importErrors.Add(new ImportError(row, "Product code is missing."));
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(product.product_name))
                        {
                            importErrors.Add(new ImportError(row, "Product name is missing."));
                            continue;
                        }
                        
                        //if (string.IsNullOrWhiteSpace(product.category_id))
                        //{
                        //    importErrors.Add(new ImportError(row, "Category ID is missing."));
                        //    continue;
                        //}
                        if (!decimal.TryParse(worksheet.Cells[row, 4].Text, out var parsedPrice) || parsedPrice <= 0)
                        {
                            importErrors.Add(new ImportError(row, "Price is invalid or missing."));
                            continue;
                        }
                        if (!decimal.TryParse(worksheet.Cells[row, 5].Text, out var parsedCostPrice) || parsedCostPrice <= 0)
                        {
                            importErrors.Add(new ImportError(row, "Cost price is invalid or missing."));
                            continue;
                        }
                        if (!int.TryParse(worksheet.Cells[row, 6].Text, out var parsedStock) || parsedStock < 0)
                        {
                            importErrors.Add(new ImportError(row, "Stock quantity is invalid or missing."));
                            continue;
                        }
                        if (!int.TryParse(worksheet.Cells[row, 7].Text, out var parsedDiscount) || parsedDiscount < 0)
                        {
                            importErrors.Add(new ImportError(row, "Discount is invalid or missing."));
                            continue;
                        }

                        Debug.WriteLine("Image path");
                        Debug.WriteLine(product.image_path);
                        if (!string.IsNullOrWhiteSpace(product.image_path))
                        {
                            Debug.WriteLine("image path: ", product.image_path);
                            var imageFile = await imageFolder.GetFileAsync(product.image_path);
                            bool check = await _productViewModel.SearchProduct(product.product_code);
                            if (imageFile != null &&  check == false)
                            {
                                product.image_path = await _cloudinaryService.UploadImageAsync(imageFile);
                                Debug.WriteLine($"Uploaded image for {product.product_name}: {product.image_path}");
                            }
                            else if (imageFile == null)
                            {
                                Debug.WriteLine($"Image not found for {product.product_name}: {product.image_path}");
                                product.image_path = string.Empty;
                            }
                        }

                        Debug.WriteLine("product code: ",product.product_code);
                        products.Add(product);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error reading row {row}: {ex.Message}");
                        importErrors.Add(new ImportError(row, $"Error reading data: {ex.Message}"));
                        continue;
                    }
                }
            }

            return (products, importErrors);
        }

        private async void ImportProductsAsync(List<ProductCreateDTO> products)
        {
            int successCount = 0;

            try
            {
                var addedProduct = await _productService.AddProductsAsync(products);
                if (addedProduct != null)
                {
                    successCount++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error importing product: {ex}");
            }
            
        }
    }
}
