using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using vuapos.Presentation.Services;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Category
{

    public sealed partial class CategoryPage : UserControl
    {
        public CategoryViewModel ViewModel { get; }

        public CategoryPage()
        {
            this.InitializeComponent();
            ViewModel = new CategoryViewModel();
            LoadCategories();
        }
        private async void LoadCategories()
        {
            await ViewModel.LoadCategoriesAsync();
            CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

        }
        private void CategorySearchBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            var query = SearchTextBox.Text.ToLower();
            var filteredCategories = ViewModel.Categories.Where(c => c.Name.ToLower().Contains(query)).ToList();

            CategoryListView.ItemsSource = filteredCategories;
        }

        private void CategorySearchBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            var query = e.QueryText.ToLower();
            var filteredCategories = ViewModel.Categories.Where(c => c.Name.ToLower().Contains(query)).ToList();

            CategoryListView.ItemsSource = filteredCategories;
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Category category)
            {
                var newNameTextBox = new TextBox
                {
                    PlaceholderText = "Enter new category name",
                    Text = category.Name
                };
                var messageTextBlock = new TextBlock
                {
                    Text = "",
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                var contentPanel = new StackPanel
                {
                    Children = { messageTextBlock, newNameTextBox }
                };

                var dialog = new ContentDialog
                {
                    Title = "Edit Category",
                    Content = contentPanel,
                    PrimaryButtonText = "Update",
                    CloseButtonText = "Cancel",
                    XamlRoot = CategoryListView.XamlRoot
                };

                while (true)
                {
                    var result = await dialog.ShowAsync();
                    if (result != ContentDialogResult.Primary)
                    {
                        break;
                    }

                    var newName = newNameTextBox.Text;
                    Debug.WriteLine($"New category name: '{newName}'");

                    dialog.IsPrimaryButtonEnabled = false;
                    try
                    {
                        messageTextBlock.Text = "";
                        var (success, message) = await ViewModel.UpdateCategoryAsync(category, newName);
                        Debug.WriteLine($"Update result: {success}, message: {message}");

                        if (!success)
                        {
                            messageTextBlock.Text = message;
                            dialog.IsPrimaryButtonEnabled = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error updating category: {ex.Message}");
                        messageTextBlock.Text = $"Error: {ex.Message}";
                        dialog.IsPrimaryButtonEnabled = true;
                    }
                }
            }
        }

        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Category category)
            {

                var result = await new ContentDialog
                {
                    Title = "Delete Category",
                    Content = $"Are you sure you want to delete {category.Name}?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await ViewModel.DeleteCategoryAsync(category);
                    await ViewModel.LoadCategoriesAsync();
                    CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                }
            }
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine("AddCategory_Click triggered");

            var categoryNameTextBox = new TextBox
            {
                PlaceholderText = "Enter category name"
            };
            var messageTextBlock = new TextBlock
            {
                Text = "",
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red),
                Margin = new Thickness(0, 0, 0, 10)
            };
            var contentPanel = new StackPanel
            {
                Children =
                            {
                                messageTextBlock,
                                new TextBlock { Text = "Category Name:" },
                                categoryNameTextBox
                            }
            };

            var dialog = new ContentDialog
            {
                Title = "Add New Category",
                Content = contentPanel,
                PrimaryButtonText = "Add",
                CloseButtonText = "Cancel",
                XamlRoot = CategoryListView.XamlRoot
            };

            while (true)
            {
                Debug.WriteLine("Showing dialog");
                var dialogResult = await dialog.ShowAsync();
                Debug.WriteLine($"Dialog result: {dialogResult}");

                if (dialogResult != ContentDialogResult.Primary)
                {
                    Debug.WriteLine("Exiting loop due to Cancel or close");
                    break;
                }

                var categoryName = categoryNameTextBox.Text;
                Debug.WriteLine($"Category name entered: '{categoryName}'");

                dialog.IsPrimaryButtonEnabled = false;
                try
                {
                    messageTextBlock.Text = "";
                    var (result, message) = await ViewModel.AddNewCategoryAsync(categoryName);
                    Debug.WriteLine($"result: {result}, message: {message}");
                    if (!result)
                    {
                        messageTextBlock.Text = message;
                        Debug.WriteLine($"Updated messageTextBlock.Text: '{messageTextBlock.Text}'");
                        dialog.IsPrimaryButtonEnabled = true;
                    }
                    else
                    {
                        await ViewModel.LoadCategoriesAsync();
                        CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                        Debug.WriteLine($"Category '{categoryName}' added successfully");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error adding category: {ex.Message}");
                    messageTextBlock.Text = $"Error: {ex.Message}";
                    dialog.IsPrimaryButtonEnabled = true;
                }
            }
            Debug.WriteLine("AddCategory_Click completed");
            //await ViewModel.LoadCategoriesAsync();

        }

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.currentPage < ViewModel.totalPages)
            {
                ViewModel.currentPage++;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                await ViewModel.LoadCategoriesAsync();
            }
        }

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {

            if (ViewModel.currentPage > 1)
            {
                ViewModel.currentPage--;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";
                await ViewModel.LoadCategoriesAsync();
            }
        }
    }
}
