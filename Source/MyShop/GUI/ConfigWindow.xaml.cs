using Microsoft.Data.SqlClient;
using MyShop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop.GUI
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }
        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            string server = ServerInput.Text;
            string username = UsernameInput.Text;
            string password = PasswordInput.Text;
            string database = DatabaseInput.Text;

            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = server;
            builder.InitialCatalog = database;
            builder.UserID = username;
            builder.Password = password;
            builder.TrustServerCertificate = true;

            string connectionString = builder.ConnectionString;
            var connection = await Task.Run(() => {
                var _connection = new SqlConnection(connectionString);

                try
                {
                    _connection.Open();
                }
                catch (Exception ex)
                {
                    _connection = null;
                    MessageBox.Show($"Can't connect to the database! {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return _connection;
            });
            if(connection != null)
            {
                Database.Instance.ConnectionString = connectionString;
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }    
        }
    }
}
