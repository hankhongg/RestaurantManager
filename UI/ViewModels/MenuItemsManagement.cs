using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using RestaurantManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XAct;
using MenuItem = RestaurantManager.Models.MenuItem;

namespace RestaurantManager.ViewModels
{
    class MenuItemsManagement : BaseViewModel
    {
        private int itemID = -1;
        public int ItemID { get { return itemID; } set { if (itemID != value) itemID = value; OnPropertyChanged(); } }

        private string currentQuantity = "0";
        public string CurrentQuantity
        {
            get { return currentQuantity; }
            set { currentQuantity = value; OnPropertyChanged(nameof(CurrentQuantity)); }
        }


        // Binding View
        private DataTemplate currentView;
        public DataTemplate CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }
        private string currentViewName;
        public string CurrentViewName
        {
            get { return currentViewName; }
            set { currentViewName = value; OnPropertyChanged(nameof(CurrentViewName)); }
        }

        private int selectedIdxType = 0;
        public int SelectedIdxType
        {
            get { return selectedIdxType; }
            set { selectedIdxType = value; OnPropertyChanged(nameof(SelectedIdxType)); UpdateView(); }
        }

        // current ingres
        private ObservableCollection<Ingredient> currentIngredients;
        public ObservableCollection<Ingredient> CurrentIngredients
        {
            get { return currentIngredients; }
            set { currentIngredients = value; OnPropertyChanged(nameof(CurrentIngredients)); }
        }

        // current ingres names
        private IEnumerable<string> currentIngredientsNames;
        public IEnumerable<string> CurrentIngredientsNames
        {
            get { return currentIngredientsNames; }
            set { currentIngredientsNames = value; OnPropertyChanged(nameof(CurrentIngredientsNames)); }
        }

        private int currentIdxIngredientName;
        public int CurrentIdxIngredientName
        {
            get { return currentIdxIngredientName; }
            set { currentIdxIngredientName = value; OnPropertyChanged(nameof(CurrentIdxIngredientName)); }
        }

        // added ingres names
        private ObservableCollection<IngredientWrapper> addedLVIngres;
        public ObservableCollection<IngredientWrapper> AddedLVIngres
        {
            get { return addedLVIngres; }
            set { addedLVIngres = value; OnPropertyChanged(nameof(AddedLVIngres)); }
        }

        private IngredientWrapper lvIngreSelected;
        public IngredientWrapper LVIngreSelected
        {
            get { return lvIngreSelected; }
            set { lvIngreSelected = value; OnPropertyChanged(nameof(lvIngreSelected)); IsLVIngreSelected = lvIngreSelected != null; }
        }

        private bool isLVIngreSelected = false;
        public bool IsLVIngreSelected
        {
            get { return isLVIngreSelected; }
            set { isLVIngreSelected = value; OnPropertyChanged(nameof(IsLVIngreSelected)); }
        }

        private bool isNotEditing = true;
        public bool IsNotEditing
        {
            get { return isNotEditing; }
            set { isNotEditing = value; OnPropertyChanged(nameof(IsNotEditing)); }
        }

        // commands
        public ICommand AddIngreIntoRecipe { get; set; }
        public ICommand RemoveIngreFromRecipe { get; set; }
        public ICommand AddItemCommand { get; set; }



        public void LoadRecipeInformation(int ItemID)
        {
            if (ItemID != null)
            {
                ObservableCollection<int> ingresIDs = new ObservableCollection<int>();
                var item = DataProvider.Instance.DB.MenuItems.FirstOrDefault(x => x.ItemId == ItemID);
                if (item != null)
                {
                    ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>(
                        DataProvider.Instance.DB.Recipes.Where(x => x.ItemId == item.ItemId)
                    );

                    if (recipes.Any())  
                    {
                        foreach (Recipe recipe in recipes)
                        {
                            ingresIDs.Add(recipe.IngreId);
                        }

                        var ingredientWrappers = new ObservableCollection<IngredientWrapper>(
                            DataProvider.Instance.DB.Ingredients
                                .Where(x => ingresIDs.Contains(x.IngreId))  
                                .Select(x => new IngredientWrapper(x))  
                        );

                        foreach (var wrapper in ingredientWrappers)
                        {
                            var recipe = recipes.FirstOrDefault(r => r.IngreId == wrapper.Ingredient.IngreId);
                            if (recipe != null)
                            {
                                wrapper.InstockKg = recipe.IngreQuantityKg;  
                            }
                        }
                        AddedLVIngres = ingredientWrappers;
                    }
                }
            }
        }


