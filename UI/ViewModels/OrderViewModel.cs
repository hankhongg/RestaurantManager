using RestaurantManager.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.ViewModels
{
    class OrderViewModel : BaseViewModel
    {
        private string orderNumber;
        public string OrderNumber
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

        private string orderTimer;
        public string OrderTimer
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
        private string orderTable;
        public string OrderTable
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

        private string orderBill;
        public string OrderBill
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

        void LoadOrderInformation()
        {
            // load order information from foodlayoutwindow
        }
        public OrderViewModel()
        {
            OrderBill = "1.000.000";
            OrderCustomer = "Nguyễn Văn A";
            OrderEmployee = "Nguyễn Văn B";
            OrderNumber = "1";
            OrderTimer = "00:00:00";
            OrderTable = "1";

        }
    }
}
