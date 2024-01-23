using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class CategoryDTO : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? image { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
