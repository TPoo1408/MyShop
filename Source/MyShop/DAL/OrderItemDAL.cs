using Microsoft.Data.SqlClient;
using MyShop.BLL;
using MyShop.DTO;
using MyShop.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAL
{
    public class OrderItemDAL
    {
        SqlConnection? Connection = Database.Instance.Connection;

        public List<OrderItemDTO> getAll(int orderID)
        {
            List<OrderItemDTO> list = new List<OrderItemDTO>();
            var query = "SELECT * FROM OrderItems WHERE order_id = @orderID";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                OrderItemDTO orderItem = new OrderItemDTO
                {
                    id = (int)reader["id"],
                    orderID = (int)reader["order_id"],
                    productID = (int)reader["product_id"],
                    quantity = (int)reader["quantity"],
                    totalPrice = (decimal)reader["total_price"]
                };
                list.Add(orderItem);
            }
            reader.Close();

            return list;
        }

        public void create(OrderItemDTO orderItem)
        {
            var query = """
				INSERT INTO OrderItems (order_id, product_id, quantity, total_price)
				VALUES (@orderID, @productID, @quantity, @totalPrice);
				SELECT ident_current('OrderItems')
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderItem.orderID;
            command.Parameters.Add("@productID", SqlDbType.Int).Value = orderItem.productID;
            command.Parameters.Add("@quantity", SqlDbType.Int).Value = orderItem.quantity;
            command.Parameters.Add("@totalPrice", SqlDbType.Money).Value = orderItem.totalPrice;

            command.ExecuteScalar();

           
                ProductBLL productBLL = new ProductBLL();
                var product = productBLL.getProductById(orderItem.productID);
                product.stock -= orderItem.quantity;
                productBLL.updateProduct(product);
            
        }

        public void delete(int orderID)
        {
            List<OrderItemDTO> orderItems = getAll(orderID);

            foreach (var orderItem in orderItems)
            {
                ProductBLL productBLL = new ProductBLL();
                var product = productBLL.getProductById(orderItem.productID);
                product.stock += orderItem.quantity;
                productBLL.updateProduct(product);
            }

            var query = """
				DELETE FROM OrderItems
				WHERE order_id = @orderID
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;
            
            command.ExecuteNonQuery();
        }
    }
}
