using Microsoft.IdentityModel.Tokens;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using XAct;

namespace RestaurantManager.ViewModels
{
    public class NewDishViewModel : BaseViewModel
    {
        public ICommand AddItemCommand { get; set; }
        public ICommand OpenPictureCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private string _ItemName;
        public string ItemName { get => _ItemName; set { _ItemName = value; OnPropertyChanged(); } }

        private decimal _ItemSprice;
        public decimal ItemSprice { get => _ItemSprice; set { _ItemSprice = value; OnPropertyChanged(); } }

        private string _ItemType;
        public string ItemType { get => _ItemType; set { _ItemType = value; OnPropertyChanged(); } }

        private string _ImageImg;
        public string ItemImg { get => _ImageImg; set { _ImageImg = value; OnPropertyChanged(); } }

        private string _ItemCode; // Trường lưu trữ giá trị thực tế
        public string ItemCode
        {
            get => _ItemCode; // Trả về giá trị từ trường lưu trữ
            set
            {
                _ItemCode = value; // Gán giá trị mới cho trường lưu trữ
                OnPropertyChanged(); // Gọi sự kiện thông báo thay đổi giá trị
            }
        }

        public ObservableCollection<string> ItemTypes { get; set; }

        private decimal _ItemCprice;
        public decimal ItemCprice { get => _ItemCprice; set { _ItemCprice = value; OnPropertyChanged(); } }

        public NewDishViewModel()
        {
            ItemTypes = new ObservableCollection<string> { "FOOD", "DRINK", "OTHER" };

            // Command mở ảnh
            OpenPictureCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    ItemImg = openFileDialog.FileName;
                }
            });

            // Command thêm món ăn
            AddItemCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(ItemName) && string.IsNullOrEmpty(ItemType) && string.IsNullOrEmpty(ItemCode))
                    return false;
                return true;
                // Kiểm tra xem các trường có được điền đầy đủ không
                //return !string.IsNullOrEmpty(ItemName) &&                       
                //       !string.IsNullOrEmpty(ItemType) &&
                //       !string.IsNullOrEmpty(ItemCode);
            }, (p) =>
            {
                // Kiểm tra xem ItemCode đã tồn tại chưa
                var existingItem = DataProvider.Instance.DB.MenuItems
                                     .FirstOrDefault(x => x.ItemCode == ItemCode);

                if (existingItem != null)
                {
                    // Mã ItemCode đã tồn tại, thông báo lỗi
                    System.Windows.MessageBox.Show($"Mã '{ItemCode}' đã tồn tại. Vui lòng chọn mã khác!", "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"ItemCode: {ItemCode}");
                // Thêm món ăn vào cơ sở dữ liệu
                var item = new MenuItem()
                {
                    ItemCode = ItemCode,
                    ItemName = ItemName,
                    ItemCprice = ItemCprice,
                    ItemSprice = ItemSprice,
                    ItemType = ItemType,
                    ItemImg = ItemImg
                };
                DataProvider.Instance.DB.MenuItems.Add(item);
                DataProvider.Instance.DB.SaveChanges();

                // Thông báo thêm thành công
                System.Windows.MessageBox.Show("Thêm món ăn thành công!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            });

            // Command thoát ứng dụng
            ExitCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                System.Windows.Application.Current.Shutdown();
            });
        }
    }
}
