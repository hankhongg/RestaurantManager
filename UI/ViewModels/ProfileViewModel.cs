using RestaurantManager.Models;
using RestaurantManager.Models.Database;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XSystem.Security.Cryptography;

namespace RestaurantManager.ViewModels
{
    internal class ProfileViewModel : BaseViewModel
    {
       // private ObservableCollection<Account> accounts;
        //public ObservableCollection<Account> Accounts { get { return accounts; } set { if (accounts != value) accounts = value; OnPropertyChanged(); } }


        private bool isButtonPressed;
        public bool IsButtonPressed
        {
            get => isButtonPressed; set
            {
               
                isButtonPressed = value;
                OnPropertyChanged();
            }
        }

        private string accountName;
        public string AccountName
        {
            get { return accountName; }
            set
            {
                if (accountName != value)
                {

                    accountName = value;
                    OnPropertyChanged();  // Notify the UI that AccountName has changed
                    LoadAccountInformation();

                }
            }
        }

        private string accountEmail;
        public string AccountEmail
        {
            get { return accountEmail; }
            set
            {
                if (accountEmail != value)
                {
                    accountEmail = value;
                    OnPropertyChanged();  // Notify the UI that AccountName has changed
                    LoadAccountInformation();
                }
            }
        }

        private string accountPhoneNumber;
        public string AccountPhoneNumber
        {
            get { return accountPhoneNumber; }
            set
            {
                if (accountPhoneNumber != value)
                {
                    accountPhoneNumber = value;
                    OnPropertyChanged();  // Notify the UI that AccountName has changed
                    LoadAccountInformation();
                }
            }
        }

        private string accountID;
        public string AccountID
        {
            get { return accountID; }
            set
            {
                if (accountID != value)
                {
                    accountID = value;
                    OnPropertyChanged();  // Notify the UI that AccountName has changed
                    LoadAccountInformation();
                }
            }
        }

        private string accountPassword;
        public string AccountPassword
        {
            get { return accountPassword; }
            set
            {
                if (accountPassword != value)
                {
                    accountPassword = value;
                    OnPropertyChanged();  // Notify the UI that AccountName has changed
                    LoadAccountInformation();
                }
            }
        }

        private string currentPassword;
        public string CurrentPassword { get => currentPassword; set { currentPassword = value; OnPropertyChanged(); } }

        public void LoadAccountInformation()
        {
            var acc = DataProvider.Instance.DB.Accounts.Where(x => x.AccUsername == AccountID || x.AccEmail == AccountEmail).FirstOrDefault();
            if (acc != null)
            {
                AccountName = acc.AccDisplayname;
                AccountEmail = acc.AccEmail;
                AccountPhoneNumber = acc.AccPhone;
                AccountID = acc.AccUsername;
                AccountPassword = acc.AccPassword;
            }

        }

        public void SaveAccountInformation()
        {
            bool checkEmptyOrNull = string.IsNullOrEmpty(AccountName) || string.IsNullOrEmpty(AccountEmail) || string.IsNullOrEmpty(AccountPhoneNumber) || string.IsNullOrEmpty(AccountID);
            var acc = DataProvider.Instance.DB.Accounts.Where(y => y.AccUsername == AccountID).FirstOrDefault();
            if (checkEmptyOrNull)
            {
                MessageBox.Show("Không được để trống thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if (acc == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                acc.AccDisplayname = AccountName;
                acc.AccEmail = AccountEmail;
                acc.AccPhone = AccountPhoneNumber;
                acc.AccUsername = AccountID;
                if (acc.AccPassword != AccountPassword)
                {
                    acc.AccPassword = MD5Hash(Base64Encode(AccountPassword));
                }
                DataProvider.Instance.DB.SaveChanges();
                MessageBox.Show($"Lưu thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public ICommand CheckPasswordChanged { get; set; }
        public ICommand PasswordBoxLoadedCommand { get; set; }
        public ICommand IsButtonPressedCommand { get; set; }
        public ICommand SaveButtonPressedCommand { get; set; }
        public ProfileViewModel()
        {
            IsButtonPressedCommand = new RelayCommand<object>((p) =>
                {
                    return !IsButtonPressed;
                },
                (p) =>
                {
                    currentPassword = AccountPassword;
                    IsButtonPressed = !IsButtonPressed;
                }
            );
            SaveButtonPressedCommand = new RelayCommand<object>((p) =>
                {
                    return IsButtonPressed;
                },
                (p) =>
                {
                    MessageBoxResult r = MessageBox.Show("Bấm Yes để xác nhận lưu thông tin!", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                    if (r == MessageBoxResult.Yes)
                    {
                        
                        SaveAccountInformation();
                        IsButtonPressed = false;
                    }
                }
            );
            PasswordBoxLoadedCommand = new RelayCommand<ProfileWindow>((p) => { return true; }, (p) =>
            {
                p.passwordBox.Password = AccountPassword;
            });
            CheckPasswordChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => {
                //MessageBox.Show($"{AccountPassword}");
                if (!string.IsNullOrEmpty(p.Password))
                {
                    AccountPassword = p.Password;  
                }
            }) ;
        }
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string MD5Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            StringBuilder sb = new StringBuilder();
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
