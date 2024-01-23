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
    /// Interaction logic for UpdatePromotionScreen.xaml
    /// </summary>
    public partial class UpdatePromotionScreen : Page
    {
        Frame _navigation;
        PromotionDTO _promotion;
        PromotionBLL _promotionBLL;
        
        public UpdatePromotionScreen(Frame pageNavigation, PromotionDTO promotion)
        {
            InitializeComponent();
            _navigation = pageNavigation;
            _promotion = promotion;
            DataContext = _promotion;
            _promotionBLL = new PromotionBLL();
        }

        private bool IsDiscountValid(string discountText)
        {
            if (int.TryParse(discountText, out int discountPercent))
            {
                return discountPercent > 0 && discountPercent < 100;
            }
            return false;
        }

        private void SavePromo()
        {
            _promotionBLL.updatePromotion(_promotion);
            MessageBox.Show("Cập nhật mã giảm giá thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            _navigation.NavigationService.GoBack();
        }

        private void SavePromotionClick(object sender, RoutedEventArgs e)
        {
            string code = NameCodeTextBox.Text.Trim();
            string discountText = NameDiscountTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(code))
            {
                if (IsDiscountValid(discountText))
                {
                    _promotion.code = code;
                    _promotion.discountPercentage = int.Parse(discountText);
                    SavePromo();
                }
                else
                {
                    MessageBox.Show("Giá trị giảm giá phải lớn hơn 0 và nhỏ hơn 100!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void DeletePromotionClick(object sender, RoutedEventArgs e)
        {
           
            var choice = MessageBox.Show("Bạn có chắc muốn xóa mã giảm giá này không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (choice == MessageBoxResult.OK)
            {
                var promotionBLL = new PromotionBLL();
                int isSuccess = promotionBLL.deletePromotion((int)_promotion.id);
                if (isSuccess == 0)
                {
                    MessageBox.Show("Lỗi! Đang có sản phẩm sử dụng mã giảm giá này", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    MessageBox.Show("Xóa mã giảm giá thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    _navigation.NavigationService.GoBack();
                }
            }
        }
    }
}
