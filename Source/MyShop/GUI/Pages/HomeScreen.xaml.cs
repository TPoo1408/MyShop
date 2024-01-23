using Microsoft.Win32;
using MyShop.BLL;
using MyShop.DAL;
using MyShop.DTO;
using MyShop.GUI.Pages.Product;
using MyShop.Utilities;
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

namespace MyShop.GUI.Pages
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Page
    {
        class Resource
        {
            public string? name { get; set; }
            public string? image { get; set; }
            public string? categoryName { get; set; }
            public decimal? promotionPrice { get; set; }
            public string? description { get; set; }
            public int discount { get; set; }

            public Resource(ProductDTO productDTO, CategoryDTO categoryDTO, int discountPercent)
            {
                name = productDTO.name;
                image = productDTO.image;
                description = productDTO.description;
                promotionPrice = productDTO.promotionPrice;
                categoryName = categoryDTO.name;
                discount = discountPercent;
            }
        }

        private CategoryBLL _categoryBLL;
        private CategoryDTO _currentCategory;
        private List<ProductDTO>? _products = null;
        private string _currentKey = "";
        private int _currentPage = 1;
        private int _rowsPerPage = 9;
        private int _totalItems = 0;
        private int _totalPages = 0;
        private string? _sortType = null;
        private bool _orderAsc = true;
        private Decimal? _startPrice = null;
        private Decimal? _endPrice = null;
        private Frame _navigation;

        public HomeScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _categoryBLL = new CategoryBLL();
            var listCategory = _categoryBLL.getAllCategory();
            listCategory.Insert(0, new CategoryDTO() {id = -1 ,name = "Tất cả" });
            _currentCategory = listCategory[0];
            ListOfCategory.ItemsSource = listCategory;
            ListOfCategory.SelectedIndex = 0;
            _navigation = pageNavigation;

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataSource();
        }

        private async void UpdateDataSource()
        {
            MessageText.Text = string.Empty;
            List<Resource> list = new List<Resource>();
            ProductBLL productBLL = new ProductBLL();
            CategoryBLL categoryBLL = new CategoryBLL();
            PromotionBLL promotionBLL = new PromotionBLL();
            
            try
            {
                (_products, _totalItems) = await productBLL.findProductBySearch(_currentPage, _rowsPerPage, _currentKey,
                        _startPrice, _endPrice, _sortType, _orderAsc, _currentCategory.id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            foreach (var product in _products)
            {
                int discountpercentage = 0;
                if (product.promotionID != null)
                {
                    discountpercentage = promotionBLL.getPromotionById((int)product.promotionID).discountPercentage;
                }
                var category = categoryBLL.getCategoryById(product.categoryID);
                list.Add(new Resource(product, category, discountpercentage));
            }

            if (list.Count == 0)
            {
                MessageText.Text = "Opps! No Data!";
            }

            dataListView.ItemsSource = list;
            

            infoTextBlock.Text = $"Đang hiển thị {_products.Count}/{_totalItems} sản phẩm";
            UpdatePagingInfo();
        }

        private void UpdatePagingInfo()
        {
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            _totalPages = _totalPages == 0 ? 1 : _totalPages;
            pageInfoTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        private void SearchTermTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _currentKey = SearchTermTextBox.Text.Trim();
                _currentPage = 1;
                UpdateDataSource();
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

        private void UpdatePriceFilter(int selectedIndex)
        {
            _currentPage = 1;
            switch (selectedIndex)
            {
                case 1:
                    _startPrice = 0;
                    _endPrice = 5000000;
                    break;
                case 2:
                    _startPrice = 5000000;
                    _endPrice = 10000000;
                    break;
                case 3:
                    _startPrice = 10000000;
                    _endPrice = 15000000;
                    break;
                case 4:
                    _startPrice = 15000000;
                    _endPrice = Decimal.MaxValue;
                    break;
                default:
                    _startPrice = null;
                    _endPrice = null;
                    break;
            }
            UpdateDataSource();
            UpdatePagingInfo();
        }

        private void UpdateSortFilter(int selectedIndex)
        {
            _currentPage = 1;
            switch (selectedIndex)
            {
                case 1:
                    _sortType = "price";
                    _orderAsc = true;
                    break;
                case 2:
                    _sortType = "price";
                    _orderAsc = false;
                    break;
                case 3:
                    _sortType = "name";
                    _orderAsc = true;
                    break;
                case 4:
                    _sortType = "name";
                    _orderAsc = false;
                    break;
                default:
                    _sortType = null;
                    _orderAsc = true;
                    break;
            }
            UpdateDataSource();
        }

        private void PreviousButtonOnClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(_currentPage - 1);
        }

        private void NextButtonOnClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(_currentPage + 1);
        }

        private void StartPageButtonOnClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(1);
        }

        private void EndPageButtonOnClick(object sender, RoutedEventArgs e)
        {
            UpdateCurrentPage(_totalPages);
        }

        private void PriceComboboxSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ListOfPrice.SelectedIndex > 0)
            {
                UpdatePriceFilter(ListOfPrice.SelectedIndex);
            }
        }
        private void ListOfCategorySelected(object sender, SelectionChangedEventArgs e)
        {
            if (ListOfCategory.SelectedIndex >= 0)
            {
                _currentCategory = (CategoryDTO)ListOfCategory.SelectedValue;
                UpdateDataSource();
            }
        }
        private void SortComboboxSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ListOfSortType.SelectedIndex > 0)
            {
                UpdateSortFilter(ListOfSortType.SelectedIndex);
            }
        }

        private void ListViewItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = dataListView.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _products!.Count)
            {
                _navigation.NavigationService.Navigate(new ProductDetailScreen(this, _products[selectedIndex], _navigation));
            }
        }

        private void AddProductOnClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.Navigate(new AddProductScreen(_navigation));
        }

    
        private void AddBySheetClick(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Files|*.xlsx; *.csv;";

            if (screen.ShowDialog() == true)
            {
                var sheetHelper = new AddSheetHelper();
                var productBLL = new ProductBLL();

                var products = sheetHelper.GetProductBySheet(screen.FileName);

                if (products != null && products.Count > 0)
                {
                    foreach (var product in products)
                    {
                        productBLL.addProduct(product);
                    }
                    UpdateDataSource();
                    MessageBox.Show("Thêm danh sách sản phầm từ Sheet thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ItemsPerPageSelected(object sender, SelectionChangedEventArgs e)
        {
            if (pageInfoTextBlock != null)
            {
                switch (ItemsPerPage.SelectedIndex)
                {
                    case 0:
                        _rowsPerPage = 3;
                        break;
                    case 1:
                        _rowsPerPage = 6;
                        break;
                    case 2:
                        _rowsPerPage = 9;
                        break;
                }
                UpdateDataSource();
            }
        }
    }
}