        public void LoadBlankRecipeInformation()
        {
            AddedLVIngres = new ObservableCollection<IngredientWrapper>();
        }

        public MenuItemsManagement()
        {
            SelectedIdxType = 0;
            CurrentIdxIngredientName = 0;
            CurrentIngredients = new ObservableCollection<Ingredient>(DataProvider.Instance.DB.Ingredients);
            CurrentIngredientsNames = CurrentIngredients.Select(x => x.IngreName);
            AddIngreIntoRecipe = new RelayCommand<object>((p) => CheckValid(), (p) =>
            {
                bool foundIngre = false;
                foreach (IngredientWrapper ingre in AddedLVIngres)
                {
                    if (ingre.IngreName == CurrentIngredients.ElementAt(CurrentIdxIngredientName).IngreName)
                    {
                        ingre.InstockKg = double.Parse(CurrentQuantity);
                        foundIngre = true;
                        break;
                    }
                }
                if (foundIngre == false)
                {
                    Ingredient d = new Ingredient();
                    d.IngreName = CurrentIngredients.ElementAt(CurrentIdxIngredientName).IngreName;
                    d.InstockKg = double.Parse(CurrentQuantity);

                    AddedLVIngres.Add(new IngredientWrapper(d));
                }
            }
            );
            RemoveIngreFromRecipe = new RelayCommand<object>((p) => LVIngreSelected != null, (p) =>
            {
                AddedLVIngres.Remove(LVIngreSelected);
            }
            );
            AddItemCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SetNameRecipeWindow setNameRecipeWindow = new SetNameRecipeWindow();
                //var setNameRecipeVM = setNameRecipeWindow.DataContext as SetNameRecipeViewModel;
                var setNameRecipeVM = new SetNameRecipeViewModel();

                decimal supposedCostPrice = 0;

                if (AddedLVIngres!= null)
                {
                    foreach (IngredientWrapper ingre in AddedLVIngres)
                    {
                        decimal ingrePrice = DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreName == ingre.IngreName).IngrePrice ?? 0;
                        supposedCostPrice += ingrePrice * (decimal)ingre.InstockKg;
                    }
                }
                if (setNameRecipeVM != null)
                {
                    if (ItemID != -1)
                    {
                        setNameRecipeVM.RecipeName = DataProvider.Instance.DB.MenuItems.FirstOrDefault(x => x.ItemId == ItemID).ItemName;
                        setNameRecipeVM.RecipeSellPrice = DataProvider.Instance.DB.MenuItems.FirstOrDefault(x => x.ItemId == ItemID).ItemSprice.ToString();
                    }
                    setNameRecipeVM.RecipeCostPrice = supposedCostPrice.ToString();

                    setNameRecipeWindow.DataContext = setNameRecipeVM;
                    setNameRecipeWindow.ShowDialog();
                }

