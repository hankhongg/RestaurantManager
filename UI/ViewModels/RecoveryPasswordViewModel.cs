using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class RecoveryPasswordViewModel : BaseViewModel
    {
        private string typeInEmail;
        public string TypeInEmail
        {
            get
            {
                return typeInEmail;
            }
            set
            {
                typeInEmail = value;
                OnPropertyChanged();
            }
        }

        private string sentCode;
        public string SentCode
        {
            get
            {
                return sentCode;
            }
            set
            {
                sentCode = value;
                OnPropertyChanged();
            }
        }

        private string typeInCode;
        public string TypeInCode
        {
            get
            {
                return typeInCode;
            }
            set
            {
                typeInCode = value;
                OnPropertyChanged();
            }
        }
        public ICommand ConfirmCodeCommand { get; set; }
        public RecoveryPasswordViewModel()
        {
            ConfirmCodeCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (TypeInCode == SentCode)
                {
                    System.Windows.MessageBox.Show("Mã xác nhận thành công!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    p.Close();
                    ProfileWindow profileWindow = new ProfileWindow();
                    ProfileViewModel? profileVM = profileWindow.DataContext as ProfileViewModel;
                    if (profileVM != null)
                    {
                        profileVM.AccountEmail = TypeInEmail;
                        profileVM.LoadAccountInformation();
                        profileWindow.DataContext = profileVM;
                        profileWindow.ShowDialog();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Mã xác nhận không đúng!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            });
        }
    }
}
