using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class FoodLayoutViewModel : BaseViewModel
    {
        public ICommand NewDishCommand { get; set; }
        public RelayCommand<object> FoodItemCommand { get; private set; }
        public ICommand FoodItemComand { get; set; }
        public FoodLayoutViewModel()
        {

            NewDishCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                NewDishWindow wd = new NewDishWindow();
                wd.ShowDialog();
            });
            FoodItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                FoodItemWindow wd = new FoodItemWindow();
                wd.ShowDialog();
            });
        }
    }
}
