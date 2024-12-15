using RestaurantManager.Models;
using System.ComponentModel;

public class IngredientWrapper : INotifyPropertyChanged
{
    private Ingredient ingredient;
    public Ingredient Ingredient
    {
        get { return ingredient; }
        set { ingredient = value; OnPropertyChanged(nameof(Ingredient)); }
    }

    public string IngreName => ingredient.IngreName;

    private double instockKg;
    public double InstockKg
    {
        get { return instockKg; }
        set
        {
            if (instockKg != value)
            {
                instockKg = value;
                OnPropertyChanged(nameof(InstockKg));  // Notify UI that InstockKg has changed
            }
        }
    }

    public IngredientWrapper(Ingredient ingredient)
    {
        this.ingredient = ingredient;
        this.instockKg = ingredient.InstockKg;
    }

    public IngredientWrapper(Ingredient ingredient, double kg)
    {
        this.ingredient = ingredient;
        this.instockKg = kg;
    }
    public IngredientWrapper()
    {

    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
