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
    /// Interaction logic for AddProductScreen.xaml
    /// </summary>
    public partial class AddProductScreen : Page
    {
        
        private FileInfo? _selectedImage = null;
        private ProductBLL _productBLL;
        private CategoryBLL _categoryBLL;
        private Frame _navigation;

        public AddProductScreen(Frame pageNavigation)
        {
            InitializeComponent();

            _productBLL = new ProductBLL();
            _categoryBLL = new CategoryBLL();

            var categories = _categoryBLL.getAllCategory();

            SetupCategoryComboBox(categories);
            _navigation = pageNavigation;
        }

        private void SetupCategoryComboBox(IEnumerable<CategoryDTO> categories)
        {
            ListOfCategory.ItemsSource = categories;
            ListOfCategory.SelectedIndex = 0;
        }

        private void AddImageButtonClick(object sender, RoutedEventArgs e)
        {
            SelectImage();
        }

        private void SelectImage()
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Files|*.png; *.jpg; *.jpeg;";
            if (screen.ShowDialog() == true)
            {
                LoadSelectedImage(screen.FileName);
            }
        }

        private void LoadSelectedImage(string fileName)
        {
            _selectedImage = new FileInfo(fileName);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileName, UriKind.Absolute);
            bitmap.EndInit();

            ImageAdd.Source = bitmap;
        }

        private void SaveProductClick(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                ShowImageNotSelectedWarning();
                return;
            }

            if (TryGetCategory(out var categoryDTO))
            {
                var productDTO = CreateProductDTO(categoryDTO);

                SaveProductToDatabase(productDTO);
                ShowProductAddedSuccessfullyMessage();
                NavigateBack();
            }
        }

        private bool TryGetCategory(out CategoryDTO categoryDTO)
        {
            categoryDTO = (CategoryDTO)ListOfCategory.SelectedValue;
            return categoryDTO != null;
        }

        private ProductDTO CreateProductDTO(CategoryDTO categoryDTO)
        {
            return new ProductDTO
            {
                name = NameTextBox.Text,
                image = "",
                description = DesTextBox.Text,
                price = Decimal.Parse(PriceTextBox.Text),
                promotionPrice = Decimal.Parse(PriceTextBox.Text),
                brand = BrandTextBox.Text,
                categoryID = categoryDTO.id,
                stock = int.Parse(StockTextBox.Text)
            };
        }

        private void SaveProductToDatabase(ProductDTO productDTO)
        {
            int id = _productBLL.addProduct(productDTO);
            string key = Guid.NewGuid().ToString();
            _productBLL.uploadImage(_selectedImage, id, key);
        }

        private void ShowProductAddedSuccessfullyMessage()
        {
            MessageBox.Show("Sản phẩm đã thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowImageNotSelectedWarning()
        {
            MessageBox.Show("Vui lòng Chọn ảnh sản phẩm!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            _navigation.NavigationService.GoBack();
        }
    }
}
