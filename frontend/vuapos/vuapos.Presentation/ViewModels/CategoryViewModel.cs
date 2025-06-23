using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.Views.Customer;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.ViewModels
{

    public class CategoryViewModel
    {
        private readonly CategoryService _categoryService;

        public ObservableCollection<Category> Categories { get; set; } = new();
        public int currentPage { get; set; } = 1;
        public int totalPages { get; set; } = 1;
        public CategoryViewModel()
        {
            _categoryService = App.Services.GetRequiredService<CategoryService>();

        }

        public async Task LoadCategoriesAsync()
        {
            var pagedResponse = await _categoryService.GetPaginationCategoriesAsync(currentPage);
            if (pagedResponse != null)
            {
                totalPages = pagedResponse.TotalPages;
                Categories.Clear();
                var categories = pagedResponse.Data;

                foreach (var category in categories)
                    Categories.Add(category);
            }
        }
        public async Task<List<Category>?> LoadAllCategoriesAsync()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        public async Task<(bool result, string message)> AddNewCategoryAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (false, "Category name cannot be empty or whitespace.");
            }
            Debug.WriteLine($"Checking if category '{name}' already exists.");

            if (Categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, $"Category '{name}' already exists.");
            }
            var addedCategory = await _categoryService.AddCategoryAsync(name);
            if (addedCategory != null)
            {
                Categories.Add(addedCategory);
                return (true, $"Category {name} added successfully.");
            }

            return (false, "Failed to add category.");
        }

        public async Task<(bool result, string message)> UpdateCategoryAsync(Category category, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (false, "Category name cannot be empty or whitespace.");
            }

            if (Categories.Any(c => c.Category_Id != category.Category_Id && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, $"Category '{name}' already exists.");
            }

            var updateCategory = await _categoryService.UpdateCategoryAsync(category.Category_Id, name);
            if (updateCategory != null)
            {
                await LoadCategoriesAsync();
                return (true, $"Category '{name}' updated successfully.");
            }

            return (false, "Failed to update category. Category not found or server error.");
        }
        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            var deletedCategory = await _categoryService.DeleteCategoryAsync(category.Category_Id);
            if (deletedCategory != null)
            {
                Categories.Remove(category);
            }
            if (deletedCategory != null)
            {
                return true;
            }
            return false;
        }
    }
}
