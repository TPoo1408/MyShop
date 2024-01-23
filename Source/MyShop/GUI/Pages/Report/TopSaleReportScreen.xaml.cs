using MyShop.BLL;
using LiveCharts;
using LiveCharts.Wpf;
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

namespace MyShop.GUI.Pages.Report
{
    /// <summary>
    /// Interaction logic for TopSaleReportScreen.xaml
    /// </summary>
    public partial class TopSaleReportScreen : Page
    {
        class Resources
        {
            public int totalProduct { get; set; }
            public int totalOrderByWeek { get; set; }
            public int totalOrderByMonth { get; set; }
        }

        Frame _pageNavigation;
        ProductBLL _productBLL;
        OrderBLL _orderBLL;
        ReportHelper _reportHelper;
        Func<ChartPoint, string> labelPoint;
        public TopSaleReportScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
            _productBLL = new ProductBLL();
            _orderBLL = new OrderBLL();
            _reportHelper = new ReportHelper();
            labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }

        private async void TopSaleReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            int totalProduct = await _productBLL.getTotalProduct();
            int totalOrderByWeek = _orderBLL.getTotalOrderThisWeek();
            int totalOrderByMonth = _orderBLL.getTotalOrderThisMonth();

            DataContext = new Resources()
            {
                totalProduct = totalProduct,
                totalOrderByWeek = totalOrderByWeek,
                totalOrderByMonth = totalOrderByMonth
            };

            displayYearChart();
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void optionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (optionComboBox.SelectedIndex == 0)
            {
                displayYearChart();
            }
            else if (optionComboBox.SelectedIndex == 1)
            {
                displayMonthChart();
            }
            else
            {
                displayWeekChart();
            }
        }

        private void displayYearChart()
        {
            if(topSalesPieChart != null)
            {
                var topSalesProduct = _reportHelper.getTopSalesByYear(DateTime.Now.Year);
                topSalesPieChart.Series = new SeriesCollection();
                foreach (var sale in topSalesProduct)
                {
                    topSalesPieChart.Series.Add(new PieSeries
                    {
                        Title = sale.product.name,
                        Values = new ChartValues<int> { sale.salesQuantity },
                        DataLabels = true,
                        LabelPoint = labelPoint,
                    });
                }
            }    
        }
        private void displayMonthChart()
        {
            var topSalesProduct = _reportHelper.getTopSalesByMonth(DateTime.Now.Year, DateTime.Now.Month);
            topSalesPieChart.Series = new SeriesCollection();
            foreach (var sale in topSalesProduct)
            {
                topSalesPieChart.Series.Add(new PieSeries
                {
                    Title = sale.product.name,
                    Values = new ChartValues<int> { sale.salesQuantity },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            }
        }
        private void displayWeekChart()
        {
            var topSalesProduct = _reportHelper.getTopSalesByWeek();
            topSalesPieChart.Series = new SeriesCollection();
            foreach (var sale in topSalesProduct)
            {
                topSalesPieChart.Series.Add(new PieSeries
                {
                    Title = sale.product.name,
                    Values = new ChartValues<int> { sale.salesQuantity },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            }
        }
    }
}
