using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Newtonsoft.Json.Linq;
using SmartHealthTest.Database;
using SmartHealthTest.Models;
using SmartHealthTest.Utilities;

namespace SmartHealthTest.Views
{
    public partial class HealthCheckPage : Page
    {
        private JsonDatabase<HealthCheckItemMasterModel> _healthCheckDatabase;
        private List<HealthCheckItemMasterModel> _allHealthCheck;
        private List<HealthCheckItemMasterModel> _currentHealthCheck;
        private List<HealthCheckItemMasterModel> _paginatedList;
        private List<HealthCheckItemMasterModel> _mortifiedList;
        public ObservableCollection<HealthCheckItemMasterModel> _showHealthCheck;
        private SolidColorBrush _btnBrush;
        private SolidColorBrush _winBrush;
        private SolidColorBrush _altBrush;
        private SolidColorBrush _rowColor;
        private int _itemId;
        private string _itemName;
        private string _unit;
        private int _currentPage;
        private int _pageSize;
        //private bool _isInitializing;

        public HealthCheckPage()
        {
            InitializeComponent();
            _healthCheckDatabase = new JsonDatabase<HealthCheckItemMasterModel>("health_check_item_master.json");
            LoadData();
            dgHealthCheck.ItemsSource = _showHealthCheck;
            CheckIfNoData();
            pageSize.SelectionChanged += ComboBoxSelectionChanged;

            this.SizeChanged += (s, e) =>
            {
                dgHealthCheck.MaxHeight = this.ActualHeight - 320;
            };
        }

        #region behind
        private void LoadData()
        {
            _allHealthCheck = new List<HealthCheckItemMasterModel>(_healthCheckDatabase.GetAll());
            _currentHealthCheck = _allHealthCheck.ToList();
            _paginatedList = new List<HealthCheckItemMasterModel>();
            _mortifiedList = new List<HealthCheckItemMasterModel>();
            _showHealthCheck = new ObservableCollection<HealthCheckItemMasterModel>(_allHealthCheck);
            _btnBrush = Application.Current.Resources["ButtonBrush"] as SolidColorBrush ?? new SolidColorBrush(Colors.DarkOrange);
            _winBrush = Application.Current.Resources["WindowBackground"] as SolidColorBrush ?? new SolidColorBrush(Colors.LightYellow);
            _altBrush = Application.Current.Resources["AlternateBrush"] as SolidColorBrush ?? new SolidColorBrush(Colors.MintCream);
            _rowColor = new SolidColorBrush();
            TextBoxData();
            _currentPage = 0;
            _pageSize = 5;
            pageSize.SelectedIndex = 0;
            RefreshList();
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
            Pagination();
            _showHealthCheck.Clear();
            foreach(HealthCheckItemMasterModel item in _paginatedList)
                _showHealthCheck.Add(item);
            dgHealthCheck.Items.Refresh();
            CheckIfNoData();
        }

        private void Pagination()
        {
            //MessageBox.Show(_allHealthCheck.Count.ToString());
            _paginatedList = _currentHealthCheck.Skip((_currentPage)*_pageSize).Take(_pageSize).ToList();
            txtPage.Text = $"Page {_currentPage + 1} of {Math.Ceiling((double)_currentHealthCheck.Count / _pageSize)}";
        }

        private void ClearTextBox()
        {
            searchID.Text = string.Empty;
            newName.Text = string.Empty;
            unit.Text = string.Empty;
            TextBoxData();
        }

        private void AssignTextBoxs(HealthCheckItemMasterModel copyModel)
        { 
            searchID.Text = copyModel.ItemId.ToString();
            newName.Text = copyModel.ItemName;
            unit.Text = copyModel.Unit;
            TextBoxData();
        }

        private void ClearPagination()
        {
            _currentPage = 0;
        }

        private DataGridCell GetCell(DataGrid dataGrid, DataGridRow row, int columnIndex)
        {
            DataGridCell cell = null;
            // Get the cell content, and then the cell itself
            var cellContent = dataGrid.Columns[columnIndex].GetCellContent(row);
            if (cellContent != null)
            {
                cell = cellContent.Parent as DataGridCell;
            }
            return cell;
        }
        #endregion behind

