using Microsoft.Data.SqlClient;
using MyShop.BLL;
using MyShop.DTO;
using MyShop.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAL
{
    public class OrderDAL
    {
        SqlConnection? Connection = Database.Instance.Connection;
        public ObservableCollection<OrderDTO> getAll()
        {
            ObservableCollection<OrderDTO> list = new ObservableCollection<OrderDTO>();

            var sql = "SELECT * FROM Orders";
            var command = new SqlCommand(sql, Connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                OrderDTO order = new OrderDTO
                {
                    id = (int)reader["id"],
                    customerID = (int)reader["customer_id"],
                    totalRevenue = reader["total_revenue"] == DBNull.Value ? 0 : (decimal)reader["total_revenue"],
                    totalProfit = reader["total_profit"] == DBNull.Value ? 0 : (decimal)reader["total_profit"],
                    orderDate = (DateTime)reader["order_date"],
                 
                };
                list.Add(order);
            }
            reader.Close();

            return list;
        }

        public int create(OrderDTO order)
        {
            var query = """
				INSERT INTO Orders ( customer_id, total_revenue, total_profit, order_date)
				VALUES ( @customerID, @totalRevenue, @totalProfit, @orderDate);
				SELECT ident_current('Orders')
				""";
            var command = new SqlCommand(query, Connection);

       
            command.Parameters.Add("@customerID", SqlDbType.Int).Value = order.customerID;
            command.Parameters.Add("@totalRevenue", SqlDbType.Money).Value = order.totalRevenue;
            command.Parameters.Add("@totalProfit", SqlDbType.Money).Value = order.totalProfit;
            command.Parameters.Add("@orderDate", SqlDbType.DateTime).Value = order.orderDate;
     

            int id = (int)((decimal)command.ExecuteScalar());

            return id;
        }

        public int update(OrderDTO order)
        {
            string query = """
				UPDATE Orders
				SET customer_id = @customerID, total_revenue = @totalRevenue, total_profit = @totalProfit, order_date = @orderDate
				WHERE id = @id
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = order.id;
            command.Parameters.Add("@customerID", SqlDbType.Int).Value = order.customerID;
            command.Parameters.Add("@totalRevenue", SqlDbType.Money).Value = order.totalRevenue;
            command.Parameters.Add("@totalProfit", SqlDbType.Money).Value = order.totalProfit;
            command.Parameters.Add("@orderDate", SqlDbType.DateTime).Value = order.orderDate;
           

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        public int delete (int id)
        {
            OrderItemBLL orderItemBLL = new OrderItemBLL();
            orderItemBLL.deleteOrderItemByOrderId(id);

            var query = """
                DELETE FROM Orders 
                WHERE id = @id
                """;
            var command = new SqlCommand(query, Connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        
    }
}
