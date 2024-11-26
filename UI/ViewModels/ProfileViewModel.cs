using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    internal class ProfileViewModel : BaseViewModel
    {
        private bool isButtonPressed;
        public bool IsButtonPressed
        {
            get => isButtonPressed; set
            {
                isButtonPressed = value;
                OnPropertyChanged();
            }
        }
        public ICommand IsButtonPressedCommand { get; set; }
        public ICommand SaveButtonPressedCommand { get; set; }
        public ProfileViewModel()
        {
            IsButtonPressedCommand = new RelayCommand<object>((p) =>
                {
                    return !IsButtonPressed;
                },
                (p) =>
                {
                    IsButtonPressed = !IsButtonPressed;
                }
            );
            SaveButtonPressedCommand = new RelayCommand<object>((p) =>
                {
                    return IsButtonPressed;
                },
                (p) =>
                {
                    MessageBoxResult r = MessageBox.Show("Bấm Yes để xác nhận lưu thông tin!", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                    if (r == MessageBoxResult.Yes)
                    {
                        // lưu vào database! <=
                        IsButtonPressed = false;
                    }
                }
            );
        }
    }
}
