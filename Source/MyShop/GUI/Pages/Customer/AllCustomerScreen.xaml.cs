using MyShop.BLL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AllCustomerScreen.xaml
    /// </summary>
    public partial class AllCustomerScreen : Page
    {
        Frame _pageNavigation;
        private UserBLL _customerBLL;
        private ObservableCollection<UserDTO>? _listCustomers = null;
        private int _currentPage = 1;
        private int _rowsPerPage = 8;
        private int _totalItems = 0;
        private int _totalPages = 0;
        private string query = "";

        public AllCustomerScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _customerBLL = new UserBLL();
            _pageNavigation = pageNavigation;
        }

        private void PageCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            updateDataSource();
        }

        private void updateDataSource()
        {
            (_listCustomers, _totalItems) = _customerBLL.getCustomersBySearchQuery(_currentPage, _rowsPerPage, query) ;
            customersListView.ItemsSource = _listCustomers;
            infoTextBlock.Text = $"Đang hiển thị {_listCustomers.Count}/{_totalItems} khách hàng";
            updatePagingInfo();
        }

        private void updatePagingInfo()
        {
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            if (_totalPages == 0) _totalPages = 1;

            pageInfoTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        private void addCustomer_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new AddCustomerScreen(_pageNavigation));
        }

        private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int i = customersListView.SelectedIndex;
            var customer = _listCustomers![i];

            if (customer != null)
            {
                _pageNavigation.NavigationService.Navigate(new UpdateCustomerScreen(customer, _pageNavigation));
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            updateDataSource();
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                updateDataSource();
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                updateDataSource();
            }
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _totalPages;
            updateDataSource();
        }

        private void searchCustomerBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                query = searchCustomerTextBox.Text.Trim();
                _currentPage = 1;
                updateDataSource();
            }
        }
    }
}
