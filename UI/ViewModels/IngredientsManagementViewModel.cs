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

        private int ingredientId;
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
        StockinDetailsIngre SelectedStockInIngreDetails; 
        public void LoadIngredientInformation()
        {
            SelectedIngreDetails = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == IngredientID).FirstOrDefault();
            SelectedStockInIngreDetails = DataProvider.Instance.DB.StockinDetailsIngre.Where(y => y.IngreId == IngredientID).FirstOrDefault();

            if (SelectedIngreDetails != null)
            {
                IngreName = SelectedIngreDetails.IngreName;
            }
            if (SelectedStockInIngreDetails != null)
            {
                IngreCPrice = SelectedStockInIngreDetails.Cprice.ToString();
            }
        }

        public ICommand ConfirmIngredientCommand { get; set; }
        public ICommand CancelIngredientCommand { get; set; }

        public IngredientsManagementViewModel()
        {
            //LoadIngredientInformation();
            ConfirmIngredientCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                int addedIngreID = -1;
                SelectedIngreDetails = DataProvider.Instance.DB.Ingredients.Where(x => x.IngreId == IngredientID).FirstOrDefault();
                SelectedStockInIngreDetails = DataProvider.Instance.DB.StockinDetailsIngre.Where(y => y.IngreId == IngredientID).FirstOrDefault();

                bool checkValidIngre = !String.IsNullOrEmpty(IngreName) && !String.IsNullOrEmpty(IngreCPrice);
                if (!checkValidIngre) { MessageBox.Show("Không được để trống thông tin!\nThông tin không được lưu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Asterisk); p.Close(); }
                int existedIngre = DataProvider.Instance.DB.Ingredients.Count();
                // đã có trong db
                if (SelectedIngreDetails != null)
                {
                    var dbIngre = SelectedIngreDetails;
                    dbIngre.IngreName = IngreName;
                    DataProvider.Instance.DB.SaveChanges();
                    p.Close();
                }
                else
                {                   
                    var newIngre = new Ingredient() { IngreName = IngreName, IngreCode = $"NL{existedIngre + 1:D3}" };
                    addedIngreID = newIngre.IngreId;
                    DataProvider.Instance.DB.Ingredients.Add(newIngre);
                    var newStockInIngre = new StockinDetailsIngre() { IngreId = addedIngreID, Cprice = decimal.Parse(IngreCPrice) };
                    DataProvider.Instance.DB.SaveChanges();
                    MessageBox.Show("Thêm nguyên liệu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    p.Close();
                } // chưa có trong db
                //// đã có giá trong db
                //if (SelectedStockInIngreDetails != null)
                //{
                //    var dbStockInIngre = SelectedStockInIngreDetails;
                //}
                //else { } // chưa có giá trong db
            });
            CancelIngredientCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });



        }
    }
}
