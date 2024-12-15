using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManager.ViewModels
{
    internal class SetNameRecipeViewModel : BaseViewModel
    {
        private bool confirm = false;
        public bool Confirm { get { return confirm; } set { confirm = value; OnPropertyChanged(nameof(confirm)); } }

        // recipe name
        private string recipeName;
        public string RecipeName
        {
            get { return recipeName; }
            set { recipeName = value; OnPropertyChanged(nameof(RecipeName)); }
        }
        private string recipeCostPrice = "0";
        public string RecipeCostPrice
        {
            get { return recipeCostPrice; }
            set { recipeCostPrice = value; OnPropertyChanged(nameof(RecipeCostPrice)); }
        }
        private string recipeSellPrice;
        public string RecipeSellPrice
        {
            get { return recipeSellPrice; }
            set { recipeSellPrice = value; OnPropertyChanged(nameof(RecipeSellPrice)); }
        }
        public ICommand ConfirmRecipeName { get; set; }
        public ICommand CancelRecipeName { get; set; }
        public SetNameRecipeViewModel()
        {
            CancelRecipeName = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Confirm = false;
                p.Close();
            }
            );
            ConfirmRecipeName = new RelayCommand<Window>((p) => { return true; }, (p) => {
                Confirm = true;
                p.Close();
            });
        }
    }
}
