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
    /// Interaction logic for UpdateCustomerScreen.xaml
    /// </summary>
    public partial class UpdateCustomerScreen : Page
    {
        Frame _pageNavigation;
        private UserDTO _customer;

        public UpdateCustomerScreen(UserDTO customer, Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
            _customer = customer;
            DataContext = _customer;
            if (_customer.gender.Trim() == "Nam")
            {
                genderCombobox.SelectedIndex = 0;
            }
            else if (_customer.gender.Trim() == "Nữ")
            {
                genderCombobox.SelectedIndex = 1;
            }
            else
            {
                genderCombobox.SelectedIndex = 2;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void updateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text.Trim() == "" || phoneNumberTextBox.Text.Trim() == "" ||
                dobPicker.SelectedDate == null || addressTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (genderCombobox.SelectedIndex == 0)
            {
                _customer.gender = "Nam";
            }
            else if (genderCombobox.SelectedIndex == 1)
            {
                _customer.gender = "Nữ";
            }
            else
            {
                _customer.gender = "Khác";
            }

            var customerBLL = new UserBLL();
            int result = customerBLL.updateCustomer(_customer);

            if(result > 0)
            {
                MessageBox.Show("Chỉnh sửa khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _pageNavigation.NavigationService.GoBack();
            }    
        }

        private void deleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult choice = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (choice == MessageBoxResult.OK)
            {
                var customerBLL = new UserBLL();
                int result = customerBLL.deleteCustomer(_customer.id);

                if(result > 0)
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    _pageNavigation.NavigationService.GoBack();
                }    
            }
        }
    }
}
