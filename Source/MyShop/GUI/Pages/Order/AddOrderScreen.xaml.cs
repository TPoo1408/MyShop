using MyShop.BLL;
using MyShop.DTO;
using MyShop.GUI.Pages.Customer;
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
    /// Interaction logic for AddOrderScreen.xaml
    /// </summary>
    public partial class AddOrderScreen : Page
    {
       
        private Frame _navigation;
        private ProductBLL _productBLL;
        private UserBLL _customerBLL;
        private OrderBLL _orderBLL;
        private OrderItemBLL _orderItemBLL;

        private ObservableCollection<Resource> _data;
        private ObservableCollection<UserDTO> _listCustomer;
        private ObservableCollection<ProductDTO> _listProduct;

        private ProductDTO _currentProduct;
        private Decimal _currentTotalPrice = 0;
        private List<OrderItemDTO> _listOrderItem;

        public class Resource
        {
            public string nameProduct { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }
            public decimal totalPrice { get; set; }
        }

        public AddOrderScreen(Frame pageNavigation)
        {
            _navigation = pageNavigation;

            _productBLL = new ProductBLL();
            _customerBLL = new UserBLL();
            _orderBLL = new OrderBLL();
            _orderItemBLL = new OrderItemBLL();

            _currentProduct = new ProductDTO();
            _data = new ObservableCollection<Resource>();
            _listOrderItem = new List<OrderItemDTO>();

       
            InitializeData();
            InitializeComponent();
        }

       
        private void InitializeData()
        {
            _listProduct = _productBLL.getAllProduct();

            _currentProduct = (ProductDTO)_listProduct[0].Clone();
            _listCustomer = _customerBLL.getAllCustomer();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeProductAndCustomerComboBoxes();
            SetInitialValues();
        }

        private void InitializeProductAndCustomerComboBoxes()
        {
            _listCustomer = _customerBLL.getAllCustomer();
            ListOfProducts.ItemsSource = _listProduct;
            ListOfCustomer.ItemsSource = _listCustomer;
        }

        private void SetInitialValues()
        {
            ListOfProducts.SelectedIndex = 0;
            ListOfCustomer.SelectedIndex = 0;
            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
            ListOfOrder.ItemsSource = _data;
            this.DataContext = _currentProduct;
        }

        private void SaveOrderOnClick(object sender, RoutedEventArgs e)
        {
            if (_listOrderItem.Count > 0)
            {
                SaveShopOrder();
            }
            else
            {
                ShowNoProductSelectedMessage();
            }
        }

        private void SaveShopOrder()
        {
            var newOrder = CreateShopOrder();
            int orderID = _orderBLL.addOrder(newOrder);

            SavePurchasesToDatabase(orderID);

            ShowOrderSavedSuccessfullyMessage();
            _navigation.NavigationService.GoBack();
        }

        private OrderDTO CreateShopOrder()
        {
            var customerDTO = (UserDTO)ListOfCustomer.SelectedValue;
            var newOrder = new OrderDTO
            {
                customerID = customerDTO.id,
                orderDate = DateTime.Now.Date,
                totalRevenue = _currentTotalPrice,
                totalProfit = _orderBLL.calculateProfit(_currentTotalPrice)
            };
            return newOrder;
        }

        private void SavePurchasesToDatabase(int orderID)
        {
            foreach (var item in _listOrderItem)
            {
                item.orderID = orderID;
                _orderItemBLL.addOrderItem(item);
            }
        }

        private void ShowNoProductSelectedMessage()
        {
            MessageBox.Show("Bạn chưa chọn sản phẩm nào! Vui lòng chọn sản phẩm để tạo đơn hàng!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void AddProductOnClick(object sender, RoutedEventArgs e)
        {
            HandleAddingProduct();
        }

        private void HandleAddingProduct()
        {
            if (int.TryParse(QuantityTermTextBox.Text, out int quantity))
            {
                var productDTO = (ProductDTO)ListOfProducts.SelectedValue;

                if (IsQuantityInvalid(quantity))
                {
                    ShowInvalidQuantityMessage();
                    return;
                }

                var purchaseDTO = CreateOrderItem(productDTO, quantity);

                if (IsPurchaseAlreadyExists(purchaseDTO))
                {
                    ShowUpdateQuantityMessage();
                    return;
                }

                AddPurchaseAndRefreshUI(purchaseDTO, productDTO, quantity);
            }
            else
            {
                ShowQuantityMustBeNumberMessage();
            }
        }

        private bool IsQuantityInvalid(int quantity)
        {
            return quantity <= 0 || quantity > _currentProduct.stock;
        }

        private void ShowInvalidQuantityMessage()
        {
            MessageBox.Show("Số lượng sản phẩm phải nhỏ hơn số lượng tồn kho!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private OrderItemDTO CreateOrderItem(ProductDTO productDTO, int quantity)
        {
            var purchaseDTO = new OrderItemDTO
            {
                productID = productDTO.id,
                quantity = quantity,
                totalPrice = _currentProduct.promotionPrice * quantity
            };
            return purchaseDTO;
        }

        private bool IsPurchaseAlreadyExists(OrderItemDTO orderItem)
        {
            return _listOrderItem.Any(purchase => purchase.productID == orderItem.productID);
        }

        private void ShowUpdateQuantityMessage()
        {
            MessageBox.Show("Vui lòng xóa sản phẩm cũ để cập nhật số lượng!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void AddPurchaseAndRefreshUI(OrderItemDTO orderItem, ProductDTO productDTO, int quantity)
        {
            AddPurchaseToBuffer(orderItem);
            UpdateProductQuantityAndTotalPrice(orderItem, productDTO, quantity);
            DisplayPurchaseOnUI(orderItem, productDTO, quantity);
        }

        private void AddPurchaseToBuffer(OrderItemDTO orderItem)
        {
            _listOrderItem.Add(orderItem);
        }

        private void UpdateProductQuantityAndTotalPrice(OrderItemDTO orderItem, ProductDTO productDTO, int quantity)
        {
            _currentProduct.stock -= quantity;
            _listProduct.First(product => product.id == _currentProduct.id).stock -= quantity;
            _currentTotalPrice += orderItem.totalPrice;
        }

        private void DisplayPurchaseOnUI(OrderItemDTO orderItem, ProductDTO productDTO, int quantity)
        {
            var data = new Resource
            {
                quantity = quantity,
                price = _currentProduct.promotionPrice,
                nameProduct = productDTO.name!,
                totalPrice = orderItem.totalPrice
            };

            _data.Add(data);
            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
        }

        private void ShowQuantityMustBeNumberMessage()
        {
            MessageBox.Show("Số lượng không hợp lệ! Vui lòng nhập lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowOrderSavedSuccessfullyMessage()
        {
            MessageBox.Show("Thêm đơn hàng thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Đơn hàng chưa được lưu. Bạn có muốn tiếp tục không?", "Thông Báo",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                _navigation.NavigationService.GoBack();
            }
        }

      

        private void DeleteSelectedItemAndUpdateUI()
        {
            int selectedIndex = ListOfOrder.SelectedIndex;
            

                _currentTotalPrice -= _data[selectedIndex].totalPrice;
                FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);

                var deleteProduct = _listProduct.First(product => product.id == _listOrderItem[selectedIndex].productID);
                deleteProduct.stock += _listOrderItem[selectedIndex].quantity;

                if (deleteProduct.id == _currentProduct.id)
                {

                    _currentProduct = (ProductDTO)deleteProduct.Clone();
                }

                _data.RemoveAt(selectedIndex);
                _listOrderItem.RemoveAt(selectedIndex);
            
        }

        private void ProductComboboxSelected(object sender, SelectionChangedEventArgs e)
        {
            int index = ListOfProducts.SelectedIndex;

            if (index != -1)
            {
                _currentProduct = (ProductDTO)_listProduct[index].Clone();
                this.DataContext = _currentProduct;
            }
        }

        private void AddCustomerOnClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.Navigate(new AddCustomerScreen(_navigation));
        }

        private void DeleteOrderItemOnClick(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListOfOrder.SelectedIndex;
            if (selectedIndex == -1)
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
                    DeleteSelectedItemAndUpdateUI();
                }
            }
        }
    }
}
