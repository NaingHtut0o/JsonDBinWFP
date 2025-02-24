using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartHealthTest.Database;
using SmartHealthTest.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartHealthTest.Views
{
    public partial class AttributePage : Page
    {
        private JsonDatabase<AttributeMasterModel> _attributeDatabase;
        public ObservableCollection<AttributeMasterModel> AttributeData;
        private SolidColorBrush _colorBrush = new SolidColorBrush(Colors.LightYellow);
        private SolidColorBrush _altBrush = new SolidColorBrush(Colors.MintCream);
        public AttributePage()
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
            if (Application.Current.Resources["WindowBackground"] is SolidColorBrush brush)
                _colorBrush = brush;
            if (Application.Current.Resources["AlternateBrush"] is SolidColorBrush abrush)
                _altBrush = abrush;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (searchID.Text == "" || newName.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("You have to fill all data", "Fill All Data", MessageBoxButton.OK);
                if (searchID.Text == "")
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
            List<AttributeMasterModel> attributeMasterModels = new List<AttributeMasterModel>();
            if (searchID.Text != "" || newName.Text != "")
            {
                attributeMasterModels = _attributeDatabase.GetAll();
                if(searchID.Text != "")
                    attributeMasterModels = attributeMasterModels.Where(r => r.AttributeId == searchID.Text).ToList();
                if(newName.Text != "")
                    attributeMasterModels = attributeMasterModels.Where(r => r.AttributeName.Contains(newName.Text)).ToList();
            }
            AttributeData.Clear();
            foreach(var attModel in attributeMasterModels)
                AttributeData.Add(attModel);
            dgAttributes.Items.Refresh();
            CheckIfNoData();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            searchID.Text = string.Empty;
            newName.Text = string.Empty;
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

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + deleteAttributeModel.AttributeName, "Confirm Delete", MessageBoxButton.YesNo);
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
                e.Row.Background = _colorBrush;
            }
            else
            {
                e.Row.Background = _altBrush;
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
