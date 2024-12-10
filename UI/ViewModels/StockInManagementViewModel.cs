using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using UI.Views;
using XAct;
using XAct.Entities.Implementations;

namespace RestaurantManager.ViewModels
{
    internal class StockInManagementViewModel : BaseViewModel
    {
        public bool isConfirmed { get; set; }
        public bool isEdited { get; set; }
        public bool IsEnable { get; set; }
        public int StockInNumber { get; set; }
        public bool isConfigured { get; set; }
        public int stockInDetailsManagementID { get; set; }
        // add new stockin => stockInDetailsManagementID = 0
        // enter an exited stockin => stockInDetailsManagementID = 1

        //-----------------------------------------

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

        //-----------------------------------------

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
        private object _selectedStockinDetails;

        public object SelectedStockinDetails
        {
            get { return _selectedStockinDetails; }
            set 
            { 
                _selectedStockinDetails = value;
                OnPropertyChanged();
            }
        }

        //-----------------------------------------

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

        //-----------------------------------------

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


        //-----------------------------------------

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
        private string _stockInDetailsQuantityType;

        public string StockInDetailsQuantityType
        {
            get { return _stockInDetailsQuantityType; }
            set 
            {
                _stockInDetailsQuantityType = value;
                OnPropertyChanged();
            }
        }

        //-----------------------------------------
        
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
        
        //-----------------------------------------

        private string _stockInDetailsQuantity;
        public string StockInDetailsQuantity
        {
            get { return _stockInDetailsQuantity; }
            set 
            { 
                _stockInDetailsQuantity = value; 
                OnPropertyChanged();
            }
        }
        private string _stockinDetailsCostPrice;

        public string StockinDetailsCostPrice
        {
            get { return _stockinDetailsCostPrice; }
            set 
            { 
                _stockinDetailsCostPrice = value; 
                OnPropertyChanged();
            }
        }
        private StockinDetailsIngre _selectedStockinDetailsIngre;

        public StockinDetailsIngre SelectedStockinDetailsIngre
        {
            get { return _selectedStockinDetailsIngre; }
            set 
            {
                _selectedStockinDetailsIngre = value; 
                OnPropertyChanged();
            }
        }
        private StockinDetailsDrinkOther _selectedStockinDetailsDrinkOther;

        public StockinDetailsDrinkOther SelectedStockinDetailsDrinkOther
        {
            get { return _selectedStockinDetailsDrinkOther; }
            set 
            { 
                _selectedStockinDetailsDrinkOther = value;
                OnPropertyChanged();
            }
        }


        private string _selectedStockinDetailsName;

        public string SelectedStockinDetailsName
        {
            get { return _selectedStockinDetailsName; }
            set
            {
                _selectedStockinDetailsName = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmInfo { get; set; }
        public ICommand CancelInfo { get; set; }
        public ICommand exitBtn { get; set; }
        public ICommand AddStockinDetailsCommand { get; set; }
        public ICommand DelStockinDetailsCommand { get; set; }
        public ICommand EditStockinCommand { get; set; }

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
        private IEnumerable<string> _selectedStockinDetailsNameList;

        public IEnumerable<string> SelectedStockinDetailsNameList
        {
            get { return _selectedStockinDetailsNameList; }
            set 
            { 
                _selectedStockinDetailsNameList = value; 
                OnPropertyChanged();
            }
        }  

        private Visibility _isItemVisibility;

        public Visibility isItemVisibility
        {
            get { return _isItemVisibility; }
            set 
            { 
                _isItemVisibility = value; 
                OnPropertyChanged();
            }
        }
        private Visibility _isIngreVisibility;
        public Visibility isIngreVisibility
        {
            get { return _isIngreVisibility; }
            set
            {
                _isIngreVisibility = value;
                OnPropertyChanged();
            }
        }
        public void UpdateStockInType()
        {
            if (SelectedIdxStockin == 0)
            {
                // Header
                StockInDetailsIDType = "Tên nguyên liệu";
                StockInDetailsQuantityType = "Số lượng nhập kho (kg)";
                stockInTypeIDHeader = "Tên nguyên liệu";
                QuantityTypeHeader = "Số lượng nhập kho";

                // Data
                SelectedStockinDetailsNameList = IngredientNameList;
                isItemVisibility = Visibility.Hidden;
                isIngreVisibility = Visibility.Visible;
            }
            else
            {
                // Header
                StockInDetailsIDType = "Tên mặt hàng";
                StockInDetailsQuantityType = "Số lượng nhập kho (Đơn vị)"; 
                stockInTypeIDHeader = "Tên mặt hàng";
                QuantityTypeHeader = "Số lượng nhập kho";

                // Data
                SelectedStockinDetailsNameList = MenuItemsNameList;
                isIngreVisibility = Visibility.Hidden;
                isItemVisibility = Visibility.Visible;
            }
        }
        public StockInManagementViewModel()
        {
            // Load data from database
            SelectedIdxStockin = 0;
            isConfirmed = false;
            stockInDetailsManagementID = 0;
            IsEnable = true;
            isConfigured = false;

            // Add Stockin
            ConfirmInfo = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    StockInNumber = DataProvider.Instance.DB.Stockins.Count();
                    addStockin(p, StockInNumber + 1);
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi! Vui lòng thử lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            });
            CancelInfo = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                isConfirmed = false;
                StockInID = "";
                StockInDate = DateTime.Now;
                p.Close();
            });

