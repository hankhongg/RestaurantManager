using Microsoft.IdentityModel.Tokens;
using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using RestaurantManager.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Primitives;
using MenuItem = RestaurantManager.Models.MenuItem;


namespace RestaurantManager.ViewModels
{
    class FoodLayoutViewModel : BaseViewModel
    {
        public enum MenuItemType
        {
            FOOD,
            DRINK,
            OTHER,
            All
        }
        public ICommand NewDishCommand { get; set; }

        public RelayCommand<object> FoodItemCommand { get; private set; }
        public ICommand FoodItemComand { get; set; }

        public ICommand EditDishComand { get; set; }
        public RelayCommand<object> EditDishCommand { get; private set; }

        public ICommand ExitCommand { get; set; }

        public ICommand SaveOrderCommand { get; set; }

        public ICommand SaveTemporaryInvoiceCommand { get; set; }
        public RelayCommand<BillUCViewModel> DeleteBillCommand { get; private set; }
        public RelayCommand<object> IncreaseQuantityCommand { get; private set; }
        public RelayCommand<object> DecreaseQuantityCommand { get; private set; }
        public ICommand FilterByFoodCommand { get; set; }
        public ICommand FilterByDrinkCommand { get; set; }
        public ICommand FilterByOthersCommand { get; set; }
        public RelayCommand<object> FilterByAllCommand { get; }
        public ICommand FilterAllCommand { get; set; }

        public ICommand AddBillCommand { get; }

        private ObservableCollection<BillUCViewModel> _bill;
        public ObservableCollection<BillUCViewModel> Bills
        {
            get { return _bill; }
            set
            {
                _bill = value;
                OnPropertyChanged(nameof(Bills));
            }
        }

        // Danh sách các món ăn
        private ObservableCollection<FoodItemUCViewModel> foodItemUCViewModels;
        public ObservableCollection<FoodItemUCViewModel> FoodItemUCViewModels
        {
            get { return foodItemUCViewModels; }
            set
            {
                foodItemUCViewModels = value;
                OnPropertyChanged(nameof(FoodItemUCViewModels));
            }
        }
        private ObservableCollection<MenuItem> menuFoodItems;
        public ObservableCollection<MenuItem> MenuFoodItems
        {
            get { return menuFoodItems; }
            set
            {
                menuFoodItems = value;
                OnPropertyChanged(nameof(MenuFoodItems));
            }
        }

        // emp list
        private ObservableCollection<Employee> empList;
        public ObservableCollection<Employee> EmpList
        {
            get { return empList; }
            set
            {
                empList = value;
                OnPropertyChanged(nameof(EmpList));
            }
        }

        private ObservableCollection<int> empIdList;
        public ObservableCollection<int> EmpIdList
        {
            get { return empIdList; }
            set
            {
                empIdList = value;
                OnPropertyChanged(nameof(EmpIdList));
            }
        }

        private byte? inputTabNum;
        public byte? InputTabNum
        {
            get { return inputTabNum; }
            set
            {
                inputTabNum = value;
                OnPropertyChanged(nameof(InputTabNum));
            }
        }



        // selected
        private int? selectedEmpId = -1;
        public int? SelectedEmpId
        {
            get { return selectedEmpId; }
            set
            {
                selectedEmpId = value;
                OnPropertyChanged(nameof(SelectedEmpId));
            }
        }

        private byte? selectedTabNum = null;
        public byte? SelectedTabNum
        {
            get { return selectedTabNum; }
            set
            {
                selectedTabNum = value;
                OnPropertyChanged(nameof(SelectedTabNum));
            }
        }
        private ObservableCollection<byte?> tabsNum;
        public ObservableCollection<byte?> TabsNum
        {
            get { return tabsNum; }
            set
            {
                tabsNum = value;
                OnPropertyChanged(nameof(TabsNum));
            }
        }

        private Receipt inputReceipt;
        public Receipt InputReceipt
        {
            get { return inputReceipt; }
            set
            {
                inputReceipt = value;
                OnPropertyChanged(nameof(InputReceipt));
            }
        }



