using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using vuapos.Presentation.Commands;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.DAO.MockData;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;

namespace vuapos.Presentation.ViewModels
{
    public class ProductSearchViewModel : INotifyPropertyChanged
    {
        private readonly IProductDao _productDao;
        private string _searchTerm;
        private bool _isLoading;
        private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm != value)
                {
                    _searchTerm = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                    ((RelayCommand)SearchCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public ProductSearchViewModel(IProductDao productDao)
        {
            _productDao = productDao;
            SearchCommand = new RelayCommand(async p => await PerformSearch(), p => !IsLoading);
            ClearSearchCommand = new RelayCommand(async p => await ClearSearch(), p => !string.IsNullOrWhiteSpace(SearchTerm));

            // Load initial products
            _ = LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                IsLoading = true;
                var products = await _productDao.GetAllProductsAsync(); 

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task PerformSearch()
        {
            try
            {
                IsLoading = true;
                var searchResults = await _productDao.SearchProductsByNameAsync(SearchTerm);

                Products.Clear();
                foreach (var product in searchResults)
                {
                    Products.Add(product);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ClearSearch()
        {
            SearchTerm = string.Empty;
            await LoadProductsAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
