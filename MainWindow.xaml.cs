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
            MainFrame.Navigate(new AttributePage());
        }

        private void NavigateRegisterAttribute(object sender, RoutedEventArgs e)
        {
            RegisterAttributePage page = new RegisterAttributePage();
            page.DataContext = page;
            MainFrame.Navigate(page);
        }

        private void NavigateCompany(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CompanyPage());
        }

        private void NavigateCompanyLink(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CompanyLinkPage());
        }

        private void NavigateBlank(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BlankPage());
        }
    }
}