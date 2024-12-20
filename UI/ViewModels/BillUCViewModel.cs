using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using XAct.UI.Views;
using RestaurantManager.Views;

namespace RestaurantManager.ViewModels
{
    class BillUCViewModel : BaseViewModel
    {
        private int _quantity;
        private decimal _price;

        private int stt;
        public int STT
        {
            get => stt;
            set
            {
                if (stt != value)
                {
                    stt = value;
                    OnPropertyChanged(nameof(STT)); // Gửi thông báo thay đổi
                }
            }
        }
        public string ItemName { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity)); // Gửi thông báo thay đổi
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price)); // Gửi thông báo thay đổi
                }
            }
        }

        private int recId;
        public int RecId
        {
            get => recId;
            set
            {
                if (recId != value)
                {
                    recId = value;
                    OnPropertyChanged(nameof(RecId)); // Gửi thông báo thay đổi
                }
            }
        }

        public decimal ItemSprice { get; set; }

        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }
        public RelayCommand<FoodLayoutViewModel> DeleteCommand { get; private set; }
        public ICommand EditBillCommand { get; set; }

        public decimal TotalAmount { get; internal set; }
        public int Isdeleted { get; internal set; }
        public int RecNum { get; set; }

        public BillUCViewModel()
        {
            IncreaseQuantityCommand = new RelayCommand<object>(p => true, p =>
            {
                Quantity++; // Tăng số lượng
                Price += ItemSprice; // Cập nhật giá

                // Gửi thông báo thay đổi cho giao diện
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Price));
            });

            DecreaseQuantityCommand = new RelayCommand<object>(p => Quantity > 0, p =>
            {
                if (Quantity > 1)
                {
                    Quantity--; // Giảm số lượng
                    Price -= ItemSprice; // Cập nhật giá
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Price));
                }
            });


            // Gọi hàm xóa từ ViewModel cha thông qua CommandParameter
            DeleteCommand = new RelayCommand<FoodLayoutViewModel>(
                (parentVM) => true,
                (parentVM) =>
                {
                    parentVM.DeleteBillCommand.Execute(this);
                });
        }

    }
}
