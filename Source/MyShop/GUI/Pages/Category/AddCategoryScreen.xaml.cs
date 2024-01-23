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
    /// Interaction logic for AddCategoryScreen.xaml
    /// </summary>
    public partial class AddCategoryScreen : Page
    {
        Frame _pageNavigation;
        public AddCategoryScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void saveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text.Trim() == "" || descriptionTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var categoryDTO = new CategoryDTO();

            categoryDTO.name = nameTextBox.Text;
            categoryDTO.description = descriptionTextBox.Text;

            CategoryBLL categoryBLL = new CategoryBLL();
            int id = categoryBLL.addCategory(categoryDTO); ;

            if(id > 0)
            {
                MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _pageNavigation.NavigationService.GoBack();
            }    
        }

    }
}
