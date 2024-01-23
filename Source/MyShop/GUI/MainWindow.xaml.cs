using MyShop.GUI;
using MyShop.GUI.Pages;
using MyShop.GUI.Pages.Category;
using MyShop.GUI.Pages.Customer;
using MyShop.GUI.Pages.Order;
using MyShop.GUI.Pages.Promotion;
using MyShop.GUI.Pages.Report;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Page
        {
            DashBoard,
            Home,
            Category,
            Promotion,
            Customer,
            Order,
            Report
        }
        private Page _currentPage = Page.DashBoard;

        private Style _menuButtonStyle = (Style)Application.Current.Resources["menuButton"];
        private Style _menuButtonActiveStyle = (Style)Application.Current.Resources["menuButtonActive"];
        public Style ButtonStyle
        {
            get { return _menuButtonStyle; }
            set { _menuButtonStyle = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new
            {
                Logo = "Assets/Images/Logo/ecommerce.png"
            };
            var currentpage = Properties.Settings.Default.CurrentPage;
            _currentPage = (Page)currentpage;
            changePage(_currentPage);
            activateButtonByPage(_currentPage);
        }

        private void changePage(Page selectedPage)
        {
            if (selectedPage == Page.DashBoard)
            {
                pageNavigation.NavigationService.Navigate(new DashboardScreen(pageNavigation));
            }
            else if (selectedPage == Page.Home)
            {
                pageNavigation.NavigationService.Navigate(new HomeScreen(pageNavigation));
            }
            else if (selectedPage == Page.Category)
            {
                pageNavigation.NavigationService.Navigate(new AllCategoryScreen(pageNavigation));
            }
            else if (selectedPage == Page.Promotion)
            {
                pageNavigation.NavigationService.Navigate(new AllPromotionScreen(pageNavigation));
            }
            else if (selectedPage == Page.Customer)
            {
                pageNavigation.NavigationService.Navigate(new AllCustomerScreen(pageNavigation));
            }
            else if (selectedPage == Page.Order)
            {
                pageNavigation.NavigationService.Navigate(new AllOrderScreen(pageNavigation));
            }
            else if (selectedPage == Page.Report)
            {
                pageNavigation.NavigationService.Navigate(new ReportByRevenueScreen(pageNavigation));
            }
        }

        private void sidebar_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in SidebarPanel.Children.OfType<Button>())
            {
                if (child is Button button)
                {
                    button.Style = _menuButtonStyle;
                }
            }

            if (sender is Button clickedButton)
            {
                clickedButton.Style = _menuButtonActiveStyle;
                int activeIndex = SidebarPanel.Children.OfType<Button>().ToList().IndexOf(clickedButton);
                Properties.Settings.Default.CurrentPage = activeIndex;
                Properties.Settings.Default.Save();
                changePage((Page)activeIndex);
            }
        }

        private void activateButtonByPage(Page selectedPage)
        {
            foreach (var child in SidebarPanel.Children.OfType<Button>())
            {
                child.Style = _menuButtonStyle;
            }

            switch (selectedPage)
            {
                case Page.DashBoard:
                    activateButton(DashboardButton);
                    break;
                case Page.Home:
                    activateButton(HomeButton);
                    break;
                case Page.Category:
                    activateButton(CategoryButton);
                    break;
                case Page.Promotion:
                    activateButton(PromotionButton);
                    break;
                case Page.Customer:
                    activateButton(CustomerButton);
                    break;
                case Page.Order:
                    activateButton(OrderButton);
                    break;
                case Page.Report:
                    activateButton(ReportButton);
                    break;
                default:
                    break;
            }
        }

        private void activateButton(Button button)
        {
            if (button != null)
            {
                button.Style = _menuButtonActiveStyle;
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
