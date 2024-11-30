﻿using RestaurantManager.Models;
using RestaurantManager.Models.Database;
using RestaurantManager.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                    LoadAccount();
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
                }
            }
        }

        public void LoadAccount()
        {
            var acc = DataProvider.Instance.DB.Accounts.Where(x => x.AccUsername == AccountName).FirstOrDefault();
            if (acc != null)
            {
                AccountName = acc.AccDisplayname;
                AccountEmail = acc.AccEmail;
                AccountPhoneNumber = acc.AccPhone;
                AccountID = acc.AccUsername;
                AccountPassword = acc.AccPassword;
            }

        }


        public ICommand PasswordChangedCommand { get; set; }
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
            //PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            //{
            //    p.Password = AccountPassword;
            //});

        }
    }
}
