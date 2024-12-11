using RestaurantManager.Models;
using RestaurantManager.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    class IngredientsManagementViewModel : BaseViewModel
    {
        public bool AddButtonIngredient { get; set; } = false;

        private int ingredientID;
        public int IngredientID { get; set; }

        private ObservableCollection<Ingredient> ingredientsList;
        public ObservableCollection<Ingredient> IngredientsList
        {
            get { return ingredientsList; }
            set
            {
                ingredientsList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StockinDetailsIngre> stockinDetailsIngres;
        public ObservableCollection<StockinDetailsIngre> StockinDetailsIngres
        {
            get { return stockinDetailsIngres; }
            set
            {
                stockinDetailsIngres = value;
                OnPropertyChanged();
            }
        }

        private string ingreName;
        public string IngreName
        {
            get { return ingreName; }
            set
            {
                if (ingreName != value)
                {
                    ingreName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string ingreCPrice;
        public string IngreCPrice
        {
            get { return ingreCPrice; }
            set
            {
                if (ingreCPrice != value)
                {
                    ingreCPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        Ingredient SelectedIngreDetails; 
        public void LoadIngredientInformation()
        {
            SelectedIngreDetails = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == IngredientID).FirstOrDefault();

            if (SelectedIngreDetails != null)
            {
                IngreName = SelectedIngreDetails.IngreName;
                if (SelectedIngreDetails.IngrePrice != null)
                    IngreCPrice = SelectedIngreDetails.IngrePrice.ToString();
                else IngreCPrice = "0";
            }

        }

        public ICommand ConfirmIngredientCommand { get; set; }
        public ICommand CancelIngredientCommand { get; set; }

        public IngredientsManagementViewModel()
        {
            //LoadIngredientInformation();
            ConfirmIngredientCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                decimal parsedPrice;
                if (!decimal.TryParse(IngreCPrice, out parsedPrice))
                {
                    MessageBox.Show("Giá nguyên liệu không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                SelectedIngreDetails = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == IngredientID).FirstOrDefault();


                bool checkValidIngre = !String.IsNullOrEmpty(IngreName) && !String.IsNullOrEmpty(IngreCPrice);
                if (!checkValidIngre) { MessageBox.Show("Không được để trống thông tin!\nThông tin không được lưu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Asterisk); p.Close(); return; }
                int existedIngre = DataProvider.Instance.DB.Ingredients.Count();
              
                if (SelectedIngreDetails != null && AddButtonIngredient == false)  // đã có trong db
                {
                    try
                    {
                        var dbIngre = SelectedIngreDetails;
                        dbIngre.IngreName = IngreName;
                        dbIngre.IngrePrice = decimal.Parse(IngreCPrice);

                        DataProvider.Instance.DB.SaveChanges(); 
                        p.Close(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}");
                    }
                }
                //else if (SelectedIngreDetails != null && SelectedIngreDetails.IngrePrice == null) 
                //{
                //    MessageBox.Show("YES");
                //    IngreCPrice = "0";
                //}
                else // chưa có trong db
                {
                    try
                    {
                        var newIngre = new Ingredient() { IngreName = IngreName, IngreCode = $"NL{existedIngre + 1:D3}", IngrePrice = decimal.Parse(IngreCPrice) };
                        DataProvider.Instance.DB.Ingredients.Add(newIngre);
                        DataProvider.Instance.DB.SaveChanges();
                        MessageBox.Show("Thêm nguyên liệu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        p.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}");
                    }                    
                } 
            });
            CancelIngredientCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
