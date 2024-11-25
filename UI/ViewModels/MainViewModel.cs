using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public ICommand WindowIsLoaded { get; set; }

        public bool isLoaded = false;
        public MainViewModel() { 

            WindowIsLoaded = new RelayCommand<object>((p) => { return true; }, (p) => { isLoaded = true; LoginWindow loginWindow = new LoginWindow(); loginWindow.ShowDialog(); });
        }
    }
}