        public void LoadMenuItems()
        {
            MenuFoodItems = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems.Where(x => x.Isdeleted == false));
            FoodItemUCViewModels = new ObservableCollection<FoodItemUCViewModel>(
                MenuFoodItems.Select(item =>
                {
                    var foodItemViewModel = new FoodItemUCViewModel();
                    foodItemViewModel.SetFoodItemData(item.ItemId, item.ItemName, item.ItemImg, item.ItemSprice);
                    foodItemViewModel.FoodItemType = item.ItemType;
                    return foodItemViewModel;
                })
            );
            OnPropertyChanged(nameof(FoodItemUCViewModels));
            //MenuItems = new ObservableCollection<FoodItemUCViewModel>(
            //    items.Select(item =>
            //    {
            //        var foodItemViewModel = new FoodItemUCViewModel();
            //        foodItemViewModel.SetFoodItemData(item.ItemName, item.ItemImg, item.ItemSprice);
            //        foodItemViewModel.ItemType = item.ItemType;
            //        return foodItemViewModel;
            //    })
            //);

            // Cập nhật FilteredMenuItems
            FilteredMenuItems = CollectionViewSource.GetDefaultView(FoodItemUCViewModels);

            FoodItemUCViewModels.CollectionChanged += (s, e) => OnPropertyChanged(nameof(FoodItemUCViewModels));
        }

        public void AddBill(FoodItemUCViewModel selectedFoodItemBill)
        {
            if (selectedFoodItemBill == null) return;
            //

            if (Bills == null)
            {
                Bills = new ObservableCollection<BillUCViewModel>();
            }

            var existingBill = Bills.FirstOrDefault(b => b.ItemName == selectedFoodItemBill.FoodItemName);
            if (existingBill != null)
            {
                // Nếu món ăn đã tồn tại, hiển thị thông báo
                MessageBox.Show($"Đã thêm món: {selectedFoodItemBill.FoodItemName} vào hóa đơn");
                return;
            }
            else
            {
                // Tìm giá trị RecId không trùng
                //  var usedRecIds = Bills.Select(b => b.RecId).ToHashSet(); // Tập hợp các RecId đã dùng
                //  int newRecId = Enumerable.Range(0, 100).FirstOrDefault(id => !usedRecIds.Contains(id));

                // Nếu món ăn chưa tồn tại, thêm hóa đơn mới
                var newBill = new BillUCViewModel
                {
                    STT = Bills.Count + 1,
                    // RecId = newRecId, // Giá trị RecId mới
                    ItemName = selectedFoodItemBill.FoodItemName,
                    Quantity = 1,
                    ItemSprice = selectedFoodItemBill.FoodItemSprice,
                    Price = selectedFoodItemBill.FoodItemSprice,
                    Isdeleted = 0
                };

                Bills.Add(newBill);
            }

            OnPropertyChanged(nameof(Bills));
        }

        public void LoadOrderInformation(Receipt selectedReceipt)
        {
            int count = 1;
            // các hóa đơn
            ObservableCollection<ReceiptDetail> receiptDetails = new ObservableCollection<ReceiptDetail>(
                DataProvider.Instance.DB.ReceiptDetails
                .Where(rd => rd.RecId == selectedReceipt.RecId)
                .ToList()
            );
            Bills = new ObservableCollection<BillUCViewModel>(
                receiptDetails.Select(rd =>
                {
                    var menuItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemId == rd.ItemId);
                    return new BillUCViewModel
                    {
                        RecId = rd.RecId,
                        STT = count++,
                        ItemName = menuItem.ItemName,
                        Quantity = rd.Quantity,
                        ItemSprice = menuItem.ItemSprice,
                        Price = rd.Quantity * menuItem.ItemSprice,
                        Isdeleted = 0
                    };
                })
            );

