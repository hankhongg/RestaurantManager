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
        private byte tabNumber;
        public byte TabNumber { get { return tabNumber; } set { if (tabNumber != value) tabNumber = value; OnPropertyChanged(nameof(TabNumber)); } }
        private bool tabStatus;
        public bool TabStatus { get { return tabStatus; } set { if (tabStatus != value) tabStatus = value; OnPropertyChanged(nameof(TabStatus)); } }

        public TableViewModel()
        {

        }
        public TableViewModel(byte tableNumber, bool tableStatus)
        {
            TabNumber = tableNumber;
            TabStatus = tableStatus;
            //MessageBox.Show($"Table {TabNumber} status: {TabStatus}");
        }
        public TableViewModel(DiningTable d)
        {
            TabNumber = (byte)d.TabNum;
            TabStatus = d.TabStatus;
            //MessageBox.Show($"Table {TabNumber} status: {TabStatus}");
        }
        
    }
}
