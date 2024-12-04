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
    internal class EmployeeManagementViewModel : BaseViewModel
    {
        public int managementID { get; set; }
        public bool isConfirmed { get; set; }
        public bool isEdited { get; set; }
        public int EmployeeNumber { get; set; }
        private bool _isReadOnly;
        public bool isReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged();
            }
        }
        private Employee newEmployee;
        public Employee NewEmployee
        {
            get { return newEmployee; }
            set
            {
                newEmployee = value;
                OnPropertyChanged();
            }
        }
        private Employee editedEmployee;

        public Employee EditedEmployee
        {
            get { return editedEmployee; }
            set
            {
                editedEmployee = value;
                OnPropertyChanged();
            }
        }
        private string employeeCode;

        public string EmployeeCode
        {
            get { return employeeCode; }
            set 
            {
                employeeCode = value;
                OnPropertyChanged();
            }
        }

        private string employeeName;

        public string EmployeeName
        {
            get { return employeeName; }
            set
            {
                employeeName = value;
                OnPropertyChanged();
            }
        }
        private string employeePhone;
        public string EmployeePhone
        {
            get { return employeePhone; }
            set
            {
                employeePhone = value;
                OnPropertyChanged();
            }
        }
        private string employeeAddress;
        public string EmployeeAddress
        {
            get { return employeeAddress; }
            set
            {
                employeeAddress = value;
                OnPropertyChanged();
            }
        }
        private string employeeRole;
        public string EmployeeRole
        {
            get { return employeeRole; }
            set
            {
                employeeRole = value;
                OnPropertyChanged();
            }
        }
        private string employeeCccd;

        public string EmployeeCccd
        {
            get { return employeeCccd; }
            set { employeeCccd = value; }
        }

        private decimal employeeSalary;
        public decimal EmployeeSalary
        {
            get { return employeeSalary; }
            set
            {
                employeeSalary = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmInfo { get; set; }
        public ICommand CancelInfo { get; set; }
        public EmployeeManagementViewModel()
        {
            isConfirmed = false;
            isEdited = false;

            ConfirmInfo = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (string.IsNullOrEmpty(EmployeeName) || string.IsNullOrEmpty(EmployeePhone))
                {
                    MessageBox.Show("Please fill in all the neccessary information");
                    return;
                }
                var employeeList = DataProvider.Instance.DB.Employees.Where(x => x.EmpPhone == EmployeePhone);
                if (((employeeList == null || employeeList.Count() != 0) && managementID == 0))
                {
                    MessageBox.Show("This phone number has already been used");
                    return;
                }

                EmployeeNumber = DataProvider.Instance.DB.Employees.Count();
                if (managementID == 0)
                    addNewEmployee(p, EmployeeNumber + 1);
                else EditEmployee(p);
            });
            CancelInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        public void addNewEmployee(Window p, int EmpNumber)
        {
            isConfirmed = true;
            NewEmployee = new Employee()
            {
                EmpCode = $"NV{EmpNumber:D3}",
                EmpName = EmployeeName,
                EmpAddr = EmployeeAddress,
                EmpPhone = EmployeePhone,
                EmpCccd = EmployeeCccd,
                EmpSalary = EmployeeSalary,
                Isdeleted = false
            };

            EmployeeName = "";
            EmployeeAddress = "";
            EmployeePhone = "";
            EmployeeCccd = "";
            EmployeeRole = "";
            EmployeeSalary = 0;

            p.Close();
        }
        public void EditEmployee(Window p)
        {
            isEdited = true;
            EditedEmployee = new Employee()
            {
                EmpCode = EmployeeCode,
                EmpName = EmployeeName,
                EmpAddr = EmployeeAddress,
                EmpPhone = EmployeePhone,
                EmpCccd = EmployeeCccd,
                EmpSalary = EmployeeSalary,
                Isdeleted = false
            };

            EmployeeName = "";
            EmployeeAddress = "";
            EmployeePhone = "";
            EmployeeCccd = ""; ;

            p.Close();
        }
        public void LoadEmployeeInformation(Employee employee)
        {
            EmployeeCode = employee.EmpCode;
            EmployeeName = employee.EmpName;
            EmployeeAddress = employee.EmpAddr ?? "";
            EmployeePhone = employee.EmpPhone;
            EmployeeCccd = employee.EmpCccd ?? "";
            EmployeeRole = employee.EmpRole ?? "";
            EmployeeSalary = employee.EmpSalary;
        }
    }
}
