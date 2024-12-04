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
        private ObservableCollection<Stockin> stockinList;
        public ObservableCollection<Stockin> StockinList { get { return stockinList; } set { if (stockinList != value) stockinList = value; OnPropertyChanged(); } }
        
        private string usernameForProfileWindow;
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand {  get; set; }
        public ICommand AddOrderCommand { get; set; }

        // Customer Management
        private ObservableCollection<Customer> customerList;
        public ObservableCollection<Customer> CustomerList { get { return customerList; } set { if (customerList != value) customerList = value; OnPropertyChanged(); } }

        public ICommand AddCusCommand { get; set; }
        public ICommand DelCusCommnad { get; set; }
        public ICommand EditCusCommand { get; set; }
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

        // Employee Management
        private ObservableCollection<Employee> employeeList;
        public ObservableCollection<Employee> EmployeeList { get { return employeeList; } set { if (employeeList != value) employeeList = value; OnPropertyChanged(); } }
        public ICommand AddEmpCommand { get; set; }
        public ICommand DelEmpCommand { get; set; }
        public ICommand EditEmpCommand { get; set; }
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));

                isSelectedEmployee = _selectedEmployee != null;
            }
        }
        private bool _isSelectedEmployee = false;
        public bool isSelectedEmployee
        {
            get { return _isSelectedEmployee; }
            set
            {
                _isSelectedEmployee = value;
                OnPropertyChanged();
            }
        }


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
            // Customer Management
            //AddCusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            //{
            //    AddCusWindow addCusWindow = new AddCusWindow();
            //    addCusWindow.ShowDialog();
            //    var cusVM = addCusWindow.DataContext as CustomerManagementViewModel;
            //    if (cusVM != null)
            //    {
            //        cusVM.managementID = 0;
            //        if (cusVM.isConfirmed)
            //        {
            //            // Add new cus into data grid row
            //            string query = $"DBCC CHECKIDENT ('CUSTOMER', RESEED, {cusVM.CustomerNumber})";
            //            DataProvider.Instance.DB.Database.ExecuteSqlRaw(query);

            //            DataProvider.Instance.DB.Customers.Add(cusVM.NewCustomer);
            //            DataProvider.Instance.DB.SaveChanges();

            //            CustomerList.Add(cusVM.NewCustomer);
            //            //CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers);
            //        }
            //    }
            //});
            //EditCusCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) =>
            //{
            //    AddCusWindow EditCusWindow = new AddCusWindow();

            //    var cusVM = EditCusWindow.DataContext as CustomerManagementViewModel;
                
            //    var currCus = DataProvider.Instance.DB.Customers.Where(x => x.CusCode == SelectedCustomer.CusCode).FirstOrDefault();

            //    if (cusVM != null && currCus != null)
            //    {
            //        cusVM.LoadCustomerInformation(currCus);

            //        EditCusWindow.ShowDialog();
                    
            //        if (cusVM.isEdited)
            //        {
            //            currCus.CusName = cusVM.EditedCustomer.CusName;
            //            currCus.CusPhone = cusVM.EditedCustomer.CusPhone;
            //            currCus.CusCccd = cusVM.EditedCustomer.CusCccd;
            //            currCus.CusEmail = cusVM.EditedCustomer.CusEmail;
            //            currCus.CusAddr = cusVM.EditedCustomer.CusAddr;

            //            DataProvider.Instance.DB.SaveChanges();

            //            var updatedCustomer = CustomerList.FirstOrDefault(c => c.CusCode == currCus.CusCode);
            //            if (updatedCustomer != null)
            //            {
            //                updatedCustomer.CusName = currCus.CusName;
            //                updatedCustomer.CusPhone = currCus.CusPhone;
            //                updatedCustomer.CusCccd = currCus.CusCccd;
            //                updatedCustomer.CusEmail = currCus.CusEmail;
            //                updatedCustomer.CusAddr = currCus.CusAddr;
            //            }

            //            CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.ToList());
            //        }
            //    }
            //});
            // Employee Management
            AddEmpCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddEmpWindow addEmpWindow = new AddEmpWindow();
                addEmpWindow.ShowDialog();
                var empVM = addEmpWindow.DataContext as EmployeeManagementViewModel;
                if (empVM != null)
                {
                    empVM.managementID = 0;
                    if (empVM.isConfirmed)
                    {
                        // Add new cus into data grid row
                        string query = $"DBCC CHECKIDENT ('EMPLOYEE', RESEED, {empVM.EmployeeNumber})";
                        DataProvider.Instance.DB.Database.ExecuteSqlRaw(query);

                        DataProvider.Instance.DB.Employees.Add(empVM.NewEmployee);
                        DataProvider.Instance.DB.SaveChanges();

                        EmployeeList.Add(empVM.NewEmployee);
                        //CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers);
                    }
                }
            });
            EditEmpCommand = new RelayCommand<object>((p) => SelectedEmployee != null, (p) =>
            {
                AddEmpWindow EditEmpWindow = new AddEmpWindow();

                var empVM = EditEmpWindow.DataContext as EmployeeManagementViewModel;

                var currEmp = DataProvider.Instance.DB.Employees.Where(x => x.EmpCode == SelectedEmployee.EmpCode).FirstOrDefault();

                if (empVM != null && currEmp != null)
                {
                    empVM.LoadEmployeeInformation(currEmp);

                    EditEmpWindow.ShowDialog();

                    if (empVM.isEdited)
                    {
                        currEmp.EmpName = empVM.EditedEmployee.EmpName;
                        currEmp.EmpPhone = empVM.EditedEmployee.EmpPhone;
                        currEmp.EmpCccd = empVM.EditedEmployee.EmpCccd;
                        currEmp.EmpSalary = empVM.EditedEmployee.EmpSalary;
                        currEmp.EmpRole = empVM.EditedEmployee.EmpRole;

                        DataProvider.Instance.DB.SaveChanges();

                        var updatedEmployee = EmployeeList.FirstOrDefault(c => c.EmpCode == currEmp.EmpCode);
                        if (updatedEmployee != null)
                        {
                            updatedEmployee.EmpName = currEmp.EmpName;
                            updatedEmployee.EmpPhone = currEmp.EmpPhone;
                            updatedEmployee.EmpCccd = currEmp.EmpCccd;
                            updatedEmployee.EmpSalary = currEmp.EmpSalary;
                            updatedEmployee.EmpRole = currEmp.EmpRole;
                        }

                        EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees.ToList());
                    }
                }
            });
        }
    }
}
