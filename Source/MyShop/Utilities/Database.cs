using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop.Utilities
{
    class Database
    {
        private static Database? _instance = null;
        private SqlConnection? _connection;

        public SqlConnection? Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(ConnectionString); ;
                    _connection.Open();
                }

                return _connection;
            }
        }

        public string ConnectionString { get; set; } = "";
        public static Database Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }

                return _instance;
            }
        }
    }
}
