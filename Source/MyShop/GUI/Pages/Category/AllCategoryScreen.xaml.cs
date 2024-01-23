using Microsoft.Win32;
using MyShop.BLL;
using MyShop.DTO;
using MyShop.GUI.Pages.Customer;
using MyShop.Utilities;
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

namespace MyShop.GUI.Pages.Category
{
    /// <summary>
    /// Interaction logic for AllCategory.xaml
    /// </summary>
    public partial class AllCategoryScreen : Page
    {
        Frame _pageNavigation;
        private CategoryBLL _categoryBLL;
        private ObservableCollection<CategoryDTO> _listCategories;

        public AllCategoryScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
            _categoryBLL = new CategoryBLL();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int i = categoriesListView.SelectedIndex;

            var category = _listCategories[i];
            if (category != null)
            {
                _pageNavigation.NavigationService.Navigate(new UpdateCategoryScreen(_pageNavigation, category));
            }
        }

        private void PageCategory_Loaded(object sender, RoutedEventArgs e)
        {
            _listCategories = _categoryBLL.getAllCategory();
            categoriesListView.ItemsSource = _listCategories;
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new AddCategoryScreen(_pageNavigation));
        }

        private void AddCategoryBySheetClick(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Files|*.xlsx; *.csv;";

            if (screen.ShowDialog() == true)
            {
                var sheetHelper = new AddSheetHelper();
                var categoryBLL = new CategoryBLL();

                var categories = sheetHelper.GetCategoryBySheet(screen.FileName);

                if (categories != null && categories.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        categoryBLL.addCategory(category);
                    }
                    _listCategories = _categoryBLL.getAllCategory();
                    categoriesListView.ItemsSource = _listCategories;
                    MessageBox.Show("Thêm danh sách thể loại sản phầm từ Sheet thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}
