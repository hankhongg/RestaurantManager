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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for InfoIncomeUC.xaml
    /// </summary>
    public partial class InfoIncomeUC : UserControl
    {
        public InfoIncomeUC()
        {
            InitializeComponent();
        }
        private void Canvas_Loaded_Today(object sender, RoutedEventArgs e)
        {
            double canvasWidth = todayCanvas.ActualWidth;
            double textWidth = todayTextBlock.ActualWidth;

            if (textWidth > canvasWidth)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = canvasWidth, 
                    To = -textWidth,    
                    Duration = new Duration(TimeSpan.FromSeconds(7)), 
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(animation, todayTextBlock);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
            else
            {
                Canvas.SetLeft(todayTextBlock, (canvasWidth - textWidth) / 2);
            }
        }

        private void Canvas_Loaded_Yesterday(object sender, RoutedEventArgs e)
        {
            double canvasWidth = yesterdayCanvas.ActualWidth;
            double textWidth = yesterdayTextBlock.ActualWidth;

            if (textWidth > canvasWidth)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = canvasWidth,
                    To = -textWidth,
                    Duration = new Duration(TimeSpan.FromSeconds(7)),
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(animation, yesterdayTextBlock);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
            else
            {
                Canvas.SetLeft(yesterdayTextBlock, (canvasWidth - textWidth) / 2);
            }
        }

    }
}
