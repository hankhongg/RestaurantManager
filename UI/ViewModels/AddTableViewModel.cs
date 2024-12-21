using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    internal class AddTableViewModel : BaseViewModel
    {
        private bool allIsEmpty = true;
        public bool AllIsEmpty { get { return allIsEmpty; } set { allIsEmpty = value; OnPropertyChanged(nameof(AllIsEmpty)); } }


        private string[] tabStatuses = new string[2] { "Đang có khách", "Đang trống" };
        public string[] TabStatuses { get { return tabStatuses; } set { tabStatuses = value; OnPropertyChanged(nameof(TabStatuses)); } }
        private byte tabNumber;
        public byte TabNumber { get { return tabNumber; } set { if (tabNumber != value) tabNumber = value; OnPropertyChanged(nameof(TabNumber)); } }
        private string tabStatus;
        public string TabStatus { get { return tabStatus; } set { if (tabStatus != value) tabStatus = value; OnPropertyChanged(nameof(TabStatus)); } }

        public ICommand DelTableCommand { get; set; }
        public ICommand CancelTableCommand { get; set; }
        public ICommand SaveTableCommand { get; set; }
        public AddTableViewModel()
        {
            DelTableCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (AllIsEmpty == false)
                {
                    MessageBox.Show("Vui lòng đảm bảo các bàn trống trước khi thực hiện thao tác xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                DiningTable d = DataProvider.Instance.DB.DiningTables.Where(x => x.TabNum == TabNumber).FirstOrDefault();
                d.Isdeleted = true;
                d.TabNum = null;
                DataProvider.Instance.DB.SaveChanges();
                p.Close();
            });
            CancelTableCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
            SaveTableCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                DiningTable d = DataProvider.Instance.DB.DiningTables.Where(x => x.TabNum == TabNumber).FirstOrDefault();

                
                d.TabStatus = TabStatus == "Đang có khách" ? false : true;
                
                DataProvider.Instance.DB.SaveChanges();

                p.Close();
            });
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
        //protected FrameworkElement getParentWindow(UserControl p)
        //{
        //    FrameworkElement parentWindow = p;
        //    while (parentWindow.Parent != null)
        //    {
        //        parentWindow = parentWindow.Parent as FrameworkElement;
        //    }
        //    return parentWindow;
        //}
    }
}
