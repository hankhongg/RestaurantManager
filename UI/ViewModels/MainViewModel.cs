﻿using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand {  get; set; }
        public ICommand AddOrderCommand { get; set; }

        private string usernameForProfileWindow;

        public MainViewModel() {

            WindowIsLoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => 
                { 
                    p.Hide();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog(); 
                    var loginVM = loginWindow.DataContext as LoginViewModel;
                    if (loginVM != null)
                    {
                        if (loginVM.isLogin)
                        {
                            loginVM.isLogin = false;
                            p.Show();
                            usernameForProfileWindow = loginVM.Username;
                        }
                        else p.Close();
                    }
                }
            );
            ProfileManagementCommand = new RelayCommand<object>( (p) => { return true; }, (p) =>
                {
                    ProfileWindow profileWindow = new ProfileWindow();
                    var profileVM = profileWindow.DataContext as ProfileViewModel;
                    if (profileVM != null)
                    {
                        profileVM.AccountName = usernameForProfileWindow;
                        profileWindow.DataContext = profileVM;
                        profileWindow.ShowDialog();
                    }
                }
            );
            AddOrderCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    FoodLayoutWindow foodLayoutWindow = new FoodLayoutWindow();
                    foodLayoutWindow.ShowDialog();
                }
            );
        }

    }
}
