using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public ICommand WindowIsLoadedCommand { get; set; }
        public ICommand ProfileManagementCommand {  get; set; }

        public bool isLoaded = false;
        public MainViewModel() { 

            WindowIsLoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
                { 
                    isLoaded = true; 
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog(); 
                }
            );

            ProfileManagementCommand = new RelayCommand<object>( (p) => { return true; }, (p) =>
                {
                    ProfileWindow profileWindow = new ProfileWindow();
                    profileWindow.ShowDialog();
                }
            );
        }
    }
}
