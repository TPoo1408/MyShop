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
    public class PromotionDAL
    {
        SqlConnection? Connection = Database.Instance.Connection;

        public PromotionDTO getById(int id)
        {
            var query = "SELECT * FROM Promotions WHERE id = @id";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            PromotionDTO? promotion = null;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                promotion = new PromotionDTO
                {
                    id = (int)reader["id"],
                    code = (string)reader["code"],
                    discountPercentage = (int)reader["discount_percentage"]
                };
            }
            reader.Close();

            return promotion;
        }

        public ObservableCollection<PromotionDTO> getAll()
        {
            ObservableCollection<PromotionDTO> list = new ObservableCollection<PromotionDTO>();

            var sql = "SELECT * FROM Promotions";
            var command = new SqlCommand(sql, Connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                PromotionDTO promotion = new PromotionDTO
                {
                    id = (int)reader["id"],
                    code = (string)reader["code"],
                    discountPercentage = (int)reader["discount_percentage"]
                };

                list.Add(promotion);
            }
            reader.Close();

            return list;
        }

        public int create(PromotionDTO promotion)
        {
            var query = """
				INSERT INTO Promotions (code, discount_percentage)
				VALUES (@code, @discountPercentage);
				SELECT ident_current('Promotions')
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@code", SqlDbType.VarChar).Value = promotion.code;
            command.Parameters.Add("@discountPercentage", SqlDbType.Int).Value = promotion.discountPercentage;

            int id = (int)((decimal)command.ExecuteScalar());

            return id;
        }

        public int update(PromotionDTO promotion)
        {
            var query = """
				UPDATE Promotions
				SET code = @code,
					discount_percentage = @discountPercentage
				WHERE id = @id
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = promotion.id;
            command.Parameters.Add("@code", SqlDbType.VarChar).Value = promotion.code;
            command.Parameters.Add("@discountPercentage", SqlDbType.Int).Value = promotion.discountPercentage;

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        public int delete(int id)
        {
            var sql = """
                DELETE Promotions 
                WHERE id = @id
                """;
            int isSuccess = 1;
            var command = new SqlCommand(sql, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            try
            {
                command.ExecuteNonQuery();
                
            } catch (SqlException ex)
            {
                isSuccess = 0;
            }
            

            return isSuccess;
        }
    }
}
