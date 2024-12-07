using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using XAct.Entities.Implementations;

namespace RestaurantManager.ViewModels
{
    internal class StockInManagementViewModel : BaseViewModel
    {
        public int managementID { get; set; }
        public bool isConfirmed { get; set; }
        public bool isEdited { get; set; }
        public bool IsEnable { get; set; }
        public int StockInNumber { get; set; }
        private Stockin newStockIn;
        public Stockin NewStockIn
        {
            get { return newStockIn; }
            set
            {
                newStockIn = value;
                OnPropertyChanged();
            }
        }
        private StockinDetailsIngre newIngreStockin;

        public StockinDetailsIngre NewIngreStockin
        {
            get { return newIngreStockin; }
            set
            {
                newIngreStockin = value;
                OnPropertyChanged();
            }
        }
        private StockinDetailsDrinkOther newDrinkOtherStockin;

        public StockinDetailsDrinkOther NewDrinkOtherStockin
        {
            get { return newDrinkOtherStockin; }
            set
            {
                newDrinkOtherStockin = value;
                OnPropertyChanged();
            }
        }
        //private Stockin existedStockIn;

        //public Stockin ExistedStockIn
        //{
        //    get { return existedStockIn; }
        //    set
        //    {
        //        existedStockIn = value;
        //        OnPropertyChanged();
        //    }
        //}
        private int selectedIdxStockin;

        public int SelectedIdxStockin
        {
            get { return selectedIdxStockin; }
            set
            {
                selectedIdxStockin = value;
                OnPropertyChanged();
                UpdateStockInType();
            }
        }
        // ID, Code
        private string stockInCode;
        public string StockInCode
        {
            get { return stockInCode; }
            set { stockInCode = value; }
        }
        private string _stockInID; 
        public string StockInID // unchangeable   
        {
            get { return _stockInID; }
            set
            {
                _stockInID = value;
                OnPropertyChanged();
            }
        }
        private string _stockInDetailsID;

        public string StockInDetailsID // changeable    
        {
            get { return _stockInDetailsID; }
            set
            {
                _stockInDetailsID = value;
                OnPropertyChanged();
            }
        }
        private string _stockInDetailsIDType;

        public string StockInDetailsIDType
        {
            get { return _stockInDetailsIDType; }
            set 
            {
                _stockInDetailsIDType = value;
                OnPropertyChanged();    
            }
        }

        // Datetime
        private DateTime stockInDate = DateTime.Now;
        public DateTime StockInDate
        {
            get { return stockInDate; }
            set 
            {
                stockInDate = value; 
                OnPropertyChanged();
            }
        }
        private string _quantityStockInType;

        public string QuantityStockInType
        {
            get { return _quantityStockInType; }
            set 
            { 
                _quantityStockInType = value;
                OnPropertyChanged();
            }
        }
        private string _stockInQuantity;

        public string StockInQuantity

        {
            get { return _stockInQuantity; }
            set 
            { 
                _stockInQuantity = value;
                OnPropertyChanged();
            }
        }
        private string _costPrice;

        public string CostPrice
        {
            get { return _costPrice; }
            set 
            { 
                _costPrice = value;
                OnPropertyChanged();
            }
        }
        public ICommand ConfirmInfo { get; set; }
        public ICommand CancelInfo { get; set; }
        
        private void UpdateStockInType()
        {
            if (SelectedIdxStockin == 0)
            {
                StockInDetailsIDType = "Mã nguyên liệu";
                QuantityStockInType = "Số lượng nhập kho (kg)";
            }
            else
            {
                StockInDetailsIDType = "Mã mặt hàng";
                QuantityStockInType = "Số lượng nhập kho (Đơn vị)";
            }
        }
        public StockInManagementViewModel()
        {
            // Load data from database
            SelectedIdxStockin = 0;
            isConfirmed = false;
            isEdited = false;
            IsEnable = true;
            ConfirmInfo = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(StockInDetailsID) || string.IsNullOrEmpty(StockInDetailsID))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                    //var stockInList = from stockin in DataProvider.Instance.DB.Stockins
                    //                  join drinkOther in DataProvider.Instance.DB.StockinDetailsDrinkOthers
                    //                  on stockin.StoId equals drinkOther.StoId
                    //                  join ingre in DataProvider.Instance.DB.StockinDetailsIngres
                    //                  on stockin.StoId equals ingre.StoId
                    //                  where stockin.StoId == int.Parse(StockInID)
                    //                  select stockin;
                    var stockInList = DataProvider.Instance.DB.Stockins.Where(x => x.StoId == int.Parse(StockInID));

