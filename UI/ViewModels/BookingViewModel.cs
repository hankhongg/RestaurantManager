using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantManager.Views;

namespace RestaurantManager.ViewModels
{
    class BookingViewModel : BaseViewModel
    {
        public ICommand BookingInfo {  get; set; }
        public BookingViewModel()
        {
            BookingInfo = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    BookingWindow bookingWindow = new BookingWindow();
                    bookingWindow.ShowDialog();
                }
            );
        }
    }
}
