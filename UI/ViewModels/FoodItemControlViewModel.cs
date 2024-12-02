using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RestaurantManager.ViewModels
{
    public class FoodItemViewModel : INotifyPropertyChanged
    {
        private string _foodName;
        private string _foodPrice;
        private BitmapImage _foodImage;

        public string FoodName
        {
            get => _foodName;
            set
            {
                _foodName = value;
                OnPropertyChanged();
            }
        }

        public string FoodPrice
        {
            get => _foodPrice;
            set
            {
                _foodPrice = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage FoodImage
        {
            get => _foodImage;
            set
            {
                _foodImage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<FoodItemViewModel> FoodItems { get; set; }

        public FoodItemViewModel()
        {
            // Add some sample data
            FoodItems = new ObservableCollection<FoodItemViewModel>
            {
                new FoodItemViewModel
                {
                    FoodName = "Mỳ hải sản tôm nước",
                    FoodPrice = "45,000đ",
                    FoodImage = new BitmapImage(new Uri("Screenshot 2024-11-30 132614.png", UriKind.Relative))
                }

            };
        }
    }
}
