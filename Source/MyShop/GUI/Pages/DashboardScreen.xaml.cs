using MyShop.BLL;
using MyShop.GUI.Pages.Report;
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
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : Page
    {
        private class Resources
        {
            public int TotalProduct { get; set; }
            public int TotalOrderByWeek { get; set; }
            public int TotalOrderByMonth { get; set; }
        }

        Frame _pageNavigation;
        ProductBLL _productBLL;
        OrderBLL _orderBLL;

        public DashboardScreen(Frame pageNavigation)
        {
            _productBLL = new ProductBLL();
            _orderBLL = new OrderBLL();
            InitializeComponent();
            _pageNavigation = pageNavigation;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int totalProduct = await _productBLL.getTotalProduct();

            int totalOrderByWeek = _orderBLL.getTotalOrderThisWeek();
            int totalOrderByMonth = _orderBLL.getTotalOrderThisMonth();
            var top5Product = await _productBLL.getTop5ProductAlmostOutOfStock();

            this.DataContext = new Resources()
            {
                TotalProduct = totalProduct,
                TotalOrderByWeek = totalOrderByWeek,
                TotalOrderByMonth = totalOrderByMonth,
            };

            productsListView.ItemsSource = top5Product;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO
        }

        private void TopSalings_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new TopSaleReportScreen(_pageNavigation));
        }
    }
}
