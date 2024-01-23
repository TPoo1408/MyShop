using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class ProductDTO : INotifyPropertyChanged, ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public string? description { get; set; }
        public decimal price { get; set; }
        public decimal promotionPrice { get; set; }

        public int stock { get; set; }
        public string? image { get; set; }
        public int categoryID { get; set; }
        public int? promotionID { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
