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

namespace MyShop.GUI.Pages.Product
{
    /// <summary>
    /// Interaction logic for ProductDetailScreen.xaml
    /// </summary>
    public partial class ProductDetailScreen : Page
    {
        private ProductDTO _product;
        private CategoryDTO _category;

        private Frame _navigation;
        private ProductBLL _productBLL;
        private CategoryBLL _categoryBLL;
        private PromotionBLL _promotionBLL;
        protected PromotionDTO? _promotion;
        private HomeScreen _homeScreen;
        private Resource _currentProduct;

        class Resource : INotifyPropertyChanged
        {
            public ProductDTO Product { get; set; }
            public CategoryDTO Category { get; set; }
            public PromotionDTO Promotion { get; set; }

            public Resource(ProductDTO productDTO, CategoryDTO categoryDTO, PromotionDTO? promotion)
            {
                this.Product = productDTO;
                this.Category = categoryDTO;
                if (promotion != null)
                {
                    this.Promotion = promotion;
                }
                else
                {
                    this.Promotion = new();
                    this.Promotion.discountPercentage = 0;
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

       
        public ProductDetailScreen(HomeScreen homeScreen, ProductDTO product, Frame pageNavigation)
        {
            InitializeComponent();
            _homeScreen = homeScreen;
            _product = product;
            _navigation = pageNavigation;
            _productBLL = new ProductBLL();
            _categoryBLL = new CategoryBLL();
            _promotionBLL = new PromotionBLL();

            LoadData();
        }

        private void LoadData()
        {
            LoadCategoryAndPromotions();
            SetDataContext();
        }

        private void LoadCategoryAndPromotions()
        {
            _category = _categoryBLL.getCategoryById(_product.categoryID);
            var promotions = _promotionBLL.getAllPromotion();
            promotions.Insert(0, new PromotionDTO() { code = "Chưa áp dụng" });
            LoadPromotionsComboBox(promotions);
            LoadProductPromotion(promotions);

        }

        private void LoadPromotionsComboBox(ObservableCollection<PromotionDTO> promotions)
        {
            ListOfPromotion.ItemsSource = promotions;
        }

        private void LoadProductPromotion(ObservableCollection<PromotionDTO> promotions)
        {
            if (_product.promotionID != null)
            {
                _promotion = _promotionBLL.getPromotionById((int)_product.promotionID);
                PromotionComboboxSelected(promotions);
            }
            else
            {
                ListOfPromotion.SelectedIndex = 0;
                _promotion = null;
            }
        }

        private void PromotionComboboxSelected(ObservableCollection<PromotionDTO> promotions)
        {
            ListOfPromotion.SelectedValue = (PromotionDTO)promotions.Where(item => item.id == _product.promotionID).ToList()[0];
        }

        private void SetDataContext()
        {
            Resource data = new Resource(_product, _category, _promotion);
            _currentProduct = data;
            DataContext = data;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.GoBack();
        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            var confirmation = ShowConfirmationDialog("Bạn có chắc chắn muốn xóa không?");
            if (confirmation == MessageBoxResult.OK)
            {
                DeleteProductAndUpdateNavigation();
            }
        }

        private void DeleteProductAndUpdateNavigation()
        {
            var isSuccess =  _productBLL.deleteProduct(_product.id);
            if (isSuccess == 0)
            {
                MessageBox.Show("Lỗi!!! Đang có đơn hàng đặt sản phẩm này!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            else
                _navigation.NavigationService.GoBack();
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.Navigate(new UpdateProductScreen(_product, _category, _navigation));


        }

        int firstTime = 0; 
        private void ListOfPromotionSelected(object sender, SelectionChangedEventArgs e)
        {
            var promotion = (PromotionDTO)ListOfPromotion.SelectedValue;
            ApplySelectedPromotion(promotion);
        }

        private void ApplySelectedPromotion(PromotionDTO promotion)
        {
            if (promotion != null && firstTime != 0)
            {
                UpdateProductWithPromotion(promotion);
                ShowPromotionAppliedMessage();
            }
            firstTime = 1;
        }

        private void UpdateProductWithPromotion(PromotionDTO promotion)
        {
            _currentProduct.Promotion = (PromotionDTO)promotion.Clone();
            _product.promotionID = _currentProduct.Promotion.id;
            double percent = 1 - promotion.discountPercentage * 1.0 / 100;
            _product.promotionPrice = (decimal)((double)_product.price * percent);
            _productBLL.updateProduct(_product);
            _promotion = promotion;
        }

        private void ShowPromotionAppliedMessage()
        {
            MessageBox.Show("Đã áp dụng mã khuyến mãi thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private MessageBoxResult ShowConfirmationDialog(string message)
        {
            return MessageBox.Show(message, "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}
