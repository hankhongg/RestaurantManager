﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for TableUC.xaml
    /// </summary>
    public partial class TableUC : UserControl
    {
        public TableUC()
        {
            InitializeComponent();
            //Debug.WriteLine($"DataContext: {this.DataContext}");
            //this.DataContext = this;
        }
    }
}
