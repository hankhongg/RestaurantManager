using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Models;
using RestaurantManager.Views;
using System.Windows.Input;
using UI.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace RestaurantManager.ViewModels
{
    public class BookingConfigurationViewModel : BaseViewModel
    {
        private string newBookingCode;

        public string NewBookingCode
        {
            get { return newBookingCode; }
            set
            {
                newBookingCode = value;
                OnPropertyChanged();
            }
        }

        private string _newCusPhoneNumber;

        public string NewCusPhoneNumber
        {
            get { return _newCusPhoneNumber; }
            set
            {
                _newCusPhoneNumber = value;
                OnPropertyChanged();
            }
        }
        private string _newSelectedEmpName;
        public string NewSelectedEmpName
        {
            get { return _newSelectedEmpName; }
            set
            {
                _newSelectedEmpName = value;
                OnPropertyChanged();
            }
        }
        private string _newSelectedCustomerName;
        public string NewSelectedCustomerName
        {
            get { return _newSelectedCustomerName; }
            set
            {
                _newSelectedCustomerName = value;
                if (_newSelectedCustomerName != null)
                {
                    //BookingWindow bookingWindow = new BookingWindow();
                    //var bookingVM = bookingWindow.DataContext as BookingViewModel;
                    //NewCusPhoneNumber = DataProvider.Instance.DB.Customers.Where(x => x.CusName == bookingVM.SelectedCustomerName).Select(x => x.CusPhone).FirstOrDefault();
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _newSelectedBookingDate;

        public DateTime NewSelectedBookingDate
        {
            get { return _newSelectedBookingDate; }
            set
            {
                _newSelectedBookingDate = value;
                OnPropertyChanged();
            }
        }
        private string _newSelectedTime;

        public string NewSelectedTime
        {
            get { return _newSelectedTime; }
            set
            {
                _newSelectedTime = value;
                OnPropertyChanged();
            }
        }
        private string _newSelectedTable;

        public string NewSelectedTable
        {
            get { return _newSelectedTable; }
            set
            {
                _newSelectedTable = value;
                if (_newSelectedTable != null)
                {
                    OnPropertyChanged();
                }
            }
        }
        private string _newTableNumber;

        public string NewTableNumber
        {
            get { return _newTableNumber; }
            set
            {
                _newTableNumber = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<BookingConfigurationViewModel> _bookingList;
        public ObservableCollection<BookingConfigurationViewModel> BookingList
        {
            get { return _bookingList; }
            set
            {
                _bookingList = value;
                OnPropertyChanged();
            }
        }

        public BookingConfigurationViewModel()
        {
            MainWindow mainWindow = new MainWindow();
            var mainVM = mainWindow.DataContext as MainViewModel;

            if (mainVM != null)
            {
                mainVM.LoadAllBookingInformation();
                BookingList = mainVM.BookingViewList;
            }
        }


        public BookingConfigurationViewModel(Booking booking)
        {
            NewTableNumber = booking.TabId == null ? "?" : DataProvider.Instance.DB.DiningTables.Where(x => x.TabId == booking.TabId).Select(x => x.TabNum).FirstOrDefault().ToString();
            NewSelectedCustomerName = booking.CusId == null ? "" : DataProvider.Instance.DB.Customers.Where(x => x.CusId == booking.CusId).Select(x => x.CusName).FirstOrDefault();
            NewCusPhoneNumber = DataProvider.Instance.DB.Customers.Where(x => x.CusId == booking.CusId).Select(x => x.CusPhone).FirstOrDefault();
            NewSelectedEmpName = booking.EmpId == null ? "" : DataProvider.Instance.DB.Employees.Where(x => x.EmpId == booking.EmpId).Select(x => x.EmpName).FirstOrDefault();
            NewSelectedBookingDate = booking.BkOtime;
            NewBookingCode = booking.BkCode;
            NewSelectedTime = $"{booking.BkOtime.Hour}:{booking.BkOtime.Minute:D2}";
        }
    }
}
