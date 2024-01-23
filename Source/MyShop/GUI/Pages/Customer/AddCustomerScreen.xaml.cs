using MyShop.BLL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.GUI.Pages.Customer
{
    /// <summary>
    /// Interaction logic for AddCustomerScreen.xaml
    /// </summary>
    public partial class AddCustomerScreen : Page
    {
        Frame _pageNavigation;
        public AddCustomerScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void saveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text.Trim() == "" || phoneNumberTextBox.Text.Trim() == "" ||
                dobPicker.SelectedDate == null || addressTextBox.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var customerDTO = new UserDTO();

            customerDTO.username = "";
            customerDTO.password = "";
            customerDTO.role = "Customer";
            customerDTO.name = nameTextBox.Text;
            customerDTO.phoneNumber = phoneNumberTextBox.Text;
            customerDTO.dateOfBirth = (DateTime)dobPicker.SelectedDate;
            customerDTO.address = addressTextBox.Text;

            if (genderCombobox.SelectedIndex == 0)
            {
                customerDTO.gender = "Nam";
            }
            else if (genderCombobox.SelectedIndex == 1)
            {
                customerDTO.gender = "Nữ";
            }
            else
            {
                customerDTO.gender = "Khác";
            }

            UserBLL customerBLL = new UserBLL();
            int id = customerBLL.addCustomer(customerDTO);

            if(id > 0)
            {
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _pageNavigation.NavigationService.GoBack();
            }    
        }
    }
}
