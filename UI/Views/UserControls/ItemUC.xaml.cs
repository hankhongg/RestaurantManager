﻿using RestaurantManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ItemUC.xaml
    /// </summary>
    public partial class ItemUC : UserControl
    {
        public ItemUC()
        {
            InitializeComponent();
            //this.DataContext = new FoodItemUCViewModel();
        }
    }
}