                if (setNameRecipeVM.Confirm)
                {
                    if (IsNotEditing) // không edit => add thêm công thức mới
                    {
                        int existedItem = DataProvider.Instance.DB.MenuItems.Count();

                        if (!string.IsNullOrEmpty(setNameRecipeVM.RecipeName) && !string.IsNullOrEmpty(setNameRecipeVM.RecipeSellPrice))
                        {
                            MenuItem newItem = new MenuItem();
                            newItem.ItemName = setNameRecipeVM.RecipeName;
                            newItem.ItemCprice = decimal.Parse(setNameRecipeVM.RecipeCostPrice);
                            newItem.ItemSprice = decimal.Parse(setNameRecipeVM.RecipeSellPrice);
                            newItem.ItemType = SelectedIdxType == 0 ? "FOOD" : SelectedIdxType == 1 ? "DRINK" : "OTHER";
                            newItem.ItemImg = "https://www.google.com";
                            newItem.ItemCode = $"MN{existedItem + 1:D2}";
                            newItem.Isdeleted = false;
                            newItem.Instock = 0;
                            DataProvider.Instance.DB.MenuItems.Add(newItem);
                            DataProvider.Instance.DB.SaveChanges();
                            foreach (IngredientWrapper ingre in AddedLVIngres)
                            {
                                Recipe newRecipe = new Recipe();
                                newRecipe.ItemId = newItem.ItemId;
                                newRecipe.IngreId = DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreName == ingre.IngreName).IngreId;
                                newRecipe.IngreQuantityKg = ingre.InstockKg;
                                DataProvider.Instance.DB.Recipes.Add(newRecipe);
                            }
                            DataProvider.Instance.DB.SaveChanges();
                        }
                        p.Close();
                    }
                    else // đang edit => chỉ save công thức lại thôi
                    {
                        if (!string.IsNullOrEmpty(setNameRecipeVM.RecipeName)
                            && !string.IsNullOrEmpty(setNameRecipeVM.RecipeSellPrice)
                            && ItemID != -1)
                        {
                            MenuItem menuItem = DataProvider.Instance.DB.MenuItems.FirstOrDefault(x => x.ItemId == ItemID);

                            if (menuItem != null)
                            {
                                menuItem.ItemName = setNameRecipeVM.RecipeName;
                                menuItem.ItemCprice = decimal.Parse(setNameRecipeVM.RecipeCostPrice);
                                menuItem.ItemSprice = decimal.Parse(setNameRecipeVM.RecipeSellPrice);
                            }

                            var existingRecipes = DataProvider.Instance.DB.Recipes.Where(x => x.ItemId == ItemID).ToList();

                            var currentIngredientIDs = AddedLVIngres.Select(ingre => ingre.Ingredient.IngreId).ToList();

                            foreach (var recipe in existingRecipes)
                            {
                                if (!currentIngredientIDs.Contains(recipe.IngreId))
                                {
                                    DataProvider.Instance.DB.Recipes.Remove(recipe); // nếu ko có trong list tổng thì đánh dấu xóa và xóa thật
                                    supposedCostPrice -= DataProvider.Instance.DB.Ingredients.FirstOrDefault(x => x.IngreId == recipe.IngreId).IngrePrice ?? 0;
                                }
                            }

                            foreach (IngredientWrapper ingre in AddedLVIngres)
                            {
                                var recipe = existingRecipes.FirstOrDefault(x => x.IngreId == ingre.Ingredient.IngreId);

                                if (recipe != null)
                                {
                                    // cập nhật lại số lượng nếu có recipe rồi
                                    recipe.IngreQuantityKg = ingre.InstockKg;
                                }
                                else
                                {
                                    // thêm mới recipe nếu chưa có
                                    Recipe newRecipe = new Recipe
                                    {
                                        ItemId = ItemID,
                                        IngreId = ingre.Ingredient.IngreId,
                                        IngreQuantityKg = ingre.InstockKg
                                    };
                                    DataProvider.Instance.DB.Recipes.Add(newRecipe);
                                }
                            }

                            DataProvider.Instance.DB.SaveChanges();
                        }

                        p.Close();
                    }


                }
                else p.Close();
            }
            );

        }


        bool CheckValid()
        {
            if (SelectedIdxType == 0)
            {
                if (CurrentIdxIngredientName == -1)
                {
                    MessageBox.Show("Hãy chọn nguyên liệu cần dùng cho công thức!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                if (CurrentQuantity == null || CurrentQuantity == "" || !double.TryParse(CurrentQuantity, out double parsedQuantity))
                {
                    MessageBox.Show("Hãy nhập số lượng hợp lệ nguyên liệu cần dùng cho công thức!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }
            return true;
        }

        public void UpdateView()
        {
            if (SelectedIdxType == 0)
            {
                CurrentViewName = "FoodView";
                if (Application.Current.MainWindow.Resources["FoodView"] is DataTemplate foodView)
                {
                    CurrentView = foodView;
                }
            }
            else if (SelectedIdxType == 1)
            {
                CurrentViewName = "DrinkView";
                if (Application.Current.MainWindow.Resources["DrinkView"] is DataTemplate drinkView)
                {
                    CurrentView = drinkView;
                }
            }
            else if (SelectedIdxType == 2)
            {
                CurrentViewName = "OtherView";
                if (Application.Current.MainWindow.Resources["OtherView"] is DataTemplate otherView)
                {
                    CurrentView = otherView;
                }
            }
        }

    }
}
