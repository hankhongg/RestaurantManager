using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantManager.Models;
using RestaurantManager.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RestaurantManager.ViewModels
{
    class TableViewModel : BaseViewModel
    {
        //public TableUC TableUCControl { get; set; }
        private string tabNumber;
        public string TabNumber { get { return tabNumber; } set { if (tabNumber != value) tabNumber = value; OnPropertyChanged(nameof(TabNumber)); } }
        private string tabStatus;
        public string TabStatus { get { return tabStatus; } set { if (tabStatus != value) tabStatus = value; OnPropertyChanged(nameof(TabStatus)); } }

        private bool boolTabStatus;
        public bool BoolTabStatus { get { return boolTabStatus; } set { if (boolTabStatus != value) boolTabStatus = value; OnPropertyChanged(nameof(BoolTabStatus)); } }

        public TableViewModel()
        {

        }
        public TableViewModel(byte tableNumber, bool tableStatus)
        {
            TabNumber = $"Bàn {tableNumber}";
            if (tableStatus == true)
            {
                TabStatus = "Trống";
                BoolTabStatus = true;
            }
            else { 
            TabStatus = "Đang có khách";
                BoolTabStatus = false;
            }
        }
        public TableViewModel(DiningTable d)
        {
            TabNumber = $"Bàn {(byte)d.TabNum}";
            if (d.TabStatus == true)
            {
                TabStatus = "Trống";
                boolTabStatus = true;
            }
            else
            {
                TabStatus = "Đang có khách";
                boolTabStatus = false;
            }
        }
        
    }
}
