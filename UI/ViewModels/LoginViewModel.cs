using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public bool isLogin {  get; set; }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }
        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            isLogin = false;
            ForgotPasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
                { 
                    ForgotPasswordWindow passwordWindow = new ForgotPasswordWindow();
                    passwordWindow.ShowDialog();
                }
            );
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => 
                {
                    Login(p);
                }
            );
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            });
        }

        public void Login(Window p)
        {
            var account = DataProvider.Instance.DB.Accounts.Where(x => x.AccUsername == Username && x.AccPassword == Password).Count();

            if (account > 0)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                isLogin = true;
                p.Close();
            }
            else { 
                isLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


        }
    }
}
