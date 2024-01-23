using LiveCharts;
using LiveCharts.Wpf;
using MyShop.BLL;
using MyShop.DTO;
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

namespace MyShop.GUI.Pages.Report
{
    /// <summary>
    /// Interaction logic for ReportByProductScreen.xaml
    /// </summary>
    public partial class ReportByProductScreen : Page
    {
        private enum Status
        {
            Year,
            Month,
            Week,
            Date
        }

        Frame _pageNavigation;
        private ReportHelper _reportHelper;
        private ProductBLL _productBLL;
        private int _currentYear;
        private ProductDTO _selectedProduct;
        Status _currentStatus = Status.Year;

        public ReportByProductScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
            _productBLL = new ProductBLL();
            _reportHelper = new ReportHelper();
        }

        private void ReportProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 2; year <= currentYear; year++)
            {
                var item = new ComboBoxItem();
                item.Content = $"Năm {year.ToString()}";
                yearCombobox.Items.Add(item);
            }

            chart.AxisY.Add(new Axis
            {
                Foreground = Brushes.Black,
                Title = "Số lượng đã bán",
                MinValue = 0,
                LabelFormatter = value => value.ToString("N0")
            });
            Title.Text = "Số lượng sản phẩm đã bán theo từng năm";

            ObservableCollection<ProductDTO> products = _productBLL.getAllProduct();

            productCombobox.ItemsSource = products;
            productCombobox.SelectedIndex = 0;

            _selectedProduct = (ProductDTO)productCombobox.SelectedValue;
            displayAnnualChart(_selectedProduct);
        }

        private void displayAnnualChart(ProductDTO product)
        {
            var quantities = _reportHelper.getQuantityOfProductAnnual(product);

            var valuesColChart = new ChartValues<int>(quantities);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Số lượng sản phẩm đã bán",
                Values = valuesColChart
            });

            var currentYear = DateTime.Now.Year;
            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Năm",
                    Labels = new string[] {
                        $"{currentYear - 2}",
                        $"{currentYear - 1}",
                        $"{currentYear}",
                    }
                });
            Title.Text = "Số lượng sản phẩm đã bán theo từng năm";
            _currentStatus = Status.Year;
        }

        private void displayYearChart(ProductDTO product, int year)
        {
            var quantities = _reportHelper.getQuantityOfProductByYear(product, year);

            var valuesColChart = new ChartValues<int>(quantities);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Số lượng sản phẩm đã bán",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Tháng",
                    Labels = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12",
                    }
                });
            Title.Text = $"Số lượng sản phẩm đã bán trong năm {year}";
            monthCombobox.IsEnabled = true;
            monthCombobox.SelectedIndex = 0;
            _currentStatus = Status.Month;
        }

        private void displayMonthChart(ProductDTO product, int month, int year)
        {
            var quantities = _reportHelper.getQuantityOfProductByMonth(product, year, month);

            var valuesColChart = new ChartValues<int>(quantities);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Số lượng sản phẩm đã bán",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Tuần",
                    Labels = new string[] { "1", "2", "3", "4", "5",
                    }
                });
            Title.Text = $"Số lượng sản phẩm đã bán trong tháng {month} năm {year}";
            _currentStatus = Status.Week;
        }

        private void displayDateToDateChart(ProductDTO product, DateTime startDate, DateTime endDate)
        {
            var pricesByDate = _reportHelper.getQuantityOfProductFromDatetoDate(product, startDate, endDate);

            var valuesColChart = new ChartValues<int>(pricesByDate);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Số lượng sản phẩm đã bán",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Date",
                    Labels = _reportHelper.dayConverter(startDate, endDate)
                });
            Title.Text = $"Số lượng sản phẩm đã bán từ {startDate.ToString("dd-MM-yyyy")} đến {endDate.ToString("dd-MM-yyyy")}";
            _currentStatus = Status.Date;
        }

        private void yearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = yearCombobox.SelectedIndex;
            var currentYear = DateTime.Now.Year;
            if (index == 1)
            {
                displayYearChart(_selectedProduct, currentYear - 2);
                _currentYear = currentYear - 2;
            }
            else if (index == 2)
            {
                displayYearChart(_selectedProduct, currentYear - 1);
                _currentYear = currentYear - 1;
            }
            else if (index == 3)
            {
                displayYearChart(_selectedProduct, currentYear);
                _currentYear = currentYear;
            }
        }

        private void monthCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = monthCombobox.SelectedIndex;
            if (index > 0)
            {
                displayMonthChart(_selectedProduct, index, _currentYear);
            }
        }

        private void productCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProduct = (ProductDTO)productCombobox.SelectedValue;
            if (_currentStatus == Status.Year)
            {
                displayAnnualChart(_selectedProduct);
            }
            else if (_currentStatus == Status.Month)
            {
                int index = yearCombobox.SelectedIndex;
                var currentYear = DateTime.Now.Year;
                if (index == 1)
                {
                    displayYearChart(_selectedProduct, currentYear - 2);
                    _currentYear = currentYear - 2;
                }
                else if (index == 2)
                {
                    displayYearChart(_selectedProduct, currentYear - 1);
                    _currentYear = currentYear - 1;
                }
                else if (index == 3)
                {
                    displayYearChart(_selectedProduct, currentYear);
                    _currentYear = currentYear;
                }
            }
            else if (_currentStatus == Status.Week)
            {
                int index = monthCombobox.SelectedIndex;
                if (index > 0)
                {
                    displayMonthChart(_selectedProduct, index, _currentYear);
                }
            }
            else if (_currentStatus == Status.Date)
            {
                var startDate = StartDate.SelectedDate;
                var endDate = EndDate.SelectedDate;

                if (startDate == null || endDate == null)
                {
                    MessageBox.Show("Vui lòng chọn đủ ngày bắt đầu và kết thúc!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    yearCombobox.SelectedIndex = 0;
                    monthCombobox.SelectedIndex = 0;
                    displayDateToDateChart(_selectedProduct, (DateTime)startDate, (DateTime)endDate);
                }
            }
        }

        private void view_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDate.SelectedDate;
            var endDate = EndDate.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Vui lòng chọn đủ ngày bắt đầu và kết thúc!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                yearCombobox.SelectedIndex = 0;
                monthCombobox.SelectedIndex = 0;
                displayDateToDateChart(_selectedProduct, (DateTime)startDate, (DateTime)endDate);
            }
        }

        private void backToRevenueButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new ReportByRevenueScreen(_pageNavigation));
        }
    }
}
