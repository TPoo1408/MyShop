using Microsoft.Win32;
using MyShop.BLL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for UpdateProductScreen.xaml
    /// </summary>
    public partial class UpdateProductScreen : Page
    {
        private bool _imageChange = false;
        private FileInfo? _selectedImg = null;
        private Frame _navigation;

        private CategoryBLL _categoryBLL;
        private ProductBLL _productBLL;
        private PromotionBLL _promotionBLL;

        private ProductDTO _productDTO;
        private CategoryDTO _categoryDTO;

        public UpdateProductScreen(ProductDTO product, CategoryDTO categoryDTO, Frame pageNavigation)
        {
            InitializeComponent();
            _categoryDTO = categoryDTO;
            _navigation = pageNavigation;
            _categoryBLL = new CategoryBLL();
            _productBLL = new ProductBLL();
            _promotionBLL = new PromotionBLL();

            var categories = _categoryBLL.getAllCategory();

            ListOfCategory.ItemsSource = categories;

            foreach (var category in categories)
            {
                if (category.id == categoryDTO.id)
                {
                    categoryDTO = category;
                    break;
                }
            }

            ListOfCategory.SelectedValue = categoryDTO;
            _productDTO = product;

            DataContext = product;
        }

        private void AddImageButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Files|*.png; *.jpg; *.jpeg;";

            if (openFileDialog.ShowDialog() == true)
            {
                LoadSelectedImage(openFileDialog.FileName);
            }
        }

        private void LoadSelectedImage(string imagePath)
        {
            _imageChange = true;
            _selectedImg = new FileInfo(imagePath);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            ImageAdd.Source = bitmap;
        }

        private void SaveProductClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var categoryDTO = (CategoryDTO)ListOfCategory.SelectedValue;
                CalculateAndSetProductValues(categoryDTO);
                UpdateCategoryDetails(_productDTO);

                _productBLL.updateProduct(_productDTO);

                if (_imageChange)
                    UpdateProductImage();

                ShowSuccessMessageAndNavigateBack();
            }
            catch (FormatException)
            {
                ShowErrorMessage("Định dạng dữ liệu không hợp lệ!");
            }
        }

        private void CalculateAndSetProductValues(CategoryDTO categoryDTO)
        {
            int discountPercentage = _productDTO.promotionID != null ?
               (int)_promotionBLL.getPromotionById((int)_productDTO.promotionID).discountPercentage : 0;
            double percent = 1 - discountPercentage * 1.0 / 100;

            _productDTO.name = NameTextBox.Text;
            _productDTO.description = DesTextBox.Text;
            _productDTO.price = decimal.Parse(PriceTextBox.Text);
            _productDTO.promotionPrice = (decimal)((double)_productDTO.price * percent);
            _productDTO.brand = BrandTextBox.Text;
            _productDTO.categoryID = categoryDTO.id;
            _productDTO.stock = int.Parse(StockTextBox.Text);
        }

        private void UpdateCategoryDetails(ProductDTO productDTO)
        {
            var categoryTemp = _categoryBLL.getCategoryById(productDTO.categoryID);
            _categoryDTO.id = categoryTemp.id;
            _categoryDTO.name = categoryTemp.name;
        }

        private void UpdateProductImage()
        {
            string key = Guid.NewGuid().ToString();
            _productDTO.image = _productBLL.uploadImage(_selectedImg, _productDTO.id, key);
        }

        private void ShowSuccessMessageAndNavigateBack()
        {
            MessageBox.Show("Sản phẩm đã chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            _navigation.NavigationService.GoBack();
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.GoBack();
        }
    }
}
