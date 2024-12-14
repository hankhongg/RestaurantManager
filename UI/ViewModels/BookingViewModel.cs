using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private Visibility isBooked;

        public ICommand AddCusNum { get; set; }
        public ICommand RemoveCusNum { get; set; }
        public ICommand BookingInfo {  get; set; }
        public ICommand cancelBooking {  get; set; }
        public ICommand confirmBooking {  get; set; }
        public ICommand setDate { get; set; }
        public BookingViewModel()
        {

            BookingInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    BookingWindow bookingWindow = new BookingWindow(p);
                    bookingWindow.ShowDialog();
                }
            );
            cancelBooking = new RelayCommand<Window>((p) => { return true; }, (p) =>
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
                    
                }
            );
            cancelBooking = new RelayCommand<Window>((p) => { return true; }, (p) =>
                p.Close()
            );
            confirmBooking = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    MessageBox.Show("Booking confirmed");
                }
            );
        }
    }
}
