using RestaurantManager.Models;
using RestaurantManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    internal class ProfileViewModel : BaseViewModel
    {
        private ObservableCollection<Account> accounts;
        public ObservableCollection<Account> Accounts { get { return accounts; } set { if (accounts != value) accounts = value; OnPropertyChanged(); } }


        private bool isButtonPressed;
        public bool IsButtonPressed
        {
            get => isButtonPressed; set
            {
                AccountName = accounts[0].AccDisplayname;
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
                }
            }
        }

        public void LoadAccounts()
        {
            using (var context = new Models.Database.QlnhContext())
            {
                var accountsList = context.Accounts.ToList();
                Accounts = new ObservableCollection<Account>(accountsList);
            }
            
        }

        public ICommand IsButtonPressedCommand { get; set; }
        public ICommand SaveButtonPressedCommand { get; set; }
        public ProfileViewModel()
        {

            LoadAccounts();
            IsButtonPressedCommand = new RelayCommand<object>((p) =>
                {
                    return !IsButtonPressed;
                },
                (p) =>
                {
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
                        // lưu vào database! <=
                        IsButtonPressed = false;
                    }
                }
            );
            
        }
    }
}
