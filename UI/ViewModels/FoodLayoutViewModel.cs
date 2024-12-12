using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    using global::RestaurantManager.Models.DataProvider;
    using RestaurantManager.Models.DataProvider;
    using RestaurantManager.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

            public ICommand EditDishComand { get; set; }
            public RelayCommand<object> EditDishCommand { get; private set; }

            // Danh sách các món ăn
            private ObservableCollection<FoodItemUCViewModel> _MenuItems;
            public ObservableCollection<FoodItemUCViewModel> MenuItems
            {
                get => _MenuItems;
                set
                {
                    _MenuItems = value;
                    OnPropertyChanged();
                }
            }

            public void LoadMenuItems()
            {
                // Lấy dữ liệu từ cơ sở dữ liệu và ánh xạ vào FoodItemUCViewModel
                var items = DataProvider.Instance.DB.MenuItems.ToList();
                MenuItems = new ObservableCollection<FoodItemUCViewModel>(
                    items.Select(item => new FoodItemUCViewModel
                    {

                        ItemName = item.ItemName,
                        ItemImg = item.ItemImg,
                        ItemSprice = item.ItemSprice
                    })
                );
            }
            public FoodLayoutViewModel()
            {
                LoadMenuItems();

                NewDishCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                    NewDishWindow wd = new NewDishWindow();
                    wd.ShowDialog();
                });
                FoodItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                    FoodItemWindow wd = new FoodItemWindow();
                    wd.ShowDialog();
                });
                EditDishComand = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    EditDishWindow wd = new EditDishWindow();
                    wd.ShowDialog();
                });


            }
        }
    }

}
