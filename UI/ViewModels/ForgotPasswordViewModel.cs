using RestaurantManager.Models;
using RestaurantManager.Views;
using RestaurantManager.Views.UserControls;
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
    class ForgotPasswordViewModel : BaseViewModel
    {
        public ICommand RecoveryPasswordCommand { get; set; }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string recoveryCode;
        public string RecoveryCode
        {
            get { return recoveryCode; }
            set
            {
                recoveryCode = value;
                OnPropertyChanged();
            }
        }

        private readonly RecoveryModel recoveryModel;
        public ForgotPasswordViewModel()
        {
            recoveryModel = new RecoveryModel();
           
            RecoveryPasswordCommand = new RelayCommand<TextBox>((p) => { return !string.IsNullOrEmpty(p.Text); }, (p) =>
            {
                var code = recoveryModel.generateCode();
                Email = p.Text;
                bool isEmailSent = recoveryModel.sendPasswordRecoveryEmail(Email, code);

                if (isEmailSent)
                {
                    RecoveryCode = code;
                    MessageBox.Show("Đã gửi mã xác nhận đến email của bạn.\nMời nhập mã xác nhận trong cửa số tiếp theo!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    RecoveryPasswordWindow recoveryPasswordWindow = new RecoveryPasswordWindow();
                    var recoveryVM = recoveryPasswordWindow.DataContext as RecoveryPasswordViewModel;
                    if (recoveryVM != null)
                    {
                        recoveryVM.SentCode = code;
                        recoveryVM.TypeInEmail = Email;
                        recoveryPasswordWindow.DataContext = recoveryVM;
                        recoveryPasswordWindow.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Không thể gửi mã xác nhận đến email của bạn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
    