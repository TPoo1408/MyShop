using MyShop.DAL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop.BLL
{
    public class UserBLL
    {
        private UserDAL _userDAL;
        public UserBLL()
        {
            _userDAL = new UserDAL();
        }

        public bool existUser(string username)
        {
            return _userDAL.exist(username);
        }

        public UserDTO? getUser(string username, string password)
        {
            return _userDAL.getOne(username, password);
        }

        public int createNewUser(UserDTO user)
        {
            return _userDAL.create(user);
        }

        public ObservableCollection<UserDTO> getAllCustomer()
        {
            var result = _userDAL.getAll();
            return result;
        }

        public UserDTO getCustomerById (int id)
        {
            return _userDAL.getById(id);
        }

        public Tuple<ObservableCollection<UserDTO>, int> getCustomersBySearchQuery(int currentPage = 1, int rowsPerPage = 8, string? query = null)
        {
            var customers = _userDAL.getAll().ToList();

            var list = customers.Where(customer => 
                       string.IsNullOrEmpty(query) ||
                       customer.name.ToLower().Contains(query.ToLower()) ||
                       customer.address.ToLower().Contains(query.ToLower()) ||
                       customer.phoneNumber.Contains(query.ToLower()));

            var items = list.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            var result = new Tuple<ObservableCollection<UserDTO>, int>(new ObservableCollection<UserDTO>(items), list.Count());

            return result;
        }

        public int addCustomer (UserDTO customer)
        {
            return _userDAL.create(customer);
        }

        public int updateCustomer (UserDTO customer)
        {
            return _userDAL.update(customer);
        }

        public int deleteCustomer (int id)
        {
            return _userDAL.delete(id);
        }
    }
}
