using MyShop.BLL;
using MyShop.DTO;
using MyShop.Utilities;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            string inputUsername = usernameInput.Text;
            string inputPassword = passwordInput.Password;
            string inputFullname = fullNameInput.Text;
            string inputAdress = addressInput.Text;
            string inputNumberphone = phoneNumberInput.Text;

            if (string.IsNullOrEmpty(inputUsername) || string.IsNullOrEmpty(inputPassword) ||
                string.IsNullOrEmpty(inputFullname) || string.IsNullOrEmpty(inputAdress) ||
                string.IsNullOrEmpty(inputNumberphone))
            {
                errorMessage.Text = "Please enter all information";
            }
            else
            {
                UserBLL userBLL = new UserBLL();
                bool isExist = userBLL.existUser(inputUsername);
                if(isExist)
                {
                    errorMessage.Text = "User already existed";
                } else
                {
                    string encryptedPassword = CryptoHelper.Encrypt(inputPassword);
                    UserDTO userDTO = new UserDTO
                    {
                        username = inputUsername,
                        password = encryptedPassword,
                        name = inputFullname,
                        role = "Admin",
                        gender = "Nam",
                        dateOfBirth = DateTime.Parse("11/09/2003"),
                        address = inputAdress,
                        phoneNumber = inputNumberphone,
                    };
                    int signupSucess = userBLL.createNewUser(userDTO);
                    if (signupSucess > 0)
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                    }
                    else
                    {
                        errorMessage.Text = "Failed to create new account";
                    }
                } 
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
