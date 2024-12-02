using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Views;

namespace RestaurantManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> customerList;
        private ObservableCollection<Employee> employeeList;
        private ObservableCollection<Stockin> stockinList;
        
        private Customer newCus;

        public Customer NewCus
        {
            get { return newCus; }
            set 
            { 
                newCus = value; 
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> CustomerList { get { return customerList; } set { if (customerList != value) customerList = value; OnPropertyChanged(); } }
        public ObservableCollection<Employee> EmployeeList { get { return employeeList; } set { if (employeeList != value) employeeList = value; OnPropertyChanged(); } }
        public ObservableCollection<Stockin> StockinList { get { return stockinList; } set { if (stockinList != value) stockinList = value; OnPropertyChanged(); } }
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand {  get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand AddCus { get; set; }
        public ICommand DelCus { get; set; }
        public ICommand ConfigCus { get; set; }
        public ICommand SaveCus { get; set; }
        public ICommand CancelInfo { get; set; }
        private string usernameForProfileWindow;

        public MainViewModel() {
            CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers);
            EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees);
            StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);

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
                        profileVM.AccountID = usernameForProfileWindow;
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
            AddCus = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddCusWindow addACustomer = new AddCusWindow();
                addACustomer.ShowDialog();
            });
        }

    }
}
