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

namespace MyShop.GUI.Pages.Order
{
    /// <summary>
    /// Interaction logic for AllOrderScreen.xaml
    /// </summary>
    public partial class AllOrderScreen : Page
    {
        public class Resource
        {
            public int id { get; set; }
            public DateTime orderDate { get; set; }
            public string customerName { get; set; }
            public string totalRevenue { get; set; }

            public Resource(OrderDTO order)
            {
                InitializeData(order);
            }

            private void InitializeData(OrderDTO order)
            {
                var customerBLL = new UserBLL();
                id = order.id;
                orderDate = order.orderDate.Date;
                customerName = customerBLL.getCustomerById(order.customerID).name;
                totalRevenue = FormatFinalTotal((decimal)order.totalRevenue);
            }

            private string FormatFinalTotal(decimal finalTotal)
            {
                return string.Format("{0:N0} đ", finalTotal);
            }
        }

        private Frame _navigation;
        private OrderBLL _orderBLL;
        private List<OrderDTO>? _listOrder = null;
        private ObservableCollection<Resource>? _listData = null;
        private int _currentPage = 1;
        private int _rowsPerPage = 8;
        private int _totalItems = 0;
        private int _totalPages = 0;
        private DateTime? _currentStartDate = null;
        private DateTime? _currentEndDate = null;

        

        public AllOrderScreen(Frame pageNavigation)
        {
            _navigation = pageNavigation;
            _orderBLL = new OrderBLL();
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            UpdateDataSource();
        }

        private void UpdateDataSource()
        {
            _listData = new ObservableCollection<Resource>();
            RetrieveOrdersAndRefreshList();

            DisplayOrdersInListView();
            DisplayInfoText();
            UpdatePagingInfo();
        }

        private void RetrieveOrdersAndRefreshList()
        {
            (_listOrder, _totalItems) = _orderBLL.getOrderBySearch(_currentPage, _rowsPerPage, _currentStartDate, _currentEndDate);

            foreach (var order in _listOrder)
            {
                _listData.Add(new Resource(order));
            }
        }

        private void DisplayOrdersInListView()
        {
            ordersListView.ItemsSource = _listData;
        }

        private void DisplayInfoText()
        {
            infoTextBlock.Text = $"Đang hiển thị {_listOrder.Count}/{_totalItems} hóa đơn";
        }

        private void UpdatePagingInfo()
        {
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            _totalPages = _totalPages == 0 ? 1 : _totalPages;

            pageText.Text = $"{_currentPage}/{_totalPages}";
        }

        private void AddOrderClick(object sender, RoutedEventArgs e)
        {
            NavigateToAddOrderPage();
        }

        private void NavigateToAddOrderPage()
        {
            _navigation.NavigationService.Navigate(new AddOrderScreen(_navigation));
        }

        private void ListViewItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = ordersListView.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _listOrder.Count)
            {
                var order = _listOrder[selectedIndex];
                _navigation.NavigationService.Navigate(new OrderDetailScreen(order, _navigation));

            }
        }

        private void UpdateCurrentPage(int page)
        {
            if (page >= 1 && page <= _totalPages)
            {
                _currentPage = page;
                UpdateDataSource();
            }
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(_currentPage - 1);
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(_currentPage + 1);
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(1);
        }
        private void EndButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(_totalPages);
        }

        private void SearchOnClick(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            _currentStartDate = StartDate.SelectedDate;
            _currentEndDate = EndDate.SelectedDate;
            UpdateDataSource();
            UpdatePagingInfo();
        }
    }
}
