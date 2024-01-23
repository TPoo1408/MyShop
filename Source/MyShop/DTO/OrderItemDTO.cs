using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class OrderItemDTO : INotifyPropertyChanged
	{
		public int id { get; set; }
        public int orderID { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }   
}
