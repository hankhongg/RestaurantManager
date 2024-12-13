using RestaurantManager.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.ViewModels
{
    class FoodItemUCViewModel : BaseViewModel
    {


        private string _itemImg;
        private string _itemName;
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

        public FoodItemUCViewModel() // Constructor mặc định
        {
            ItemName = "DefaultName";
            ItemImg = "DefaultImagePath";
            ItemSprice = 0;
        }

        public FoodItemUCViewModel(string itemName, string itemImg, decimal itemSprice) // Constructor có tham số
        {
            this.ItemName = itemName;
            this.ItemImg = itemImg;
            this.ItemSprice = itemSprice;
        }

    }
}
