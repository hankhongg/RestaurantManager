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


        private string _itemImg;
        private string _itemName;
        private string _itemType;
        private decimal _itemSprice;

        public string ItemImg
        {
            get { return _itemImg; }
            set
            {
                if (_itemImg != value)
                {
                    _itemImg = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (_itemName != value)
                {
                    _itemName = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal ItemSprice
        {
            get { return _itemSprice; }
            set
            {
                if (_itemSprice != value)
                {
                    _itemSprice = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                if (_itemType != value)
                {
                    _itemType = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ItemTypeEnum)); // Cập nhật cả giá trị enum khi chuỗi thay đổi
                }
            }
        }

        // Thuộc tính Enum để sử dụng trong Filter
        public MenuItemType ItemTypeEnum
        {
            get
            {
                // Chuyển đổi từ string sang enum, mặc định là Others nếu không khớp
                if (Enum.TryParse(_itemType, true, out MenuItemType result))
                    return result;

                return MenuItemType.OTHER;
            }
        }


        public void SetFoodItemData(string itemName, string itemImg, decimal itemSprice)
        {
            this.ItemName = itemName;
            this.ItemImg = itemImg;
            this.ItemSprice = itemSprice;


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