            // Stockin Details
            exitBtn = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MainWindow mainWindow = new MainWindow(); 
                var mainVM = mainWindow.DataContext as MainViewModel;
                if (mainVM != null)
                {
                    checkConfigured(mainVM);
                    if (isConfigured)
                    {
                        var dialogResult = MessageBox.Show("Bạn có muốn lưu thay đổi không?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            if (stockInDetailsManagementID == 0)
                            {
                                NewStockIn.StoDate = StockInDate;
                            }
                            else if (stockInDetailsManagementID == 1)
                            {
                                mainVM.SelectedStockin.StoDate = StockInDate;
                            }
                            DataProvider.Instance.DB.Stockins.Update(mainVM.SelectedStockin);
                            DataProvider.Instance.DB.SaveChanges();
                            mainVM.StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins);

                            SelectedStockinDetails = null;
                            StockinDetailsCostPrice = "";
                            StockInDetailsQuantity = "";
                            p.Close();
                        }
                        else if (dialogResult == MessageBoxResult.Cancel) { return; }
                        else 
                        {
                            SelectedStockinDetails = null;
                            StockinDetailsCostPrice = "";
                            StockInDetailsQuantity = "";
                            p.Close(); 
                        }
                    }
                    else 
                    {
                        SelectedStockinDetails = null;
                        StockinDetailsCostPrice = "";
                        StockInDetailsQuantity = "";
                        p.Close();
                    }
                }
            });

            // Add Stockin Details

            AddStockinDetailsCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (string.IsNullOrEmpty(StockInDetailsQuantity) || string.IsNullOrEmpty(StockinDetailsCostPrice))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                if (!int.TryParse(StockInID, out int stoId) ||
                    !double.TryParse(StockInDetailsQuantity, out double quantityKg) ||
                    !decimal.TryParse(StockinDetailsCostPrice, out decimal costPrice))
                {
                    MessageBox.Show("Dữ liệu không hợp lệ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (SelectedIdxStockin == 0)
                {
                    var isIngreNameExist = DataProvider.Instance.DB.StockinDetailsIngre
                        .FirstOrDefault(x => x.Ingre.IngreName == SelectedStockinDetailsName
                         && x.StoId == int.Parse(StockInID));

                    // Kiểm tra nếu nguyên liệu đã tồn tại
                    if (isIngreNameExist != null)
                    {
                        var diaResult = MessageBox.Show(
                            "Nguyên liệu đã tồn tại trong kho, bạn có muốn tiếp tục?",
                            "Thông báo",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (diaResult == MessageBoxResult.No)
                            return;

                        /// Cập nhật số lượng nguyên liệu
                        isIngreNameExist.QuantityKg = double.Parse(StockInDetailsQuantity);
                        isIngreNameExist.Cprice = int.Parse(StockinDetailsCostPrice);
                        DataProvider.Instance.DB.StockinDetailsIngre.Update(isIngreNameExist);
                    }

                    else
                    {
                        var getIngreElement = DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreName == SelectedStockinDetailsName);

                        if (getIngreElement == null)
                        {
                            MessageBox.Show("Nguyên liệu không tồn tại trong danh sách.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        NewIngreStockin = new StockinDetailsIngre
                        {
                            StoId = int.Parse(StockInID),
                            IngreId = getIngreElement.IngreId,
                            QuantityKg = double.Parse(StockInDetailsQuantity),
                            Cprice = decimal.Parse(StockinDetailsCostPrice)
                        };
                        StockInDetailsIngresList.Add(NewIngreStockin);
                        DataProvider.Instance.DB.StockinDetailsIngre.Add(NewIngreStockin);
                    }
                    DataProvider.Instance.DB.SaveChanges();

                    foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
                    {
                        entry.State = EntityState.Detached;
                    }

                    StockInDetailsIngresList = new ObservableCollection<StockinDetailsIngre>(
                        (from stkInDetailsIngre in DataProvider.Instance.DB.StockinDetailsIngre
                            join ingre in DataProvider.Instance.DB.Ingredients
                            on stkInDetailsIngre.IngreId equals ingre.IngreId
                            select new StockinDetailsIngre
                            {
                                Cprice = stkInDetailsIngre.Cprice,
                                IngreId = stkInDetailsIngre.IngreId,
                                QuantityKg = stkInDetailsIngre.QuantityKg,
                                StoId = stkInDetailsIngre.StoId,
                                Sto = stkInDetailsIngre.Sto,
                                Ingre = ingre // Gán dữ liệu từ bảng Ingredients
                            }).Where(x => x.Sto.StoCode == StockInCode));
                }

                else
                {
                    var isItemNameExist = DataProvider.Instance.DB.StockinDetailsDrinkOthers.FirstOrDefault(x => x.Item.ItemName == SelectedStockinDetailsName && x.StoId == stoId);

                    // Kiểm tra nếu nguyên liệu đã tồn tại
                    if (isItemNameExist != null)
                    {
                        var diaResult = MessageBox.Show(
                            "Mặt hàng đã tồn tại trong kho, bạn có muốn tiếp tục?",
                            "Thông báo",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (diaResult == MessageBoxResult.No)
                            return;

                        // Cập nhật số lượng nguyên liệu
                        isItemNameExist.QuantityUnits = int.Parse(StockInDetailsQuantity);
                        isItemNameExist.Cprice = int.Parse(StockinDetailsCostPrice);

                    }
                    else
                    {
                        var getItemElement = DataProvider.Instance.DB.MenuItems.Where(x => x.ItemName == SelectedStockinDetailsName).FirstOrDefault();

                        if (getItemElement == null)
                        {
                            MessageBox.Show("Mặt hàng không tồn tại trong danh sách.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        NewDrinkOtherStockin = new StockinDetailsDrinkOther
                        {
                            StoId = int.Parse(StockInID),
                            ItemId = getItemElement.ItemId,
                            QuantityUnits = int.Parse(StockInDetailsQuantity),
                            Cprice = decimal.Parse(StockinDetailsCostPrice)
                        };
                        StockInDetailsDrinkOtherList.Add(NewDrinkOtherStockin);
                        DataProvider.Instance.DB.StockinDetailsDrinkOthers.Add(NewDrinkOtherStockin);
                    }
                    DataProvider.Instance.DB.SaveChanges();

                    foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
                    {
                        entry.State = EntityState.Detached;
                    }

                    StockInDetailsDrinkOtherList = new ObservableCollection<StockinDetailsDrinkOther>(
                        (from stkInDetailsDrinkOther in DataProvider.Instance.DB.StockinDetailsDrinkOthers
                         join item in DataProvider.Instance.DB.MenuItems
                         on stkInDetailsDrinkOther.ItemId equals item.ItemId
                         select new StockinDetailsDrinkOther
                         {
                             Cprice = stkInDetailsDrinkOther.Cprice,
                             ItemId = stkInDetailsDrinkOther.ItemId,
                             QuantityUnits = stkInDetailsDrinkOther.QuantityUnits,
                             StoId = stkInDetailsDrinkOther.StoId,
                             Sto = stkInDetailsDrinkOther.Sto,
                             Item = item // Gán dữ liệu từ bảng MenuItems    
                         }).Where(x => x.Sto.StoCode == StockInCode));
                }

                
                // Will have a save button
                //IsEnable = true;
                //StockInID = "";
                //CostPrice = "";
                //StockInDate = DateTime.Now;
                //return;
            });
            DelStockinDetailsCommand = new RelayCommand<Window>((p) => SelectedStockinDetailsIngre != null || SelectedStockinDetailsDrinkOther != null, (p) =>
            {
                foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
                {
                    if (entry.Entity != SelectedStockinDetailsIngre && entry.Entity != SelectedStockinDetailsDrinkOther)
                    {
                        entry.State = EntityState.Detached;
                    }
                }
                if (SelectedIdxStockin == 0)
                {
                    DataProvider.Instance.DB.StockinDetailsIngre.Remove(SelectedStockinDetailsIngre);
                    StockInDetailsIngresList.Remove(SelectedStockinDetailsIngre);
                }
                else
                {
                    DataProvider.Instance.DB.StockinDetailsDrinkOthers.Remove(SelectedStockinDetailsDrinkOther);
                    StockInDetailsDrinkOtherList.Remove(SelectedStockinDetailsDrinkOther);
                }
                DataProvider.Instance.DB.SaveChanges();
            });
        }
        public void addStockin(Window p, int StkNumber)
        {
            isConfirmed = true;
            NewStockIn = new Stockin()
            {
                StoCode = $"ST{StkNumber:D6}",
                StoDate = StockInDate,
            };

            StockInDate = DateTime.Now;

            StockInID = "";
            StockInDate = DateTime.Now;
            
            p.Close();
        }
        public void checkConfigured(MainViewModel mainVM)
        {
            if (NewStockIn != null && StockInDate != NewStockIn.StoDate || StockInDate != mainVM.SelectedStockin.StoDate)
            {
                isConfigured = true;
            }
            else
            {
                isConfigured = false;
            }
        }
    }
}