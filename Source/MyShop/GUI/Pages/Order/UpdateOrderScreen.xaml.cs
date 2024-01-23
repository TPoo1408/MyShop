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
    /// Interaction logic for UpdateOrderScreen.xaml
    /// </summary>
    public partial class UpdateOrderScreen : Page
    {
        private Frame _navigation;
        private OrderDetailScreen.OrderInfo? _orderInfo;
        private ObservableCollection<OrderDetailScreen.Resource>? _listData;

        private ProductBLL _productBLL;
        private UserBLL _customerBLL;
        private OrderBLL _orderBLL;
        private OrderItemBLL _orderItemBLL;

        private ProductDTO _currentProduct;
        private ObservableCollection<UserDTO> _listCustomer;
        private ObservableCollection<ProductDTO> _listProducts;
        private Decimal _currentTotalPrice = 0;


        public UpdateOrderScreen(Frame pageNavigation, OrderDetailScreen.OrderInfo orderInfo, ObservableCollection<OrderDetailScreen.Resource> dataList)
        {
            InitializeComponent();
            _navigation = pageNavigation;
            _orderInfo = orderInfo;
            _listData = dataList;
            _currentProduct = new ProductDTO();

            _productBLL = new ProductBLL();
            _customerBLL = new UserBLL();
            _orderBLL = new OrderBLL();
            _orderItemBLL = new OrderItemBLL();

            _listProducts = _productBLL.getAllProduct();
            _currentProduct = (ProductDTO)_listProducts[0].Clone();
  
            _listCustomer = _customerBLL.getAllCustomer();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProductsAndCustomers();
            LoadOrderInfo();
            LoadOrderItems();
        }

        private void LoadProductsAndCustomers()
        {
            ListOfProducts.ItemsSource = _listProducts;
            ListOfProducts.SelectedIndex = 0;
            DataContext = _currentProduct;

            ListOfCustomer.ItemsSource = _listCustomer;
            SetSelectedCustomer();
        }
        private void SetSelectedCustomer()
        {
            for (int i = 0; i < _listCustomer.Count; i++)
            {
                if (_listCustomer[i].id == _orderInfo!.Customer.id)
                {
                    ListOfCustomer.SelectedIndex = i;
                    break;
                }
            }
        }

        private void LoadOrderInfo()
        {
            _currentTotalPrice = (decimal)_orderInfo!.Order.totalRevenue!;
            TotalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
        }

        private void LoadOrderItems()
        {
            ordersListView.ItemsSource = _listData;
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Đơn hàng chưa được lưu. Bạn có muốn tiếp tục không?", "Thông Báo",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                RestoreDataAndNavigateBack();
            }
        }

        private void RestoreDataAndNavigateBack()
        {
            _listData!.Clear();
            List<OrderItemDTO> _listOrderItem = _orderItemBLL.getAllOrderItemByOrderId(_orderInfo!.Order.id);

            foreach (var orderItem in _listOrderItem)
            {
                var product = _productBLL.getProductById(orderItem.productID);
                OrderDetailScreen.Resource item = new OrderDetailScreen.Resource();
                item.Product = product;
                item.OrderItem = orderItem;
                _listData.Add(item);
            }

            UserDTO _customer = _customerBLL.getCustomerById(_orderInfo!.Order.customerID);
            _orderInfo.Customer = _customer;
            _navigation.NavigationService.GoBack();
        }

        private void ProductComboboxSelected(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ListOfProducts.SelectedIndex;

            if (selectedIndex != -1)
            {
             
                _currentProduct = (ProductDTO)_listProducts[selectedIndex].Clone();
                DataContext = _currentProduct;
            }
                
        }



        private void RemoveSelectedOrderItem()
        {
            int index = ordersListView.SelectedIndex;
            

                _currentTotalPrice -= _listData![index].OrderItem.totalPrice;


                var deleteProduct = _listProducts.First(product => product.id == _listData![index].OrderItem.productID);
                deleteProduct.stock += _listData![index].OrderItem.quantity;

                if (deleteProduct.id == _currentProduct.id)
                {
                    _currentProduct = (ProductDTO)deleteProduct.Clone();

                }

                TotalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
                _listData.RemoveAt(index);
            
        }

  

        private bool TryParseQuantity(out int quantity)
        {
            return int.TryParse(QuantityTermTextBox.Text, out quantity);
        }

        private bool IsQuantityValid(int quantity)
        {
            return quantity > 0 && quantity <= _currentProduct.stock;
        }

        private ProductDTO GetSelectedProduct()
        {
            return (ProductDTO)((ProductDTO)ListOfProducts.SelectedValue).Clone();
        }

        private bool IsProductInList(ProductDTO product)
        {
            return _listData.Any(item => item.Product.id == product.id);
        }

        private void ShowMessageWarning(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        

        private void AddProductToOrder(ProductDTO product, int quantity)
        {
            var orderItemDTO = new OrderItemDTO();
            decimal priceOfProduct = product.promotionPrice;
            orderItemDTO.productID = product.id;
            orderItemDTO.quantity = quantity;
            orderItemDTO.totalPrice = priceOfProduct * quantity;

            _listData.Add(new OrderDetailScreen.Resource()
            {
                Product = product,
                OrderItem = orderItemDTO,
            });

            _currentProduct.stock -= quantity;
            _listProducts.First(p => p.id == _currentProduct.id).stock -= quantity;
            _currentTotalPrice += orderItemDTO.totalPrice;
            TotalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
        }

        private void AddProductOnClick(object sender, RoutedEventArgs e)
        {
            if (!TryParseQuantity(out int quantity))
            {
                ShowMessageWarning("Số lượng không hợp lệ! Vui lòng nhập lại!");
                return;
            }

            if (!IsQuantityValid(quantity))
            {
                ShowMessageWarning("Số lượng sản phẩm phải nhỏ hơn số lượng tồn kho!");
                return;
            }

            var selectedProduct = GetSelectedProduct();

            if (IsProductInList(selectedProduct))
            {
                ShowMessageWarning("Vui lòng xóa sản phẩm cũ để cập nhật số lượng!");
                return;
            }

            AddProductToOrder(selectedProduct, quantity);
        }

        private void SaveOrderOnClick(object sender, RoutedEventArgs e)
        {
            if (_listData!.Count > 0)
            {
                UpdateOrderDetails();
            }
            else
            {
                ShowMessageWarning("Bạn chưa chọn sản phẩm nào! Vui lòng chọn sản phẩm để cập nhật đơn hàng!");
            }
        }

        private void UpdateOrderDetails()
        {
            var customerDTO = (UserDTO)ListOfCustomer.SelectedValue;
            var order = _orderInfo!.Order;

            order.customerID = customerDTO.id;
            order.totalRevenue = _currentTotalPrice;
            order.totalProfit = _orderBLL.calculateProfit(_currentTotalPrice);

            _orderBLL.updateOrder(order);

            int orderID = order.id;
            _orderItemBLL.deleteOrderItemByOrderId(orderID);

            foreach (var item in _listData)
            {
                item.OrderItem.orderID = orderID;
                _orderItemBLL.addOrderItem(item.OrderItem);
            }

            ShowOrderUpdateSuccessMessage();
            _navigation.NavigationService.GoBack();
        }


        private void ShowOrderUpdateSuccessMessage()
        {
            MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông Báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteOrderItemOnClick(object sender, RoutedEventArgs e)
        {
            int index = ordersListView.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Vui lòng ấn chọn sản phẩm để xóa!", "Thông Báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var result = MessageBox.Show("Bạn có muốn xóa sản phẩm này ra khỏi đơn hàng không?", "Thông Báo",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    RemoveSelectedOrderItem();
                }
            }
        }
    }
}
