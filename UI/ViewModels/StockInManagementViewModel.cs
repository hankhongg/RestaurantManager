using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
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
        private Stockin _editedStockIn;

        public Stockin EditedStockIn
        {
            get { return _editedStockIn; }
            set
            {
                _editedStockIn = value;
                OnPropertyChanged();
            }
        }
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
        private string _stockInDetailName;

        public string StockInDetailName
        {
            get { return _stockInDetailName; }
            set { _stockInDetailName = value; }
        }
        private string _stockInTypeIDHeader;

        public string stockInTypeIDHeader
        {
            get { return _stockInTypeIDHeader; }
            set 
            { 
                _stockInTypeIDHeader = value; 
                OnPropertyChanged();
            }
        }
        private string _quantityTypeHeader;

        public string QuantityTypeHeader
        {
            get { return _quantityTypeHeader; }
            set 
            {
                _quantityTypeHeader = value; 
                OnPropertyChanged();
            }
        }


        public ICommand ConfirmInfo { get; set; }
        public ICommand CancelInfo { get; set; }
        public ICommand exitBtn { get; set; }
        public ICommand AddDetails { get; set; }
        public ICommand DelDetails { get; set; }
        public ICommand EditDetails { get; set; }

        private ObservableCollection<StockinDetailsDrinkOther> _stockinDetailsDrinkOtherList;
        public ObservableCollection<StockinDetailsDrinkOther> StockInDetailsDrinkOtherList
        {
            get { return _stockinDetailsDrinkOtherList; }
            set
            {
                _stockinDetailsDrinkOtherList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StockinDetailsIngre> _stockinDetailsIngresList;
        public ObservableCollection<StockinDetailsIngre> StockInDetailsIngresList
        {
            get { return _stockinDetailsIngresList; }
            set
            {
                _stockinDetailsIngresList = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<object> _selectedTable;
        public IEnumerable<object> SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _IngredientNameList;

        public ObservableCollection<string> IngredientNameList
        {
            get { return _IngredientNameList; }
            set 
            {
                _IngredientNameList = value; 
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _menuItemsNameList;

        public ObservableCollection<string> MenuItemsNameList
        {
            get { return _menuItemsNameList; }
            set
            {
                _menuItemsNameList = value;
                OnPropertyChanged();
            }
        }
        private IEnumerable<string> _selectedStockinDetailsName;

        public IEnumerable<string> SelectedStockinDetailsName
        {
            get { return _selectedStockinDetailsName; }
            set 
            { 
                _selectedStockinDetailsName = value; 
                OnPropertyChanged();
            }
        }


        public void UpdateStockInType()
        {
            if (SelectedIdxStockin == 0)
            {
                // Header
                StockInDetailsIDType = "Tên nguyên liệu";                
                QuantityStockInType = "Số lượng nhập kho (kg)";
                stockInTypeIDHeader = "Tên nguyên liệu";
                QuantityTypeHeader = "Số lượng nhập kho";

                // Data
                SelectedTable = StockInDetailsIngresList;
                SelectedStockinDetailsName = IngredientNameList;
            }
            else
            {
                // Header
                StockInDetailsIDType = "Tên mặt hàng";
                QuantityStockInType = "Số lượng nhập kho (Đơn vị)"; 
                stockInTypeIDHeader = "Tên mặt hàng";
                QuantityTypeHeader = "Số lượng nhập kho";

                // Data
                SelectedTable = StockInDetailsDrinkOtherList;
                SelectedStockinDetailsName = MenuItemsNameList;

            }
        }
        public StockInManagementViewModel()
        {
            // Load data from database
            SelectedIdxStockin = 0;
            isConfirmed = false;
            isEdited = false;
            IsEnable = true;
            


            // Add Stockin
            ConfirmInfo = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    //if (string.IsNullOrEmpty(StockInDetailsID) || string.IsNullOrEmpty(StockInDetailsID))
                    //{
                    //    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    //    return;
                    //}
                    //var stockInList = from stockin in DataProvider.Instance.DB.Stockins
                    //                  join drinkOther in DataProvider.Instance.DB.StockinDetailsDrinkOthers
                    //                  on stockin.StoId equals drinkOther.StoId
                    //                  join ingre in DataProvider.Instance.DB.StockinDetailsIngres
                    //                  on stockin.StoId equals ingre.StoId
                    //                  where stockin.StoId == int.Parse(StockInID)
                    //                  select stockin;
                    var stockInList = DataProvider.Instance.DB.Stockins.Where(x => x.StoId == int.Parse(StockInID));

                    if (stockInList == null || stockInList.Count() != 0)
                    {
                        IsEnable = false;
                        isConfirmed = true;

                        /*
                        var isIngreIdExisted = DataProvider.Instance.DB.StockinDetailsIngre.Where(x => x.IngreId == int.Parse(StockInDetailsID));
                        var isDrinkOtherIdExisted = DataProvider.Instance.DB.StockinDetailsDrinkOthers.Where(x => x.ItemId == int.Parse(StockInDetailsID));

                        if (SelectedIdxStockin == 0)
                        {
                            if (isIngreIdExisted.Count() != 0 || isIngreIdExisted == null)
                            {
                                MessageBox.Show("Nguyên liệu đã tồn tại trong kho, vui lòng kiểm tra lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                return;
                            }
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
                            if (isDrinkOtherIdExisted.Count() != 0 || isDrinkOtherIdExisted == null)
                            {
                                MessageBox.Show("Mặt hàng đã tồn tại trong kho, vui lòng kiểm tra lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                return;
                            }
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
                    */
                    }
                    StockInNumber = DataProvider.Instance.DB.Stockins.Count();
                    if (managementID == 0)
                    {
                        addStockin(p, StockInNumber + 1);
                    }
                    else EditStockin(p);
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
            exitBtn = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });

            // Add Stockin Details
            
            AddDetails = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (string.IsNullOrEmpty(StockInDetailsID) || string.IsNullOrEmpty(StockInQuantity))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                if (SelectedIdxStockin == 0) 
                {
                    // Thiết lập dữ liệu bảng StockinDetailsIngre
                    SelectedTable = StockInDetailsIngresList.Where(x => x.StoId == 1);
                    if (StockInDetailsIngresList.Count() != 0 || StockInDetailsIngresList == null)
                    {
                        MessageBox.Show("Nguyên liệu đã tồn tại trong kho, vui lòng kiểm tra lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                    
                    NewIngreStockin = new StockinDetailsIngre()
                    {
                        IngreId = int.Parse(StockInDetailsID),
                        QuantityKg = StockInQuantity == null ? 0 : double.Parse(StockInQuantity),
                        Cprice = CostPrice == null ? 0 : decimal.Parse(CostPrice)
                    };
                }
                else
                {
                    // Thiết lập dữ liệu bảng StockinDetailsDrinkOther
                    SelectedTable = StockInDetailsDrinkOtherList.Where(x => x.StoId == 1);
                    
                    if (StockInDetailsDrinkOtherList.Count() != 0 || StockInDetailsDrinkOtherList == null)
                    {
                        MessageBox.Show("Mặt hàng đã tồn tại trong kho, vui lòng kiểm tra lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
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
                return;
            });
        }



        public void addStockin(Window p, int StkNumber)
        {
            isConfirmed = true;
            NewStockIn = new Stockin()
            {
                //StoId = StkNumber,
                StoCode = $"ST{StkNumber:D6}",
                StoDate = StockInDate,
                //StoPrice = decimal.Parse(StockInPrice),
            };

            StockInDate = DateTime.Now;

            StockInID = "";
            //StockInQuantity = "";
            //CostPrice = "";
            //StockInDetailsID = "";
            StockInDate = DateTime.Now;

            
            p.Close();
        }
        public void EditStockin(Window p)
        {
            isEdited = true;
            EditedStockIn = new Stockin()
            {
                StoCode = StockInCode,
                StoDate = StockInDate,
            };
            StockInDate = DateTime.Now;
            p.Close();
        }
        public void LoadStockin(Stockin CurrStockIn)
        {
            managementID = 1;
            StockInID = CurrStockIn.StoId.ToString();
            StockInCode = CurrStockIn.StoCode;
            StockInDate = CurrStockIn.StoDate;
        }

        // Add, edit Stockin Details



    }
}