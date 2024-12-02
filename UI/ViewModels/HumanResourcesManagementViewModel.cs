using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantManager.Views;
using System.Windows.Controls;

namespace RestaurantManager.ViewModels 
{
    public class HumanResourcesManagementViewModel : BaseViewModel
    {

        private string customerName;
        public string CustomerName
        {
            get { return customerName; }
            set 
            { 
                customerName = value; 
                OnPropertyChanged();
            }
        }
        public ICommand AddCus { get; set; }
        public ICommand DelCus { get; set; }
        public ICommand ConfigCus { get; set; }
        public ICommand SaveCus { get; set; }
        public ICommand CancelInfo { get; set; }
        public HumanResourcesManagementViewModel()
        {
            CancelInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MessageBox.Show("Window Closed");
                p.Close();
            }); 
        }
    }
}
