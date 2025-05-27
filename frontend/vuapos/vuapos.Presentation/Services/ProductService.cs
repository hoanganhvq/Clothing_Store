using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Product;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Category;

namespace vuapos.Presentation.Services
{
    public class ProductService: ApiService
    {
        CloudinaryService _cloudinaryService;
        public ProductService(HttpClient httpClient) : base(httpClient)
        {
            //base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjhmOWUwNmUxLTM1ZWQtNDViYy05M2Y2LWExN2YyZGIyNmMzOSIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NjYxODA5LCJleHAiOjE3NDYyNjY2MDl9.3Myou0ILU61jkT4B0Xv75qrQA7qGWBOegBCREpjnEoI";
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
            _cloudinaryService = new CloudinaryService();

        }
        public async Task<Product?> GetProductAsync(string productId)
        {
            return await SendRequestAsync<Product>(HttpMethod.Get, $"product/{productId}");
        }
        //public async Task<List<Product>?> GetAllProductsAsync()
        //{
        //    return await SendRequestAsync<List<Product>>(HttpMethod.Get, "product");
        //}   
        public async Task<PageProductResponse<Product>?> GetAllProductsAsync(int page = 1)
        {
            return await SendRequestAsync<PageProductResponse<Product>>(HttpMethod.Get, $"product?page={page}");
        }

        public async Task<Product?> SearchProduct(string product_code)
        {
            return await SendRequestAsync<Product>(HttpMethod.Get, $"product/search/{product_code}");
        }


        public async Task<Product?> AddProductAsync(ProductCreateDTO productCreateDTO)
        {
            Debug.WriteLine($"ProductCreateDTO: {productCreateDTO}");
            return await SendRequestAsync<Product>(HttpMethod.Post, "product", productCreateDTO);
        }

        public async Task<List<Product>?> AddProductsAsync(List<ProductCreateDTO> products)
        {
            var wrapper = new ProductImportDTOList(products);
            Debug.WriteLine(wrapper);
            return await SendRequestAsync<List<Product>>(HttpMethod.Post, "product/import", wrapper);
        }


        public async Task<bool> DeleteProductAsync(string productId)
        {
            var product = await GetProductAsync(productId);
            if (product == null)
            {
                Debug.WriteLine($"Product with ID {productId} not found.");
                return false;
            }
            var publicId = ExtractPublicIdFromImagePath(product.Image_Path);
            Debug.WriteLine($"Public ID: {publicId}");
            if (string.IsNullOrEmpty(publicId))
            {
                Debug.WriteLine("Failed to extract public_id from Image_Path.");
                return false;
            }

            var deleteImageSuccess = await _cloudinaryService.DeleteImageAsync(publicId);
            if (deleteImageSuccess)
            {
                Debug.WriteLine($"Image {publicId} deleted from Cloudinary.");
            }
            else
            {
                return false;
            }

            var deleteProductSuccess = await SendRequestAsync<Product>(HttpMethod.Delete, $"product/{productId}");
            if (deleteProductSuccess != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //search
        public async Task<Response<Product>?> SearchProductsAsync(string searchTerm)
        {
            return await SendRequestAsync<Response<Product>>(HttpMethod.Get, $"product?search={searchTerm}");
        }

        public string ExtractPublicIdFromImagePath(string imagePath)
        {
                var uri = new Uri(imagePath);
                var segments = uri.AbsolutePath.Split('/');

                int uploadIndex = Array.IndexOf(segments, "image");
                if (uploadIndex == -1 || uploadIndex + 2 >= segments.Length)
                {
                    throw new ArgumentException("Invalid Cloudinary URL format.");
                }

                var publicIdSegments = segments.Skip(uploadIndex + 3);
                var publicIdWithExtension = string.Join("/", publicIdSegments);
                var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);

                if (publicIdSegments.Count() > 1)
                {
                    publicId = string.Join("/", publicIdSegments.Take(publicIdSegments.Count() - 1)) + "/" + publicId;
                }

                return publicId;
            
        }

        public async Task<Product?> UpdateProductAsync(string productId, ProductUpdateDTO productUpdateDTO)
        {
            return await SendRequestAsync<Product>(HttpMethod.Patch, $"product/{productId}", productUpdateDTO);
        }

    }
}
