using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public ICommand ForgotPasswordCommand { get; set; }
        public LoginViewModel()
        {
            ForgotPasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
                { 
                    ForgotPasswordWindow passwordWindow = new ForgotPasswordWindow();
                    passwordWindow.ShowDialog();
                }
            );
        }
    }
}
