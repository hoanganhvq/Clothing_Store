using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Product;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.Views.Category;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace vuapos.Presentation.ViewModels
{

    public class ProductViewModel
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly ProductService _productService;

        public ObservableCollection<Product> Products { get; set; } = new();
        public int currentPage { get; set; } = 1;
        public int totalPages { get; set; } = 1;
        public ProductViewModel()
        {
            _productService = App.Services.GetRequiredService<ProductService>();
            _cloudinaryService = App.Services.GetRequiredService<CloudinaryService>();
        }

        public async Task LoadProductsAsync()
        {

            var pagedResponse = await _productService.GetAllProductsAsync(currentPage);
            if (pagedResponse != null)
            {
                totalPages = pagedResponse.TotalPages;
                Products.Clear();
                var products = pagedResponse.Data;

                foreach (var product in products)
                    Products.Add(product);
            }
        }

        public async Task AddProductAsync(string productCode,string productName, string categoryId, decimal price, decimal costPrice, int stockQuantity, StorageFile imageFile = null)
        {
            try
            {
                string imageUrl = string.Empty;
                if (imageFile != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(imageFile);
                    Debug.WriteLine($"Uploaded image URL: {imageUrl}");
                }

                var productCreateDTO = new ProductCreateDTO
                {
                    product_code = productCode,
                    product_name = productName,
                    category_id = categoryId,
                    price = price,
                    cost_price = costPrice,
                    stock_quantity = stockQuantity,
                    discount = 0,
                    image_path = imageUrl
                };

                var product = await _productService.AddProductAsync(productCreateDTO);
                Debug.WriteLine($"Product: {product}");
                if (product != null)
                {
                    Products.Add(product);

                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating product.", ex);
            }
        }

        public async Task<string> UploadImageAsync(StorageFile imageFile)
        {
            if (imageFile != null)
            {
                return await _cloudinaryService.UploadImageAsync(imageFile);
            }
            return string.Empty;
        }

        public async Task UpdateProductAsync(Product product, ProductUpdateDTO updateDto, StorageFile newImageFile = null)
        {
            try
            {
                var existingProduct = await _productService.GetProductAsync(product.Product_Id);
                Debug.WriteLine($"Existing product Image_Path: {existingProduct?.Image_Path}");
                Debug.WriteLine($"New image file: {(newImageFile != null ? newImageFile.Path : "null")}");

                if (existingProduct != null && !string.IsNullOrEmpty(existingProduct.Image_Path) && newImageFile != null)
                {
                    var publicId = _productService.ExtractPublicIdFromImagePath(existingProduct.Image_Path);
                    Debug.WriteLine($"Extracted publicId: {publicId}");
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }
                    updateDto.image_path = await _cloudinaryService.UploadImageAsync(newImageFile);
                    Debug.WriteLine($"Uploaded new image URL: {updateDto.image_path}");
                }

                Debug.WriteLine($"Updating product: {updateDto.product_name}");
                var updatedProduct = await _productService.UpdateProductAsync(product.Product_Id, updateDto);
                Debug.WriteLine($"Response: {updatedProduct}");
                Debug.WriteLine("------------------------------------");

                if (updatedProduct != null)
                {
                    await LoadProductsAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating product: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> SearchProduct(string productCode)
        {
            var product = await _productService.SearchProduct(productCode);

            Debug.WriteLine("check search product ");
            Debug.WriteLine(product);
            if (product != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteProductAsync(string productId)
        {
            var success = await _productService.DeleteProductAsync(productId);
            Debug.WriteLine($"view model Product with ID {productId} deleted: {success}");
            if (success)
            {
                var product = Products.FirstOrDefault(p => p.Product_Id == productId);
                if (product != null)
                {
                    Products.Remove(product);
                }
            }
            return success;
        }
    }
}
