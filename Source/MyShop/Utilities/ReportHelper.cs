using MyShop.BLL;
using MyShop.DAL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Utilities
{
    public class ReportHelper
    {
        private List<OrderDTO> _orders;
        private List<ProductDTO> _products;
        private OrderDAL _orderDAL;
        private ProductDAL _productDAL;
        private OrderItemBLL _orderItemBLL;

        private int top = 5;

        public ReportHelper()
        {
            _orderDAL = new OrderDAL();
            _productDAL = new ProductDAL();
            _orderItemBLL= new OrderItemBLL();
            _orders = getAllOrder();
            _products = getAllProduct();
        }

        private List<OrderDTO> getAllOrder()
        {
            return _orderDAL.getAll().ToList();
        }

        private List<ProductDTO> getAllProduct()
        {
            return _productDAL.getAll().ToList();
        }

        public class ProductSales
        {
            public ProductDTO product { get; set; }
            public int salesQuantity { get; set; }
        }

        // DOANH THU VÀ LỢI NHUẬN
        // Thống kê doanh thu và lợi nhuận hàng năm
        public Tuple<List<decimal>, List<decimal>> getRevenueAndProfitAnnual()
        {
            List<decimal> renevues = new List<decimal>();
            List<decimal> profits = new List<decimal>();
            int currentYear = DateTime.Now.Year;

            for (int year = currentYear - 2; year <= currentYear; year++)
            {
                decimal totalRevenue = 0;
                decimal totalProfit = 0;
                foreach (var order in _orders)
                {
                    if (order.orderDate.Year == year)
                    {
                        totalRevenue += (decimal)order.totalRevenue;
                        totalProfit += (decimal)order.totalProfit;
                    }
                }
                renevues.Add(totalRevenue);
                profits.Add(totalProfit);
            }
            return new Tuple<List<decimal>, List<decimal>>(renevues, profits);
        }

        // Thống kê doanh thu và lợi nhuận trong 1 năm cụ thể (theo từng tháng từ 1 -> 12)
        public Tuple<List<decimal>, List<decimal>> getRevenueAndProfitByYear(int year)
        {
            List<decimal> renevues = new List<decimal>();
            List<decimal> profits = new List<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                decimal totalRevenue = 0;
                decimal totalProfit = 0;
                foreach (var order in _orders)
                {
                    if (order.orderDate.Month == month && order.orderDate.Year == year)
                    {
                        totalRevenue += (decimal)order.totalRevenue;
                        totalProfit += (decimal)order.totalProfit;
                    }
                }
                renevues.Add(totalRevenue);
                profits.Add(totalProfit);
            }
            return new Tuple<List<decimal>, List<decimal>>(renevues, profits);
        }

        // Thống kê doanh thu và lợi nhuận trong 1 tháng cụ thể (4 tuần trong tháng)
        public Tuple<List<decimal>, List<decimal>> getRevenueAndProfitByMonth(int month, int year)
        {
            List<decimal> renevues = new List<decimal>();
            List<decimal> profits = new List<decimal>();

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            for (int week = 1; week <= 5; week++)
            {
                decimal totalRevenue = 0;
                decimal totalProfit = 0;

                DateTime startDate = firstDayOfMonth.AddDays((week - 1) * 7);
                DateTime endDate = startDate.AddDays(6);
                if (week == 5) endDate = lastDayOfMonth;

                foreach (var order in _orders)
                {
                    if (order.orderDate.Date >= startDate.Date && order.orderDate.Date <= endDate.Date)
                    {
                        totalRevenue += (decimal)order.totalRevenue;
                        totalProfit += (decimal)order.totalProfit;
                    }
                }
                renevues.Add(totalRevenue);
                profits.Add(totalProfit);
            }

            return new Tuple<List<decimal>, List<decimal>>(renevues, profits);
        }

        // Thống kê doanh thu và lợi nhuận từ ngày đến ngày
        public Tuple<List<decimal>, List<decimal>> getRevenueAndProfitFromDatetoDate(DateTime startDate, DateTime endDate)
        {
            List<decimal> renevues = new List<decimal>();
            List<decimal> profits = new List<decimal>();

            for (DateTime day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                decimal totalRevenue = 0;
                decimal totalProfit = 0;

                foreach (var order in _orders)
                {
                    if (order.orderDate.Date == day)
                    {
                        totalRevenue += (decimal)order.totalRevenue;
                        totalProfit += (decimal)order.totalRevenue;
                    }
                }
                renevues.Add(totalRevenue);
                profits.Add(totalProfit);
            }

            return new Tuple<List<decimal>, List<decimal>>(renevues, profits);
        }

        // SỐ LƯỢNG SẢN PHẨM
        // Số lượng sản phẩm đã bán hàng năm
        public List<int> getQuantityOfProductAnnual(ProductDTO product)
        {
            List<int> result = new List<int>();

            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 2; year <= currentYear; year++)
            {
                int quantity = 0;
                foreach (var order in _orders)
                {
                    if (order.orderDate.Year == year)
                    {
                        var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                        foreach (var orderItem in orderItems)
                        {
                            if (orderItem.productID == product.id)
                            {
                                quantity += orderItem.quantity;
                            }
                        }
                    }
                }
                result.Add(quantity);
            }
            return result;
        }

        // Số lượng sản phẩm đã bán trong 1 năm cụ thể (từ tháng 1 -> 12)
        public List<int> getQuantityOfProductByYear(ProductDTO product, int year)
        {
            List<int> result = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                int quantity = 0;
                foreach (var order in _orders)
                {
                    if (order.orderDate.Year == year && order.orderDate.Month == month)
                    {
                        var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                        foreach (var orderItem in orderItems)
                        {
                            if (orderItem.productID == product.id)
                            {
                                quantity += orderItem.quantity;
                            }
                        }
                    }
                }
                result.Add(quantity);
            }
            return result;
        }

        // Số lượng sản phẩm đã bán trong 1 tháng cụ thể (4 tuần trong tháng)
        public List<int> getQuantityOfProductByMonth(ProductDTO product, int year, int month)
        {
            List<int> result = new List<int>();

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            for (int week = 1; week <= 5; week++)
            {
                int quantity = 0;
                DateTime startDate = firstDayOfMonth.AddDays((week - 1) * 7);
                DateTime endDate = startDate.AddDays(6);
                if (week == 5) endDate = lastDayOfMonth;

                foreach (var order in _orders)
                {
                    if (order.orderDate.Date >= startDate.Date && order.orderDate.Date <= endDate.Date)
                    {
                        var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                        foreach (var orderItem in orderItems)
                        {
                            if (orderItem.productID == product.id)
                            {
                                quantity += orderItem.quantity;
                            }
                        }
                    }
                }
                result.Add(quantity);
            }
            return result;
        }

        // Số lượng sản phẩm từ ngày đến ngày
        public List<int> getQuantityOfProductFromDatetoDate(ProductDTO product, DateTime startDate, DateTime endDate)
        {
            List<int> result = new List<int>();

            for (DateTime day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                int quantity = 0;
                foreach (var order in _orders)
                {
                    if (order.orderDate.Date == day)
                    {
                        var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                        foreach (var orderItem in orderItems)
                        {
                            if (orderItem.productID == product.id)
                            {
                                quantity += orderItem.quantity;
                            }
                        }
                    }
                }
                result.Add(quantity);
            }
            return result;
        }

        // SẢN PHẨM BÁN CHẠY
        // Bán chạy trong năm
        public List<ProductSales> getTopSalesByYear(int year)
        {
            List<ProductSales> result = new List<ProductSales>();

            foreach (var product in _products)
            {
                result.Add(new ProductSales { product = product, salesQuantity = 0 });
            }

            foreach (var order in _orders)
            {
                if (order.orderDate.Year == year)
                {
                    var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                    foreach (var orderItem in orderItems)
                    {
                        result.Find(item => item.product.id == orderItem.productID).salesQuantity += orderItem.quantity;
                    }
                }
            }
            return result.OrderByDescending(item => item.salesQuantity).Take(top).ToList();
        }

        // Bán chạy trong tháng
        public List<ProductSales> getTopSalesByMonth(int year, int month)
        {
            List<ProductSales> result = new List<ProductSales>();

            foreach (var product in _products)
            {
                result.Add(new ProductSales { product = product, salesQuantity = 0 });
            }

            foreach (var order in _orders)
            {
                if (order.orderDate.Year == year && order.orderDate.Month == month)
                {
                    var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                    foreach (var orderItem in orderItems)
                    {
                        result.Find(item => item.product.id == orderItem.productID).salesQuantity += orderItem.quantity;
                    }
                }
            }
            return result.OrderByDescending(item => item.salesQuantity).Take(top).ToList();
        }

        // Bán chạy trong tuần
        public List<ProductSales> getTopSalesByWeek()
        {
            List<ProductSales> result = new List<ProductSales>();

            foreach (var product in _products)
            {
                result.Add(new ProductSales { product = product, salesQuantity = 0 });
            }

            DateTime today = DateTime.Today;
            int daysUntilMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

            DateTime startOfWeek = today.AddDays(-daysUntilMonday);
            DateTime endOfWeek = startOfWeek.AddDays(6); // 6 ngày sau để đến Chủ nhật

            foreach (var order in _orders)
            {
                if (order.orderDate.Date >= startOfWeek && order.orderDate.Date <= endOfWeek)
                {
                    var orderItems = _orderItemBLL.getAllOrderItemByOrderId(order.id);
                    foreach (var orderItem in orderItems)
                    {
                        result.Find(item => item.product.id == orderItem.productID).salesQuantity += orderItem.quantity;
                    }
                }
            }
            return result.OrderByDescending(item => item.salesQuantity).Take(top).ToList();
        }

        public List<string> dayConverter(DateTime startDate, DateTime endDate)
        {
            List<string> result = new List<string>();

            for (DateTime day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                result.Add(day.ToString("dd/MM/yyyy"));
            }
            return result;
        }
    }
}
