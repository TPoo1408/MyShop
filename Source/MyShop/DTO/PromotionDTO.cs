using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class PromotionDTO : INotifyPropertyChanged, ICloneable
    {
		public int? id { get; set; }
        public string code { get; set; }
        public int discountPercentage { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