            // load LẠI 1 bàn và 1 nhân viên ĐÃ CHỌN
            SelectedTabNum = DataProvider.Instance.DB.DiningTables.Where(t => t.TabId == selectedReceipt.TabId).FirstOrDefault().TabNum;
            SelectedEmpId = selectedReceipt.EmpId;
            TotalAmount = Bills.Sum(item => item.Price);


        }

        private bool isEditing = false;
        public bool IsEditing
        {
            get => isEditing;
            set
            {
                isEditing = value;
                OnPropertyChanged();
            }
        }

        // Món ăn được chọn
        private FoodItemUCViewModel _selectedFoodItem;
        public FoodItemUCViewModel SelectedFoodItem
        {
            get => _selectedFoodItem;
            set
            {
                _selectedFoodItem = value;
                OnPropertyChanged();
                // Khi thay đổi SelectedFoodItem, các nút tự động cập nhật trạng thái
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private FoodItemUCViewModel _selectedFoodItemBill;
        public FoodItemUCViewModel SelectedFoodItemBill
        {
            get => _selectedFoodItemBill;
            set
            {
                _selectedFoodItemBill = value;
                OnPropertyChanged();
                // Khi thay đổi SelectedFoodItem, các nút tự động cập nhật trạng thái
                CommandManager.InvalidateRequerySuggested();
            }
        }


        // Command xử lý nhấp chuột phải vào món ăn
        public ICommand RightClickCommand { get; set; }
        public ICommand MiddleClickCommand { get; set; }


        // Command xử lý Xóa món ăn
        public ICommand DeleteDishCommand { get; set; }


        // Command khi nhấn chuột xuống
        public ICommand MouseDownCommand { get; set; }

        private MenuItemType _selectedMenuItemType = MenuItemType.All;
        public MenuItemType SelectedMenuItemType
        {
            get => _selectedMenuItemType;
            set
            {
                _selectedMenuItemType = value;
                OnPropertyChanged();
                FilterMenuItems();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterMenuItems();
            }
        }
        private string _searchTextEm;
        public string SearchTextEm
        {
            get { return _searchTextEm; }
            set
            {
                if (_searchTextEm != value)
                {
                    _searchTextEm = value;
                    OnPropertyChanged(nameof(SearchTextEm));
                }
            }
        }

        private ICollectionView _filteredMenuItems;
        public ICollectionView FilteredMenuItems
        {
            get => _filteredMenuItems;
            set
            {
                _filteredMenuItems = value;
                OnPropertyChanged(nameof(FilteredMenuItems));
            }
        }

        private decimal _totalAmount;

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }



        private void FilterMenuItems()
        {
            if (FilteredMenuItems == null)
                return;

            FilteredMenuItems.Filter = obj =>
            {
                if (obj is FoodItemUCViewModel menuItem)
                {
                    bool matchesType = SelectedMenuItemType == MenuItemType.All ||
                                       menuItem.ItemTypeEnum == SelectedMenuItemType;
                    bool matchesSearch = string.IsNullOrWhiteSpace(SearchText) ||
                                         menuItem.FoodItemName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

                    return matchesType && matchesSearch;
                }
                return false;
            };
            FilteredMenuItems.Refresh();
        }
        public void LoadBills()
        {
            var billsFromDb = Bills.ToList(); // Load từ database
            Bills = new ObservableCollection<BillUCViewModel>(
                billsFromDb.Select(bill => new BillUCViewModel
                {
                    RecId = bill.RecId,
                    STT = bill.STT,
                    ItemName = bill.ItemName,
                    Quantity = bill.Quantity,
                    ItemSprice = bill.ItemSprice,
                    Price = bill.Price,
                    Isdeleted = 0
                }).Where(b => b.Isdeleted == 0) // Chỉ load những bill chưa bị xóa
            );
        }

        public FoodLayoutViewModel()
        {
            LoadMenuItems();
            LoadDiningTables();
            LoadEmpList();

            FilterByFoodCommand = new RelayCommand<object>((p) => true, (p) => SelectedMenuItemType = MenuItemType.FOOD);
            FilterByDrinkCommand = new RelayCommand<object>((p) => true, (p) => SelectedMenuItemType = MenuItemType.DRINK);
            FilterByOthersCommand = new RelayCommand<object>((p) => true, (p) => SelectedMenuItemType = MenuItemType.OTHER);
            FilterByAllCommand = new RelayCommand<object>((p) => true, (p) => SelectedMenuItemType = MenuItemType.All);

            NewDishCommand = new RelayCommand<object>((p) => { return true; }, (p) => {

                AddItemWindow addItemWindow = new AddItemWindow();
                var itemVM = new MenuItemsManagement();
                if (itemVM != null)
                {
                    itemVM.SelectedIdxType = 0;
                    itemVM.IsNotEditing = true;
                    itemVM.LoadBlankRecipeInformation();
                    itemVM.LoadBlankDrinkInformation();
                    itemVM.LoadBlankOtherInformation();
                    itemVM.ItemID = -1;
                    addItemWindow.DataContext = itemVM;
                    addItemWindow.ShowDialog();
                }
                //ItemsList = new ObservableCollection<MenuItem>(DataProvider.Instance.DB.MenuItems); // load lại list menu item
                LoadMenuItems();
            });
            FoodItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                FoodItemWindow wd = new FoodItemWindow();
                wd.ShowDialog();
            });

            ExitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                // Đóng cửa sổ hiện tại
                foreach (var window in Application.Current.Windows)
                {
                    if (window is FoodLayoutWindow)
                    {
                        (window as FoodLayoutWindow).Close();
                        break;
                    }
                }

            });

            Bills = new ObservableCollection<BillUCViewModel>();

            MouseDownCommand = new RelayCommand<FoodItemUCViewModel>(
                (p) => p != null, // CanExecute
                (p) =>
                {
                    SelectedFoodItemBill = p;
                    AddBill(SelectedFoodItemBill);

                    LoadMenuItems();

                    //  MessageBox.Show("Danh sách món ăn đã được làm mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);


                    LoadBills();

                    //    MessageBox.Show("Danh sách bill đã được làm mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                });

            // chọn món ăn
            RightClickCommand = new RelayCommand<FoodItemUCViewModel>((p) => p != null, // CanExecute
               (p) =>
               {
                   // Deselect tất cả các món ăn trước đó
                   foreach (var item in FoodItemUCViewModels)
                   {
                       item.IsSelected = false;
                   }

                   // Đánh dấu món ăn hiện tại là được chọn
                   p.IsSelected = true;
                   SelectedFoodItem = p; // Cập nhật món ăn được chọn
                                         // Reload danh sách MenuItems từ SQL

               });

            DeleteDishCommand = new RelayCommand<object>(
                (p) => SelectedFoodItem != null, // CanExecute
                (p) =>
                {
                    MenuItem SelectedItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemId == SelectedFoodItem.FoodItemId);
                    var item = DataProvider.Instance.DB.MenuItems.Where(x => x.ItemId == SelectedItem.ItemId).FirstOrDefault();
                    var DialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (item != null && DialogResult == MessageBoxResult.Yes)
                    {
                        DataProvider.Instance.DB.MenuItems.Remove(item);
                        DataProvider.Instance.DB.SaveChanges();
                        LoadMenuItems();
                    }

                });

            EditDishCommand = new RelayCommand<object>(
                (p) => SelectedFoodItem != null, // CanExecute
                (p) =>
                {
                    AddItemWindow addItemWindow = new AddItemWindow();
                    //var itemVM = addItemWindow.DataContext as MenuItemsManagement;
                    MenuItem SelectedItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemId == SelectedFoodItem.FoodItemId);
                    var itemVM = new MenuItemsManagement();
                    if (itemVM != null)
                    {
                        if (SelectedItem.ItemType == "FOOD")
                        {
                            itemVM.SelectedIdxType = 0;
                            itemVM.LoadRecipeInformation(SelectedItem.ItemId);
                        }
                        else if (SelectedItem.ItemType == "DRINK")
                        {
                            itemVM.SelectedIdxType = 1;
                            itemVM.LoadDrinkInformation(SelectedItem.ItemId);
                        }
                        else
                        {
                            itemVM.SelectedIdxType = 2;
                            itemVM.LoadOtherInformation(SelectedItem.ItemId);
                        }
                        itemVM.IsNotEditing = false;
                        itemVM.ItemID = SelectedItem.ItemId;
                        addItemWindow.DataContext = itemVM;
                        addItemWindow.ShowDialog();
                    }
                    LoadMenuItems();
                    SelectedFoodItem.IsSelected = false;
                    SelectedFoodItem = null; // Xóa xong thì bỏ chọn
                });



            SaveOrderCommand = new RelayCommand<object>(
            (p) => Bills != null && Bills.Count > 0 && SelectedEmpId != -1 && SelectedTabNum != null,
            (p) =>
            {
                // Tính tổng tiền
                TotalAmount = Bills.Sum(item => item.Price);
                DataProvider.Instance.DB.DiningTables.Where(t => t.TabNum == SelectedTabNum).FirstOrDefault().TabStatus = false;
                // Hiển thị tổng tiền
                // MessageBox.Show($"Tổng tiền các món ăn: {TotalAmount}", "Thông báo");
            });



            SaveTemporaryInvoiceCommand = new RelayCommand<Window>((p) =>
            {
                // Điều kiện để nút "Lưu" khả dụng
                return Bills != null && Bills.Count > 0 && SelectedEmpId != -1 && SelectedTabNum != null;
            }, (p) =>
            {

                if (isEditing == false) // thêm hóa đơn
                {
                    bool corrupted = false;
                    var billCopy = new ObservableCollection<BillUCViewModel>(Bills);
                    foreach (var billTest in billCopy)
                    {
                        var menuItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemName == billTest.ItemName);
                        if (menuItem.ItemType == "FOOD")
                        {
                            var recipes = DataProvider.Instance.DB.Recipes.Where(r => r.ItemId == menuItem.ItemId).ToList();
                            if (menuItem.Instock < billTest.Quantity)
                            {
                                Bills.Remove(billTest);
                                string announce = $"Không đủ nguyên liệu cho món: {billTest.ItemName}!\nSố lượng nguyên liệu còn lại cho món ăn là:\n";
                                foreach (var recipe in recipes)
                                {
                                    var ingredient = DataProvider.Instance.DB.Ingredients.FirstOrDefault(i => i.IngreId == recipe.IngreId);
                                    announce += $"\n{ingredient.IngreName}: {ingredient.InstockKg}";
                                }
                                MessageBox.Show(announce, "Thông báo!", MessageBoxButton.OK, MessageBoxImage.Information);
                                corrupted = true;
                                continue;
                            }
                            else menuItem.Instock -= billTest.Quantity;
                        }
                        else // cho drink và other
                        {
                            if (menuItem.Instock < billTest.Quantity)
                            {
                                Bills.Remove(billTest);
                                MessageBox.Show($"Không đủ số lương sản phẩm cho món: {billTest.ItemName}!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                MessageBox.Show($"Số lượng sản phẩm còn lại: {menuItem.Instock}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                corrupted = true;
                                continue;
                            }
                            else menuItem.Instock -= billTest.Quantity;
                        }
                    }



                    if (corrupted)
                    {
                        MessageBox.Show("Hóa đơn không thể lưu do thiếu nguyên liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    DataProvider.Instance.DB.DiningTables.Where(t => t.TabNum == SelectedTabNum).FirstOrDefault().TabStatus = false;
                    int selectedTabId = DataProvider.Instance.DB.DiningTables.Where(t => t.TabNum == SelectedTabNum).FirstOrDefault().TabId;
                    string invoiceNumber = "HD" + (DataProvider.Instance.DB.Receipts.Where(r => r.Isdeleted == false).Count() + 1).ToString("D3");  // Tạo mã hóa đơn, như HD001, HD002, ...
                    DateTime currentTime = DateTime.Now;  // Thời gian hiện tại
                    TotalAmount = Bills.Sum(item => item.Price);
                    decimal totalAmount = TotalAmount;  // Tính tổng tiền từ danh sách Bills

                    var receipt = new Receipt()
                    {
                        RecCode = invoiceNumber,  // Mã hóa đơn                        
                        RecTime = currentTime,     // Thời gian tạo hóa đơn
                        RecPay = totalAmount,      // Tổng tiền (số tiền phải trả)
                        Isdeleted = false,     // Trạng thái xóa (đang lưu trữ)
                        TabId = selectedTabId,
                        EmpId = SelectedEmpId,

                    };

                    try
                    {
                        // Lưu Receipt và lấy giá trị RecId
                        DataProvider.Instance.DB.Receipts.Add(receipt);
                        DataProvider.Instance.DB.SaveChanges();

                        // MessageBox.Show($"RecId của Receipt: {receipt.RecId}", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);

                        foreach (var bill in Bills)
                        {
                            var menuItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemName == bill.ItemName);
                            if (menuItem == null)
                            {
                                MessageBox.Show($"Không tìm thấy món: {bill.ItemName} trong MenuItems!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                continue;
                            }
                            var existingRecId = DataProvider.Instance.DB.Receipts
                            .OrderByDescending(r => r.RecId)
                            .Select(r => r.RecId)
                            .FirstOrDefault();
                            var receiptDetail = new ReceiptDetail()
                            {
                                RecId = existingRecId, // Dùng RecId thực tế
                                ItemId = menuItem.ItemId,
                                // ItemName = bill.ItemName,
                                Quantity = bill.Quantity,
                                Price = bill.ItemSprice,
                            };
                            DataProvider.Instance.DB.ReceiptDetails.Add(receiptDetail);
                        }

                        // Lưu ReceiptDetails
                        DataProvider.Instance.DB.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    MessageBox.Show($"Hóa đơn đã lưu thành công!\nSố hóa đơn: {receipt.RecCode}",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    p.Close();
                }
                else // sửa hóa đơn
                {
                    TotalAmount = Bills.Sum(item => item.Price);
                    if (inputReceipt != null)
                    {
                        bool corrupted = false;
                        if (SelectedTabNum != InputTabNum)
                        {
                            DiningTable inputTable = DataProvider.Instance.DB.DiningTables.FirstOrDefault(t => t.TabNum == InputTabNum);
                            if (inputTable != null)
                            {
                                inputTable.TabStatus = true;
                            }
                            DiningTable selectedTable = DataProvider.Instance.DB.DiningTables.FirstOrDefault(t => t.TabNum == SelectedTabNum);
                            if (selectedTable != null)
                            {
                                selectedTable.TabStatus = false;
                            }
                            DataProvider.Instance.DB.SaveChanges();
                        }

                        ObservableCollection<ReceiptDetail> existedReceiptDetails = new ObservableCollection<ReceiptDetail>(
                            DataProvider.Instance.DB.ReceiptDetails
                            .Where(rd => rd.RecId == inputReceipt.RecId)
                            .ToList()
                        );
                        ObservableCollection<BillUCViewModel> existedBills = new ObservableCollection<BillUCViewModel>(
                            Bills
                            .Where(b => b.RecId == inputReceipt.RecId)
                            .ToList()
                        );

                        foreach (var bill in Bills)
                        {
                            var menuItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemName == bill.ItemName);
                            if (menuItem == null)
                            {
                                MessageBox.Show($"Không tìm thấy món: {bill.ItemName} trong MenuItems!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                continue;
                            }
                            var existedReceiptDetail = existedReceiptDetails.FirstOrDefault(rd => rd.ItemId == menuItem.ItemId);
                            if (existedReceiptDetail != null)
                            {
                                

                                //var existedReceiptDetail = existedReceiptDetails.FirstOrDefault(rd => rd.ItemId == menuItem.ItemId);
                                var existedBill = existedBills.FirstOrDefault(b => b.ItemName == bill.ItemName);

                                if (menuItem.ItemType == "FOOD")
                                {
                                    var recipes = DataProvider.Instance.DB.Recipes.Where(r => r.ItemId == menuItem.ItemId).ToList();
                                    if (menuItem.Instock < bill.Quantity)
                                    {
                                        string announce = $"Không đủ nguyên liệu cho món: {bill.ItemName}!\nSố lượng nguyên liệu còn lại cho món ăn là:\n";
                                        foreach (var recipe in recipes)
                                        {
                                            var ingredient = DataProvider.Instance.DB.Ingredients.FirstOrDefault(i => i.IngreId == recipe.IngreId);
                                            announce += $"\n{ingredient.IngreName}: {ingredient.InstockKg}";
                                        }
                                        MessageBox.Show(announce, "Thông báo!", MessageBoxButton.OK, MessageBoxImage.Information);
                                        corrupted = true;
                                        continue;
                                    }
                                    else menuItem.Instock = menuItem.Instock + existedBill.Quantity - bill.Quantity;
                                }
                                else // cho drink và other
                                {
                                    if (menuItem.Instock < bill.Quantity)
                                    {
                                        MessageBox.Show($"Không đủ số lương sản phẩm cho món: {bill.ItemName}!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                        MessageBox.Show($"Số lượng sản phẩm còn lại: {menuItem.Instock}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                        corrupted = true;
                                        continue;
                                    }
                                    else menuItem.Instock = menuItem.Instock + existedBill.Quantity - bill.Quantity;
                                }
                                existedReceiptDetail.Quantity = bill.Quantity;
                                existedReceiptDetail.Price = bill.ItemSprice;

                            }
                            else
                            {
                                if (menuItem.ItemType == "FOOD")
                                {
                                    var recipes = DataProvider.Instance.DB.Recipes.Where(r => r.ItemId == menuItem.ItemId).ToList();
                                    if (menuItem.Instock < bill.Quantity)
                                    {
                                        string announce = $"Không đủ nguyên liệu cho món: {bill.ItemName}!\nSố lượng nguyên liệu còn lại cho món ăn là:\n";
                                        foreach (var recipe in recipes)
                                        {
                                            var ingredient = DataProvider.Instance.DB.Ingredients.FirstOrDefault(i => i.IngreId == recipe.IngreId);
                                            announce += $"\n{ingredient.IngreName}: {ingredient.InstockKg}";
                                        }
                                        MessageBox.Show(announce, "Thông báo!", MessageBoxButton.OK, MessageBoxImage.Information);
                                        corrupted = true;
                                        continue;
                                    }
                                    else menuItem.Instock -= bill.Quantity;
                                }
                                else // cho drink và other
                                {
                                    if (menuItem.Instock < bill.Quantity)
                                    {
                                        MessageBox.Show($"Không đủ số lương sản phẩm cho món: {bill.ItemName}!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                        MessageBox.Show($"Số lượng sản phẩm còn lại: {menuItem.Instock}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                        corrupted = true;
                                        continue;
                                    }
                                    else menuItem.Instock -= bill.Quantity;
                                }

                                var receiptDetail = new ReceiptDetail()
                                {
                                    RecId = inputReceipt.RecId,
                                    ItemId = menuItem.ItemId,
                                    Quantity = bill.Quantity,
                                    Price = bill.ItemSprice,
                                };
                                DataProvider.Instance.DB.ReceiptDetails.Add(receiptDetail);
                            }
                        }


                        if (corrupted)
                        {
                            MessageBox.Show("Hóa đơn không thể lưu do thiếu nguyên liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        int selectedTabId = DataProvider.Instance.DB.DiningTables.Where(t => t.TabNum == SelectedTabNum).FirstOrDefault().TabId;
                        decimal totalAmount = TotalAmount;  // Tính tổng tiền từ danh sách Bills

                        InputReceipt.RecPay = totalAmount;
                        InputReceipt.TabId = selectedTabId;
                        InputReceipt.EmpId = SelectedEmpId;

                        DataProvider.Instance.DB.SaveChanges();

                    }
                    p.Close();
                }

                // Xóa danh sách Bills và đặt lại tổng tiền
                Bills.Clear();
                TotalAmount = 0;

            });

            // Command xóa hóa đơn
            DeleteBillCommand = new RelayCommand<BillUCViewModel>(
                (bill) => bill != null, // Kiểm tra bill hợp lệ
                (bill) =>
                {
                    ObservableCollection<ReceiptDetail> receiptDetails = new ObservableCollection<ReceiptDetail>(
                        DataProvider.Instance.DB.ReceiptDetails
                        .Where(rd => rd.RecId == bill.RecId)
                        .ToList()
                    );
                    foreach (var rd in receiptDetails)
                    {
                        if (rd.ItemId == DataProvider.Instance.DB.MenuItems.FirstOrDefault(mi => mi.ItemName == bill.ItemName).ItemId)
                            DataProvider.Instance.DB.ReceiptDetails.Remove(rd);
                    }

                    Bills.Remove(bill); // Xóa hóa đơn khỏi danh sách

                    UpdateBillOrder(); // Cập nhật lại RecId

                    OnPropertyChanged(nameof(Bills)); // Cập nhật giao diện
                    MessageBox.Show($"Đã xóa hóa đơn: {bill.ItemName}");
                });



        }

        public void UpdateBillOrder()
        {
            int count = 1;
            foreach (var bill in Bills)
            {
                bill.STT = count++;
            }
        }

        public void LoadDiningTables()
        {
            TabsNum = new ObservableCollection<byte?>(
                DataProvider.Instance.DB.DiningTables
                .Where(tab => tab.Isdeleted == false && tab.TabStatus == true)
                .Select(tab => tab.TabNum)
                .ToList()
            );
            OnPropertyChanged(nameof(TabsNum));
        }
        public void LoadEmpList()
        {
            EmpList = new ObservableCollection<Employee>(
                DataProvider.Instance.DB.Employees
                .Where(emp => emp.Isdeleted == false)
                .ToList()
            );
            OnPropertyChanged(nameof(EmpList));
            EmpIdList = new ObservableCollection<int>(
                EmpList.Select(emp => emp.EmpId)
                .ToList()
            );
        }


    }
}
