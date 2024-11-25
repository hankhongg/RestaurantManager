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
using System.Windows.Shapes;

namespace RestaurantManager.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        //private void exitButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult r = MessageBox.Show("Bạn có chắc muốn thoát?", "Thoát", MessageBoxButton.YesNo, MessageBoxImage.Information);
        //    if (r == MessageBoxResult.Yes)
        //    {
        //        Application.Current.Shutdown();

        //    }
        //}

        //private void minimizeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowState = WindowState.Minimized;
        //}

        //private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DragMove();
        //}
    }
}
