using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartHealthTest.Database;
using SmartHealthTest.Models;
using SmartHealthTest.Views;

namespace SmartHealthTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MainFrame.Navigate(new AttributePage()); // Load HomePage by default
        }

        private void NavigateAttribute(object sender, RoutedEventArgs e)
        {
            ResetAllButtonStyle();
            var button = sender as Button;
            button.Style = (Style)FindResource("ClickButtonStyle");
            MainFrame.Navigate(new AttributePage());
        }

        private void NavigateRegisterAttribute(object sender, RoutedEventArgs e)
        {
            ResetAllButtonStyle();
            var button = sender as Button;
            button.Style = (Style)FindResource("ClickButtonStyle");
            RegisterAttributePage page = new RegisterAttributePage();
            page.DataContext = page;
            MainFrame.Navigate(page);
        }

        private void NavigateCompany(object sender, RoutedEventArgs e)
        {
            ResetAllButtonStyle();
            var button = sender as Button;
            button.Style = (Style)FindResource("ClickButtonStyle");
            MainFrame.Navigate(new CompanyPage());
        }

        private void NavigateCompanyLink(object sender, RoutedEventArgs e)
        {
            ResetAllButtonStyle();
            var button = sender as Button;
            button.Style = (Style)FindResource("ClickButtonStyle");
            MainFrame.Navigate(new CompanyLinkPage());
        }

        private void NavigateHealthCheck(object sender, RoutedEventArgs e)
        {
            ResetAllButtonStyle();
            var button = sender as Button;
            button.Style = (Style)FindResource("ClickButtonStyle");
            MainFrame.Navigate(new HealthCheckPage());
        }

        private void NavigateBlank(object sender, RoutedEventArgs e)
        {
            ResetAllButtonStyle();
            var button = sender as Button;
            button.Style = (Style)FindResource("ClickButtonStyle");
            MainFrame.Navigate(new BlankPage());
        }

        private void ResetAllButtonStyle()
        {
            // List to store all the buttons found in the window
            List<Button> buttons = new List<Button>();

            // Get all buttons inside the window by traversing the visual tree
            GetButtonsRecursive(this, buttons);

            // Example: You can loop through the buttons and do something with them
            foreach (Button button in buttons)
            {
                // You can print the button's name or manipulate it here
                button.Style = (Style)FindResource("LinkButtonStyle"); ;
            }
        }

        private void GetButtonsRecursive(DependencyObject parent, List<Button> buttons)
        {
            // Check if the current parent is a Button
            if (parent is Button button)
            {
                buttons.Add(button);
            }

            // Recursively search for buttons in all child elements
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                GetButtonsRecursive(child, buttons);
            }
        }
        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Apply Dark Theme
            var _winBrush = Application.Current.Resources["WindowBackgroundDark"];
            var _btnBrush = Application.Current.Resources["ButtonBrushDark"];
            var _prmBrush = Application.Current.Resources["PrimaryBrushDark"];
            var _altBrush = Application.Current.Resources["AlternateBrushDark"];
            var _clearBrush = Application.Current.Resources["ClearBrushDark"];
            var _dgBrush = Application.Current.Resources["DataGridBrushDark"];

            Application.Current.Resources["WindowBackground"] = _winBrush;
            Application.Current.Resources["ButtonBrush"] = _btnBrush;
            Application.Current.Resources["PrimaryBrush"] = _prmBrush;
            Application.Current.Resources["AlternateBrush"] = _altBrush;
            Application.Current.Resources["ClearBrush"] = _clearBrush;
            Application.Current.Resources["DataGridBrush"] = _dgBrush;

            // Update UI
            DarkModeToggle.Content = "🌙Dark Mode";
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Apply Light Theme
            var _winBrush = Application.Current.Resources["WindowBackgroundLight"];
            var _btnBrush = Application.Current.Resources["ButtonBrushLight"];
            var _prmBrush = Application.Current.Resources["PrimaryBrushLight"];
            var _altBrush = Application.Current.Resources["AlternateBrushLight"];
            var _clearBrush = Application.Current.Resources["ClearBrushLight"];
            var _dgBrush = Application.Current.Resources["DataGridBrushLight"];

            Application.Current.Resources["WindowBackground"] = _winBrush;
            Application.Current.Resources["ButtonBrush"] = _btnBrush;
            Application.Current.Resources["PrimaryBrush"] = _prmBrush;
            Application.Current.Resources["AlternateBrush"] = _altBrush;
            Application.Current.Resources["ClearBrush"] = _clearBrush;
            Application.Current.Resources["DataGridBrush"] = _dgBrush;

            // Update UI
            DarkModeToggle.Content = "☀️Light Mode";
        }
    }
}