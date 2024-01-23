using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class OrderDTO : INotifyPropertyChanged
    {
        public int id { get; set; }
        public int customerID { get; set; }
        public decimal totalRevenue { get; set; }
        public decimal totalProfit { get; set; }
        public DateTime orderDate { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
