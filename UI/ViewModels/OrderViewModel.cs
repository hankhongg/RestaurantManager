using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.ViewModels
{
    class OrderViewModel : BaseViewModel
    {
        private int orderNumber;
        public int OrderNumber
        {
            get
            {
                return orderNumber;
            }
            set
            {
                orderNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime orderTimer;
        public DateTime OrderTimer
        {
            get
            {
                return orderTimer;
            }
            set
            {
                orderTimer = value;
                OnPropertyChanged();
            }
        }

        private string orderEmployee;
        public string OrderEmployee
        {
            get
            {
                return orderEmployee;
            }
            set
            {
                orderEmployee = value;
                OnPropertyChanged();
            }
        }

        private string orderCustomer;
        public string OrderCustomer
        {
            get
            {
                return orderCustomer;
            }
            set
            {
                orderCustomer = value;
                OnPropertyChanged();
            }
        }
        private byte? orderTable;
        public byte? OrderTable
        {
            get
            {
                return orderTable;
            }
            set
            {
                orderTable = value;
                OnPropertyChanged();
            }
        }

        private decimal orderBill;
        public decimal OrderBill
        {
            get
            {
                return orderBill;
            }
            set
            {
                orderBill = value;
                OnPropertyChanged();
            }
        }
        private decimal orderRecid;
        public decimal OrderRecid
        {
            get
            {
                return orderRecid;
            }
            set
            {
                orderRecid = value;
                OnPropertyChanged();
            }
        }

        public int RecId { get; internal set; }

        public OrderViewModel()
        {
            LoadOrderInformation();
        }
        void LoadOrderInformation()
        {
            // load order information from foodlayoutwindow
        }
        //public void SetorderItemData(int ordernum, decimal orderbill, int orderem, byte? ordertab, DateTime ordertimer)
        //{
        //    this.OrderRecid = ordernum;
        //    this.OrderBill = orderbill;
        //    this.OrderEmployee = orderem;
        //    this.OrderTable = ordertab;
        //    this.OrderTimer = ordertimer;

        //}
    }
}
