using MyShop.BLL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for OrderDetailScreen.xaml
    /// </summary>
    public partial class OrderDetailScreen : Page
    {
        public class Resource : INotifyPropertyChanged
        {
            public ProductDTO Product { get; set; }
            public OrderItemDTO OrderItem { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        public class OrderInfo : INotifyPropertyChanged
        {
            public UserDTO Customer { get; set; }
            public OrderDTO Order { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

       
        private readonly OrderDTO _order;
        private ObservableCollection<Resource> _listData = new ObservableCollection<Resource>();
        private OrderInfo _currentOrderInfo = new OrderInfo();
        private readonly Frame _navigation;

        public OrderDetailScreen(OrderDTO order, Frame pageNavigation)
        {
            InitializeComponent();
            _navigation = pageNavigation;
            _order = order;

            LoadOrderData();
        }

        private void LoadOrderData()
        {
            var productBLL = new ProductBLL();
            var orderBLL = new OrderBLL();
            var customerBLL = new UserBLL();
            var orderItemBLL = new OrderItemBLL();

            LoadPurchases(productBLL, orderItemBLL);
            LoadOrderInfo(customerBLL);
            SetDataContext();
        }

        private void LoadPurchases(ProductBLL productBLL, OrderItemBLL orderItemBLL)
        {
            List<OrderItemDTO> listOrderItem = orderItemBLL.getAllOrderItemByOrderId(_order.id);

            foreach (var orderItem in listOrderItem)
            {
                var product = productBLL.getProductById(orderItem.productID);
                var item = new Resource { Product = product, OrderItem = orderItem };
                _listData.Add(item);
            }

            productsListView.ItemsSource = _listData;
        }

        private void LoadOrderInfo(UserBLL userBLL)
        {
            UserDTO customer = userBLL.getCustomerById(_order.customerID);
            _currentOrderInfo.Customer = customer;
            _currentOrderInfo.Order = _order;
        }

        private void SetDataContext()
        {
            DataContext = _currentOrderInfo;
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.GoBack();
        }

        private void DeleteOrderOnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult choice = ShowConfirmationMessage("Bạn có chắc chắn muốn xóa không?");

            if (choice == MessageBoxResult.OK)
            {
                var orderBLL = new OrderBLL();
                var isSuccess = orderBLL.deleteOrder(_order.id);
                
                _navigation.NavigationService.GoBack();
            }
        }

        private void UpdateOrderOnClick(object sender, RoutedEventArgs e)
        {
            NavigateToUpdateOrder();
        }

        private void NavigateToUpdateOrder()
        {
             _navigation.NavigationService.Navigate(new UpdateOrderScreen(_navigation, _currentOrderInfo, _listData));

        }

        private MessageBoxResult ShowConfirmationMessage(string message)
        {
            return MessageBox.Show(message, "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}
