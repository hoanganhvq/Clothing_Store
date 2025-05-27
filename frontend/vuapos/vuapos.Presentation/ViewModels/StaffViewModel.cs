using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;
using System.Windows.Input;
using vuapos.Presentation.Commands;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using vuapos.Presentation.Views.Staff;
using vuapos.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.DAO.Implement;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using vuapos.Presentation.DTO.Staff;
using vuapos.Presentation.Utils;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels;
using vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels;
using Windows.Web.AtomPub;

namespace vuapos.Presentation.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
    { 
        private Response<Staff> _staffRepsponse;
        private readonly StaffService _staffService;
        private ObservableCollection<Staff> _staffs;
        private Staff _selectedStaff;
        private string _newPassword;
        private string _confirmPassword;
        private string _passwordError;
        private bool _isPasswordValid;
        private bool _isAddMode;
        private XamlRoot _xamlRoot; // Để hiển thị dialog
        private string _passwordLabel;
        private string _searchText;

        public bool IsCash = false;


        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                // Khi thay đổi SearchText, thực hiện tìm kiếm
                _ = SearchStaffs();
            }
        }
        public string PasswordLabel
        {
            get { return _passwordLabel; }
            set { SetProperty(ref _passwordLabel, value); }
        }


        public ObservableCollection<Staff> Staffs 
        {
            get { return _staffs; }
            set {
                SetProperty(ref _staffs, value); 
            }
        } 

        public Staff SelectedStaff
        {
            get { return _selectedStaff; }
            set
            {
                SetProperty(ref _selectedStaff, value);
                // Khi chọn nhân viên, reset các trường mật khẩu và lỗi
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                PasswordError = string.Empty;
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                SetProperty(ref _newPassword, value);
                ValidatePassword();
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                SetProperty(ref _confirmPassword, value);
                ValidatePassword();
            }
        }

        public string PasswordError
        {
            get { return _passwordError; }
            set { SetProperty(ref _passwordError, value); }
        }

        public bool IsPasswordValid
        {
            get { return _isPasswordValid; }
            set { SetProperty(ref _isPasswordValid, value); }
        }

        public bool IsAddMode
        {
            get { return _isAddMode; }
            set { 
                SetProperty(ref _isAddMode, value);
                // Cập nhật nhãn mật khẩu khi thay đổi chế độ
                PasswordLabel = value ? "Password" : "New Password";
            }
        }

        public PaginationViewModel PaginationViewModel { get; private set; }



        public ICommand AddStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand SearchStaffCommand { get; } // Lệnh tìm kiếm nhân viên

        // Cần XamlRoot để hiện dialog trong WinUI 3
        public StaffViewModel(StaffService staffService)
        {
            _staffService = staffService;
            // Update the initialization of LoadItemsForCurrentPageCommand to properly await the asynchronous method
            PaginationViewModel = new PaginationViewModel();
            PaginationViewModel.LoadItemsForCurrentPageCommand = new RelayCommand(async _ => await LoadStaffsForCurrentPage());

            // Khởi tạo các lệnh
            AddStaffCommand = new RelayCommand(param => ShowAddStaffDialog());
            EditStaffCommand = new RelayCommand<Staff>(param => ShowEditStaffDialog(param));
            SearchStaffCommand = new RelayCommand(param => SearchStaffs());
            DeleteStaffCommand = new RelayCommand<Staff>(ExecuteDeleteStaff);

            // Load dữ liệu ban đầu
            _ = LoadStaff();
        }


        private async Task SearchStaffs()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Nếu không có từ khóa tìm kiếm, hiển thị tất cả nhân viên
                _ = LoadStaff();
            }
            else
            {
                _staffRepsponse = await _staffService.GetStaffsByNameAsync(SearchText);
                var staffs = _staffRepsponse.Data;
                if (staffs != null)
                {
                    Staffs.Clear();
                    foreach (var staff in staffs)
                    {
                            Staffs.Add(staff);
                    }
                    PaginationViewModel.TotalItems = _staffRepsponse.TotalCount;
                }
            }
        }

        // Phương thức để cập nhật XamlRoot khi cần
        public void UpdateXamlRoot(XamlRoot xamlRoot)
        {
            _xamlRoot = xamlRoot;
        }

        private async Task LoadStaffsForCurrentPage()
        {
            var respponse = await _staffService.GetAllStaffsAsync(PaginationViewModel.CurrentPage);
            if (respponse != null)
            {
                Staffs.Clear();
                foreach (var staff in respponse.Data)
                {
                    Staffs.Add(staff);
                }
            }
            else
            {
                Staffs.Clear();
            }
        }


        private async Task LoadStaff()
        {
            if (Staffs == null) Staffs = new ObservableCollection<Staff>();
            Staffs.Clear();
            _staffRepsponse = await _staffService.GetAllStaffsAsync(1);
            if (_staffRepsponse == null) return;

            var staffs = _staffRepsponse.Data;
            if (staffs != null)
            {
                Debug.WriteLine(staffs.Count);
                foreach (var staff in staffs)
                {
                    Staffs.Add(staff);
                }
            }
            PaginationViewModel.Initialize(_staffRepsponse.TotalCount);
        }

        private void ValidatePassword()
        {
            // Nếu đang ở chế độ chỉnh sửa và không nhập mật khẩu mới, thì không cần kiểm tra
            if (!IsAddMode && string.IsNullOrEmpty(NewPassword) && string.IsNullOrEmpty(ConfirmPassword))
            {
                PasswordError = string.Empty;
                IsPasswordValid = true;
                return;
            }

            // Kiểm tra mật khẩu khi ở chế độ thêm mới hoặc đã nhập mật khẩu mới
         
                if (string.IsNullOrEmpty(NewPassword))
                {
                    PasswordError = "Please enter a password";
                    IsPasswordValid = false;
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    PasswordError = "Passwords do not match";
                    IsPasswordValid = false;
                    return;
                }

                if (!PasswordValidator.IsValidPassword(NewPassword))
                {
                    PasswordError = "Password must be at least 6 characters long and include both letters and numbers";
                    IsPasswordValid = false;
                    return;
                }

            PasswordError = string.Empty;
            IsPasswordValid = true;
        }

        private async void ShowAddStaffDialog()
        {

            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            IsAddMode = true;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            PasswordError = string.Empty;
            IsPasswordValid = false;

            SelectedStaff = new Staff();

            ContentDialog dialog = new ContentDialog
            {
                Title = "Add new staff",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = _xamlRoot,
                Content = new StaffDialogContent(this)
            };

            dialog.PrimaryButtonClick += async (s, e) =>
            {
                if (CanSaveStaff())
                {
                    // Cho phép dialog đóng
                    e.Cancel = false;

                    // Lưu lại logic xử lý sau khi dialog đã đóng
                    dialog.Closed += async (_s, _e) =>
                    {
                        await SaveStaffAsync();
                    };
                }
                else
                {
                    e.Cancel = true; // Ngăn dialog đóng nếu chưa hợp lệ
                }
            };


            await dialog.ShowAsync();
        }

        private async void ShowEditStaffDialog(Staff staff)
        {
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            if (staff != null)
            {
                IsAddMode = false;
                // Tạo bản sao để tránh sửa trực tiếp vào đối tượng gốc
                SelectedStaff = new Staff
                {
                    Staff_Id = staff.Staff_Id,
                    Username = staff.Username,
                    Phone = staff.Phone,
                    Role = staff.Role,
                };

                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                PasswordError = string.Empty;
                IsPasswordValid = true; 

                // Tạo và hiển thị dialog
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Edit Staff",
                    PrimaryButtonText = "Save",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = _xamlRoot,
                    Content = new StaffDialogContent(this)
                };

                // Gắn lệnh vào sự kiện của dialog
                dialog.PrimaryButtonClick += async (s, e) =>
                {
                    if (CanSaveStaff())
                    {
                        // Cho phép dialog đóng
                        e.Cancel = false;

                        // Lưu lại logic xử lý sau khi dialog đã đóng
                        dialog.Closed += async (_s, _e) =>
                        {
                            await SaveStaffAsync();
                        };
                    }
                    else
                    {
                        e.Cancel = true; // Ngăn dialog đóng nếu chưa hợp lệ
                    }
                };

                await dialog.ShowAsync();
            }
        }

        private async void ExecuteDeleteStaff(Staff staff)
        {
           
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            if (staff != null)
            {
                ContentDialog dialog = new ContentDialog
                {
          
                    Title = "Confirm Deletion",
                    Content = $"Are you sure you want to delete the staff member {staff.Username}?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = _xamlRoot
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        await _staffService.DeleteStaffAsync(staff.Staff_Id);
                        Staffs.Remove(staff);
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị thông báo lỗi
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = $"Unable to delete staff: {ex.Message}",
                            CloseButtonText = "Close",
                            XamlRoot = _xamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private bool CanSaveStaff()
        {
            if (SelectedStaff == null)
                return false;
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(SelectedStaff.Username) ||
                string.IsNullOrWhiteSpace(SelectedStaff.Phone))
                return false;

            if (IsAddMode)
            {
                return IsPasswordValid;
            }
            else
            {
                // Chế độ chỉnh sửa, nếu không nhập mật khẩu mới hoặc mật khẩu hợp lệ
                return IsPasswordValid;
            }
        }

        private async Task SaveStaffAsync()
        {
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            try
            {
                if (IsAddMode)
                {
                    // Thêm mật khẩu vào đối tượng nhân viên
                    SelectedStaff.Password = NewPassword;
                    // creatDto DTO
                    var staffCreateDto = new StaffDTO
                    {
                        username = SelectedStaff.Username,
                        password = SelectedStaff.Password,
                        phone = SelectedStaff.Phone,
                        role = SelectedStaff.Role
                    };

                    // Gọi API thêm nhân viên
                    var newStaff = await _staffService.CreateStaffAsync(staffCreateDto);
                    if (newStaff)
                    {
                        SelectedStaff = null;
                        ContentDialog successDialog = new ContentDialog
                        {
                            Title = "Success",
                            Content = "New employee has been added successfully.",
                            CloseButtonText = "Close",
                            XamlRoot = _xamlRoot
                        };
                        await successDialog.ShowAsync();
                        await LoadStaff();
                    }
                    else
                    {
                        // Thông báo lỗi
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "Cannot add new employee (Username already exists).",
                            CloseButtonText = "Close",
                            XamlRoot = _xamlRoot

                        };
                        await errorDialog.ShowAsync();
                    }
                }
                else
                {
                    // Chỉ cập nhật mật khẩu nếu đã nhập
                    if (!string.IsNullOrEmpty(NewPassword))
                    {
                        SelectedStaff.Password = NewPassword;
                    }

                    // Gọi API cập nhật nhân viên
                    StaffDTO staffUpdate = new StaffDTO
                    {
                        username = SelectedStaff.Username,
                        password = SelectedStaff.Password,
                        phone = SelectedStaff.Phone,
                        role = SelectedStaff.Role
                    };
                    var updatedStaff = await _staffService.UpdateStaffAsync(SelectedStaff.Staff_Id, staffUpdate);
                    if (updatedStaff)
                    {
                        // Cập nhật lại danh sách
                        int index = Staffs.IndexOf(Staffs.FirstOrDefault(s => s.Staff_Id == SelectedStaff.Staff_Id));
                        if (index >= 0)
                        {
                            Staffs[index] = SelectedStaff;
                        }

                        // Thông báo thành công
                        ContentDialog successDialog = new ContentDialog
                        {
                            Title = "Success",
                            Content = "Employee updated successfully.",
                            CloseButtonText = "Close",
                            XamlRoot = _xamlRoot

                        };
                        await successDialog.ShowAsync();
                    }
                    else
                    {
                        // Thông báo lỗi
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "Unable to update employee (Username already exists).",
                            CloseButtonText = "Close",
                            XamlRoot = _xamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Unable to save employee: {ex.Message}",
                    CloseButtonText = "Close",
                    XamlRoot = _xamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}