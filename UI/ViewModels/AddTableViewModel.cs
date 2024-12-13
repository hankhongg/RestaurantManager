using RestaurantManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    internal class AddTableViewModel : BaseViewModel
    {
        private string[] tabStatuses = new string[2] { "Đang có khách", "Đang trống" };
        public string[] TabStatuses { get { return tabStatuses; } set { tabStatuses = value; OnPropertyChanged(nameof(TabStatuses)); } }
        private byte tabNumber;
        public byte TabNumber { get { return tabNumber; } set { if (tabNumber != value) tabNumber = value; OnPropertyChanged(nameof(TabNumber)); } }
        private string tabStatus;
        public string TabStatus { get { return tabStatus; } set { if (tabStatus != value) tabStatus = value; OnPropertyChanged(nameof(TabStatus)); } }

        public ICommand DelTableCommand { get; set; }
        public ICommand CancelTableCommand { get; set; }
        public AddTableViewModel()
        {

        }
        public void LoadTableInformation(DiningTable d)
        {
            TabNumber = (byte)d.TabNum;
            if (d.TabStatus == false)
            {
                TabStatus = "Đang có khách";
            }
            else
            {
                TabStatus = "Đang trống";
            }
        }
    }
}
