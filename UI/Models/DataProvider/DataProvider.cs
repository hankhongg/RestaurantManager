using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RestaurantManager.Models.DataProvider
{
    internal class DataProvider
    { 
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get {
                if (instance == null)
                {
                    instance = new DataProvider();
                }
                return instance;
            }
            set { instance = value; }
        }
        public QlnhContext DB { get; set; }
        private DataProvider()
        {
            DB = new QlnhContext();
        }
    }

}
