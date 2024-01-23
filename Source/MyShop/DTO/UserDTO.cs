using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class UserDTO
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
    }
}
