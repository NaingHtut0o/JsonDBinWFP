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
using NLog;
using SmartHealthTest.Database;
using SmartHealthTest.Models;
using SmartHealthTest.Utilities;
using SmartHealthTest.Views;

namespace SmartHealthTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public MainWindow()
        {
            InitializeComponent();
            //MainFrame.Navigate(new AttributePage()); // Load HomePage by default
            logger.Info(MessageClass.WelcomeMsg);
        }

        private void NavigateAttribute(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleChangeMsg);
                ResetAllButtonStyle();
                var button = sender as Button;
                button.Style = (Style)FindResource("ClickButtonStyle");
                logger.Info(MessageClass.NavigatePageMsg);
                MainFrame.Navigate(new AttributePage());
            }
            catch (Exception ex)
            {
                logger.Error(ex, MessageClass.NavigatePageErr);
            }
        }

        private void NavigateRegisterAttribute(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleChangeMsg);
                ResetAllButtonStyle();
                var button = sender as Button;
                button.Style = (Style)FindResource("ClickButtonStyle");
                RegisterAttributePage page = new RegisterAttributePage();
                page.DataContext = page;
                logger.Info(MessageClass.NavigatePageMsg);
                MainFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                logger.Error(ex, MessageClass.NavigatePageErr);
            }
        }

        private void NavigateCompany(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleChangeMsg);
                ResetAllButtonStyle();
                var button = sender as Button;
                button.Style = (Style)FindResource("ClickButtonStyle");
                logger.Info(MessageClass.NavigatePageMsg);
                MainFrame.Navigate(new CompanyPage());
            }
            catch (Exception ex)
            {
                logger.Error(ex, MessageClass.NavigatePageErr);
            }
        }

        private void NavigateCompanyLink(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleChangeMsg);
                ResetAllButtonStyle();
                var button = sender as Button;
                button.Style = (Style)FindResource("ClickButtonStyle");
                logger.Info(MessageClass.NavigatePageMsg);
                MainFrame.Navigate(new CompanyLinkPage());
            }
            catch (Exception ex)
            {
                logger.Error(ex, MessageClass.NavigatePageErr);
            }
        }

        private void NavigateHealthCheck(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleChangeMsg);
                ResetAllButtonStyle();
                var button = sender as Button;
                button.Style = (Style)FindResource("ClickButtonStyle");
                logger.Info(MessageClass.NavigatePageMsg);
                MainFrame.Navigate(new HealthCheckPage());
            }
            catch (Exception ex)
            {
                logger.Error(ex, MessageClass.NavigatePageErr);
            }
        }

        private void NavigateBlank(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleChangeMsg);
                ResetAllButtonStyle();
                var button = sender as Button;
                button.Style = (Style)FindResource("ClickButtonStyle");
                logger.Info(MessageClass.NavigatePageMsg);
                MainFrame.Navigate(new BlankPage());
            }
            catch (Exception ex)
            {
                logger.Error(ex, MessageClass.NavigatePageErr);
            }
        }

        private void ResetAllButtonStyle()
        {
            try
            {
                logger.Info(MessageClass.ButtonStyleResetMsg);
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
            catch(Exception ex)
            {
                logger.Error(ex, MessageClass.ButtonStyleResetErr);
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
            try
            {
                logger.Info(MessageClass.DarkModeMsg);
                // Apply Dark Theme
                var _winBrush = Application.Current.Resources["WindowBackgroundDark"];
                var _btnBrush = Application.Current.Resources["ButtonBrushDark"];
                var _prmBrush = Application.Current.Resources["PrimaryBrushDark"];
                var _altBrush = Application.Current.Resources["AlternateBrushDark"];
                var _clearBrush = Application.Current.Resources["ClearBrushDark"];
                var _dgBrush = Application.Current.Resources["DataGridBrushDark"];
                var _txtBrush = Application.Current.Resources["TextBrushDark"];

                Application.Current.Resources["WindowBackground"] = _winBrush;
                Application.Current.Resources["ButtonBrush"] = _btnBrush;
                Application.Current.Resources["PrimaryBrush"] = _prmBrush;
                Application.Current.Resources["AlternateBrush"] = _altBrush;
                Application.Current.Resources["ClearBrush"] = _clearBrush;
                Application.Current.Resources["DataGridBrush"] = _dgBrush;
                Application.Current.Resources["TextBrush"] = _txtBrush;

                // Update UI
                DarkModeToggle.Content = "🌙Dark Mode";
            }
            catch(Exception ex)
            {
                logger.Error(ex, MessageClass.DarkModeErr);
            }
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info(MessageClass.LightModeMsg);
                // Apply Light Theme
                var _winBrush = Application.Current.Resources["WindowBackgroundLight"];
                var _btnBrush = Application.Current.Resources["ButtonBrushLight"];
                var _prmBrush = Application.Current.Resources["PrimaryBrushLight"];
                var _altBrush = Application.Current.Resources["AlternateBrushLight"];
                var _clearBrush = Application.Current.Resources["ClearBrushLight"];
                var _dgBrush = Application.Current.Resources["DataGridBrushLight"];
                var _txtBrush = Application.Current.Resources["TextBrushLight"];

                Application.Current.Resources["WindowBackground"] = _winBrush;
                Application.Current.Resources["ButtonBrush"] = _btnBrush;
                Application.Current.Resources["PrimaryBrush"] = _prmBrush;
                Application.Current.Resources["AlternateBrush"] = _altBrush;
                Application.Current.Resources["ClearBrush"] = _clearBrush;
                Application.Current.Resources["DataGridBrush"] = _dgBrush;
                Application.Current.Resources["TextBrush"] = _txtBrush;

                // Update UI
                DarkModeToggle.Content = "☀️Light Mode";
            }
            catch(Exception ex)
            {
                logger.Error(ex, MessageClass.LightModeErr);
            }
        }
    }
}