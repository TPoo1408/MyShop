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
    /// Interaction logic for ReportByRevenueScreen.xaml
    /// </summary>
    public partial class ReportByRevenueScreen : Page
    {
        Frame _pageNavigation;
        private ReportHelper _reportHelper;
        private int _currentYear;
        public ReportByRevenueScreen(Frame pageNavigation)
        {
            InitializeComponent();
            _pageNavigation = pageNavigation;
            _reportHelper = new ReportHelper();
        }

        private void ReportRevenuePage_Loaded(object sender, RoutedEventArgs e)
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
                Title = "VNĐ",
                MinValue = 0,
                LabelFormatter = value => value.ToString("N0")
            });
            Title.Text = "Doanh thu và lợi nhuận theo từng năm";

            displayAnnualChart();
        }

        private void displayAnnualChart()
        {
            var (annualRevenue, annualProfit) = _reportHelper.getRevenueAndProfitAnnual();

            var valuesColChart = new ChartValues<decimal>(annualRevenue);
            var valuesLineChart = new ChartValues<decimal>(annualProfit);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Doanh thu theo năm",
                Values = valuesColChart
            });

            chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DarkViolet,
                Title = "Lợi nhuận theo năm",
                Values = valuesLineChart
            });

            var currentYear = DateTime.Now.Year;
            chart.AxisX.Add(new Axis()
            {
                Foreground = Brushes.Black,
                Title = "Năm",
                Labels = new string[] {
                    $"{currentYear - 2}",
                    $"{currentYear - 1}",
                    $"{currentYear}",
                }
            });
            Title.Text = "Doanh thu và lợi nhuận theo từng năm";
        }

        private void displayYearChart(int year)
        {
            var (revenueByYear, profitByYear) = _reportHelper.getRevenueAndProfitByYear(year);

            var valuesColChart = new ChartValues<decimal>(revenueByYear);
            var valuesLineChart = new ChartValues<decimal>(profitByYear);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Doanh thu theo tháng",
                Values = valuesColChart
            });

            chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DarkViolet,
                Title = "Lợi nhuận theo tháng",
                Values = valuesLineChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Tháng",
                    Labels = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12",
                    }
                });
            Title.Text = $"Doanh thu và lợi nhuận trong năm {year}";
            monthCombobox.IsEnabled = true;
            monthCombobox.SelectedIndex = 0;
        }

        private void displayMonthChart(int month, int year)
        {
            var (revenueByMonth, profitByMonth) = _reportHelper.getRevenueAndProfitByMonth(month, year);

            var valuesColChart = new ChartValues<decimal>(revenueByMonth);
            var valuesLineChart = new ChartValues<decimal>(profitByMonth);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Doanh thu theo tuần",
                Values = valuesColChart
            });

            chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DarkViolet,
                Title = "Lợi nhuận theo tuần",
                Values = valuesLineChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Tuần",
                    Labels = new string[] { "1", "2", "3", "4", "5",
                    }
                });
            Title.Text = $"Doanh thu và lợi nhuận trong tháng {month} năm {year}";
        }

        private void displayDateToDateChart(DateTime startDate, DateTime endDate)
        {
            var (revenueDateToDate, profitDateToDate) = _reportHelper.getRevenueAndProfitFromDatetoDate(startDate, endDate);

            var valuesColChart = new ChartValues<decimal>(revenueDateToDate);
            var valuesLineChart = new ChartValues<decimal>(profitDateToDate);

            chart.Series = new SeriesCollection();
            chart.AxisX = new AxesCollection();

            chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Indigo,
                Title = "Doanh thu theo ngày",
                Values = valuesColChart
            });

            chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DarkViolet,
                Title = "Lợi nhuận theo ngày",
                Values = valuesLineChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Ngày",
                    Labels = _reportHelper.dayConverter(startDate, endDate)
                });
            Title.Text = $"Doanh thu và lợi nhuận từ {startDate.ToString("dd-MM-yyyy")} đến {endDate.ToString("dd-MM-yyyy")}";
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
                displayDateToDateChart((DateTime)startDate, (DateTime)endDate);
            }
        }

        private void monthCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = monthCombobox.SelectedIndex;
            if (index > 0)
            {
                displayMonthChart(index, _currentYear);
            }
        }

        private void yearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = yearCombobox.SelectedIndex;
            var currentYear = DateTime.Now.Year;
            if (index == 1)
            {
                displayYearChart(currentYear - 2);
                _currentYear = currentYear - 2;
            }
            else if (index == 2)
            {
                displayYearChart(currentYear - 1);
                _currentYear = currentYear - 1;
            }
            else if (index == 3)
            {
                displayYearChart(currentYear);
                _currentYear = currentYear;
            }
        }

        private void toProductReport_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new ReportByProductScreen(_pageNavigation));
        }
    }
}
