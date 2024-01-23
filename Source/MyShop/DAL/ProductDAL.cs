using Microsoft.Data.SqlClient;
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
    public class ProductDAL
    {
        SqlConnection? Connection = Database.Instance.Connection;
        public ProductDTO getById(int id)
        {
            var query = "SELECT * FROM Products WHERE id = @id";
            var command = new SqlCommand(query, Connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            ProductDTO? product = null;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                product = new ProductDTO
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    brand = (string)reader["brand"],
                    description = reader["description"] == DBNull.Value ? null : (string)reader["description"],
                    price = (decimal)reader["price"],
                    promotionPrice = (decimal)reader["promotion_price"],
                    stock = (int)reader["stock"],
                    image = reader["image"] == DBNull.Value ? null : (string)reader["image"],
                    categoryID = (int)reader["category_id"],
                    promotionID = reader["promotion_id"] == DBNull.Value ? null : (int)reader["promotion_id"]
                };
            }
            reader.Close();

            return product;
        }

        public ObservableCollection<ProductDTO> getAll()
        {
            var query = "SELECT * FROM Products";
            var command = new SqlCommand(query, Connection);
            var reader = command.ExecuteReader();

            ObservableCollection<ProductDTO> list = new ObservableCollection<ProductDTO>();

            while (reader.Read())
            {
                ProductDTO product = new ProductDTO
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    brand = (string)reader["brand"],
                    description = reader["description"] == DBNull.Value ? null : (string)reader["description"],
                    price = (decimal)reader["price"],
                    promotionPrice = (decimal)reader["promotion_price"],
                    stock = (int)reader["stock"],
                    image = reader["image"] == DBNull.Value ? null : (string)reader["image"],
                    categoryID = (int)reader["category_id"],
                    promotionID = reader["promotion_id"] == DBNull.Value ? null : (int)reader["promotion_id"]
                };
                list.Add(product);
            }
            reader.Close();

            return list;
        }

        public async Task<int> countAll()
        {
            var query = "SELECT COUNT(*) AS total FROM Products";
            var command = new SqlCommand(query, Connection);
            var total = await command.ExecuteScalarAsync();
            int result = total == null ? 0 : (int)total;

            return result;
        }

        public async Task<ObservableCollection<ProductDTO>> getTop5()
        {
            ObservableCollection<ProductDTO> list = new ObservableCollection<ProductDTO>();

            await Task.Run(() =>
            {
                var query = "SELECT TOP 5 * FROM Products WHERE stock <= 5 ORDER BY stock";
                var command = new SqlCommand(query, Connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductDTO product = new ProductDTO
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        brand = (string)reader["brand"],
                        description = reader["description"] == DBNull.Value ? null : (string)reader["description"],
                        price = (decimal)reader["price"],
                        promotionPrice = (decimal)reader["promotion_price"],
                        stock = (int)reader["stock"],
                        image = reader["image"] == DBNull.Value ? null : (string)reader["image"],
                        categoryID = (int)reader["category_id"],
                        promotionID = reader["promotion_id"] == DBNull.Value ? null : (int)reader["promotion_id"]
                    };
                    list.Add(product);
                }
                reader.Close();
            });

            return list;
        }

        public int create(ProductDTO product)
        {
            var query = """
				INSERT INTO Products (name, description, brand, price, promotion_price, stock, image, category_id, promotion_id)
				VALUES (@name, @description, @brand, @price,@promotionPrice, @stock, @image, @categoryID, @promotionID);
				SELECT ident_current('Products')
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.name;
            command.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.description == null ? DBNull.Value : product.description;
            command.Parameters.Add("@brand", SqlDbType.NVarChar).Value = product.brand;
            command.Parameters.Add("@price", SqlDbType.Money).Value = product.price;
            command.Parameters.Add("@promotionPrice", SqlDbType.Money).Value = product.promotionPrice;
            command.Parameters.Add("@stock", SqlDbType.Int).Value = product.stock;
            command.Parameters.Add("@image", SqlDbType.Text).Value = product.image == null ? DBNull.Value : product.image;
            command.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.categoryID;
            command.Parameters.Add("@promotionID", SqlDbType.Int).Value = product.promotionID == null ? DBNull.Value : product.promotionID;

            int id = (int)((decimal)command.ExecuteScalar());

            return id;
        }

        public int update(ProductDTO product)
        {
            var query = """
				UPDATE Products
				SET name =  @name, description = @description, brand = @brand, price = @price,promotion_price = @promotionPrice, stock = @stock ,image = @image, category_id = @categoryID, promotion_id = @promotionID
				WHERE id = @id
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = product.id;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.name;
            command.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.description == null ? DBNull.Value : product.description;
            command.Parameters.Add("@brand", SqlDbType.NVarChar).Value = product.brand;
            command.Parameters.Add("@price", SqlDbType.Money).Value = product.price;
            command.Parameters.Add("@promotionPrice", SqlDbType.Money).Value = product.promotionPrice;
            command.Parameters.Add("@stock", SqlDbType.Int).Value = product.stock;
            command.Parameters.Add("@image", SqlDbType.Text).Value = product.image == null ? DBNull.Value : product.image;
            command.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.categoryID;
            command.Parameters.Add("@promotionID", SqlDbType.Int).Value = product.promotionID == null ? DBNull.Value : product.promotionID;

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        public int updateThumbnail(int id, string token)
        {
            string query = $"""
                UPDATE Products
                SET image = 'Assets/Images/Product/{token}.png'
                WHERE id = @id
                """;
            var command = new SqlCommand(query, Connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        public int delete(int id)
        {
            var query = "DELETE Products WHERE id = @id";
            var command = new SqlCommand(query, Connection);
            int isSuccess = 1;
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            try
            {
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                isSuccess = 0;
            }

            return isSuccess;
        }
    }
}
