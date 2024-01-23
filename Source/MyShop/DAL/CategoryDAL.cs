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
    public class CategoryDAL
    {
        SqlConnection? Connection = Database.Instance.Connection;
        public CategoryDTO getById(int id)
        {
            var query = "SELECT * FROM Categories WHERE id = @id";
            var command = new SqlCommand(query, Connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            CategoryDTO? category = null;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                category = new CategoryDTO
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    description = reader["description"] == DBNull.Value ? null : (string)reader["description"],
                    image = reader["image"] == DBNull.Value ? null : (string)reader["image"]
                };
            }
            reader.Close();

            return category;
        }

        public ObservableCollection<CategoryDTO> getAll()
        {

            var query = "SELECT * FROM Categories";
            var command = new SqlCommand(query, Connection);
            var reader = command.ExecuteReader();

            ObservableCollection<CategoryDTO> list = new ObservableCollection<CategoryDTO>();

            while (reader.Read())
            {
                CategoryDTO category = new CategoryDTO
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    description = reader["description"] == DBNull.Value ? null : (string)reader["description"],
                    image = reader["image"] == DBNull.Value ? null : (string)reader["image"]
                };
                list.Add(category);
            }
            reader.Close();

            return list;
        }

        public int create(CategoryDTO category)
        {
            var query = """
				INSERT INTO Categories (name, description)
				VALUES (@name, @description);
				SELECT ident_current('Categories')
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = category.name;
            command.Parameters.Add("@description", SqlDbType.NVarChar).Value = category.description;

            int id = (int)((decimal)command.ExecuteScalar());

            return id;
        }

        public int update(CategoryDTO category)
        {
            var query = """
				UPDATE Categories
				SET name =  @name, description = @description
				WHERE id = @id
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = category.id;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = category.name;
            command.Parameters.Add("@description", SqlDbType.NVarChar).Value = category.description == null ? DBNull.Value : category.description;

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        public int delete(int id)
        {
            var query = "DELETE Categories WHERE id = @id";
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
