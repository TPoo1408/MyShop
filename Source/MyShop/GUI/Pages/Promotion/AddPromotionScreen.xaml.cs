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

namespace MyShop.GUI.Pages.Promotion
{
    /// <summary>
    /// Interaction logic for AddPromotionScreen.xaml
    /// </summary>
    public partial class AddPromotionScreen : Page
    {
        Frame _navigation;
        PromotionDTO _promotion;
        PromotionBLL _promotionBLL;
        
        public AddPromotionScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _navigation = pageNavigation;
            _promotionBLL = new PromotionBLL();
            _promotion = new PromotionDTO();
        }

        private bool IsDiscountValid(string discountText)
        {
            if (int.TryParse(discountText, out int discountPercent))
            {
                return discountPercent > 0 && discountPercent < 100;
            }
            return false;
        }

        private void AddPromo()
        {
            int id = _promotionBLL.addPromotion(_promotion);
            if (id != 0)
            {
                _promotion.id = id;
                MessageBox.Show("Thêm mã giảm giá thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigation.NavigationService.GoBack();
            }
        }

        private void AddPromotionClick(object sender, RoutedEventArgs e)
        {
            string nameCode = NameCodeTextBox.Text.Trim();
            string discountText = NameDiscountTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(nameCode))
            {
                if (IsDiscountValid(discountText))
                {
                    _promotion.code = nameCode;
                    _promotion.discountPercentage = int.Parse(discountText);
                    AddPromo();
                }
                else
                {
                    MessageBox.Show("Giá trị giảm giá phải là số nguyên, lớn hơn 0 và nhỏ hơn 100!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên mã!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.GoBack();
        }
    }
}
