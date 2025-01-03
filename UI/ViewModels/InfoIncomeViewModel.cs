﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RestaurantManager.ViewModels
{
    class InfoIncomeViewModel : BaseViewModel
    {
        private ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }
        private string textToday;
        public string TextToday
        {
            get { return textToday; }
            set
            {
                textToday = value;
                OnPropertyChanged();
            }
        }
        private string textYesterday;
        public string TextYesterday
        {
            get { return textYesterday; }
            set
            {
                textYesterday = value;
                OnPropertyChanged();
            }
        }
        private decimal valueToday;
        public decimal ValueToday
        {
            get { return valueToday; }
            set
            {
                valueToday = value;
                OnPropertyChanged();
            }
        }
        private decimal valueYesterday;
        public decimal ValueYesterday
        {
            get { return valueYesterday; }
            set
            {
                valueYesterday = value;
                OnPropertyChanged();
            }
        }
        private double maxValue;
        public double MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged();
            }
        }

        public InfoIncomeViewModel()
        {
            //ImagePath = "Images/money.png";
        }
        public InfoIncomeViewModel(string imagePath)
        {
            Image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }
    }
}
