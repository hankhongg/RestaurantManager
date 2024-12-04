using Microsoft.EntityFrameworkCore;
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
using XAct;

namespace RestaurantManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> customerList;
        private ObservableCollection<Employee> employeeList;
        private ObservableCollection<Stockin> stockinList;
        
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));

                isSelectedCustomer = _selectedCustomer != null;
            }
        }
        private bool _isSelectedCustomer = false;

        public bool isSelectedCustomer
        {
            get { return _isSelectedCustomer; }
            set 
            {
                _isSelectedCustomer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> CustomerList { get { return customerList; } set { if (customerList != value) customerList = value; OnPropertyChanged(); } }
        public ObservableCollection<Employee> EmployeeList { get { return employeeList; } set { if (employeeList != value) employeeList = value; OnPropertyChanged(); } }
        public ObservableCollection<Stockin> StockinList { get { return stockinList; } set { if (stockinList != value) stockinList = value; OnPropertyChanged(); } }
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand {  get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand AddCusCommand { get; set; }
        public ICommand DelCusCommnad { get; set; }
        public ICommand EditCusCommand { get; set; }
        public ICommand SaveCusCommand { get; set; }
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
                    if (profileVM != null && usernameForProfileWindow != null)
                    {
                        profileVM.AccountID = usernameForProfileWindow;
                        profileVM.isMainProfile = true;
                        profileVM.LoadAccountInformation();
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
            AddCusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddCusWindow addCusWindow = new AddCusWindow();
                addCusWindow.ShowDialog();
                var cusVM = addCusWindow.DataContext as CustomerManagementViewModel;
                if (cusVM != null)
                {
                    cusVM.managementID = 0;
                    if (cusVM.isConfirmed)
                    {
                        // Add new cus into data grid row
                        string query = $"DBCC CHECKIDENT ('CUSTOMER', RESEED, {cusVM.CustomerNumber})";
                        DataProvider.Instance.DB.Database.ExecuteSqlRaw(query);

                        DataProvider.Instance.DB.Customers.Add(cusVM.NewCustomer);
                        DataProvider.Instance.DB.SaveChanges();

                        CustomerList.Add(cusVM.NewCustomer);
                        //CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers);
                    }
                }
            });
            EditCusCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) =>
            {
                AddCusWindow EditCusWindow = new AddCusWindow();

                var cusVM = EditCusWindow.DataContext as CustomerManagementViewModel;
                
                var currCus = DataProvider.Instance.DB.Customers.Where(x => x.CusCode == SelectedCustomer.CusCode).FirstOrDefault();

                if (cusVM != null && currCus != null)
                {
                    cusVM.isReadOnly = true;
                    cusVM.managementID = 1;
                    cusVM.CustomerCode = currCus.CusCode;
                    cusVM.CustomerName = currCus.CusName;
                    cusVM.CustomerPhone = currCus.CusPhone == null ? "" : currCus.CusPhone;
                    cusVM.CustomerCccd = currCus.CusCccd == null ? "" : currCus.CusCccd;
                    cusVM.CustomerEmail = currCus.CusEmail == null ? "" : currCus.CusEmail;
                    cusVM.CustomerAddress = currCus.CusAddr == null ? "" : currCus.CusAddr;
                    
                    EditCusWindow.ShowDialog();
                    
                    if (cusVM.isEdited)
                    {
                        currCus.CusName = cusVM.EditedCustomer.CusName;
                        currCus.CusPhone = cusVM.EditedCustomer.CusPhone;
                        currCus.CusCccd = cusVM.EditedCustomer.CusCccd;
                        currCus.CusEmail = cusVM.EditedCustomer.CusEmail;
                        currCus.CusAddr = cusVM.EditedCustomer.CusAddr;

                        DataProvider.Instance.DB.SaveChanges();

                        var updatedCustomer = CustomerList.FirstOrDefault(c => c.CusCode == currCus.CusCode);
                        if (updatedCustomer != null)
                        {
                            updatedCustomer.CusName = currCus.CusName;
                            updatedCustomer.CusPhone = currCus.CusPhone;
                            updatedCustomer.CusCccd = currCus.CusCccd;
                            updatedCustomer.CusEmail = currCus.CusEmail;
                            updatedCustomer.CusAddr = currCus.CusAddr;
                        }

                        CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.ToList());

                    }
                }
            });
        }
    }
}
