using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantManager.Views;
using System.Windows.Controls;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManager.ViewModels 
{
    public class CustomerManagementViewModel : BaseViewModel
    {
        public bool isConfirmed { get; set; }
        public int CustomerNumber { get; set; }
        private Customer newCustomer;
        public Customer NewCustomer
        {
            get { return newCustomer; }
            set
            {
                newCustomer = value;
                OnPropertyChanged();
            }
        }

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
        // Customer Address
        private string customerAddress;
        public string CustomerAddress
        {
            get { return customerAddress; }
            set
            {
                customerAddress = value;
                OnPropertyChanged();
            }
        }
        // Customer Phone
        private string customerPhone;
        public string CustomerPhone
        {
            get { return customerPhone; }
            set
            {
                customerPhone = value;
                OnPropertyChanged();
            }
        }
        // Customer Cccd
        private string customerCccd;
        public string CustomerCccd
        {
            get { return customerCccd; }
            set
            {
                customerCccd = value;
                OnPropertyChanged();
            }
        }
        // Customer Email
        private string customerEmail;
        public string CustomerEmail
        {
            get { return customerEmail; }
            set
            {
                customerEmail = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmInfo { get; set; }
        public ICommand CancelInfo { get; set; }
        public CustomerManagementViewModel()
        {
            isConfirmed = false;
            ConfirmInfo = new RelayCommand<Window>((p) => 
            {
                return true; 
            }, (p) =>
            {
                if (string.IsNullOrEmpty(CustomerName) || string.IsNullOrEmpty(CustomerPhone))
                {
                    MessageBox.Show("Please fill in all the neccessary information");
                    return;
                }
                var customerList = DataProvider.Instance.DB.Customers.Where(x => x.CusPhone == CustomerPhone);
                if (customerList == null || customerList.Count() != 0)
                {
                    MessageBox.Show("This phone number has already been used");
                    return;
                }

                CustomerNumber = DataProvider.Instance.DB.Customers.Count();
                addNewCustomer(p, CustomerNumber + 1);
            });

            CancelInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        public void addNewCustomer(Window p, int cusNumber)
        {
            isConfirmed = true;
            NewCustomer = new Customer()
            {
                CusCode = $"KH{cusNumber:D3}",
                CusName = CustomerName,
                CusAddr = CustomerAddress,
                CusPhone = CustomerPhone,
                CusCccd = CustomerCccd,
                CusEmail = CustomerEmail,
                Isdeleted = false
            };

            CustomerName = "";
            CustomerAddress = "";
            CustomerPhone = "";
            CustomerCccd = "";
            CustomerEmail = "";

            p.Close();
        }
    }
}


