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
                ///
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
                if (_selectedStockinDetailsIngre != null)
                {
                    StockInDetailsQuantity = _selectedStockinDetailsIngre.QuantityKg.ToString();
                    StockinDetailsCostPrice = _selectedStockinDetailsIngre.Cprice.ToString();
                    if (_selectedStockinDetailsIngre.Ingre != null)
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
                    if (_selectedStockinDetailsDrinkOther.Item != null)
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

        public bool IngreCheckConfiguration { get; set; }
        public bool ItemCheckConfiguration { get; set; }

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

        public int InStockIngreCheck { get; set; }
        public int InStockItemCheck { get; set; }


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
            IngreCheckConfiguration = false;
            ItemCheckConfiguration = false;
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
                RefreshStockinList(mainVM);

                var tempStockin = DataProvider.Instance.DB.Stockins.FirstOrDefault(x => x.StoCode == StockInCode);
                if (mainVM != null && tempStockin != null)
                {
                    checkConfigured(tempStockin);
                    if (isConfigured)
                    {
                        var dialogResult = MessageBox.Show("Bạn có muốn lưu thay đổi không?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            tempStockin.StoDate = StockInDate;
                            DataProvider.Instance.DB.Stockins.Update(tempStockin);
                       
                            UpdateStockPrice(tempStockin);
                        }
                        else if (dialogResult == MessageBoxResult.Cancel) { return; }
                        SelectedStockinDetailsName = null;
                        StockinDetailsCostPrice = "";
                        StockInDetailsQuantity = "";
                        p.Close();
                        StockInDate = DateTime.Now;
                    }
                    else
                    {
                        UpdateStockPrice(tempStockin);

                        SelectedStockinDetails = null;
                        StockinDetailsCostPrice = "";
                        StockInDetailsQuantity = "";
                        p.Close();
                    }
                    RefreshStockinList(mainVM);
                    mainVM.StockinList = new ObservableCollection<Stockin>(DataProvider.Instance.DB.Stockins.ToList());
                }
            });

            // Add Stockin Details
            AddStockinDetailsCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                MainWindow mainWindow = new MainWindow();
                var mainVM = mainWindow.DataContext as MainViewModel;
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
                    var isIngreNameExist = DataProvider.Instance.DB.StockinDetailsIngres.Include(x => x.Ingre)
                        .FirstOrDefault(x => x.Ingre.IngreName == SelectedStockinDetailsName
                         && x.StoId == int.Parse(StockInID));

                    var getIngreElement = DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreName == SelectedStockinDetailsName);

                    IngreCheckConfiguration = true;

                    // Kiểm tra nếu nguyên liệu đã tồn tại
                    if (isIngreNameExist != null)
                    {
                        InStockIngreCheck = 0;
                        var diaResult = MessageBox.Show(
                            "Nguyên liệu đã tồn tại trong kho, bạn có muốn tiếp tục?",
                            "Thông báo",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (diaResult == MessageBoxResult.No)
                            return;

                        /// Cập nhật số lượng nguyên liệu
                        /// 
                        double temp = isIngreNameExist.QuantityKg;
                        isIngreNameExist.QuantityKg = double.Parse(StockInDetailsQuantity);
                        isIngreNameExist.Cprice = decimal.Parse(StockinDetailsCostPrice);
                        isIngreNameExist.Ingre.InstockKg -= temp - double.Parse(StockInDetailsQuantity);
                        isIngreNameExist.TotalCprice = decimal.Parse(StockinDetailsCostPrice) * decimal.Parse(StockInDetailsQuantity);
                        DataProvider.Instance.DB.StockinDetailsIngres.Update(isIngreNameExist);

                        DataProvider.Instance.DB.SaveChanges();

                        //using (var context = new QlnhContext())
                        //{
                        //    if (isIngreNameExist != null)
                        //    {
                        //        context.Entry(isIngreNameExist).State = EntityState.Detached;
                        //    }
                        //}
                    }

                    else
                    {
                        if (getIngreElement != null)
                        {
                            string totalPrice = (double.Parse(StockinDetailsCostPrice) * double.Parse(StockInDetailsQuantity)).ToString();

                            NewIngreStockin = new StockinDetailsIngre
                            {
                                StoId = int.Parse(StockInID),
                                IngreId = getIngreElement.IngreId,
                                QuantityKg = double.Parse(StockInDetailsQuantity),
                                Cprice = decimal.Parse(StockinDetailsCostPrice),
                                TotalCprice = decimal.Parse(totalPrice),
                            };
                            StockInDetailsIngresList.Add(NewIngreStockin);
                            DataProvider.Instance.DB.StockinDetailsIngres.Add(NewIngreStockin);
                            getIngreElement.InstockKg += NewIngreStockin.QuantityKg;
                            DataProvider.Instance.DB.Ingredients.Update(getIngreElement);


                            DataProvider.Instance.DB.SaveChanges();

                            //string query3 = @"
                            //        UPDATE INGREDIENTS
                            //        SET INSTOCK_KG = INSTOCK_KG + sub.TotalQuantity
                            //        FROM INGREDIENTS
                            //        INNER JOIN (
                            //            SELECT INGRE_ID, SUM(QUANTITY_KG) AS TotalQuantity
                            //            FROM STOCKIN_DETAILS_INGRE
                            //            GROUP BY INGRE_ID
                            //        ) sub
                            //        ON INGREDIENTS.INGRE_ID = sub.INGRE_ID
                            //        WHERE INGREDIENTS.INSTOCK_KG != sub.TotalQuantity;
                            //        ";

                            //DataProvider.Instance.DB.Database.ExecuteSqlRaw(query3);
                        }
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
                                 TotalCprice = stkInDetailsIngre.TotalCprice,
                                 Ingre = ingre // Gán dữ liệu từ bảng Ingredients
                             }).AsNoTracking().Where(x => x.Sto.StoCode == StockInCode));

                        RefreshStockinList(mainVM);

                    //mainVM.IngredientsList = new ObservableCollection<Ingredient>(
                    //DataProvider.Instance.DB.Ingredients.AsNoTracking().ToList());
                }

                else
                {
                    var isItemNameExist = DataProvider.Instance.DB.StockinDetailsDrinkOthers.FirstOrDefault(x => x.Item.ItemName == SelectedStockinDetailsName && x.StoId == stoId);
                    var getItemElement = DataProvider.Instance.DB.MenuItems.Where(x => x.ItemName == SelectedStockinDetailsName).FirstOrDefault();

                    ItemCheckConfiguration = true;

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
                        int temp = isItemNameExist.QuantityUnits;
                        isItemNameExist.QuantityUnits = int.Parse(StockInDetailsQuantity);
                        isItemNameExist.Item.Instock -= temp - int.Parse(StockInDetailsQuantity);
                        isItemNameExist.Cprice = decimal.Parse(StockinDetailsCostPrice);
                        isItemNameExist.TotalCprice = decimal.Parse(StockinDetailsCostPrice) * int.Parse(StockInDetailsQuantity);
                        DataProvider.Instance.DB.StockinDetailsDrinkOthers.Update(isItemNameExist);
                        
                        DataProvider.Instance.DB.SaveChanges();

                        //using (var context = new QlnhContext())
                        //{
                        //    if (isItemNameExist != null)
                        //    {
                        //        context.Entry(isItemNameExist).State = EntityState.Detached;
                        //    }
                        //}
                        //DataProvider.Instance.DB.Entry(isItemNameExist).Reload();
                    }
                    else
                    {
                        if (getItemElement != null)
                        {
                            NewDrinkOtherStockin = new StockinDetailsDrinkOther
                            {
                                StoId = int.Parse(StockInID),
                                ItemId = getItemElement.ItemId,
                                QuantityUnits = int.Parse(StockInDetailsQuantity),
                                Cprice = decimal.Parse(StockinDetailsCostPrice),
                                TotalCprice = decimal.Parse(StockinDetailsCostPrice) * int.Parse(StockInDetailsQuantity)
                            };

                            getItemElement.Instock += NewDrinkOtherStockin.QuantityUnits;
                            StockInDetailsDrinkOtherList.Add(NewDrinkOtherStockin);
                            DataProvider.Instance.DB.StockinDetailsDrinkOthers.Add(NewDrinkOtherStockin);
                            DataProvider.Instance.DB.MenuItems.Update(getItemElement);
                            DataProvider.Instance.DB.SaveChanges();
                            //string query4 = @"
                            //        UPDATE MENU_ITEMS
                            //        SET INSTOCK = sub.TotalQuantity
                            //        FROM MENU_ITEMS
                            //        INNER JOIN (
                            //            SELECT ITEM_ID, SUM(QUANTITY_UNITS) AS TotalQuantity
                            //            FROM STOCKIN_DETAILS_DRINK_OTHER
                            //            GROUP BY ITEM_ID
                            //        ) sub
                            //        ON MENU_ITEMS.ITEM_ID = sub.ITEM_ID
                            //        WHERE MENU_ITEMS.INSTOCK != sub.TotalQuantity;
                            //        ";

                            //DataProvider.Instance.DB.Database.ExecuteSqlRaw(query4);
                        }
                    }
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
                             TotalCprice = stkInDetailsDrinkOther.TotalCprice,
                             Item = item // Gán dữ liệu từ bảng MenuItems    
                         }).AsNoTracking().Where(x => x.Sto.StoCode == StockInCode));

                    RefreshStockinList(mainVM);

            });
            DelStockinDetailsCommand = new RelayCommand<Window>((p) => SelectedStockinDetailsIngre != null || SelectedStockinDetailsDrinkOther != null, (p) =>
            {
                MainWindow mainWindow = new MainWindow();
                var mainVM = mainWindow.DataContext as MainViewModel;

                var diaResult = MessageBox.Show("Bạn có chắc muốn xóa?\n" +
                                                "Nhấn 'Yes' để xóa số lượng có trong tồn kho.\n" +
                                                "Nhấn 'No' để giữ nguyên số lượng trong tồn kho.", "Thông báo", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (diaResult == MessageBoxResult.Cancel)
                    return;
                else if (diaResult == MessageBoxResult.Yes)
                    UpdateInStockAfterDelete();
                else
                {
                    foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
                    {
                        entry.State = EntityState.Detached;
                    }
                }

                if (SelectedIdxStockin == 0)
                {
                    IngreCheckConfiguration = true;
                    DataProvider.Instance.DB.StockinDetailsIngres.Remove(SelectedStockinDetailsIngre);
                    StockInDetailsIngresList.Remove(SelectedStockinDetailsIngre);
                    DataProvider.Instance.DB.SaveChanges();
                }
                else
                {
                    DataProvider.Instance.DB.StockinDetailsDrinkOthers.Remove(SelectedStockinDetailsDrinkOther);
                    ItemCheckConfiguration = true;
                    StockInDetailsDrinkOtherList.Remove(SelectedStockinDetailsDrinkOther);
                    DataProvider.Instance.DB.SaveChanges();

                }
                RefreshStockinList(mainVM);
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

            StockInID = "";


            p.Close();
        }
        public void checkConfigured(Stockin stk)
        {
            if (stk == null)
            {
                isConfigured = false;
                return;
            }
            if (NewStockIn != null && StockInDate != NewStockIn.StoDate && stockInDetailsManagementID == 0 ||
                stk.StoDate != null && StockInDate != stk.StoDate && stockInDetailsManagementID == 1)
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
            // Please detach all the attached entity
            foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            // Cập nhật lại danh sách Stockin
            mainVM.StockinList = new ObservableCollection<Stockin>(
                DataProvider.Instance.DB.Stockins.AsNoTracking().ToList());

            mainVM.IngredientsList = new ObservableCollection<Ingredient>(
                DataProvider.Instance.DB.Ingredients.AsNoTracking().ToList());

            mainVM.ItemsList = new ObservableCollection<MenuItem>(
                DataProvider.Instance.DB.MenuItems.AsNoTracking().ToList());

            /*
             * Xóa trạng thái theo dõi (EntityState.Detached)
                Nếu thiếu: Entity Framework có thể sử dụng các thực thể cũ được theo dõi (cached). Điều này khiến giao diện không được làm mới, ngay cả khi bạn cập nhật dữ liệu trong cơ sở dữ liệu.
                Tác động: Mặc dù cơ sở dữ liệu đã được cập nhật, giao diện vẫn hiển thị dữ liệu cũ.

             AsNoTracking() khi làm mới danh sách
                Nếu thiếu: Entity Framework có thể trả về dữ liệu được lưu trong bộ nhớ (cache) thay vì lấy dữ liệu mới nhất từ cơ sở dữ liệu.
                Tác động: Giao diện hiển thị thông tin lỗi thời, gây nhầm lẫn.
             */
        }
        public void UpdateExpenseFinancialHistory()
       {
            var stockin = DataProvider.Instance.DB.Stockins.FirstOrDefault(x => x.StoCode == StockInCode);
            if (stockin != null)
            {
                // Thêm mới FinancialHistory nếu chưa tồn tại stockin
                if (stockInDetailsManagementID == 0)
                {
                    FinancialHistory expenseFinancialHistory = new FinancialHistory
                    {
                        FinDate = stockin.StoDate,
                        Description = "Chi phí nhập kho",
                        Type = "EXPENSE",
                        Amount = stockin.StoPrice,
                        ReferenceId = stockin.StoId,
                        ReferenceType = "STOCKIN"
                    };
                    DataProvider.Instance.DB.FinancialHistories.Add(expenseFinancialHistory);
                }
                else
                {
                    var currFinancialHistory = DataProvider.Instance.DB.FinancialHistories.FirstOrDefault(x => x.ReferenceId == stockin.StoId);
                    if (currFinancialHistory != null)
                    {
                        currFinancialHistory.Amount = stockin.StoPrice;
                        currFinancialHistory.FinDate = stockin.StoDate;
                        DataProvider.Instance.DB.FinancialHistories.Update(currFinancialHistory);
                    }
                }
                DataProvider.Instance.DB.SaveChanges();
            }
        }
        public void UpdateStockPrice(Stockin stockin)
        {
            decimal totalCprice = 0;

            var stkIngreDetailsList = DataProvider.Instance.DB.StockinDetailsIngres.Where(x => x.StoId == stockin.StoId);
            var stkItemDetailsList = DataProvider.Instance.DB.StockinDetailsDrinkOthers.Where(x => x.StoId == stockin.StoId);

            if (stockin != null)
            {
                foreach (var stkIngreDetails in stkIngreDetailsList)
                {
                    totalCprice += stkIngreDetails.TotalCprice ?? 0;
                }
                for (int i = 0; i < stkItemDetailsList.Count(); i++)
                {
                    totalCprice += stkItemDetailsList.ElementAt(i).TotalCprice ?? 0;
                }
                stockin.StoPrice = totalCprice;
                DataProvider.Instance.DB.Stockins.Update(stockin);
                DataProvider.Instance.DB.SaveChanges();
            }
        }
        public void UpdateInStockAfterDelete()
        {
            if (SelectedIdxStockin == 0)
            {
                var totalInStock = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == SelectedStockinDetailsIngre.IngreId).FirstOrDefault();
                if (totalInStock != null)
                {
                    totalInStock.InstockKg = totalInStock.InstockKg - SelectedStockinDetailsIngre.QuantityKg;
                    DataProvider.Instance.DB.Ingredients.Update(totalInStock);
                }
            }
            else
            {
                // Cập nhật lại số lượng mặt hàng
                var totalInStock = DataProvider.Instance.DB.MenuItems.Where(x => x.ItemId == SelectedStockinDetailsDrinkOther.ItemId).FirstOrDefault();
                if (totalInStock != null)
                {
                    totalInStock.Instock = totalInStock.Instock - SelectedStockinDetailsDrinkOther.QuantityUnits;
                    DataProvider.Instance.DB.MenuItems.Update(totalInStock);
                }
            }
            DataProvider.Instance.DB.SaveChanges();

            foreach (var entry in DataProvider.Instance.DB.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}