                    StockInNumber = DataProvider.Instance.DB.Stockins.Count();
                    if (stockInList == null || stockInList.Count() != 0)
                    {
                        managementID = 2;
                        IsEnable = false;
                        isConfirmed = true;
                        if (SelectedIdxStockin == 0)
                        {
                            NewIngreStockin = new StockinDetailsIngre()
                            {
                                StoId = int.Parse(StockInID),
                                IngreId = int.Parse(StockInDetailsID),
                                QuantityKg = StockInQuantity == null ? 0 : double.Parse(StockInQuantity),
                                Cprice = CostPrice == null ? 0 : decimal.Parse(CostPrice)
                            };
                        }
                        else
                        {
                            newDrinkOtherStockin = new StockinDetailsDrinkOther()
                            {
                                StoId = int.Parse(StockInID),
                                ItemId = int.Parse(StockInDetailsID),
                                QuantityUnits = StockInQuantity == null ? 0 : int.Parse(StockInQuantity),
                                Cprice = CostPrice == null ? 0 : decimal.Parse(CostPrice)
                            };
                        }
                        IsEnable = true;

                        StockInID = "";
                        StockInQuantity = "";
                        CostPrice = "";
                        StockInDetailsID = "";
                        StockInDate = DateTime.Now;
                        p.Close();
                        return;
                    }
                    if (managementID == 0)
                        addStockin(p, StockInNumber + 1);
                    else EditStockinDetails(p);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Sai định dạng giá nhập kho, vui lòng thử lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            });
            CancelInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                isConfirmed = false;
                isEdited = false;
                StockInID = "";
                StockInQuantity = "";
                CostPrice = "";
                StockInDetailsID = "";
                StockInDate = DateTime.Now;
                p.Close();
            });
        }
        public void addStockin(Window p, int StkNumber)
        {
            isConfirmed = true;
            NewStockIn = new Stockin()
            {
                StoCode = $"NV{StkNumber:D6}",
                StoDate = StockInDate,
                //StoPrice = decimal.Parse(StockInPrice),
            };

            if (SelectedIdxStockin == 0)
            {
                NewIngreStockin = new StockinDetailsIngre()
                {
                    StoId = StkNumber,
                    IngreId = int.Parse(StockInDetailsID),
                    QuantityKg = double.Parse(StockInQuantity),
                    Cprice = decimal.Parse(CostPrice)
                }; 
            }
            else
            {
                NewDrinkOtherStockin = new StockinDetailsDrinkOther()
                {
                    StoId = StkNumber,
                    ItemId = int.Parse(StockInDetailsID),
                    QuantityUnits = int.Parse(StockInQuantity),
                    Cprice = decimal.Parse(CostPrice)
                };
            }
            StockInDate = DateTime.Now;

            StockInID = "";
            StockInQuantity = "";
            CostPrice = "";
            StockInDetailsID = "";
            StockInDate = DateTime.Now;
            p.Close();
        }
        public void EditStockinDetails(Window p)
        {
            isEdited = true;

            StockInDate = DateTime.Now;
            p.Close();
        }
        public void LoadStockinDetailsInformation(Stockin CurrStockIn)
        {
            managementID = 1;
            StockInCode = CurrStockIn.StoCode;
            //StockInPrice = CurrStockIn.StoPrice.ToString();
            StockInDate = CurrStockIn.StoDate;
        }
    }
}