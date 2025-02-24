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
    }
}