using RestaurantManager.Views;
using RestaurantManager.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Views;

namespace RestaurantManager.ViewModels
{
    public class ControlBarModel : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MoveByMouse {  get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand {  get; set; }
        public ICommand TurnOffMaximize {  get; set; }

        private Visibility _visible = Visibility.Visible;
        public Visibility Visible {  get { return _visible; } set { _visible = value; OnPropertyChanged(); } }

        public ControlBarModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => { FrameworkElement window = getParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    //MessageBoxResult r = MessageBox.Show("Bạn có chắc muốn thoát?", "Thoát", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //if (r == MessageBoxResult.Yes)
                    //{
                    //    w.Close();
                    //}
                    w.Close(); 
                }
            });
            MoveByMouse = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = getParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
            MinimizeCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = getParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState == WindowState.Normal || w.WindowState == WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                    else if (w.WindowState == WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Normal;
                    }
                }
            });
            MaximizeCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = getParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState == WindowState.Normal)
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                    else if (w.WindowState == WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Normal;
                    }
                }
            });
            TurnOffMaximize = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = getParentWindow(p);
                var w = window as Window;
                if (w!= null)
                {
                    if (w is MainWindow || w is ProfileWindow || w is ForgotPasswordWindow)
                    {
                        Visible = Visibility.Collapsed;
                    }
                }
            });
        }
        protected FrameworkElement getParentWindow(UserControl p)
        {
            FrameworkElement parentWindow = p;
            while (parentWindow.Parent != null)
            {
                parentWindow = parentWindow.Parent as FrameworkElement;
            }
            return parentWindow;
        }

    }
}
