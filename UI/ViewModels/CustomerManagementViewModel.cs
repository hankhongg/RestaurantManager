﻿using System;
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
        public int managementID { get; set; }
        public bool isConfirmed { get; set; }
        public bool isEdited { get; set; }
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
        private Customer editedCustomer;

        public Customer EditedCustomer
        {
            get { return editedCustomer; }
            set
            {
                editedCustomer = value;
                OnPropertyChanged();
            }
        }
        private string customerCode;

        public string CustomerCode
        {
            get { return customerCode; }
            set { customerCode = value; }
        }
        // Customer Name
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
            get{ return customerCccd; }
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
        private bool _isReadOnly = false;
        public bool isReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged();
            }
        }
        private bool _isDeleted = false;

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
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
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                var customerList = DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false && x.CusPhone == CustomerPhone);
                if (((customerList == null || customerList.Count() != 0) && managementID == 0))
                {
                    MessageBox.Show("Số điện thoại này đã tồn tại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }

                CustomerNumber = DataProvider.Instance.DB.Customers.Where(x => x.Isdeleted == false).Count();
                if (managementID == 0)
                    addNewCustomer(p, CustomerNumber + 1);
                else EditCustomer(p);
            });

            CancelInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                isConfirmed = false;
                isEdited = false;
                isReadOnly = false;
                CustomerName = "";
                CustomerAddress = "";
                CustomerPhone = "";
                CustomerCccd = "";
                CustomerEmail = "";
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
                Isdeleted = IsDeleted
            };

            CustomerName = "";
            CustomerAddress = "";
            CustomerPhone = "";
            CustomerCccd = "";
            CustomerEmail = "";

            p.Close();
        }
        public void EditCustomer(Window p)
        {
            isEdited = true;
            EditedCustomer = new Customer()
            {
                CusCode = CustomerCode,
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
        public void LoadCustomerInformation(Customer currCus)
        {
            // load customer information from customerlayoutwindow
            isReadOnly = true;
            managementID = 1;
            CustomerCode = currCus.CusCode;
            CustomerName = currCus.CusName;
            CustomerPhone = currCus.CusPhone == null ? "" : currCus.CusPhone;
            CustomerCccd = currCus.CusCccd == null ? "" : currCus.CusCccd;
            CustomerEmail = currCus.CusEmail == null ? "" : currCus.CusEmail;
            CustomerAddress = currCus.CusAddr == null ? "" : currCus.CusAddr;
        }
    }
}


