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

namespace SmartHealthTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JsonDatabase<AttributeMasterModel> _attributeDatabase;
        public ICommand Register;
        public ObservableCollection<AttributeMasterModel> AttributeData;
        public MainWindow()
        {
            InitializeComponent();
            _attributeDatabase = new JsonDatabase<AttributeMasterModel>("attribute_master.json");
            LoadData();
            dgAttributes.ItemsSource = AttributeData;
            CheckIfNoData();
        }

        private void CheckIfNoData()
        {
            if (dgAttributes.Items.Count == 0)
            {
                dgAttributes.Visibility = Visibility.Collapsed;
                NoDataGrid.Visibility = Visibility.Visible; // Show "No Data Found" message
            }
            else
            {
                dgAttributes.Visibility = Visibility.Visible;
                NoDataGrid.Visibility = Visibility.Collapsed; // Hide message
            }
        }

        private void LoadData()
        {
            AttributeData = new ObservableCollection<AttributeMasterModel>(_attributeDatabase.GetAll());
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (searchID.Text == "" || newName.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("You have to fill all data", "Fill All Data", MessageBoxButton.OK);
                if(searchID.Text == "")
                    searchID.Focus();
                else
                    newName.Focus();
            }
            else
            {
                List<AttributeMasterModel> attributeMasterModels = _attributeDatabase.GetAll();
                AttributeMasterModel attModel = attributeMasterModels.FirstOrDefault(r => r.AttributeId == searchID.Text);
                AttributeMasterModel newAttributeModel = new AttributeMasterModel
                {
                    AttributeId = searchID.Text,
                    AttributeName = newName.Text
                };
                if (attModel != null)
                    attributeMasterModels.Remove(attModel);
                attributeMasterModels.Add(newAttributeModel);
                attributeMasterModels = attributeMasterModels.OrderBy(r => r.AttributeId).ToList();
                _attributeDatabase.SaveAll(attributeMasterModels);
                RefreshList();
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            List<AttributeMasterModel> attributeMasterModels = _attributeDatabase.GetAll();
            AttributeMasterModel attModel = attributeMasterModels.FirstOrDefault(r => r.AttributeId == searchID.Text);
            AttributeData.Clear();
            if (attModel != null)
                AttributeData.Add(attModel);
            dgAttributes.Items.Refresh();
            CheckIfNoData();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            searchID.Text = string.Empty;
            List<AttributeMasterModel> attributeMasterModels = _attributeDatabase.GetAll();
            AttributeData.Clear();
            foreach (var item in attributeMasterModels)
            {
                AttributeData.Add(item);
            }
            dgAttributes.Items.Refresh();
            CheckIfNoData();
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.Tag;
            AttributeMasterModel deleteAttributeModel = (AttributeMasterModel)item;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete "+ deleteAttributeModel.AttributeName, "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                List<AttributeMasterModel> attributeMasterModels = _attributeDatabase.GetAll();
                AttributeMasterModel attModel = attributeMasterModels.FirstOrDefault(r => r.AttributeId == deleteAttributeModel.AttributeId);
                if (attModel != null)
                    attributeMasterModels.Remove(attModel);
                _attributeDatabase.SaveAll(attributeMasterModels);
                AttributeData.Remove(deleteAttributeModel);
            }
            dgAttributes.Items.Refresh();
            CheckIfNoData();
        }

        private void dgAttributes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() % 2 == 0)
            {
                e.Row.Background = Brushes.LightYellow;
            }
            else
            {
                e.Row.Background = Brushes.MintCream;
            }
        }

        public void RefreshList()
        {
            AttributeData.Clear();
            List<AttributeMasterModel> newAttributeMasterModels = _attributeDatabase.GetAll();
            foreach (var item in newAttributeMasterModels)
            {
                AttributeData.Add(item);
            }
            dgAttributes.Items.Refresh();
        }
    }
}