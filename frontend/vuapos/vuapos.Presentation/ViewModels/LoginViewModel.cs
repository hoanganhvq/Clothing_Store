using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using vuapos.Presentation.Commands;
using vuapos.Presentation.DTO.Login;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IUserSession _userSession;
        private readonly IAuthService _authService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler LoginSuccessful;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
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
                }
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IUserSession userSession, IAuthService authService)
        {
            _userSession = userSession;
            _authService = authService;
            LoginCommand = new RelayCommand(param => Login());
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoading;
        }

        private async void Login()
        {
            Debug.WriteLine("Login() started");

            if (CanLogin())
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                Debug.WriteLine("CanLogin: true");
            }
            else
            {
                ErrorMessage = "Vui lòng nhập tên đăng nhập và mật khẩu.";
                Debug.WriteLine("CanLogin: false - thiếu username hoặc password");
                return;
            }

            try
            {
                var loginRequest = new LoginRequest
                {
                    username = Username,
                    password = Password
                };

                Debug.WriteLine($"Sending login request with username: {Username}");

                var result = await _authService.LoginAsync(loginRequest);

                if (result != null)
                {
                    Debug.WriteLine("✅ Login result:");
                    Debug.WriteLine($"   AccessToken: {result.Access_token}");
                    Debug.WriteLine($"   Role: {result.Role}");
                    Debug.WriteLine($"   StaffId: {result.Staff_id}");  
                }
                else
                {
                    Debug.WriteLine("❌ Login result: null");
                }


                if (result != null && !string.IsNullOrWhiteSpace(result.Access_token))
                {
                    Debug.WriteLine("Login successful. Access token: " + result.Access_token);

                    // Đăng nhập thành công - chuyển tới màn hình chính
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Login failed. Please check your account information.";
                    Debug.WriteLine("Login failed - no access token returned");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Debug.WriteLine($"Exception during login: {ex}");
            }
            finally
            {
                IsLoading = false;
                Debug.WriteLine("Login() finished");
            }
        }


        public void OnLoginSuccess()
        {
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
