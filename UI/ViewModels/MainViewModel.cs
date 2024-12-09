using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using RestaurantManager.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
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
       
        
        private string usernameForProfileWindow;
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand {  get; set; }
        public ICommand AddOrderCommand { get; set; }

        // Customer Management
        private ObservableCollection<Customer> customerList;
        public ObservableCollection<Customer> CustomerList { get { return customerList; } set { if (customerList != value) customerList = value; OnPropertyChanged(); } }

        public ICommand AddCusCommand { get; set; }
        public ICommand DelCusCommand { get; set; }
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

        // ----------------------------

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

        // ----------------------------


        // Stockin Management
        private ObservableCollection<Stockin> stockinList;
        public ObservableCollection<Stockin> StockinList { get { return stockinList; } set { if (stockinList != value) stockinList = value; OnPropertyChanged(); } }
        public ICommand AddStockinCommand { get; set; }
        public ICommand DelStockinCommand { get; set; }
        public ICommand EditStockinCommand { get; set; }
        public ICommand EnterStockinDetailsCommand { get; set; }
        private Stockin _selectedStockin;
        public Stockin SelectedStockin
        {
            get => _selectedStockin;
            set
            {
                _selectedStockin = value;
                OnPropertyChanged(nameof(SelectedStockin));

                isSelectedStockin = _selectedStockin != null;
            }
        }
        private bool _isSelectedStockin = false;
        public bool isSelectedStockin
        {
            get { return _isSelectedStockin; }
            set
            {
                _isSelectedStockin = value;
                OnPropertyChanged();
            }
        }
        private StockinDetailsDrinkOther _selectedStockinDetailsDrinkOther;

        public StockinDetailsDrinkOther SelectedStockInDetailsDrinkOther
        {
            get { return _selectedStockinDetailsDrinkOther; }
            set 
            {
                _selectedStockinDetailsDrinkOther = value;
                OnPropertyChanged();
            }
        }
        // same with ingre
        private StockinDetailsDrinkOther _selectedStockinDetailsIngre;
        public StockinDetailsDrinkOther SelectedStockInDetailsIngre
        {
            get { return _selectedStockinDetailsIngre; }
            set
            {
                _selectedStockinDetailsIngre = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel() {
            CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false));
            EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false));
            //StockinList = new ObservableCollection<Stockin>(
            //        (from stkIn in DataProvider.Instance.DB.Stockins
            //         join stkInDetails in DataProvider.Instance.DB.StockinDetailsIngre
            //         on stkIn.StoId equals stkInDetails.StoId
            //         join ingre in DataProvider.Instance.DB.Ingredients
            //         on stkInDetails.IngreId equals ingre.IngreId
            //         select stkIn)
            //        .Distinct());

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

            //Customer Management
            AddCusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddCusWindow addCusWindow = new AddCusWindow();
                var cusVM = addCusWindow.DataContext as CustomerManagementViewModel;
                if (cusVM != null)
                {
                    cusVM.managementID = 0;
                    addCusWindow.ShowDialog();
                    if (cusVM.isConfirmed)
                    {
                        // Add new cus into data grid row
                        int existedCusNumber = DataProvider.Instance.DB.Customers.Count();
                        string query = $"DBCC CHECKIDENT ('CUSTOMER', RESEED, {existedCusNumber + 1})";
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
                    cusVM.LoadCustomerInformation(currCus);

                    EditCusWindow.ShowDialog();

                    if (cusVM.isEdited)
                    {
                        cusVM.isReadOnly = false;
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

                        CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false));
                    }
                }
            });
            DelCusCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) =>
            {
                var cus = DataProvider.Instance.DB.Customers.Where(x => x.CusCode == SelectedCustomer.CusCode).FirstOrDefault();
                var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (cus != null && DialogResult == MessageBoxResult.Yes) 
                {
                    cus.Isdeleted = true;
                    cus.CusCode = "";
                    DataProvider.Instance.DB.SaveChanges();
                    
                    CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false));

                    int i = 0;
                    foreach (Customer customer in CustomerList)
                    {
                        customer.CusCode = $"KH{++i:D3}";
                    }
                    DataProvider.Instance.DB.SaveChanges();

                    CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false));
                }
            });
            // Employee Management
            AddEmpCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddEmpWindow addEmpWindow = new AddEmpWindow();
                var empVM = addEmpWindow.DataContext as EmployeeManagementViewModel;
                
                if (empVM != null)
                {
                    empVM.managementID = 0;
                    addEmpWindow.ShowDialog();
                    if (empVM.isConfirmed)
                    {
                        // Add new emp into data grid row
                        int existedEmpNumber = DataProvider.Instance.DB.Employees.Count();

                        var existedEmp = DataProvider.Instance.DB.Employees.OrderByDescending(x => x.EmpCode).FirstOrDefault();
                        string query = $"DBCC CHECKIDENT ('EMPLOYEE', RESEED, {existedEmpNumber + 1})";
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
                        empVM.isReadOnly = false;
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
                        EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false).ToList());
                    }
                }
            });
            DelEmpCommand = new RelayCommand<object>((p) => SelectedEmployee != null, (p) =>
            {
                var emp = DataProvider.Instance.DB.Employees.Where(x => x.EmpCode == SelectedEmployee.EmpCode).FirstOrDefault();
                var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (emp != null && DialogResult == MessageBoxResult.Yes)
                {
                    emp.Isdeleted = true;
                    emp.EmpCode = "";
                    DataProvider.Instance.DB.SaveChanges();

                    EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false));

                    int i = 0;
                    foreach (Employee employee in EmployeeList)
                    {
                        employee.EmpCode = $"NV{++i:D3}";
                    }
                    DataProvider.Instance.DB.SaveChanges();

                    EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false));
                }
            });

            // StockIn Management
            AddStockinCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddStockInWindow stockInManagementWindow = new AddStockInWindow();
                var stockInVM = stockInManagementWindow.DataContext as StockInManagementViewModel;
                if (stockInVM != null)
                {
                    stockInVM.managementID = 0;
                    stockInVM.StockInID = (DataProvider.Instance.DB.Stockins.Count() + 1).ToString();
                    StockInDetailsWindow stockInDetailsWindow = new StockInDetailsWindow();
                    var stockInDetailsVM = stockInDetailsWindow.DataContext as StockInManagementViewModel;
                    int existedStockInNumber = DataProvider.Instance.DB.Stockins.Count();
                    stockInManagementWindow.ShowDialog();
                    if (stockInVM.isConfirmed)
                    {
                        // Add new stockin into data grid row
                        try
                        {
                            DataProvider.Instance.DB.Stockins.Add(stockInVM.NewStockIn);
                            DataProvider.Instance.DB.SaveChanges();
                            StockinList.Add(stockInVM.NewStockIn);

                            StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);

                            stockInDetailsVM.StockInID = (existedStockInNumber + 1).ToString();
                            stockInDetailsWindow.ShowDialog();

                            //if (stockInVM.managementID == 2)
                            //{
                            //    if (stockInVM.SelectedIdxStockin == 0)
                            //    {
                            //        DataProvider.Instance.DB.StockinDetailsIngre.Add(stockInVM.NewIngreStockin);
                            //    }
                            //    else
                            //    {
                            //        DataProvider.Instance.DB.StockinDetailsDrinkOthers.Add(stockInVM.NewDrinkOtherStockin);
                            //    }
                            //    DataProvider.Instance.DB.SaveChanges();
                            //}
                            //StockinList = new ObservableCollection<Stockin>(
                            //    (from stkIn in DataProvider.Instance.DB.Stockins
                            //        join stkInDetails in DataProvider.Instance.DB.StockinDetailsIngre
                            //        on stkIn.StoId equals stkInDetails.StoId
                            //        join ingre in DataProvider.Instance.DB.Ingredients
                            //        on stkInDetails.IngreId equals ingre.IngreId
                            //        select stkIn)
                            //    .Distinct());
                            // Cập nhật STO_PRICE từ STOCKIN_DETAILS_INGRE
                            //                            string query1 = @"
                            //                                SET STO_PRICE = (
                            //                                    SELECT SUM(SI.CPRICE * SI.QUANTITY_KG)
                            //                                    FROM STOCKIN_DETAILS_INGRE SI
                            //                                    WHERE SI.STO_ID = STOCKIN.STO_ID
                            //                                )
                            //                                WHERE STO_ID IN (SELECT STO_ID FROM STOCKIN_DETAILS_INGRE);
                            //";

                            //                            // Thực thi câu lệnh SQL đầu tiên
                            //                            DataProvider.Instance.DB.Database.ExecuteSqlRaw(query1);

                            //                            // Cập nhật STO_PRICE từ STOCKIN_DETAILS_DRINK_OTHER
                            //                            string query2 = @"
                            //                                UPDATE STOCKIN
                            //                                SET STO_PRICE = COALESCE(STO_PRICE, 0) + (
                            //                                    SELECT SUM(SD.CPRICE * SD.QUANTITY_UNITS)
                            //                                    FROM STOCKIN_DETAILS_DRINK_OTHER SD
                            //                                    WHERE SD.STO_ID = STOCKIN.STO_ID
                            //                                )
                            //                                WHERE STO_ID IN (SELECT STO_ID FROM STOCKIN_DETAILS_DRINK_OTHER);
                            //                            ";

                            // Thực thi câu lệnh SQL thứ hai
                            //DataProvider.Instance.DB.Database.ExecuteSqlRaw(query2);

                        }
                        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                        {
                            MessageBox.Show("Đã tồn tại mã nguyên liệu hoặc mã mặt hàng, vui lòng thử lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        
                    }
                }
            });
            
            DelStockinCommand = new RelayCommand<object>((p) => SelectedStockin != null, (p) =>
            {
                var stockIn = DataProvider.Instance.DB.Stockins.Where(x => x.StoId == SelectedStockin.StoId).FirstOrDefault();
                var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (stockIn != null && DialogResult == MessageBoxResult.Yes)
                {
                    DataProvider.Instance.DB.Stockins.Remove(stockIn);
                    DataProvider.Instance.DB.SaveChanges();

                    StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);


                    int i = 0;
                    foreach (Stockin stkIn in StockinList)
                    {
                        stkIn.StoCode = $"ST{++i:D6}";
                    }
                    DataProvider.Instance.DB.SaveChanges();

                    StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
                }
            });
            EnterStockinDetailsCommand = new RelayCommand<object>((p) => SelectedStockin != null, (p) =>
            {
                StockInDetailsWindow stockInDetailsWindow = new StockInDetailsWindow();
                var stockInDetailsVM = stockInDetailsWindow.DataContext as StockInManagementViewModel;
                if (stockInDetailsVM != null)
                {
                    stockInDetailsVM.StockInDetailsIngresList = new ObservableCollection<StockinDetailsIngre>(DataProvider.Instance.DB.StockinDetailsIngre.Where(x => x.StoId == SelectedStockin.StoId));
                    stockInDetailsVM.StockInDetailsDrinkOtherList = new ObservableCollection<StockinDetailsDrinkOther>(DataProvider.Instance.DB.StockinDetailsDrinkOthers.Where(x => x.StoId == SelectedStockin.StoId));
                    
                    stockInDetailsVM.IngredientNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Ingredients.Select(x => x.IngreName));
                    stockInDetailsVM.MenuItemsNameList = new ObservableCollection<string>(DataProvider.Instance.DB.MenuItems.Select(x => x.ItemName));
                    //stockInDetailsVM.SelectedTable = stockInDetailsVM.StockInDetailsDrinkOtherList;

                    //stockInDetailsVM.SelectedTable = StockInDetailsIngresList;
                    //SelectedTable = StockInDetailsDrinkOtherList;

                    //stockInDetailsVM.LoadStockinDetails();
                    stockInDetailsVM.UpdateStockInType();
                    stockInDetailsWindow.ShowDialog();
                }
            });
            //EditStockinCommand = new RelayCommand<object>((p) => SelectedStockin != null, (p) =>
            //{
            //    AddStockInWindow EditStockInWindow = new AddStockInWindow();
            //    var stockInVM = EditStockInWindow.DataContext as StockInManagementViewModel;
            //    var currStockIn = DataProvider.Instance.DB.Stockins.Where(x => x.StoId == SelectedStockin.StoId).FirstOrDefault();

            //    if (stockInVM != null && currStockIn != null)
            //    {
            //        stockInVM.LoadStockin(currStockIn);

            //        EditStockInWindow.ShowDialog();

            //        if (stockInVM.isEdited)
            //        {
            //            currStockIn.StoDate = stockInVM.EditedStockIn.StoDate;

            //            DataProvider.Instance.DB.SaveChanges();

            //            var updatedStockIn = StockinList.FirstOrDefault(c => c.StoId == currStockIn.StoId);
            //            if (updatedStockIn != null)
            //            {
            //                updatedStockIn.StoPrice = currStockIn.StoPrice;
            //                updatedStockIn.StoDate = currStockIn.StoDate;
            //            }
            //            StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
            //        }
            //    }
            //});

        }
    }
}
