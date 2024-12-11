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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                StockinDetailsCostPrice = "";
                StockInDetailsQuantity = "";
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
                isReset = false;
                if (_selectedStockinDetailsIngre != null)
                {
                    StockInDetailsQuantity = _selectedStockinDetailsIngre.QuantityKg.ToString();
                    StockinDetailsCostPrice = _selectedStockinDetailsIngre.Cprice.ToString();
                    SelectedStockinDetailsName = _selectedStockinDetailsIngre.Ingre.IngreName;
                }
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
                if (_selectedStockinDetailsDrinkOther != null)
                {
                    StockInDetailsQuantity = _selectedStockinDetailsDrinkOther.QuantityUnits.ToString();
                    StockinDetailsCostPrice = _selectedStockinDetailsDrinkOther.Cprice.ToString();
                    SelectedStockinDetailsName = _selectedStockinDetailsDrinkOther.Item.ItemName;
                }

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
                if (SelectedIdxStockin == 0)
                {
                    var selectedStkDetailsIngre = DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreName == SelectedStockinDetailsName);
                    if (selectedStkDetailsIngre != null)
                        StockinDetailsCostPrice = selectedStkDetailsIngre.IngrePrice.ToString();
                }
                else if (SelectedIdxStockin == 1)
                {
                    var selectedStkDetailsDrinkOther = DataProvider.Instance.DB.MenuItems.FirstOrDefault(x => x.ItemName == SelectedStockinDetailsName);
                    if (selectedStkDetailsDrinkOther != null)
                        StockinDetailsCostPrice = selectedStkDetailsDrinkOther.ItemCprice.ToString();
                }
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmInfo { get; set; }
        public ICommand CancelInfo { get; set; }
        public ICommand exitBtn { get; set; }
        public ICommand AddStockinDetailsCommand { get; set; }
        public ICommand DelStockinDetailsCommand { get; set; }
        public ICommand EditStockinDetailsCommand { get; set; }

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

        public bool isReset { get; set; }

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
                    // Cập nhật STO_PRICE từ STOCKIN_DETAILS_DRINK_OTHER
                    string query1 = @"
                                UPDATE STOCKIN
                                SET STO_PRICE = (
                                    SELECT SUM(SI.CPRICE * SI.QUANTITY_KG)
                                    FROM STOCKIN_DETAILS_INGRE SI
                                    WHERE SI.STO_ID = STOCKIN.STO_ID
                                )
                                WHERE STO_ID IN (SELECT STO_ID FROM STOCKIN_DETAILS_INGRE);
                            ";
                    string query2 = @"
                                UPDATE STOCKIN
                                SET STO_PRICE = COALESCE(STO_PRICE, 0) + (
                                    SELECT SUM(SD.CPRICE * SD.QUANTITY_UNITS)
                                    FROM STOCKIN_DETAILS_DRINK_OTHER SD
                                    WHERE SD.STO_ID = STOCKIN.STO_ID
                                )
                                WHERE STO_ID IN (SELECT STO_ID FROM STOCKIN_DETAILS_DRINK_OTHER);
                            ";

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
                            if (mainVM.SelectedStockin != null)
                                DataProvider.Instance.DB.Stockins.Update(mainVM.SelectedStockin);
                            else DataProvider.Instance.DB.Stockins.Update(NewStockIn);

                            // Thực thi câu lệnh SQL đầu tiên
                            DataProvider.Instance.DB.Database.ExecuteSqlRaw(query1);

                            // Thực thi câu lệnh SQL thứ hai
                            DataProvider.Instance.DB.Database.ExecuteSqlRaw(query2);
                            DataProvider.Instance.DB.SaveChanges();

                            RefreshStockinList(mainVM);

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

                        // Thực thi câu lệnh SQL đầu tiên
                        DataProvider.Instance.DB.Database.ExecuteSqlRaw(query1);

                        // Thực thi câu lệnh SQL thứ hai
                        DataProvider.Instance.DB.Database.ExecuteSqlRaw(query2);

                        DataProvider.Instance.DB.SaveChanges();
                        RefreshStockinList(mainVM);


                        SelectedStockinDetails = null;
                        StockinDetailsCostPrice = "";
                        StockInDetailsQuantity = "";
                        p.Close();
                    }
                    mainVM.StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins.ToList());
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
                //else if ((!string.IsNullOrEmpty(StockInDetailsQuantity) || !string.IsNullOrEmpty(StockinDetailsCostPrice)) && isReset == false)
                //{
                //    isReset = true;
                //    StockInDetailsQuantity = null;
                //    StockinDetailsCostPrice = null;
                //    return;
                //}
                if (!int.TryParse(StockInID, out int stoId) ||
                    !double.TryParse(StockInDetailsQuantity, out double quantityKg) ||
                    !decimal.TryParse(StockinDetailsCostPrice, out decimal costPrice))
                {
                    MessageBox.Show("Dữ liệu không hợp lệ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (SelectedIdxStockin == 0)
                {
                    var isIngreNameExist = DataProvider.Instance.DB.StockinDetailsIngres
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
                        isIngreNameExist.Cprice = decimal.Parse(StockinDetailsCostPrice);
                        DataProvider.Instance.DB.StockinDetailsIngres.Update(isIngreNameExist);
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
                        DataProvider.Instance.DB.StockinDetailsIngres.Add(NewIngreStockin);
                    }
                    DataProvider.Instance.DB.SaveChanges();

                    foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
                    {
                        entry.State = EntityState.Detached;
                    }

                    StockInDetailsIngresList = new ObservableCollection<StockinDetailsIngre>(
                        (from stkInDetailsIngre in DataProvider.Instance.DB.StockinDetailsIngres
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
                        isItemNameExist.Cprice = decimal.Parse(StockinDetailsCostPrice);

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
                isReset = !isReset;
                
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
                    DataProvider.Instance.DB.StockinDetailsIngres.Remove(SelectedStockinDetailsIngre);
                    StockInDetailsIngresList.Remove(SelectedStockinDetailsIngre);
                }
                else
                {
                    DataProvider.Instance.DB.StockinDetailsDrinkOthers.Remove(SelectedStockinDetailsDrinkOther);
                    StockInDetailsDrinkOtherList.Remove(SelectedStockinDetailsDrinkOther);
                }
                DataProvider.Instance.DB.SaveChanges();
            });

            EditStockinDetailsCommand = new RelayCommand<Window>((p) => SelectedStockinDetailsIngre != null || SelectedStockinDetailsDrinkOther != null, (p) =>
            {

            } );
        }
        public void addStockin(Window p, int StkNumber)
        {
            isConfirmed = true;
            NewStockIn = new Stockin()
            {
                StoCode = $"ST{StkNumber:D6}",
                StoDate = StockInDate,
            };

            StockInID = "";
            
            
            p.Close();
        }
        public void checkConfigured(MainViewModel mainVM)
        {
            if (NewStockIn != null && StockInDate != NewStockIn.StoDate && stockInDetailsManagementID == 0 || 
                mainVM.SelectedStockin != null && StockInDate != mainVM.SelectedStockin.StoDate && stockInDetailsManagementID == 1)
            {
                isConfigured = true;
            }
            else
            {
                isConfigured = false;
            }
        }
        private void RefreshStockinList(MainViewModel mainVM)
        {
            // Xóa trạng thái theo dõi để tránh dữ liệu cũ
            foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            // Cập nhật lại danh sách Stockin
            mainVM.StockinList = new ObservableCollection<Stockin>(
                DataProvider.Instance.DB.Stockins.AsNoTracking().ToList());


            /*
             * Xóa trạng thái theo dõi (EntityState.Detached)
                Nếu thiếu: Entity Framework có thể sử dụng các thực thể cũ được theo dõi (cached). Điều này khiến giao diện không được làm mới, ngay cả khi bạn cập nhật dữ liệu trong cơ sở dữ liệu.
                Tác động: Mặc dù cơ sở dữ liệu đã được cập nhật, giao diện vẫn hiển thị dữ liệu cũ.

             AsNoTracking() khi làm mới danh sách
                Nếu thiếu: Entity Framework có thể trả về dữ liệu được lưu trong bộ nhớ (cache) thay vì lấy dữ liệu mới nhất từ cơ sở dữ liệu.
                Tác động: Giao diện hiển thị thông tin lỗi thời, gây nhầm lẫn.
             */
        }
    }
}