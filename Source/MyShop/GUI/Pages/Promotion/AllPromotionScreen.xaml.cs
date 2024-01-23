using MyShop.BLL;
using MyShop.DTO;
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

namespace MyShop.GUI.Pages.Promotion
{
    /// <summary>
    /// Interaction logic for AllPromotionScreen.xaml
    /// </summary>
    public partial class AllPromotionScreen : Page
    {
        
        private PromotionBLL _promotionBLL;
        private Frame _navigation;
        private ObservableCollection<PromotionDTO> _listPromotion;

        public AllPromotionScreen(Frame pageNavigation)
        {
            _navigation = pageNavigation;
            InitializeComponent();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            _promotionBLL = new PromotionBLL();
            _listPromotion = _promotionBLL.getAllPromotion();
            promotionListViews.ItemsSource = _listPromotion;
        }

        private void ListViewItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = promotionListViews.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < _listPromotion.Count)
            {
                var promotion = _listPromotion[selectedIndex];
                _navigation.NavigationService.Navigate(new UpdatePromotionScreen(_navigation, promotion));
            }
        }

        private void DeletePromotionClick(object sender, RoutedEventArgs e)
        {
            int selectedIndex = promotionListViews.SelectedIndex;

            if (selectedIndex == -1)
            {
                ShowWarningMessage("Vui lòng chọn mã giảm giá để xóa!");
                return;
            }

            var choice = ShowConfirmationMessage("Bạn có chắc muốn xóa mã giảm giá này không?");

            if (choice == MessageBoxResult.OK)
            {
                DeleteSelectedPromotion(selectedIndex);
            }
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private MessageBoxResult ShowConfirmationMessage(string message)
        {
            return MessageBox.Show(message, "Notification", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private void DeleteSelectedPromotion(int selectedIndex)
        {
            var promotionId = _listPromotion[selectedIndex].id;
            var isSuccess = _promotionBLL.deletePromotion((int)promotionId!);

            if (isSuccess == 0)
            {
                ShowErrorMessage("Lỗi!!! Đang có sản phẩm sử dụng mã giảm giá này!");
            }
            else
            {
                _listPromotion.RemoveAt(selectedIndex);
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AddPromotion_Click(object sender, RoutedEventArgs e)
        {
            _navigation.NavigationService.Navigate(new AddPromotionScreen(_navigation));
        }

    }
}
