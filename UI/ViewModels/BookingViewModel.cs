using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RestaurantManager.Views;

namespace RestaurantManager.ViewModels
{
    class BookingViewModel : BaseViewModel
    {
        private int numberOfCus = 1;

        public int NumberOfCustomers
        {
            get { return numberOfCus; }
            set 
            {
                numberOfCus = value; 
                OnPropertyChanged();
            }
        }

        private string dateTime;

        public string changeDateTime
        {
            get { return dateTime; }
            set 
            { 
                dateTime = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCusNum { get; set; }
        public ICommand RemoveCusNum { get; set; }
        public ICommand BookingInfo {  get; set; }
        public ICommand cancelOrder {  get; set; }
        public ICommand setDate { get; set; }
        public BookingViewModel()
        {
            BookingInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    BookingWindow bookingWindow = new BookingWindow(p);
                    bookingWindow.ShowDialog();
                }
            );
            cancelOrder = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    p.Close();
                }
            );
            AddCusNum = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    NumberOfCustomers++;
                }
            );
            RemoveCusNum = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    if (NumberOfCustomers > 1)
                    {
                        NumberOfCustomers--;
                    }
                }
            );
            setDate = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    DatePicker datePicker = new DatePicker();
                    changeDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                    datePicker.SelectedDateChanged += (s, e) =>
                    {
                        changeDateTime = datePicker.SelectedDate.Value.ToString("dd/MM/yyyy");
                    };
                }
            );
        }
        
    }
}
