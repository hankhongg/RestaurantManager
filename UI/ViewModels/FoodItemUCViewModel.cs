using RestaurantManager.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using static RestaurantManager.ViewModels.FoodLayoutViewModel;

namespace RestaurantManager.ViewModels
{
    class FoodItemUCViewModel : BaseViewModel
    {
        private int foodItemId;
        public int FoodItemId
        {
            get => foodItemId;
            set
            {
                foodItemId = value;
                OnPropertyChanged(nameof(FoodItemId));
            }
        }

        private string foodItemName;
        private string? foodItemImg;
        private decimal foodItemSprice;

        public string FoodItemName
        {
            get => foodItemName;
            set
            {
                foodItemName = value;
                OnPropertyChanged(nameof(FoodItemName));
            }
        }
        public string FoodItemImg
        {
            get => foodItemImg;
            set
            {
                foodItemImg = value;
                OnPropertyChanged(nameof(FoodItemImg));
            }
        }
        public decimal FoodItemSprice
        {
            get => foodItemSprice;
            set
            {
                foodItemSprice = value;
                OnPropertyChanged(nameof(FoodItemSprice));
            }
        }
        private string foodItemType;
        public string FoodItemType
        {
            get => foodItemType;
            set
            {
                foodItemType = value;
                OnPropertyChanged(nameof(FoodItemType));
            }
        }


        // Thuộc tính Enum để sử dụng trong Filter
        public MenuItemType ItemTypeEnum
        {
            get
            {
                // Chuyển đổi từ string sang enum, mặc định là Others nếu không khớp
                if (Enum.TryParse(FoodItemType, true, out MenuItemType result))
                    return result;

                return MenuItemType.OTHER;
            }
        }

        public FoodItemUCViewModel()
        {

        }

        public void SetFoodItemData(int itemId, string itemName, string itemImg, decimal itemSprice)
        {
            this.foodItemId = itemId;
            this.FoodItemName = itemName;
            this.FoodItemImg = itemImg;
            this.FoodItemSprice = itemSprice;


        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

    }


}
