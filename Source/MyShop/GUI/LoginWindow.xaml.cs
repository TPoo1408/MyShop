using MyShop.BLL;
using MyShop.DTO;
using MyShop.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string inputUsername = usernameInput.Text;
            string inputPassword = passwordInput.Password;

            UserBLL userBLL = new UserBLL();
            UserDTO? user = userBLL.getUser(inputUsername, inputPassword);

            if (user != null)
            {
                if (rememberMe.IsChecked == true)
                {
                    string encryptedPassword = CryptoHelper.Encrypt(inputPassword);
                    Properties.Settings.Default.RememberMe = true;
                    Properties.Settings.Default.Username = inputUsername;
                    Properties.Settings.Default.Password = encryptedPassword;
                    Properties.Settings.Default.Save();
                }

                MainWindow mainPage = new MainWindow();
                mainPage.Show();
                Close();
            }
            else
            {
                errorMessage.Text = "Invalid username or password";
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerPage = new RegisterWindow();
            registerPage.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.RememberMe)
            {
                var encryptedPassword = Properties.Settings.Default.Password;
                var username = Properties.Settings.Default.Username;
                var password = CryptoHelper.Decrypt(encryptedPassword);

                if(password.Length > 0 && username.Length > 0)
                {
                    UserBLL userBLL = new UserBLL();
                    UserDTO? user = userBLL.getUser(username, password);

                    if(user != null)
                    {
                        MainWindow mainPage = new MainWindow();
                        mainPage.Show();
                        Close();
                    } else
                    {
                        LoginWindow loginPage = new LoginWindow();
                        loginPage.Show();
                        Close();
                    }   
                }    
            }
        }
    }
}
