using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using vuapos.Presentation.Commands;

namespace vuapos.Presentation.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;

    namespace vuapos.Presentation.ViewModels
    {
        using System;
        using System.Collections.ObjectModel;
        using System.ComponentModel;
        using System.Linq;
        using System.Runtime.CompilerServices;
        using System.Threading.Tasks;
        using System.Windows.Input;

        namespace vuapos.Presentation.ViewModels
        {
            public class PaginationViewModel : INotifyPropertyChanged
            {
                private int _totalItems;
                private int _itemsPerPage = 5;
                private int _currentPage = 1;
                private int _totalPages = 1;
                private int _maxVisiblePages = 5; // Số lượng nút trang hiển thị tối đa

                public event PropertyChangedEventHandler PropertyChanged;

                public int TotalItems
                {
                    get => _totalItems;
                    set
                    {
                        if (_totalItems != value)
                        {
                            _totalItems = value;
                            CalculateTotalPages();
                            OnPropertyChanged();
                        }
                    }
                }

                public int ItemsPerPage
                {
                    get => _itemsPerPage;
                    set
                    {
                        if (_itemsPerPage != value)
                        {
                            _itemsPerPage = value;
                            CalculateTotalPages();
                            OnPropertyChanged();
                        }
                    }
                }

                public int CurrentPage
                {
                    get => _currentPage;
                    set
                    {
                        if (_currentPage != value && value > 0 && value <= TotalPages)
                        {
                            _currentPage = value;
                            OnPropertyChanged();
                            UpdatePageNumbers();
                            LoadItemsForCurrentPageCommand?.Execute(null);
                        }
                    }
                }

                public int TotalPages
                {
                    get => _totalPages;
                    private set
                    {
                        if (_totalPages != value)
                        {
                            _totalPages = value;
                            OnPropertyChanged();
                            OnPropertyChanged(nameof(PageNumbers));
                        }
                    }
                }

                // Tạo danh sách số trang hiển thị thông minh
                private ObservableCollection<int> _pageNumbers = new ObservableCollection<int>();
                public ObservableCollection<int> PageNumbers
                {
                    get => _pageNumbers;
                    private set
                    {
                        _pageNumbers = value;
                        OnPropertyChanged();
                    }
                }



                // Command để xử lý việc tải dữ liệu
                public ICommand LoadItemsForCurrentPageCommand { get; set; }
                public ICommand FirstPageCommand { get; private set; }
                public ICommand PreviousPageCommand { get; private set; }
                public ICommand NextPageCommand { get; private set; }
                public ICommand LastPageCommand { get; private set; }
                public ICommand GoToPageCommand { get; private set; }

                public PaginationViewModel()
                {
                    FirstPageCommand = new RelayCommand(_ => GoToFirstPage());
                    PreviousPageCommand = new RelayCommand( _ => GoToPreviousPage());
                    NextPageCommand = new RelayCommand( _ => GoToNextPage());
                    LastPageCommand = new RelayCommand(_ => GoToLastPage());
                    GoToPageCommand = new RelayCommand<int>(GoToPage);
                }

                public void Initialize(int totalItems)
                {
                    TotalItems = totalItems;
                    CurrentPage = 1;
                }

                private void UpdatePageNumbers()
                {
                    PageNumbers.Clear();

                    if (TotalPages <= _maxVisiblePages)
                    {
                        for (int i = 1; i <= TotalPages; i++)
                        {
                            PageNumbers.Add(i);
                        }
                    }
                    else
                    {
                        int half = _maxVisiblePages / 2;
                        int startPage = Math.Max(1, CurrentPage - half);
                        int endPage = startPage + _maxVisiblePages - 1;

                        if (endPage > TotalPages)
                        {
                            endPage = TotalPages;
                            startPage = Math.Max(1, endPage - _maxVisiblePages + 1);
                        }

                        for (int i = startPage; i <= endPage; i++)
                        {
                            PageNumbers.Add(i);
                        }
                    }
                }


                private void CalculateTotalPages()
                {
                    TotalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
                    if (CurrentPage > TotalPages)
                    {
                        CurrentPage = Math.Max(1, TotalPages);
                    }
                    UpdatePageNumbers();
                }

                private void GoToFirstPage()
                {
                    CurrentPage = 1;
                }

                private void GoToPreviousPage()
                {

                    if (CurrentPage > 1)
                    {
                        CurrentPage--;
                    }
                }


                private void GoToNextPage()
                {
                    if (CurrentPage < TotalPages)
                    {
                        CurrentPage++;
                    }
                }

                private void GoToLastPage()
                {
                    CurrentPage = TotalPages;
                }

                private void GoToPage(int pageNumber)
                {
                    if (pageNumber > 0 && pageNumber <= TotalPages)
                    {
                        CurrentPage = pageNumber;
                    }
                }

                protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }
    }
}
