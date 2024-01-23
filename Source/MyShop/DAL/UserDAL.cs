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
    public class UserDAL
    {
        SqlConnection? Connection = Database.Instance.Connection;

        public bool exist (string username)
        {
            var query = "SELECT * FROM Users WHERE username = @username";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;

            var reader = command.ExecuteReader();

            UserDTO? user = null;
            if (reader.Read())
            {
                user = new UserDTO
                {
                    id = (int)reader["id"],
                    username = (string)reader["username"],
                    password = (string)reader["password"],
                    role = (string)reader["role"],
                    name = (string)reader["name"],
                    gender = (string)reader["gender"],
                    dateOfBirth = (DateTime)reader["dob"],
                    address = (string)reader["address"],
                    phoneNumber = (string)reader["phone_number"],
                };
            }
            reader.Close();

            return user != null;
        }

        public UserDTO getOne(string username, string password)
        {
            var query = "SELECT * FROM Users WHERE username = @username AND password = @password";
            var command = new SqlCommand(query, Connection);

            string encryptedPassword = CryptoHelper.Encrypt(password);

            command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@password", SqlDbType.VarChar, 1000).Value = encryptedPassword;

            var reader = command.ExecuteReader();

            UserDTO? user = null;
            if (reader.Read())
            {
                user = new UserDTO
                {
                    id = (int)reader["id"],
                    username = (string)reader["username"],
                    password = (string)reader["password"],
                    role = (string)reader["role"],
                    name = (string)reader["name"],
                    gender = (string)reader["gender"],
                    dateOfBirth = (DateTime)reader["dob"],
                    address = (string)reader["address"],
                    phoneNumber = (string)reader["phone_number"],
                };
            }
            reader.Close();
            return user;
        }

        public ObservableCollection<UserDTO> getAll()
        {
            ObservableCollection<UserDTO> list = new ObservableCollection<UserDTO>();

            var sql = "SELECT * FROM Users WHERE role = 'Customer'";
            var command = new SqlCommand(sql, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                UserDTO customer = new UserDTO
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    gender = (string)reader["gender"],
                    dateOfBirth = (DateTime)reader["dob"],
                    address = (string)reader["address"],
                    phoneNumber = (string)reader["phone_number"]
                };
                list.Add(customer);
            }
            reader.Close();

            return list;
        }

        public UserDTO getById(int id)
        {
            var sql = "SELECT * FROM Users WHERE id = @id";
            var command = new SqlCommand(sql, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();

            UserDTO? customer = null;
            while (reader.Read())
            {
                customer = new UserDTO
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    gender = (string)reader["gender"],
                    dateOfBirth = (DateTime)reader["dob"],
                    address = (string)reader["address"],
                    phoneNumber = (string)reader["phone_number"]
                };
            }
            reader.Close();

            return customer;
        }

        public int create(UserDTO customer)
        {
            var query = """
				INSERT INTO Users (username, password, name, role, gender, dob, address, phone_number) 
				VALUES (@username, @password, @name, @role, @gender, @dob, @address, @phoneNumber);
				SELECT ident_current('Users');
				""";

            var command = new SqlCommand(query, Connection);
            command.Parameters.Add("@username", SqlDbType.Text).Value = customer.username;
            command.Parameters.Add("@password", SqlDbType.Text).Value = customer.password;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = customer.name;
            command.Parameters.Add("@role", SqlDbType.Text).Value = customer.role;
            command.Parameters.Add("@gender", SqlDbType.NVarChar).Value = customer.gender;
            command.Parameters.Add("@dob", SqlDbType.DateTime).Value = customer.dateOfBirth;   
            command.Parameters.Add("@address", SqlDbType.NVarChar).Value = customer.address;
            command.Parameters.Add("@phoneNumber", SqlDbType.Text).Value = customer.phoneNumber;

            int id = (int)((decimal)command.ExecuteScalar());

            return id;
        }

        public int update(UserDTO customer)
        {
            var query = """
				UPDATE Users
				SET name = @name, 
					gender = @gender,
					dob = @dob,
					address = @address,
					phone_number = @phoneNumber
				WHERE id = @id
				""";
            var command = new SqlCommand(query, Connection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = customer.id;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = customer.name;
            command.Parameters.Add("@gender", SqlDbType.NVarChar).Value = customer.gender;
            command.Parameters.Add("@dob", SqlDbType.Date).Value = customer.dateOfBirth;
            command.Parameters.Add("@address", SqlDbType.NVarChar).Value = customer.address;
            command.Parameters.Add("@phoneNumber", SqlDbType.Text).Value = customer.phoneNumber;

            int rowAffected = command.ExecuteNonQuery();

            return rowAffected;
        }

        public int delete (int id)
        {
            var sql = "DELETE Users WHERE id = @id";
            var command = new SqlCommand(sql, Connection);
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
