using MyShop.DAL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BLL
{
    public class OrderBLL
    {
        private OrderDAL _orderDAL;

        public OrderBLL()
        {
            _orderDAL = new OrderDAL();
        }

        public int getTotalOrderThisWeek ()
        {
            ObservableCollection<OrderDTO> orders = _orderDAL.getAll();

            DateTime currentDate = DateTime.Now;
            int daysUntilMonday = ((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime startOfWeek = currentDate.AddDays(-daysUntilMonday);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var result = orders.Where(order => order.orderDate >= startOfWeek && order.orderDate <= endOfWeek).ToList();

            return result.Count;
        }

        public int getTotalOrderThisMonth()
        {
            ObservableCollection<OrderDTO> orders = _orderDAL.getAll();

            DateTime currentDate = DateTime.Now;
            DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var result = orders.Where(order => order.orderDate >= startOfMonth && order.orderDate <= endOfMonth).ToList();

            return result.Count;
        }

        public Tuple<List<OrderDTO>, int> getAllOrder(int currentPage = 1, int rowsPerPage = 6, DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = _orderDAL.getAll().ToList();

            var list = orders.Where(order =>
            {
                var isStartDateValid = startDate == null || DateTime.Compare(order.orderDate, startDate.Value) >= 0;
                var isEndDateValid = endDate == null || DateTime.Compare(order.orderDate, endDate.Value) <= 0;

                return isStartDateValid && isEndDateValid;
            });

            var items = list.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            var result = new Tuple<List<OrderDTO>, int>(items.ToList(), list.Count());

            return result;
        }

        public int addOrder (OrderDTO order)
        {
            return _orderDAL.create(order);
        }

     

        public int updateOrder (OrderDTO order)
        {
            return _orderDAL.update(order);
        }

        public int deleteOrder (int id)
        {
            return _orderDAL.delete(id);
        }

        public decimal calculateProfit(decimal totalRevenue)
        {
            float profit = 0.2f;
            decimal result = totalRevenue * (decimal)profit;
            return result;
        }

        public Tuple<List<OrderDTO>, int> getOrderBySearch(int currentPage = 1, int rowsPerPage = 6,
             DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = _orderDAL.getAll().ToList();

            var list = orders.Where((item) => {
                bool check = true;
                if (startDate != null && endDate != null)
                {
                    check = item.orderDate >= startDate && item.orderDate <= endDate;
                }
                else if (startDate != null && endDate == null)
                {
                    check = item.orderDate >= startDate;
                }
                else if (startDate == null && endDate != null)
                {
                    check = item.orderDate <= endDate;
                }

                return check;
            });

            var items = list.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            var result = new Tuple<List<OrderDTO>, int>(items.ToList(), list.Count());

            return result;
        }
    }
}
