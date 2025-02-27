using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SmartHealthTest.Database;
using SmartHealthTest.Models;

namespace SmartHealthTest.Views
{
    public partial class HealthCheckPage : Page
    {
        private JsonDatabase<HealthCheckItemMasterModel> _healthCheckDatabase;
        private List<HealthCheckItemMasterModel> _allHealthCheck;
        private List<HealthCheckItemMasterModel> _currentHealthCheck;
        public ObservableCollection<HealthCheckItemMasterModel> _showHealthCheck;
        private SolidColorBrush _btnBrush;
        private SolidColorBrush _winBrush;
        private SolidColorBrush _altBrush;
        private int _itemId;
        private string _itemName;
        private string _unit;

        public HealthCheckPage()
        {
            InitializeComponent();
            _healthCheckDatabase = new JsonDatabase<HealthCheckItemMasterModel>("health_check_item_master.json");
            LoadData();
            dgHealthCheck.ItemsSource = _showHealthCheck;
            CheckIfNoData();

            this.SizeChanged += (s, e) =>
            {
                dgHealthCheck.MaxHeight = this.ActualHeight - 280;
            };
        }

        #region behind
        private void LoadData()
        {
            _allHealthCheck = new List<HealthCheckItemMasterModel>(_healthCheckDatabase.GetAll());
            _currentHealthCheck = _allHealthCheck;
            _showHealthCheck = new ObservableCollection<HealthCheckItemMasterModel>(_allHealthCheck);
            _btnBrush = Application.Current.Resources["ButtonBrush"] as SolidColorBrush ?? new SolidColorBrush(Colors.DarkOrange);
            _winBrush = Application.Current.Resources["WindowBackground"] as SolidColorBrush ?? new SolidColorBrush(Colors.LightYellow);
            _altBrush = Application.Current.Resources["AlternateBrush"] as SolidColorBrush ?? new SolidColorBrush(Colors.MintCream);
            TextBoxData();
        }

        private void TextBoxData()
        {
            _itemId = searchID.Text == "" ? 0 : int.Parse(searchID.Text);
            _itemName = newName.Text ?? string.Empty;
            _unit = unit.Text ?? string.Empty;
        }

        private void CheckIfNoData()
        {
            if (dgHealthCheck.Items.Count == 0)
            {
                dgHealthCheck.Visibility = Visibility.Collapsed;
                noDataGrid.Visibility = Visibility.Visible;
            }
            else
            { 
                dgHealthCheck.Visibility = Visibility.Visible;
                noDataGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void Delete(int healthCheckId)
        { 
            HealthCheckItemMasterModel? delModel = _allHealthCheck.Where(r => r.ItemId == healthCheckId).FirstOrDefault();
            if (delModel != null)
            {
                _allHealthCheck.Remove(delModel);
                _currentHealthCheck.Remove(delModel);
                _healthCheckDatabase.SaveAll(_allHealthCheck);
                //_currentHealthCheck = _allHealthCheck;
            }
            RefreshList();
        }

        private void RefreshList()
        {
            _showHealthCheck.Clear();
            foreach(HealthCheckItemMasterModel item in _currentHealthCheck)
                _showHealthCheck.Add(item);
            dgHealthCheck.Items.Refresh();
            CheckIfNoData();
        }

        private void ClearTextBox()
        {
            searchID.Text = string.Empty;
            newName.Text = string.Empty;
            unit.Text = string.Empty;
            TextBoxData();
        }
        #endregion behind

        #region front
        private void dgHealthCheck_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() % 2 == 0)
            {
                e.Row.SetResourceReference(Button.BackgroundProperty, "WindowBackground");
            }
            else
            {
                e.Row.SetResourceReference(Button.BackgroundProperty, "AlternateBrush");
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            TextBoxData();
            if(_itemId != 0 ||  _itemName !=  string.Empty || _unit != String.Empty)
            {
                _currentHealthCheck = _allHealthCheck;
                if (_itemId != 0)
                    _currentHealthCheck = _currentHealthCheck.Where(r => r.ItemId == _itemId).ToList();
                if(_itemName != string.Empty)
                    _currentHealthCheck = _currentHealthCheck.Where(r => r.ItemName.Contains(_itemName)).ToList();
                if(_unit != string.Empty)
                    _currentHealthCheck = _currentHealthCheck.Where(r => r.Unit.Contains(_unit)).ToList();
            }
            else
            {
                _currentHealthCheck.Clear();
            }
            RefreshList();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            _currentHealthCheck = _allHealthCheck;
            RefreshList();
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        { 
            Button button = sender as Button;
            HealthCheckItemMasterModel delModel = button.Tag as HealthCheckItemMasterModel;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + delModel.ItemName, "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                  Delete(delModel.ItemId);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            TextBoxData();
            if (_itemId == 0 || _itemName == string.Empty || _unit == String.Empty)
            {
                MessageBoxResult result = MessageBox.Show("You have to fill all data", "Fill All Data", MessageBoxButton.OK);
                if (_itemId == 0)
                    searchID.Focus();
                if (_itemName == string.Empty)
                    newName.Focus();
                if (_unit == string.Empty)
                    unit.Focus();
            }
            else
            {
                HealthCheckItemMasterModel? editModel = _allHealthCheck.Where(r => r.ItemId == _itemId).FirstOrDefault();
                if (editModel != null)
                    _allHealthCheck.Remove(editModel);
                HealthCheckItemMasterModel newModel = new HealthCheckItemMasterModel
                {
                    ItemId = _itemId,
                    ItemName = _itemName,
                    Unit = _unit,
                };
                _allHealthCheck.Add(newModel);
                _allHealthCheck = _allHealthCheck.OrderBy(r => r.ItemId).ToList();
                _healthCheckDatabase.SaveAll(_allHealthCheck);
                _currentHealthCheck = _allHealthCheck;
                RefreshList();
            }
        }
        #endregion front
    }
}
