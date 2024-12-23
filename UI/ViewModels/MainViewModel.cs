﻿using LiveCharts;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using RestaurantManager.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Drawing;
using System.Globalization;
using PdfDocument = PdfSharpCore.Pdf.PdfDocument;
using PdfPage = PdfSharpCore.Pdf.PdfPage;
using System.Windows.Diagnostics;
using UI.Views;
using XAct;
using System.IO;

namespace RestaurantManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        bool isAdmin = false;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }


        private string usernameForProfileWindow;
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand { get; set; }
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

        // Ingredients & Items Management
        private ObservableCollection<Ingredient> ingredientsList;
        public ObservableCollection<Ingredient> IngredientsList { get { return ingredientsList; } set { if (ingredientsList != value) ingredientsList = value; OnPropertyChanged(); } }

        private Ingredient selectedIngre;
        public Ingredient SelectedIngre
        {
            get { return selectedIngre; }
            set
            {
                selectedIngre = value;
                OnPropertyChanged();
                IsSelectedIngre = selectedIngre != null;
            }
        }
        private bool isSelectedIngre;
        public bool IsSelectedIngre
        {
            get { return isSelectedIngre; }
            set
            {
                isSelectedIngre = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MenuItem> itemsList;
        public ObservableCollection<MenuItem> ItemsList { get { return itemsList; } set { if (itemsList != value) itemsList = value; OnPropertyChanged(); } }

        private MenuItem selectedItem;
        public MenuItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
                IsSelectedItem = selectedItem != null;
            }
        }

        private bool isSelectedItem;
        public bool IsSelectedItem
        {
            get { return isSelectedItem; }
            set
            {
                isSelectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddIngreCommand { get; set; }
        public ICommand DelIngreCommand { get; set; }
        public ICommand EditIngreCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand DelItemCommand { get; set; }
        public ICommand EditItemCommand { get; set; }
        public ICommand PrintFinancialHistoryReport { get; set; }

        //
        // --TableViewModel Management
        private ObservableCollection<TableViewModel> tableViewList;
        public ObservableCollection<TableViewModel> TableViewList { get { return tableViewList; } set { if (tableViewList != value) tableViewList = value; OnPropertyChanged(); } }
        private IEnumerable<string> tableNumbers;

        public IEnumerable<string> TableNumbers
        {
            get { return tableNumbers; }
            set
            {
                tableNumbers = value;
                OnPropertyChanged();
            }
        }
        // --TableViewModel Management
        private ObservableCollection<DiningTable> tableList;
        public ObservableCollection<DiningTable> TableList { get { return tableList; } set { if (tableList != value) tableList = value; OnPropertyChanged(); } }

        public ICommand AddTableCommand { get; set; }
        public ICommand EditTableCommand { get; set; }

        // Booking Management
        // BookingViewModel Management
        public ICommand CancelBooking { get; set; }
        public ICommand ClaimBooking { get; set; }
        private ObservableCollection<BookingConfigurationViewModel> bookingViewList;
        public ObservableCollection<BookingConfigurationViewModel> BookingViewList { get { return bookingViewList; } set { if (bookingViewList != value) bookingViewList = value; OnPropertyChanged(); } }

        private ObservableCollection<Booking> bookingList;
        public ObservableCollection<Booking> BookingList { get { return bookingList; } set { if (bookingList != value) bookingList = value; OnPropertyChanged(); } }

        private string _selectedTable;

        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                if (_selectedTable != null)
                {
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabled;

        public bool isEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        private string existedBooking;

        public string ExistedBooking
        {
            get { return existedBooking; }
            set
            {
                existedBooking = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddBookingCommand { get; set; }
        public ICommand BookingInfo { get; set; }

        // ----------------------------
        // Income Management
        private ObservableCollection<InfoIncomeViewModel> infoIncomeVMs;
        public ObservableCollection<InfoIncomeViewModel> InfoIncomeVMs
        {
            get { return infoIncomeVMs; }
            set
            {
                infoIncomeVMs = value;
                OnPropertyChanged();
            }
        }
        private string nhanVienToday;
        private string nhanVienYesterday;
        private decimal incomeToday;
        private decimal incomeYesterday;
        private int billToday;
        private int billYesterday;

        public string NhanVienToday
        {
            get { return nhanVienToday; }
            set
            {
                nhanVienToday = value;
                OnPropertyChanged();
            }
        }
        public string NhanVienYesterday
        {
            get { return nhanVienYesterday; }
            set
            {
                nhanVienYesterday = value;
                OnPropertyChanged();
            }
        }

        private int nhanVienTodayPer;
        public int NhanVienTodayPer
        {
            get { return nhanVienTodayPer; }
            set
            {
                nhanVienTodayPer = value;
                OnPropertyChanged();
            }
        }

        private int nhanVienYesterdayPer;
        public int NhanVienYesterdayPer { get => nhanVienYesterdayPer; set { nhanVienYesterdayPer = value; OnPropertyChanged(); } }

        public decimal IncomeToday
        {
            get { return incomeToday; }
            set
            {
                incomeToday = value;
                OnPropertyChanged(nameof(IncomeToday));
            }
        }
        public decimal IncomeYesterday
        {
            get { return incomeYesterday; }
            set
            {
                incomeYesterday = value;
                OnPropertyChanged();
            }
        }
        public int BillToday
        {
            get { return billToday; }
            set
            {
                billToday = value;
                OnPropertyChanged();
            }
        }
        public int BillYesterday
        {
            get { return billYesterday; }
            set
            {
                billYesterday = value;
                OnPropertyChanged();
            }
        }

        // ----------------------------
        // Income chart management
        private ChartValues<decimal> incomeValues;
        private ChartValues<decimal> expenseValues;
        private ChartValues<decimal> profitValues;
        private List<string> labels;
        public ChartValues<decimal> IncomeValues
        {
            get { return incomeValues; }
            set
            {
                incomeValues = value;
                OnPropertyChanged();
            }
        }
        public ChartValues<decimal> ExpenseValues
        {
            get { return expenseValues; }
            set
            {
                expenseValues = value;
                OnPropertyChanged();
            }
        }
        public ChartValues<decimal> ProfitValues
        {
            get { return profitValues; }
            set
            {
                profitValues = value;
                OnPropertyChanged();
            }
        }
        public List<string> Labels
        {
            get { return labels; }
            set
            {
                labels = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> YFormatter { get; set; }
        private DateTime startDate = DateTime.Now.Date.AddDays(-7);
        private DateTime endDate = DateTime.Now.Date;

        public DateTime StartDate 
        {
            get { return startDate; }
            set
            {
                if (value > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                startDate = value;
                OnPropertyChanged();
                LoadChartData();
            }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value < startDate)
                {
                    MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                endDate = value;
                OnPropertyChanged();
                LoadChartData();
            }
        }
        private void UpdateLabels(List<DateTime> availableDates, string format)
        {
            if (availableDates == null || !availableDates.Any())
            {
                Labels = new List<string> { "Không có dữ liệu" };
                return;
            }

            Labels = availableDates.Select(date => date.ToString(format)).ToList();
        }

        public void LoadChartData()
        {
            YFormatter = value => value.ToString("N0");

            var financialHistories = DataProvider.Instance.DB.FinancialHistories
                .Where(con => con.FinDate.Date >= StartDate.Date && con.FinDate.Date <= EndDate.Date)
                .ToList();

            if (!financialHistories.Any())
            {
                IncomeValues = new ChartValues<decimal> { 0 };
                ExpenseValues = new ChartValues<decimal> { 0 };
                ProfitValues = new ChartValues<decimal> { 0 };
                Labels = new List<string> { "Không có dữ liệu" };
                return;
            }

            string labelFormat = "dd-MM"; // default value
            var groupedData = financialHistories.GroupBy(x =>
            {
                if ((EndDate - StartDate).TotalDays > 365)
                {
                    labelFormat = "yyyy";
                    return new DateTime(x.FinDate.Year, 1, 1);
                }
                else if ((EndDate - StartDate).TotalDays > 31)
                {
                    labelFormat = "MM-yyyy";
                    return new DateTime(x.FinDate.Year, x.FinDate.Month, 1);
                }
                else
                {
                    labelFormat = "dd-MM";
                    return x.FinDate.Date;
                }
            })
            .Select(g => new
            {
                Date = g.Key,
                Income = g.Where(x => x.Type == "INCOME").Sum(x => x.Amount),
                Expense = g.Where(x => x.Type == "EXPENSE").Sum(x => x.Amount),
                Profit = g.Where(x => x.Type == "INCOME").Sum(x => x.Amount) -
                         g.Where(x => x.Type == "EXPENSE").Sum(x => x.Amount)
            })
            .ToList();

            IncomeValues = new ChartValues<decimal>(groupedData.Select(x => x.Income));
            ExpenseValues = new ChartValues<decimal>(groupedData.Select(x => x.Expense));
            ProfitValues = new ChartValues<decimal>(groupedData.Select(x => x.Profit));

            foreach (var profit in groupedData)
            {
                var existedProfit = DataProvider.Instance.DB.FinancialHistories
                    .FirstOrDefault(x => x.FinDate.Date == profit.Date && x.Type == "PROFIT");

                if (existedProfit != null)
                {
                    existedProfit.Amount = profit.Profit;
                }
                else
                {
                    var entryProfit = new FinancialHistory
                    {
                        Amount = profit.Profit,
                        FinDate = profit.Date,
                        Type = "PROFIT"
                    };
                    DataProvider.Instance.DB.FinancialHistories.Add(entryProfit);
                }
            }

            DataProvider.Instance.DB.SaveChanges();

            var availableDates = groupedData.Select(x => x.Date).ToList();
            UpdateLabels(availableDates, labelFormat);
        }


        public void LoadFinancialData()
        {
            var today = DateTime.Now.Date;
            var yesterday = DateTime.Now.Date.AddDays(-1);

            // truy dữ liệu cho doanh thu hôm nay và hôm qua, số hóa đơn hôm nay và hôm qua
            IncomeToday = DataProvider.Instance.DB.FinancialHistories.Where(x => x.FinDate.Date == today && x.Type == "INCOME").Sum(x => x.Amount);            
            IncomeYesterday = DataProvider.Instance.DB.FinancialHistories.Where(x => x.FinDate.Date == yesterday && x.Type == "INCOME").Sum(x => x.Amount);
            BillToday = DataProvider.Instance.DB.Receipts.Where(x => x.RecTime.Date == today).Count();
            BillYesterday = DataProvider.Instance.DB.Receipts.Where(x => x.RecTime.Date == yesterday).Count();

            //Stockin stockin = DataProvider.Instance.DB.Stockins.OrderByDescending(x => x.StoDate).FirstOrDefault();
            //var newFinancial = new FinancialHistory { ReferenceType = "STOCKIN", ReferenceId = stockin.StoId,Description="Chi phí nhập kho" ,Amount = stockin.StoPrice, FinDate = stockin.StoDate, Type = "EXPENSE" };

            // truy dữ liệu cho nhân viên có hiệu suất làm việc cao nhất hôm nay và hôm qua
            // lấy số lượng booking và receipt của các nhân viên theo ngày
            var todayNhanViensBooking = DataProvider.Instance.DB.Bookings.Where(x => x.BkStime.Date == today && x.BkStatus == 0).GroupBy(x => x.EmpId).Select(g => new { EmpId = g.Key, BookingCount = g.Count() });
            var todayNhanViensReceipt = DataProvider.Instance.DB.Receipts.Where(x => x.RecTime.Date == today).GroupBy(x => x.EmpId).Select(g => new { EmpId = g.Key, ReceiptCount = g.Count() });
            var yesterdayNhanViensBooking = DataProvider.Instance.DB.Bookings.Where(x => x.BkStime.Date == yesterday && x.BkStatus == 0).GroupBy(x => x.EmpId).Select(g => new { EmpId = g.Key, BookingCount = g.Count() });
            var yesterdayNhanViensReceipt = DataProvider.Instance.DB.Receipts.Where(x => x.RecTime.Date == yesterday).GroupBy(x => x.EmpId).Select(g => new { EmpId = g.Key, ReceiptCount = g.Count() });

            // cộng lại số lượng booking và receipt của mỗi nhân viên
            //var todayNhanViensPer = todayNhanViensBooking.Join(todayNhanViensReceipt, x => x.EmpId, y => y.EmpId, (x, y) => new { x.EmpId, PerformanceCount = x.BookingCount + y.ReceiptCount });
            var todayNhanViensBookingData = todayNhanViensBooking.Select(x => new { x.EmpId, PerformanceCount = x.BookingCount });
            var todayNhanViensReceiptData = todayNhanViensReceipt.Select(x => new { x.EmpId, PerformanceCount = x.ReceiptCount });
            var todayNhanViensPer = todayNhanViensBookingData
                            .Union(todayNhanViensReceiptData)
                            .GroupBy(x => x.EmpId)  
                            .Select(g => new
                            {
                                EmpId = g.Key,
                                PerformanceCount = g.Sum(x => x.PerformanceCount)  
                            });
            var yesterdayNhanViensBookingData = yesterdayNhanViensBooking.Select(x => new { x.EmpId, PerformanceCount = x.BookingCount });
            var yesterdayNhanViensReceiptData = yesterdayNhanViensReceipt.Select(x => new { x.EmpId, PerformanceCount = x.ReceiptCount });
            var yesterdayNhanViensPer = yesterdayNhanViensBookingData   
                            .Union(yesterdayNhanViensReceiptData)
                            .GroupBy(x => x.EmpId)
                            .Select(g => new
                            {
                                EmpId = g.Key,
                                PerformanceCount = g.Sum(x => x.PerformanceCount)
                            });


            // tìm id của nhân viên có hiệu suất cao nhất
            int? todayPerEmpID = todayNhanViensPer.Count() > 0 ? todayNhanViensPer.OrderByDescending(x => x.PerformanceCount).FirstOrDefault().EmpId : 0;
            int? yesterdayPerEmpID = yesterdayNhanViensPer.Count() > 0 ? yesterdayNhanViensPer.OrderByDescending(x => x.PerformanceCount).FirstOrDefault().EmpId : 0;
            // lấy hiệu suất của nhân viên có hiệu suất cao nhất
            NhanVienTodayPer = todayPerEmpID != 0 ? todayNhanViensPer.Where(x => x.EmpId == todayPerEmpID).Select(x => x.PerformanceCount).FirstOrDefault() : 0;
            NhanVienYesterdayPer = yesterdayPerEmpID != 0 ? yesterdayNhanViensPer.Where(x => x.EmpId == yesterdayPerEmpID).Select(x => x.PerformanceCount).FirstOrDefault() : 0;
            // lấy tên của nhân viên có hiệu suất cao nhất
            NhanVienToday = todayPerEmpID != 0 ? DataProvider.Instance.DB.Employees.Where(x => x.EmpId == todayPerEmpID).Select(x => x.EmpName).FirstOrDefault() : "Không có";
            NhanVienYesterday = yesterdayPerEmpID != 0 ? DataProvider.Instance.DB.Employees.Where(x => x.EmpId == yesterdayPerEmpID).Select(x => x.EmpName).FirstOrDefault() : "Không có";
        }

        public ICommand RefreshChartDataCommand { get; set; }
        // bill management
        //--Order
        private ObservableCollection<OrderViewModel> _Orderuc;

        public ObservableCollection<OrderViewModel> Orderuc
        {
            get { return _Orderuc; }
            set
            {
                _Orderuc = value;
                OnPropertyChanged();
            }
        }

        private OrderViewModel _selectedFoodItemBill;

        public OrderViewModel SelectedFoodItemBill
        {
            get => _selectedFoodItemBill;
            set
            {
                _selectedFoodItemBill = value;
                OnPropertyChanged();
                // Khi thay đổi SelectedFoodItem, các nút tự động cập nhật trạng thái
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool areYouSure()
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thanh toán hóa đơn?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public ICommand EditOrderCommand { get; set; }

        public ICommand PayBillCommand { get; set; }

        //--
        public MainViewModel() {
            CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false));
            EmployeeList = new ObservableCollection<Employee>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false));
            StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
            IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients);
            ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems);
            LoadFinancialData();
            LoadChartData();
            InfoIncomeVMs =
            [
                new InfoIncomeViewModel("Images/money.png") {
                    TextToday = string.Format("Doanh thu hôm nay: {0:0,0} VNĐ", IncomeToday),
                    TextYesterday = string.Format("Doanh thu hôm qua: {0:0,0} VNĐ", IncomeYesterday),
                    ValueToday = IncomeToday,
                    ValueYesterday = IncomeYesterday,
                    MaxValue = (double)Math.Max(IncomeToday, IncomeYesterday),
                },
                new InfoIncomeViewModel("Images/receipt.png") {
                    TextToday = $"Số hóa đơn hôm nay: {BillToday} hóa đơn",
                    TextYesterday = $"Số hóa đơn hôm qua: {BillYesterday} hóa đơn",
                    ValueToday = BillToday,
                    ValueYesterday = BillYesterday,
                    MaxValue = Math.Max(BillToday, BillYesterday),
                },
                new InfoIncomeViewModel("Images/res_worker.png") {
                    TextToday = $"Nhân viên của hôm nay: {NhanVienToday}",
                    TextYesterday = $"Nhân viên của hôm qua: {NhanVienYesterday}",
                    ValueToday = NhanVienTodayPer,
                    ValueYesterday = NhanVienYesterdayPer,
                },
            ];
            LoadAllFOODInstock();
            LoadAllBookingInformation();
            LoadAllTableInformation();
            LoadOrderUC();
            

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
                            var account = DataProvider.Instance.DB.Accounts.Where(x => x.AccUsername == loginVM.Username).FirstOrDefault();
                            var accountRole = DataProvider.Instance.DB.AccountRoles.Where(x => x.RoleId == account.RoleId).FirstOrDefault();
                            if (accountRole.RoleName == "ADMIN")
                            {
                                // ko là đặc quyền của admin
                                IsAdmin = true;
                            }
                            loginVM.isLogin = false;
                            p.Show();
                            usernameForProfileWindow = loginVM.Username;
                        }
                        else p.Close();
                    }
                }
            );
            ProfileManagementCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
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

            // Add a new stock in 
            AddStockinCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddStockInWindow stockInManagementWindow = new AddStockInWindow();
                var stockInVM = stockInManagementWindow.DataContext as StockInManagementViewModel;
                if (stockInVM != null)
                {
                    stockInVM.IngreCheckConfiguration = false;
                    stockInVM.ItemCheckConfiguration = false;
                    stockInVM.stockInDetailsManagementID = 0;

                    stockInVM.StockInID = (DataProvider.Instance.DB.Stockins.Count() + 1).ToString();
                    StockInDetailsWindow stockInDetailsWindow = new StockInDetailsWindow();

                    var stockInDetailsVM = stockInDetailsWindow.DataContext as StockInManagementViewModel;
                    int existedStockInNumber = DataProvider.Instance.DB.Stockins.Count();
                    stockInManagementWindow.ShowDialog();
                    if (stockInVM.isConfirmed && stockInDetailsVM != null)
                    {
                        // Add new stockin into data grid row
                        try
                        {
                            DataProvider.Instance.DB.Stockins.Add(stockInVM.NewStockIn);
                            DataProvider.Instance.DB.SaveChanges();
                            StockinList.Add(stockInVM.NewStockIn);

                            StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);


                            //stockInDetailsVM.StockInID = (existedStockInNumber + 1).ToString();

                            stockInDetailsVM.StockInDetailsIngresList = new ObservableCollection<StockinDetailsIngre>(
                                (from stkInDetailsIngre in DataProvider.Instance.DB.StockinDetailsIngres
                                 join ingre in DataProvider.Instance.DB.Ingredients
                                 on stkInDetailsIngre.IngreId equals ingre.IngreId
                                 select new StockinDetailsIngre
                                 {
                                     Cprice = stkInDetailsIngre.Cprice,
                                     IngreId = stkInDetailsIngre.IngreId,
                                     QuantityKg = stkInDetailsIngre.QuantityKg,
                                     StoId = stkInDetailsIngre.StoId,
                                     Sto = stkInDetailsIngre.Sto,
                                     TotalCprice = stkInDetailsIngre.TotalCprice,
                                     Ingre = ingre // Gán dữ liệu từ bảng Ingredients
                                 }).Where(x => x.Sto.StoCode == stockInVM.NewStockIn.StoCode));

                            stockInDetailsVM.StockInDetailsDrinkOtherList = new ObservableCollection<StockinDetailsDrinkOther>(
                                (from stkInDetailsDrinkOther in DataProvider.Instance.DB.StockinDetailsDrinkOthers
                                 join item in DataProvider.Instance.DB.MenuItems
                                 on stkInDetailsDrinkOther.ItemId equals item.ItemId
                                 select new StockinDetailsDrinkOther
                                 {
                                     Cprice = stkInDetailsDrinkOther.Cprice,
                                     ItemId = stkInDetailsDrinkOther.ItemId,
                                     QuantityUnits = stkInDetailsDrinkOther.QuantityUnits,
                                     StoId = stkInDetailsDrinkOther.StoId,
                                     Sto = stkInDetailsDrinkOther.Sto,
                                     Item = item // Gán dữ liệu từ bảng MenuItems    
                                 }).Where(x => x.Sto.StoCode == stockInVM.NewStockIn.StoCode));

                            stockInDetailsVM.StockInCode = stockInVM.NewStockIn.StoCode;
                            stockInDetailsVM.StockInID = stockInVM.NewStockIn.StoId.ToString();

                            stockInDetailsVM.IngredientNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Ingredients.Select(x => x.IngreName));
                            stockInDetailsVM.MenuItemsNameList = new ObservableCollection<string>(DataProvider.Instance.DB.MenuItems.Where(x => x.ItemType != "FOOD").Select(x => x.ItemName));
                            stockInDetailsVM.SelectedStockinDetailsName = null;

                            stockInDetailsVM.UpdateStockInType();
                            stockInDetailsWindow.ShowDialog();
                            stockInDetailsVM.StockInDate = DateTime.Now;

                            stockInDetailsVM.UpdateExpenseFinancialHistory();

                        }
                        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                        {
                            stockInVM.NewStockIn = null;
                            MessageBox.Show("Đã tồn tại mã nguyên liệu hoặc mã mặt hàng, vui lòng thử lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }
                }
                StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
                ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
                LoadAllFOODInstock();
            });

            // Delete a stockin
            DelStockinCommand = new RelayCommand<object>((p) => SelectedStockin != null, (p) =>
            {
                var stockIn = DataProvider.Instance.DB.Stockins.Where(x => x.StoId == SelectedStockin.StoId).FirstOrDefault();

                var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?\n" +
                                                   "Nhấn 'Yes' để xóa số lượng có trong tồn kho.\n" +
                                                   "Nhấn 'No' để giữ nguyên số lượng trong tồn kho.", "Thông báo", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (stockIn != null)
                {
                    if (DialogResult == MessageBoxResult.No)
                    {
                        DeleteFinancialHistory();
                        //foreach (var stk in )
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
                    else if (DialogResult == MessageBoxResult.Yes)
                    {
                        StockInDetailsWindow stkDetails = new StockInDetailsWindow();
                        var stkVM = stkDetails.DataContext as StockInManagementViewModel;
                        if (stkVM != null)
                        {
                            var ingreStkinDetailsList = DataProvider.Instance.DB.StockinDetailsIngres.Where(x => x.StoId == stockIn.StoId).ToList();
                            var itemStkinDetailsList = DataProvider.Instance.DB.StockinDetailsDrinkOthers.Where(x => x.StoId == stockIn.StoId).ToList();
                            stkVM.SelectedIdxStockin = 0;
                            foreach (var selectedIngreStkIn in ingreStkinDetailsList)
                            {
                                stkVM.SelectedStockinDetailsIngre = selectedIngreStkIn;
                                stkVM.UpdateInStockAfterDelete();
                            }
                            stkVM.SelectedIdxStockin = 1;
                            foreach (var selectedItemStkIn in itemStkinDetailsList)
                            {
                                stkVM.SelectedStockinDetailsDrinkOther = selectedItemStkIn;
                                stkVM.UpdateInStockAfterDelete();
                            }
                        }
                        DeleteFinancialHistory();
                        //foreach (var stk in )
                        DataProvider.Instance.DB.Stockins.Remove(stockIn);
                        DataProvider.Instance.DB.SaveChanges();

                        StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
                        IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients);
                        ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems);
                        LoadAllFOODInstock();
                        //int i = 0;
                        //foreach (Stockin stkIn in StockinList)
                        //{
                        //    stkIn.StoCode = $"ST{++i:D6}";
                        //}

                        //DataProvider.Instance.DB.SaveChanges();

                        //StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
                    }
                }
            });

            // Enter an existed stockin to edit stockin details
            EnterStockinDetailsCommand = new RelayCommand<object>((p) => SelectedStockin != null, (p) =>
            {
                StockInDetailsWindow stockInDetailsWindow = new StockInDetailsWindow();
                var stockInDetailsVM = stockInDetailsWindow.DataContext as StockInManagementViewModel;
                if (stockInDetailsVM != null)
                {
                    stockInDetailsVM.stockInDetailsManagementID = 1;
                    stockInDetailsVM.IngreCheckConfiguration = false;
                    stockInDetailsVM.ItemCheckConfiguration = false;
                    stockInDetailsVM.StockInCode = SelectedStockin.StoCode;
                    stockInDetailsVM.StockInID = SelectedStockin.StoId.ToString();
                    //stockInDetailsVM.TemporaryStockin = SelectedStockin;
                    //stockInDetailsVM.changeDateID = 0;


                    stockInDetailsVM.StockInDetailsIngresList = new ObservableCollection<StockinDetailsIngre>(
                        (from stkInDetailsIngre in DataProvider.Instance.DB.StockinDetailsIngres
                         join ingre in DataProvider.Instance.DB.Ingredients
                         on stkInDetailsIngre.IngreId equals ingre.IngreId
                         select new StockinDetailsIngre
                         {
                             Cprice = stkInDetailsIngre.Cprice,
                             IngreId = stkInDetailsIngre.IngreId,
                             QuantityKg = stkInDetailsIngre.QuantityKg,
                             StoId = stkInDetailsIngre.StoId,
                             Sto = stkInDetailsIngre.Sto,
                             TotalCprice = stkInDetailsIngre.TotalCprice,
                             Ingre = ingre // Gán dữ liệu từ bảng Ingredients
                         }).Where(x => x.Sto.StoCode == SelectedStockin.StoCode));

                    stockInDetailsVM.StockInDetailsDrinkOtherList = new ObservableCollection<StockinDetailsDrinkOther>(
                        (from stkInDetailsDrinkOther in DataProvider.Instance.DB.StockinDetailsDrinkOthers
                         join item in DataProvider.Instance.DB.MenuItems
                         on stkInDetailsDrinkOther.ItemId equals item.ItemId
                         select new StockinDetailsDrinkOther
                         {
                             Cprice = stkInDetailsDrinkOther.Cprice,
                             ItemId = stkInDetailsDrinkOther.ItemId,
                             QuantityUnits = stkInDetailsDrinkOther.QuantityUnits,
                             StoId = stkInDetailsDrinkOther.StoId,
                             Sto = stkInDetailsDrinkOther.Sto,
                             TotalCprice = stkInDetailsDrinkOther.TotalCprice,
                             Item = item // Gán dữ liệu từ bảng MenuItems    
                         }).Where(x => x.Sto.StoCode == SelectedStockin.StoCode));

                    stockInDetailsVM.IngredientNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Ingredients.Select(x => x.IngreName));
                    stockInDetailsVM.MenuItemsNameList = new ObservableCollection<string>(DataProvider.Instance.DB.MenuItems.Where(x => x.ItemType != "FOOD").Select(x => x.ItemName));

                    stockInDetailsVM.StockInDate = SelectedStockin.StoDate;
                    stockInDetailsVM.SelectedStockinDetailsName = null;

                    stockInDetailsVM.UpdateStockInType();
                    stockInDetailsWindow.ShowDialog();

                    stockInDetailsVM.UpdateExpenseFinancialHistory();
                }
                //FinancialHistory = new ObservableCollection<FinancialHistory>(DataProvider.Instance.DB.FinancialHistories);
                StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);
                ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
                LoadAllFOODInstock();
            });


            // Ingredients & Items Management
            AddIngreCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                AddIngreWindow addIngreWindow = new AddIngreWindow();
                var ingreVM = addIngreWindow.DataContext as IngredientsManagementViewModel;
                if (ingreVM != null)
                {
                    ingreVM = new IngredientsManagementViewModel();
                    ingreVM.AddButtonIngredient = true;
                    addIngreWindow.DataContext = ingreVM;
                }
                addIngreWindow.ShowDialog();
                IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients); // reload database
            });
            EditIngreCommand = new RelayCommand<Window>((p) => SelectedIngre != null, (p) =>
            {
                AddIngreWindow addIngreWindow = new AddIngreWindow();
                var ingreVM = addIngreWindow.DataContext as IngredientsManagementViewModel;
                //var currIngre = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == SelectedIngre.IngreId).FirstOrDefault();
                if (ingreVM != null /*&& currIngre != null*/)
                {
                    ingreVM.IngredientID = SelectedIngre.IngreId;
                    ingreVM.LoadIngredientInformation();
                    addIngreWindow.DataContext = ingreVM;
                    addIngreWindow.ShowDialog();
                    IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients); // reload database
                }
            });
            DelIngreCommand = new RelayCommand<object>((p) => SelectedIngre != null, (p) =>
            {
                var ingre = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == SelectedIngre.IngreId).FirstOrDefault();
                var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ingre != null && DialogResult == MessageBoxResult.Yes)
                {
                    DataProvider.Instance.DB.Ingredients.Remove(ingre);
                    DataProvider.Instance.DB.SaveChanges();
                    IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients);
                    int i = 0;
                    foreach (Ingredient ingredient in IngredientsList)
                    {
                        ingredient.IngreCode = $"NL{++i:D3}";
                    }
                    DataProvider.Instance.DB.SaveChanges();
                    IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients);
                }
            });
            // menu item management
            AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddItemWindow addItemWindow = new AddItemWindow();
                //var itemVM = addItemWindow.DataContext as MenuItemsManagement;
                var itemVM = new MenuItemsManagement();
                //var itemVM = new MenuItemsManagement();
                if (itemVM != null)
                {
                    itemVM.SelectedIdxType = 0;
                    itemVM.IsNotEditing = true;
                    itemVM.LoadBlankRecipeInformation();
                    itemVM.LoadBlankDrinkInformation();
                    itemVM.LoadBlankOtherInformation();
                    itemVM.ItemID = -1;
                    addItemWindow.DataContext = itemVM;
                    addItemWindow.ShowDialog();
                }
                ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
            });
            EditItemCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                
                AddItemWindow addItemWindow = new AddItemWindow();
                //var itemVM = addItemWindow.DataContext as MenuItemsManagement;
                var itemVM = new MenuItemsManagement();
                if (itemVM != null)
                {
                    if (SelectedItem.ItemType == "FOOD")
                    {
                        itemVM.SelectedIdxType = 0;
                        itemVM.LoadRecipeInformation(selectedItem.ItemId);
                    }
                    else if (SelectedItem.ItemType == "DRINK")
                    {
                        itemVM.SelectedIdxType = 1;
                        itemVM.LoadDrinkInformation(selectedItem.ItemId);
                    }
                    else
                    {
                        itemVM.SelectedIdxType = 2;
                        itemVM.LoadOtherInformation(selectedItem.ItemId);
                    }
                    itemVM.IsNotEditing = false;
                    itemVM.ItemID = SelectedItem.ItemId;
                    addItemWindow.DataContext = itemVM;
                    addItemWindow.ShowDialog();
                }
                ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
                LoadAllFOODInstock();
            });
            DelItemCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) => 
            {
                var item = DataProvider.Instance.DB.MenuItems.Where(x => x.ItemId == SelectedItem.ItemId).FirstOrDefault();
                var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (item != null && DialogResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataProvider.Instance.DB.MenuItems.Remove(item);
                        DataProvider.Instance.DB.SaveChanges();
                        ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems);
                        int i = 0;
                        foreach (MenuItem menuItem in ItemsList)
                        {
                            menuItem.ItemCode = $"MN{++i:D2}";
                        }
                        DataProvider.Instance.DB.SaveChanges();
                        LoadAllFOODInstock();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                        MessageBox.Show("Xóa sản phẩm không thành công!\n Thay vào đó vui lòng thêm sản phẩm khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    
                    
                    ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems);
                    
                }

            });
            // Table Management
            AddTableCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                byte existedTableNumber = (byte)DataProvider.Instance.DB.DiningTables.Count();
                
                //string query = $"DBCC CHECKIDENT ('DININGTABLE', RESEED, {existedTableNumber + 1})";
                //DataProvider.Instance.DB.Database.ExecuteSqlRaw(query);
                DiningTable newTable = new DiningTable { TabNum = ++existedTableNumber, TabStatus = true, Isdeleted = false };
                DataProvider.Instance.DB.DiningTables.Add(newTable);
                DataProvider.Instance.DB.SaveChanges();
                TableViewList.Add(new TableViewModel(newTable));
                LoadAllTableInformation();
                
                
            });
            EditTableCommand = new RelayCommand<TableViewModel>((p) => { return true; }, (p) =>
            {
                bool AllEmpty = true;
                foreach (TableViewModel table in TableViewList)
                {
                    if (table.BoolTabStatus == false)
                    {
                        AllEmpty = false;
                        break;
                    }
                    
                }
                AddTableWindow addTableWindow = new AddTableWindow();
                var addTableVM = addTableWindow.DataContext as AddTableViewModel;
                
                string numberPart = Regex.Match(p.TabNumber, @"\d+").Value;
                byte tableFromString = byte.Parse(numberPart.ToString());
                DiningTable SelectedTable = DataProvider.Instance.DB.DiningTables.Where(x => x.TabNum == tableFromString).FirstOrDefault();
                if (addTableVM != null)
                {
                    if (AllEmpty == false)
                    {
                        addTableVM.AllIsEmpty = false;
                    }
                    else addTableVM.AllIsEmpty = true;
                    addTableVM.LoadTableInformation(SelectedTable);
                    addTableWindow.DataContext = addTableVM;
                    addTableWindow.ShowDialog();
                }
                LoadAllTableInformation();
            });
            
            /// Booking Management
            AddBookingCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                BookingWindow bookingWindow = new BookingWindow();
                var bookingVM = bookingWindow.DataContext as BookingViewModel;
                if (bookingVM != null)
                {
                    bookingVM.ReloadFillIn();
                    SelectedTable = null;
                    bookingVM.isVisible = Visibility.Hidden;
                    bookingVM.isClicked = false;
                    bookingVM.isConfigurationClicked = true;
                    bookingVM.CustomerNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false).Select(x => x.CusName));
                    bookingVM.EmpNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false).Select(x => x.EmpName));
                    bookingVM.BookingCode = $"BK{BookingViewList.Count() + 1:D3}";
                    bookingVM.bookingManagementID = 0;
                    isEnabled = true;
                    //LoadAllTableInformation();
                    if (TableNumbers.Count() == 0)
                    {
                        MessageBox.Show("Không còn bàn trống", "Lỗi đặt bàn", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    bookingWindow.ShowDialog();
                    // add new booking after being confirmed
                    if (bookingVM.isConfirmed)
                    {
                        DataProvider.Instance.DB.Bookings.Add(bookingVM.NewBooking);
                        BookingList.Add(bookingVM.NewBooking);
                        DataProvider.Instance.DB.SaveChanges();
                        BookingViewList.Add(new BookingConfigurationViewModel(bookingVM.NewBooking));
                        bookingVM.isConfirmed = false;
                    }
                }
                LoadAllTableInformation();
                LoadAllBookingInformation();
                LoadOrderUC();
                CustomerList = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false));
            });
            BookingInfo = new RelayCommand<BookingConfigurationViewModel>((p) => { return true; }, (p) =>
            {
                BookingWindow bookingWindow = new BookingWindow();
                var bookingVM = bookingWindow.DataContext as BookingViewModel;
                Booking SelectedBooking = DataProvider.Instance.DB.Bookings.Where(x => x.BkCode == p.NewBookingCode && x.BkStatus == 1).FirstOrDefault();

                if (bookingVM != null && SelectedBooking != null)
                {
                    bookingVM.isClicked = true;
                    bookingVM.isConfigurationClicked = false;
                    bookingVM.isVisible = Visibility.Visible;
                    bookingVM.CustomerNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false).Select(x => x.CusName));
                    bookingVM.EmpNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Employees.Where(x => x.Isdeleted == false).Select(x => x.EmpName));
                    bookingVM.LoadBookingInformation(SelectedBooking);
                    bookingVM.bookingManagementID = 2;
                    bookingWindow.ShowDialog();
                }
                LoadOrderUC();
                LoadAllTableInformation();
                LoadAllBookingInformation();
            });
            RefreshChartDataCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadFinancialData(); LoadChartData(); OnPropertyChanged(nameof(InfoIncomeVMs));
                InfoIncomeVMs = new ObservableCollection<InfoIncomeViewModel>
                {
                    new InfoIncomeViewModel("Images/money.png")
                    {
                        TextToday = string.Format("Doanh thu hôm nay: {0:0,0} VNĐ", IncomeToday),
                        TextYesterday = string.Format("Doanh thu hôm qua: {0:0,0} VNĐ", IncomeYesterday),
                        ValueToday = IncomeToday,
                        ValueYesterday = IncomeYesterday,
                        MaxValue = (double)Math.Max(IncomeToday, IncomeYesterday),
                    },
                    new InfoIncomeViewModel("Images/receipt.png")
                    {
                        TextToday = $"Số hóa đơn hôm nay: {BillToday} hóa đơn",
                        TextYesterday = $"Số hóa đơn hôm qua: {BillYesterday} hóa đơn",
                        ValueToday = BillToday,
                        ValueYesterday = BillYesterday,
                        MaxValue = Math.Max(BillToday, BillYesterday),
                    },
                    new InfoIncomeViewModel("Images/res_worker.png")
                    {
                        TextToday = $"Nhân viên của hôm nay: {NhanVienToday}",
                        TextYesterday = $"Nhân viên của hôm qua: {NhanVienYesterday}",
                        ValueToday = NhanVienTodayPer,
                        ValueYesterday = NhanVienYesterdayPer,
                    }
                };
            });

            // order management
            AddOrderCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FoodLayoutWindow foodLayoutWindow = new FoodLayoutWindow();
                FoodLayoutViewModel foodLayoutViewModel = new FoodLayoutViewModel();
                foodLayoutViewModel.Bills.Clear();
                foodLayoutViewModel.SelectedEmpName = null;
                foodLayoutViewModel.SelectedTabNum = null;
                foodLayoutWindow.DataContext = foodLayoutViewModel;
                foodLayoutWindow.ShowDialog();
                LoadOrderUC();
                LoadAllTableInformation();
                ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
                LoadAllFOODInstock();
                IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients); // reload database

            });

            EditOrderCommand = new RelayCommand<OrderViewModel>((p) => { return true; }, (p) =>
            {
                FoodLayoutWindow selectedFoodLayout = new FoodLayoutWindow();
                var foodLayoutVM = selectedFoodLayout.DataContext as FoodLayoutViewModel;
                Receipt selectedReceipt = DataProvider.Instance.DB.Receipts.Where(x => x.RecId == p.RecId).FirstOrDefault();
                if (foodLayoutVM != null)
                {
                    foodLayoutVM.LoadOrderInformation(selectedReceipt);
                    //foodLayoutVM.OrderManagementID = 1;
                    foodLayoutVM.IsEditing = 1;
                    foodLayoutVM.InputTabNum = DataProvider.Instance.DB.DiningTables.Where(tab => tab.TabId == selectedReceipt.TabId).FirstOrDefault().TabNum;
                    foodLayoutVM.InputReceipt = selectedReceipt;
                    foodLayoutVM.LoadMenuItems();
                    selectedFoodLayout.DataContext = foodLayoutVM;
                    selectedFoodLayout.ShowDialog();
                }
                LoadOrderUC();
                LoadAllTableInformation();
                LoadAllFOODInstock();
                ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
                IngredientsList = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients); // reload database
            });

            PayBillCommand = new RelayCommand<OrderViewModel>(
            (p) => { return true; }, // CanExecute
            (p) =>
            {
                if (areYouSure() == true)
                {
                    SelectedFoodItemBill = p;
                    var receipt = DataProvider.Instance.DB.Receipts.FirstOrDefault(r => r.RecId == SelectedFoodItemBill.RecId);
                    if (receipt != null)
                    {
                        receipt.RecCode = string.Empty;
                        receipt.Isdeleted = true; // Cập nhật trạng thái hóaddon
                        FinancialHistory financialHistory = new FinancialHistory
                        {
                            FinDate = DateTime.Now,
                            Amount = receipt.RecPay,
                            Type = "INCOME",
                            Description = $"Thanh toán hóa đơn số {receipt.RecId}",
                            ReferenceId = receipt.RecId,
                            ReferenceType = "RECEIPT"
                        };
                        DataProvider.Instance.DB.FinancialHistories.Add(financialHistory);
                        DiningTable table = DataProvider.Instance.DB.DiningTables.FirstOrDefault(t => t.TabId == receipt.TabId);
                        table.TabStatus = true; // Cập nhật trạng thái bàn

                        DataProvider.Instance.DB.SaveChanges();

                        // Tính lại số thứ tự cho danh sách
                        int index = 1;

                        foreach (var order in Orderuc)
                        {
                            order.OrderNumber = index++;
                        }
                        OnPropertyChanged(nameof(Orderuc));
                    }

                    // Tạo file PDF hóa đơn
                    try
                    {
                        var receiptDetails = DataProvider.Instance.DB.ReceiptDetails
                            .Where(rd => rd.RecId == SelectedFoodItemBill.RecId)
                            .ToList();

                        ExportOrderToPDF(
                            SelectedFoodItemBill.OrderNumber,
                            SelectedFoodItemBill.OrderBill,
                            SelectedFoodItemBill.OrderTimer,
                            receiptDetails
                        );
                        // Xóa hóa đơn khỏi danh sách hiển thị
                        //  MessageBox.Show("Hóa đơn đã được thanh toán và lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Đã xảy ra lỗi khi tạo hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                LoadAllTableInformation();
                LoadOrderUC();
            });
            PrintFinancialHistoryReport = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ObservableCollection<FinancialHistory> financialHistories = new ObservableCollection<FinancialHistory>(
                        DataProvider.Instance.DB.FinancialHistories.Where(x => x.FinDate >= StartDate && x.FinDate <= EndDate && x.Type != "PROFIT").ToList()
                    );

                decimal profit = 0;
                foreach (var financialHistory in financialHistories)
                {
                    if (financialHistory.Type == "INCOME")
                    {
                        profit += financialHistory.Amount;
                    }
                    if (financialHistory.Type == "EXPENSE")
                    {
                        profit -= financialHistory.Amount;
                    }
                }


                ExportFinancialReportToPDF(financialHistories, profit);
                //for (DateTime dt = StartDate; dt <= EndDate; dt.AddDays(1))
                //{

                //}
            });
        }
        public void DeleteFinancialHistory()
        {
            var refFinancialHitory = DataProvider.Instance.DB.FinancialHistories.Where(x => x.ReferenceId == SelectedStockin.StoId).FirstOrDefault();
            if (refFinancialHitory != null)
            {
                DataProvider.Instance.DB.FinancialHistories.Remove(refFinancialHitory);
                DataProvider.Instance.DB.SaveChanges();
            }
        }
        public void LoadAllTableInformation()
        {
            TableList = new ObservableCollection<DiningTable>(DataProvider.Instance.DB.DiningTables);
            TableViewList = new ObservableCollection<TableViewModel>();
            byte i = 1;
            byte j = 1;
            foreach (DiningTable table in TableList)
            {
                if (table.Isdeleted == false)
                {
                    table.TabNum = j++;
                    TableViewList.Add(new TableViewModel(i++, table.TabStatus));
                    DataProvider.Instance.DB.SaveChanges();
                }
            }
            TableNumbers = TableViewList.Where(x => x.BoolTabStatus == true).Select(x => x.TabNumber);
        }

        public void LoadAllFOODInstock()
        {
            ObservableCollection<MenuItem> allCurrentFOODItems = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems.Where(x => x.ItemType == "FOOD"));
            foreach (MenuItem item in allCurrentFOODItems)
            {
                double minItemInstock = double.MaxValue;
                var recipes = DataProvider.Instance.DB.Recipes.Where(x => x.ItemId == item.ItemId).ToList();
                foreach (var recipe in recipes)
                {
                    double currentIngreInstock = DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreId == recipe.IngreId).InstockKg;
                    double itemInstock;
                    itemInstock = Math.Floor(currentIngreInstock / recipe.IngreQuantityKg);
                    if (itemInstock < minItemInstock) minItemInstock = itemInstock;
                }
                if (recipes.Count == 0) minItemInstock = 0;
                item.Instock = minItemInstock;
            }
            DataProvider.Instance.DB.SaveChanges();
        }
        public void LoadAllBookingInformation()
        {
            BookingList = new ObservableCollection<Booking>(DataProvider.Instance.DB.Bookings);
            BookingViewList = new ObservableCollection<BookingConfigurationViewModel>();
            //BookingWindow bookingWindow = new BookingWindow();
            //var bookingVM = bookingWindow.DataContext as BookingViewModel;
            var currTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            foreach (Booking booking in BookingList)
            {
                if (booking.Isdeleted == false)
                {
                    if (currTime >= booking.BkOtime.AddMinutes(10))
                    {
                        booking.Isdeleted = true;
                        if (booking.Tab != null)
                            booking.Tab.TabStatus = true;
                        DataProvider.Instance.DB.SaveChanges();
                    }
                    else if (booking.BkStatus == 1)
                    {
                        BookingViewList.Add(new BookingConfigurationViewModel(booking));
                        DataProvider.Instance.DB.SaveChanges();
                    }
                }
            }
            ExistedBooking = $"Booking hiện có ({BookingViewList.Count()})";
            //BookingViewList.Clear();
        }

        public void ExportOrderToPDF(int ordernum, decimal bill, DateTime recTime, List<ReceiptDetail> receiptDetails)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DanhSachHoaDon_PhanMemQuanLyNhaHang");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            int fileIndex = 1;
            string filePath;
            do
            {
                filePath = Path.Combine(folderPath, $"HoaDon_{fileIndex}.pdf");
                fileIndex++;
            } while (File.Exists(filePath));

            try
            {
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Hóa đơn thanh toán";
                PdfPage page = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 24, XFontStyle.Bold);
                XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                XFont regularFont = new XFont("Arial", 10);

                gfx.DrawString("Hóa đơn thanh toán", titleFont, XBrushes.Black, new XPoint(page.Width / 2, 50), XStringFormats.Center);

                XImage logo = XImage.FromFile("C:\\Users\\Admin\\Downloads\\KTPM\\3rd Semester\\VP\\BCCK\\RestaurantManager-master\\UI\\Views\\Images\\logo.png");
                gfx.DrawImage(logo, 20, 20, 50, 50);

                gfx.DrawString($"Mã hóa đơn: {ordernum}", regularFont, XBrushes.Black, new XPoint(50, 100));
                gfx.DrawString($"Ngày giờ: {recTime.ToString("dd/MM/yyyy HH:mm:ss")}", regularFont, XBrushes.Black, new XPoint(50, 120));

                int yPosition = 150;
                gfx.DrawString("STT", headerFont, XBrushes.Black, new XPoint(50, yPosition));
                gfx.DrawString("Tên món", headerFont, XBrushes.Black, new XPoint(100, yPosition));
                gfx.DrawString("SL", headerFont, XBrushes.Black, new XPoint(250, yPosition));
                gfx.DrawString("Đơn giá", headerFont, XBrushes.Black, new XPoint(300, yPosition));
                gfx.DrawString("Thành tiền", headerFont, XBrushes.Black, new XPoint(400, yPosition));
                yPosition += 20;

                int index = 1;
                foreach (var detail in receiptDetails)
                {
                    string itemName = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemId == detail.ItemId)?.ItemName ?? "Không tìm thấy";
                    gfx.DrawString(index.ToString(), regularFont, XBrushes.Black, new XPoint(50, yPosition));
                    gfx.DrawString(itemName, regularFont, XBrushes.Black, new XPoint(100, yPosition));
                    gfx.DrawString(detail.Quantity.ToString(), regularFont, XBrushes.Black, new XPoint(250, yPosition));
                    gfx.DrawString(detail.Price.ToString("C0", new CultureInfo("vi-VN")), regularFont, XBrushes.Black, new XPoint(300, yPosition));
                    gfx.DrawString((detail.Price * detail.Quantity).ToString("C0", new CultureInfo("vi-VN")), regularFont, XBrushes.Black, new XPoint(400, yPosition));
                    yPosition += 20;

                    if (yPosition > page.Height - 50)
                    {
                        page = pdfDoc.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPosition = 50; 
                    }
                    index++;
                }

                yPosition += 20;
                gfx.DrawString($"Tổng tiền: {bill.ToString("C0", new CultureInfo("vi-VN"))}", titleFont, XBrushes.Black, new XPoint(page.Width / 2, yPosition), XStringFormats.Center);

                pdfDoc.Save(filePath);
                MessageBox.Show($"Hóa đơn đã được lưu thành công tại: {filePath}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Orderuc.Remove(SelectedFoodItemBill);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi lưu file: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportFinancialReportToPDF(ObservableCollection<FinancialHistory> financialHistories, decimal profit)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BaoCaoTaiChinh_PhanMemQuanLyNhaHang");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            int fileIndex = 1;
            string filePath;
            do
            {
                filePath = Path.Combine(folderPath, $"BaoCaoTaiChinh_{fileIndex}.pdf");
                fileIndex++;
            } while (File.Exists(filePath));

            try
            {
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Báo cáo lịch sử tài chính";
                PdfPage page = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 24, XFontStyle.Bold);
                XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                XFont regularFont = new XFont("Arial", 10);

                gfx.DrawString("Báo cáo lịch sử tài chính", titleFont, XBrushes.Black, new XPoint(page.Width / 2, 50), XStringFormats.Center);

                XImage logo = XImage.FromFile("C:\\Users\\Admin\\Downloads\\KTPM\\3rd Semester\\VP\\BCCK\\RestaurantManager-master\\UI\\Views\\Images\\logo.png");
                gfx.DrawImage(logo, 20, 20, 50, 50);

                gfx.DrawString($"Từ ngày: {StartDate.ToString("dd/MM/yyyy")} đến ngày {EndDate.ToString("dd/MM/yyyy")}", regularFont, XBrushes.Black, new XPoint(50, 100));

                int yPosition = 150;
                gfx.DrawString("STT", headerFont, XBrushes.Black, new XPoint(50, yPosition));
                gfx.DrawString("Thời gian", headerFont, XBrushes.Black, new XPoint(100, yPosition));
                gfx.DrawString("Mô tả", headerFont, XBrushes.Black, new XPoint(190, yPosition));
                gfx.DrawString("Phân loại", headerFont, XBrushes.Black, new XPoint(310, yPosition));
                gfx.DrawString("Thành tiền", headerFont, XBrushes.Black, new XPoint(400, yPosition));
                yPosition += 20;

                int index = 1;
                foreach (var finHis in financialHistories)
                {
                    gfx.DrawString(index.ToString(), regularFont, XBrushes.Black, new XPoint(55, yPosition));
                    gfx.DrawString(finHis.FinDate.ToString("dd/MM/yyyy"), regularFont, XBrushes.Black, new XPoint(105, yPosition));
                    gfx.DrawString(finHis.Description, regularFont, XBrushes.Black, new XPoint(185, yPosition));
                    gfx.DrawString(finHis.Type == "INCOME" ? "Doanh thu" : "Chi phí", regularFont, XBrushes.Black, new XPoint(315, yPosition));
                    gfx.DrawString($"{finHis.Amount:N2}đ", regularFont, XBrushes.Black, new XPoint(405, yPosition));
                    yPosition += 20;

                    if (yPosition > page.Height - 50)
                    {
                        page = pdfDoc.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPosition = 50; 
                    }
                    index++;
                }

                yPosition += 20;
                gfx.DrawString($"Lợi nhuận: {profit.ToString("C0", new CultureInfo("vi-VN"))}", titleFont, XBrushes.Black, new XPoint(page.Width / 2, yPosition), XStringFormats.Center);

                pdfDoc.Save(filePath);
                MessageBox.Show($"Báo cáo đã được lưu thành công tại: {filePath}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi lưu file: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void LoadOrderUC()
        {
            var items = DataProvider.Instance.DB.Receipts
                            .Where(receipt => receipt.Isdeleted == false) // Lọc hóa đơn chưa thanh toán
                            .OrderBy(receipt => receipt.RecId)  // Sắp xếp theo RecId
                            .ToList();
            //byte? tableNum = DataProvider.Instance.DB.DiningTables.Where(x => x.TabStatus == true).FirstOrDefault()?.TabNum;
            // Chuyển đổi danh sách hóa đơn thành ObservableCollection<OrderViewModel>

            Orderuc = new ObservableCollection<OrderViewModel>(

                items.Select((item, index) => new OrderViewModel
                {
                    RecId = item.RecId,
                    OrderBill = item.RecPay,
                    OrderTimer = item.RecTime,
                    OrderNumber = index + 1, // Số thứ tự bắt đầu từ 1
                    OrderEmployee = DataProvider.Instance.DB.Employees.Where(x => x.EmpId == item.EmpId).Select(x => x.EmpName).FirstOrDefault() ?? "",
                    OrderTable = DataProvider.Instance.DB.DiningTables.Where(tab => tab.TabId == item.TabId).FirstOrDefault()?.TabNum ?? 0
                })

            );
            OnPropertyChanged(nameof(Orderuc)); // Thông báo cập nhật giao diện
        }
    }
}