        #region front
        private void dgHealthCheck_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() % 2 == 0)
            {
                e.Row.SetResourceReference(DataGrid.BackgroundProperty, "WindowBackground");
            }
            else
            {
                e.Row.SetResourceReference(DataGrid.BackgroundProperty, "AlternateBrush");
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            TextBoxData();
            if(_itemId != 0 ||  _itemName !=  string.Empty || _unit != String.Empty)
            {
                _currentHealthCheck = _allHealthCheck.ToList();
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
            ClearPagination();
            RefreshList();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            ClearPagination();
            _currentHealthCheck = _allHealthCheck.ToList();
            RefreshList();
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        { 
            Button button = sender as Button;
            HealthCheckItemMasterModel delModel = button.Tag as HealthCheckItemMasterModel;

            MessageBoxResult result = MessageBox.Show(MessageClass.ConfirmDeleteMsg + delModel.ItemName, MessageClass.ConfirmDeleteTitle, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                  Delete(delModel.ItemId);
        }

        private void CopyContent(object sender, RoutedEventArgs e)
        {
            Button btnCopy = sender as Button;
            HealthCheckItemMasterModel cpyModel = btnCopy.Tag as HealthCheckItemMasterModel;

            AssignTextBoxs(cpyModel);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            TextBoxData();
            if (_itemId == 0 || _itemName == string.Empty || _unit == String.Empty)
            {
                MessageBoxResult result = MessageBox.Show(MessageClass.UnfillDataMsg, MessageClass.UnfillDataTitle, MessageBoxButton.OK);
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
                ClearPagination();
                RefreshList();
            }
        }

        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 0;
            RefreshList();
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 0)
            {
                _currentPage--;
            }
            RefreshList();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < (int)(Math.Ceiling((double)_currentHealthCheck.Count / _pageSize) - 1))
            {
                _currentPage++;
            }
            RefreshList();
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = (int)(Math.Ceiling((double)_currentHealthCheck.Count / _pageSize) - 1);
            RefreshList();
        }

        private void myDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is HealthCheckItemMasterModel modelItem)
            {
                var updatedValue = ((TextBox)e.EditingElement).Text;
                DataGridTextColumn column = (DataGridTextColumn)e.Column;
                Binding binding = column.Binding as Binding;

                if (binding != null)
                {
                    // Now you can access properties of the binding
                    string path = binding.Path.Path;
                    if (path != null)
                    {
                        var property = modelItem.GetType().GetProperty(path);

                        if (property != null)
                        {
                            // Set the property value dynamically
                            property.SetValue(modelItem, Convert.ChangeType(updatedValue, property.PropertyType));
                        }
                        else
                        {
                            var field = modelItem.GetType().GetField(path);
                            if (field != null)
                            {
                                field.SetValue(modelItem, updatedValue);
                            }
                        }
                    }
                }

                var existingItem = _mortifiedList.FirstOrDefault(item => item.ItemId == modelItem.ItemId);
                if (existingItem != null)
                {
                    _mortifiedList.Remove(existingItem);
                }
                _mortifiedList.Add(modelItem);
                var currentExistingItem = _currentHealthCheck.FirstOrDefault(item => item.ItemId == modelItem.ItemId);
                if (currentExistingItem != null)
                {
                    _currentHealthCheck.Remove(currentExistingItem);
                }
                _currentHealthCheck.Add(modelItem);
                _currentHealthCheck = _currentHealthCheck.OrderBy(r => r.ItemId).ToList();
            }
            RefreshList();
        }

        private void ComboBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (pageSize.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedString = selectedItem.Content.ToString();
                //MessageBox.Show(selectedString);
                _pageSize = int.Parse(selectedString);
                _currentPage = 0; // Reset to the first page when page size changes
                RefreshList();
                //MessageBox.Show(selectedString);
            }
        }
        #endregion front
    }
}
