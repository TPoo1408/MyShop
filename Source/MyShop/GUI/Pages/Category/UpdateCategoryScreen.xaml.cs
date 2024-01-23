using MyShop.BLL;
using MyShop.DTO;
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

namespace MyShop.GUI.Pages.Category
{
    /// <summary>
    /// Interaction logic for UpdateCategoryScreen.xaml
    /// </summary>
    public partial class UpdateCategoryScreen : Page
    {
        Frame _pageNavigation;
        private CategoryDTO _category;

        public UpdateCategoryScreen(Frame pageNavigation, CategoryDTO category)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
            _category = category;
            DataContext = _category;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void updateCategory_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text.Trim() == "" || descriptionTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cateogryBLL = new CategoryBLL();
            int result = cateogryBLL.updateCategory(_category);

            if (result > 0)
            {
                MessageBox.Show("Chỉnh sửa thể loại thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _pageNavigation.NavigationService.GoBack();
            }

        }
        private void deleteCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult choice = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (choice == MessageBoxResult.OK)
            {
                var categoryBLL = new CategoryBLL();
                int result = categoryBLL.deleteCategory(_category.id);

                if (result > 0)
                {
                    MessageBox.Show("Xóa thể loại thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    _pageNavigation.NavigationService.GoBack();
                }
            }
        }
    }
}
