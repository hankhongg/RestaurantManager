using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using UI.Views;
using XAct;



namespace RestaurantManager.ViewModels
{
    class BookingViewModel : BaseViewModel
    {
        private Booking _newBooking;
        public Booking NewBooking
        {
            get { return _newBooking; }
            set
            {
                _newBooking = value;
                OnPropertyChanged();
            }
        }

        private string bookingCode;

        public string BookingCode
        {
            get { return bookingCode; }
            set
            {
                bookingCode = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _customerNameList;

        public ObservableCollection<string> CustomerNameList
        {
            get { return _customerNameList; }
            set
            {
                _customerNameList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _empNameList;

        public ObservableCollection<string> EmpNameList
        {
            get { return _empNameList; }
            set
            {
                _empNameList = value;
                OnPropertyChanged();
            }
        }

        private string _cusPhoneNumber;

        public string CusPhoneNumber
        {
            get { return _cusPhoneNumber; }
            set
            {
                _cusPhoneNumber = value;
                OnPropertyChanged();
            }
        }
        private string _selectedEmpName;
        public string SelectedEmpName
        {
            get { return _selectedEmpName; }
            set
            {
                _selectedEmpName = value;
                OnPropertyChanged();
            }
        }
        private string _selectedCustomerName;
        public string SelectedCustomerName
        {
            get { return _selectedCustomerName; }
            set
            {
                _selectedCustomerName = value;
                if (SelectedCustomerName != null)
                {
                    CusPhoneNumber = DataProvider.Instance.DB.Customers.Where(x => x.CusName == SelectedCustomerName).Select(x => x.CusPhone).FirstOrDefault() ?? "";
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _selectedBookingDate;

        public DateTime SelectedBookingDate
        {
            get { return _selectedBookingDate; }
            set
            {
                _selectedBookingDate = value;
                OnPropertyChanged();
            }
        }
        private string _selectedHour;

        public string SelectedHour
        {
            get { return _selectedHour; }
            set
            {
                _selectedHour = value;
                OnPropertyChanged();
            }
        }
        private string _selectedMinute;

        public string SelectedMinute
        {
            get { return _selectedMinute; }
            set
            {
                _selectedMinute = value;
                OnPropertyChanged();
            }
        }
        private string _selectedTime;

        public string SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                _selectedTime = value;
                OnPropertyChanged();
            }
        }

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
        public int TableID { get; set; }
        private string _tableNumber;

        public string TableNumber
        {
            get { return _tableNumber; }
            set
            {
                _tableNumber = value;
                OnPropertyChanged();
            }
        }

        public int BookingNumber { get; set; }
        private Booking editBooking;

        public Booking EditBooking
        {
            get { return editBooking; }
            set
            {
                editBooking = value;
                OnPropertyChanged();
            }
        }
        private bool _isConfigurationClicked;
        public bool isConfigurationClicked
        {
            get { return _isConfigurationClicked; }
            set
            {
                _isConfigurationClicked = value;
                OnPropertyChanged();
            }
        }
        private bool _isClicked;
        public bool isClicked
        {
            get { return _isClicked; }
            set
            {
                _isClicked = value;
                OnPropertyChanged();
            }
        }
        private Visibility _isVisible;

        public Visibility isVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int newBookingNumber { get; set; }
        public int bookingManagementID { get; set; }
        public bool isConfirmed { get; set; }
        public ICommand CancelBookingInfo { get; set; }
        public ICommand ConfirmBookingInfo { get; set; }
        public ICommand AddNewCustomer { get; set; }
        public ICommand ConfigureBookingInfo { get; set; }  
        public ICommand ExitConfiguration { get; set; }


        public BookingViewModel()
        {
            SelectedMinute = "00";
            SelectedBookingDate = DateTime.Now;
            isConfirmed = false;
            isClicked = true;

            CancelBookingInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MainWindow mainWindow = new MainWindow();
                var mainVM = mainWindow.DataContext as MainViewModel;
                if (bookingManagementID == 1)
                {
                    if (mainVM != null)
                    {
                        RestoreStatus(mainVM.SelectedTable);
                        mainVM.LoadAllTableInformation();
                        mainVM.LoadAllBookingInformation();

                        mainVM.isEnabled = true;
                    }
                }
                else if (bookingManagementID == 2)
                {
                    var diaResult = MessageBox.Show("Bạn có muốn hủy đặt bàn không?", "Thông báo", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (diaResult == MessageBoxResult.Yes)
                    {
                        var booking = DataProvider.Instance.DB.Bookings.Where(x => x.BkCode == BookingCode && x.Isdeleted == false && x.BkStatus == 1).FirstOrDefault();
                        if (booking != null && mainVM != null)
                        {
                            if (booking.Tab != null)
                                booking.Tab.TabStatus = true;
                            booking.Isdeleted = true;
                            booking.BkCode = "NULL";
                            int i = 0;
                            foreach (Booking bk in mainVM.BookingList)
                            {
                                if (bk.Isdeleted == false && bk.BkStatus == 1)
                                {
                                    bk.BkCode = $"BK{++i:D3}";
                                }
                            }
                            DataProvider.Instance.DB.SaveChanges();
                            mainVM.LoadAllTableInformation();
                            mainVM.LoadAllBookingInformation();
                        }
                    }
                    isConfigurationClicked = false;
                    isClicked = true;
                }
                p.Close();
                mainVM.SelectedTable = null;
                ReloadFillIn();
            });
            ConfirmBookingInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MainWindow mainWindow = new MainWindow();
                var mainVM = mainWindow.DataContext as MainViewModel;
                if (SelectedHour == null || SelectedCustomerName == null || SelectedEmpName == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                    return;
                }
                var currTime = DateTime.Now;
                //var CurrTime = new DateTime(currTime.Year, currTime.Month, currTime.Day, currTime.Hour, currTime.Minute, 0);
                var selectedDateTime = new DateTime(SelectedBookingDate.Year, SelectedBookingDate.Month, SelectedBookingDate.Day, int.Parse(SelectedHour), int.Parse(SelectedMinute), 0); 
                if (selectedDateTime < currTime)
                {
                    MessageBox.Show("Thời gian đặt phải sau thời gian hiện tại", "Lỗi đặt bàn", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (int.Parse(SelectedHour) >= 24 || int.Parse(SelectedMinute) > 60)
                {
                    MessageBox.Show("Thời gian nhập không hợp lệ!", "Lỗi đặt bàn", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (bookingManagementID == 0)
                {
                    if (mainVM != null)
                    {
                        SelectedTable = mainVM.SelectedTable;
                        TableID = DataProvider.Instance.DB.DiningTables
                            .Where(x => x.TabNum == int.Parse(SelectedTable.Substring(4)))
                            .Select(x => x.TabId)
                            .FirstOrDefault();
                    }
                    AddNewBooking(p, newBookingNumber);
                    p.Close();
                }
                else if (bookingManagementID == 1)
                {
                    if (mainVM != null)
                    {
                        //SelectedTable = mainVM.SelectedTable;
                        Booking bk = DataProvider.Instance.DB.Bookings.Where(x => x.BkCode == BookingCode && x.Isdeleted == false && x.BkStatus == 1).FirstOrDefault();
                        if (bk != null)
                        {
                            bk = EditBookingInfo(bk);
                            DataProvider.Instance.DB.Bookings.Update(bk);
                        }
                        DataProvider.Instance.DB.SaveChanges();

                        RestoreStatus(mainVM.SelectedTable);
                        //mainVM.LoadAllTableInformation();
                        //mainVM.LoadAllBookingInformation();

                        mainVM.isEnabled = true;
                    }
                    isConfigurationClicked = false;
                    isClicked = true;
                    p.Close();
                }
                else
                {
                    var booking = DataProvider.Instance.DB.Bookings.Where(x => x.BkCode == BookingCode && x.Isdeleted == false && x.BkStatus == 1).FirstOrDefault();
                    if (booking != null && mainVM != null)
                    {
                        FoodLayoutWindow foodLayoutWindow = new FoodLayoutWindow();
                        var foodLayoutVM = foodLayoutWindow.DataContext as FoodLayoutViewModel;

                        if (foodLayoutVM != null)
                        {
                            foodLayoutVM.IsEditing = 0;
                            foodLayoutVM.SelectedTabNum = byte.Parse(SelectedTable.Substring(4));
                            foodLayoutVM.SelectedEmpName = SelectedEmpName;
                            foodLayoutVM.TotalAmount = 0;
                            foodLayoutVM.Bills.Clear();

                            foodLayoutWindow.ShowDialog();
                            
                            if (foodLayoutVM.IsConfirmed == false)
                            {
                                return;
                            }
                        }

                        //SelectedTable = mainVM.SelectedTable;
                        RestoreStatus(mainVM.SelectedTable);
                        //booking.Isdeleted = true;
                        booking.BkStatus = 0;
                        //booking.BkCode = $"{BookingCode} (Booked)";
                        DataProvider.Instance.DB.SaveChanges();
                        int i = 0;
                        foreach (Booking bk in mainVM.BookingList)
                        {
                            if (bk.Isdeleted == false && bk.BkStatus == 1)
                            {
                                bk.BkCode = $"BK{++i:D3}";
                            }
                        }
                        DataProvider.Instance.DB.SaveChanges();

                        mainVM.LoadOrderUC();
                        p.Close();
                        //mainVM.LoadAllTableInformation();
                        //mainVM.LoadAllBookingInformation();
                    }
                    mainVM.SelectedTable = null;
                }
                ReloadFillIn();
            });
            AddNewCustomer = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                try
                {
                    AddCusWindow addCusWindow = new AddCusWindow();
                    var customerVM = addCusWindow.DataContext as CustomerManagementViewModel;
                    if (customerVM != null)
                    {
                        addCusWindow.ShowDialog();
                        if (customerVM.isConfirmed)
                        {
                            DataProvider.Instance.DB.Customers.Add(customerVM.NewCustomer);
                            DataProvider.Instance.DB.SaveChanges();
                            CustomerNameList.Add(customerVM.CustomerName);
                        }
                        CustomerNameList = new ObservableCollection<string>(DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false).Select(x => x.CusName).ToList());
                        customerVM.isConfirmed = false;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Số điện thoại đã tồn tại hoặc đã bị xóa tạm, vui lòng thử lại!", "Lỗi thêm khách hàng", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            );
            ConfigureBookingInfo = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                bookingManagementID = 1;
                isClicked = false;
                isConfigurationClicked = true;
            });
            ExitConfiguration = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {

                MainWindow mainWindow = new MainWindow();
                var mainVM = mainWindow.DataContext as MainViewModel;
                var bk = DataProvider.Instance.DB.Bookings.Where(x => x.BkCode == BookingCode && x.Isdeleted == false && x.BkStatus == 1).FirstOrDefault();
                if (mainVM != null)
                {
                    if (bk == null)
                        mainVM.SelectedTable = null;
                    else
                    {
                        RestoreStatus(mainVM.SelectedTable);
                        //mainVM.LoadAllTableInformation();
                        //mainVM.LoadAllBookingInformation();
                    }
                }
                ReloadFillIn();
                isConfigurationClicked = false;
                isClicked = true;
                p.Close();
            });
    }


        public void AddNewBooking(Window p, int bookingNumber)
        {
            isConfirmed = true;
            NewBooking = new Booking()
            {
                BkCode = BookingCode,
                ////BookingDate = DateTime.Parse(changeDateTime),
                CusId = DataProvider.Instance.DB.Customers.Where(x => x.CusName == SelectedCustomerName && x.CusPhone == CusPhoneNumber).Select(x => x.CusId).FirstOrDefault(),
                EmpId = DataProvider.Instance.DB.Employees.Where(x => x.EmpName == SelectedEmpName).Select(x => x.EmpId).FirstOrDefault(),
                TabId = TableID,
                BkStime = DateTime.Now,
                BkOtime = new DateTime(
                    SelectedBookingDate.Year,
                    SelectedBookingDate.Month,
                    SelectedBookingDate.Day,
                    int.Parse(SelectedHour),
                    int.Parse(SelectedMinute),
                    0),
                BkStatus = 1,
                Isdeleted = false
            };
            if (NewBooking.TabId != null)
            {
                AddTableWindow addTableWindow = new AddTableWindow();
                var atvm = addTableWindow.DataContext as AddTableViewModel;
                DiningTable d = DataProvider.Instance.DB.DiningTables.Where(x => x.TabId == NewBooking.TabId).FirstOrDefault();
                if (d != null && atvm != null)
                {
                    atvm.TabStatus = "Đang có khách";
                    d.TabStatus = false;
                }
            }
            DataProvider.Instance.DB.SaveChanges();
        }
        public Booking EditBookingInfo(Booking bk)
        {
            bk.CusId = DataProvider.Instance.DB.Customers
                .Where(x => x.CusName == SelectedCustomerName && x.CusPhone == CusPhoneNumber)
                .Select(x => x.CusId)
                .FirstOrDefault();
            bk.EmpId = DataProvider.Instance.DB.Employees
                        .Where(x => x.EmpName == SelectedEmpName)
                        .Select(x => x.EmpId)
                        .FirstOrDefault();
            bk.BkOtime = new DateTime(
                SelectedBookingDate.Year,
                SelectedBookingDate.Month,
                SelectedBookingDate.Day,
                int.Parse(SelectedHour),
                int.Parse(SelectedMinute),
                0);
            bk.BkStatus = 1;
            bk.Isdeleted = false;

            return bk; // Trả về chính thực thể đã chỉnh sửa
        }

        public void ReloadFillIn()
        {
            SelectedBookingDate = DateTime.Now;
            SelectedHour = null;
            SelectedMinute = "00";
            SelectedCustomerName = null;
            CusPhoneNumber = "";
            SelectedEmpName = null;
        }
        public void LoadBookingInformation(Booking booking)
        {
            SelectedBookingDate = booking.BkOtime;
            SelectedHour = (booking.BkOtime.Hour).ToString();
            SelectedMinute = $"{(booking.BkOtime.Minute):D2}";
            SelectedCustomerName = DataProvider.Instance.DB.Customers.Where(x => x.CusId == booking.CusId).Select(x => x.CusName).FirstOrDefault() ?? "";
            CusPhoneNumber = DataProvider.Instance.DB.Customers.Where(x => x.CusId == booking.CusId).Select(x => x.CusPhone).FirstOrDefault() ?? "";
            BookingCode = booking.BkCode;
            SelectedEmpName = DataProvider.Instance.DB.Employees.Where(x => x.EmpId == booking.EmpId).Select(x => x.EmpName).FirstOrDefault() ?? "";

            MainWindow mainWindow = new MainWindow();
            var mainVM = mainWindow.DataContext as MainViewModel;
            if (mainVM != null)
            {
                DiningTable dt = booking.Tab;
                if (dt != null)
                {
                    dt.TabStatus = true;
                    DataProvider.Instance.DB.DiningTables.Update(dt);
                    DataProvider.Instance.DB.SaveChanges();
                    mainVM.LoadAllTableInformation();
                    mainVM.SelectedTable = $"Bàn {dt.TabNum}";
                }
                mainVM.isEnabled = false;
            }
        }
        public void RestoreStatus(string status)
        {
            DiningTable dt = DataProvider.Instance.DB.DiningTables.Where(x => x.TabNum == int.Parse(status.Substring(4))).FirstOrDefault();
            if (dt != null)
            {
                dt.TabStatus = false;
                DataProvider.Instance.DB.DiningTables.Update(dt);
                DataProvider.Instance.DB.SaveChanges();
            }
        }
    }
}
