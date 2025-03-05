using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using NLog;
using SmartHealthTest.Database;
using SmartHealthTest.Models;

namespace SmartHealthTest.Views
{
    /// <summary>
    /// Interaction logic for HealthCheckRegisterPage.xaml
    /// </summary>
    public partial class HealthCheckRegisterPage : Page
    {
        private JsonDatabase<HealthCheckItemMasterModel> _healthCheckItemDatabase;
        private JsonDatabase<UrbanOsUsersModel> _osUserDatabase;
        private JsonDatabase<UrbanOsHealthCheckModel> _userHealthCheckDatabase;
        private JsonDatabase<UrbanOsHealthCheckItemModel> _userHealthCheckItemDatabase;
        private List<HealthCheckItemMasterModel> _allHealthCheckItem;
        private List<UrbanOsUsersModel> _allOsUser;
        private List<UrbanOsHealthCheckModel> _allOsHealthCheck;
        private List<UrbanOsHealthCheckItemModel> _allOsHealthCheckItem;
        private List<UrbanOsHealthCheckModel> _currentOsHealthCheck;
        private List<UrbanOsHealthCheckItemModel> _currentOsHealthCheckItem;
        private List<UrbanOsHealthCheckItemModel> _currentOsHealthCheckItemCollection;
        private List<UrbanOsHealthCheckItemModel> _modifyOsHealthCheckItemCollection;
        private List<UrbanOsHealthCheckModel> _paginationOsHealthCheck;
        private ObservableCollection<Dictionary<string, string>> _showOsHealthCheck;
        private string _searchID;
        private Dictionary<int, TextBox> _textBoxes;
        private int _currentPage;
        private int _pageSize;
        private Style _headerStyle;
        private Style _cellStyle;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int _maxId;
        public HealthCheckRegisterPage()
        {
            InitializeComponent();
            LoadData();
            CreateTextBoxes();
            RefreshList();
            pageSize.SelectionChanged += ComboBoxSelectionChanged;
        }

        #region behind
        private void LoadData()
        {
            _healthCheckItemDatabase = new JsonDatabase<HealthCheckItemMasterModel>("health_check_item_master.json");
            _osUserDatabase = new JsonDatabase<UrbanOsUsersModel>("urban_os_users.json");
            _userHealthCheckDatabase = new JsonDatabase<UrbanOsHealthCheckModel>("urban_os_health_check.json");
            _userHealthCheckItemDatabase = new JsonDatabase<UrbanOsHealthCheckItemModel>("urban_os_health_check_items.json");
            _allHealthCheckItem = new List<HealthCheckItemMasterModel>(_healthCheckItemDatabase.GetAll());
            _allOsUser = new List<UrbanOsUsersModel>(_osUserDatabase.GetAll());
            _allOsHealthCheck = new List<UrbanOsHealthCheckModel>(_userHealthCheckDatabase.GetAll());
            _allOsHealthCheckItem = new List<UrbanOsHealthCheckItemModel>(_userHealthCheckItemDatabase.GetAll());
            _currentOsHealthCheck = new List<UrbanOsHealthCheckModel>();
            _currentOsHealthCheckItem = new List<UrbanOsHealthCheckItemModel>();
            _currentOsHealthCheckItemCollection = _allOsHealthCheckItem.ToList();
            _modifyOsHealthCheckItemCollection = new List<UrbanOsHealthCheckItemModel>();
            _paginationOsHealthCheck = new List<UrbanOsHealthCheckModel>();
            _showOsHealthCheck = new ObservableCollection<Dictionary<string, string>>();
            _currentPage = 0;
            _pageSize = 5;
            _maxId = _allOsHealthCheckItem.Max(item => item.Id);
            CreateDataGridStyle();
        }

        private void CreateTextBoxes()
        {
            GetTextBoxData();

            _textBoxes = new Dictionary<int, TextBox>();
            int i = 0;
            foreach (var healthCheck in _allHealthCheckItem)
            {
                itemNames.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Grid grid = new Grid
                {
                    Margin = new Thickness(80, 10, 50, 0),
                    Height = 35,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                TextBlock textBlock = new TextBlock
                {
                    Text = healthCheck.ItemName,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Width = 85,
                    TextAlignment = TextAlignment.Right,
                    //Foreground = prmBrush,
                };
                textBlock.SetResourceReference(TextBlock.ForegroundProperty, "DataGridBrush");
                Grid.SetColumn(textBlock, 0);
                grid.Children.Add(textBlock);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                TextBox textBox = new TextBox
                {
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    Width = 420,
                    Margin = new Thickness(5, 0, 5, 0),
                    //Background = _altBrush,
                    //Text = attribute.AttributeName,
                };
                textBox.SetResourceReference(TextBox.ForegroundProperty, "DataGridBrush");
                textBox.SetResourceReference(TextBox.BackgroundProperty, "AlternateBrush");
                _textBoxes.Add(healthCheck.ItemId, textBox);
                Grid.SetColumn(textBox, 1);
                grid.Children.Add(textBox);
                Grid.SetRow(grid, i);
                itemNames.Children.Add(grid);
                i++;
            }
        }

        private void CreateNotFound()
        {
            dataGridFrame.Children.Clear();
            dataGridFrame.RowDefinitions.Clear();
            dataGridFrame.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Pixel) });
            dataGridFrame.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Clear();
            int i = 0;
            foreach (var header in _allHealthCheckItem)
            {
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Border border = new Border
                {
                    Child = new TextBlock
                    {
                        Text = $"{header.ItemName} ( {header.Unit} )",
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White),
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    }
                };
                Grid.SetColumn(border, i);
                headerGrid.Children.Add(border);
                i++;
            }
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Border border1 = new Border
            {
                Child = new TextBlock
                {
                    Text = " 活動",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White),
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                }
            };
            Grid.SetColumn(border1, i);
            headerGrid.Children.Add(border1);

            headerGrid.SetResourceReference(Grid.BackgroundProperty, "PrimaryBrush");

            Grid.SetRow(headerGrid, 0);
            dataGridFrame.Children.Add(headerGrid);

            Grid bodyGrid = new Grid();
            bodyGrid.Children.Clear();
            bodyGrid.ColumnDefinitions.Clear();
            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            TextBlock textBlock = new TextBlock
            {
                Text = "データ 無し",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 50,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30)
                //Foreground = new SolidColorBrush(Colors.Red),
            };
            textBlock.SetResourceReference(TextBlock.ForegroundProperty, "ClearBrush");
            Border border2 = new Border
            {
                BorderThickness = new Thickness(1),
                Child = textBlock
            };
            border2.SetResourceReference(Border.BorderBrushProperty, "PrimaryBrush");
            Grid.SetColumn(border2, 0);
            bodyGrid.Children.Add(border2);

            Grid.SetRow(bodyGrid, 1);
            dataGridFrame.Children.Add(bodyGrid);
        }

        private void CreateFound()
        {
            dataGridFrame.Children.Clear();
            dataGridFrame.RowDefinitions.Clear();
            dataGridFrame.ColumnDefinitions.Clear();
            dataGridFrame.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            DataGrid dataGrid = new DataGrid
            {
                AutoGenerateColumns = false, // Define columns manually
                CanUserAddRows = false,
                IsReadOnly = false,
                CanUserDeleteRows = true,
                CanUserSortColumns = true,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                ColumnHeaderHeight = 50,
                RowHeight = 40,
                FontSize = 16,
                BorderThickness = new Thickness(1, 0, 0, 0),
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden
            };
            dataGrid.CellEditEnding += myDataGrid_CellEditEnding;

            dataGrid.SetResourceReference(DataGrid.BorderBrushProperty, "ButtonBrush");
            dataGrid.SetResourceReference(DataGrid.VerticalGridLinesBrushProperty, "ButtonBrush");
            dataGrid.SetResourceReference(DataGrid.HorizontalGridLinesBrushProperty, "ButtonBrush");
            dataGrid.SetResourceReference(DataGrid.RowBackgroundProperty, "WindowBackground");
            dataGrid.SetResourceReference(DataGrid.AlternatingRowBackgroundProperty, "AlternateBrush");

            dataGrid.ColumnHeaderStyle = _headerStyle;
            dataGrid.CellStyle = _cellStyle;

            dataGrid.Columns.Clear();
            dataGrid = AddHeaders(dataGrid);
            dataGrid = AddTemplateHeader(dataGrid);

            dataGrid.ItemsSource = _showOsHealthCheck;

            Grid.SetRow(dataGrid, 0);
            dataGridFrame.Children.Add(dataGrid);
        }

        private DataGrid AddHeaders(DataGrid dataGrid)
        {
            //DataGridTextColumn column1 = new DataGridTextColumn
            //{ 
            //    Header = "Id",
            //    Binding = new Binding("Id"),
            //    Visibility = Visibility.Collapsed,
            //};
            //dataGrid.Columns.Add(column1);

            //foreach (HealthCheckItemMasterModel item in _allHealthCheckItem)
            //{
            //    DataGridTextColumn column = new DataGridTextColumn
            //    {
            //        Header = item.ItemName, // Column header
            //        Binding = new Binding(item.ItemName), // Bind dictionary key
            //        Width = new DataGridLength( 1, DataGridLengthUnitType.Star),
            //    };

            //    dataGrid.Columns.Add(column); // Add column to DataGrid
            //}
            if (_showOsHealthCheck.Any())
            {
                // Get all unique keys from the first row (assuming consistent structure)
                foreach (var key in _showOsHealthCheck.First().Keys)
                {
                    DataGridTextColumn column = new DataGridTextColumn
                    {
                        Header = key, // Column header
                        Binding = new Binding($"[{key}]"), // Bind dictionary key
                        Width = new DataGridLength( 1, DataGridLengthUnitType.Star),
                    };// Set ElementStyle (display style when not in edit mode)
                    Style elementStyle = new Style(typeof(TextBlock));
                    elementStyle.BasedOn = (Style)Application.Current.Resources["DataGridTextColumnElementStyle"];
                    column.ElementStyle = elementStyle;

                    // Set EditingElementStyle (display style when editing the cell)
                    Style editingElementStyle = new Style(typeof(TextBox));
                    editingElementStyle.BasedOn = (Style)Application.Current.Resources["DataGridBoxColumnElementStyle"];
                    column.EditingElementStyle = editingElementStyle;
                    if (key == "Id")
                        column.Visibility = Visibility.Collapsed;

                    dataGrid.Columns.Add(column); // Add column to DataGrid
                }
            }
            return dataGrid;
        }

        private DataGrid AddTemplateHeader(DataGrid dataGrid)
        {
            try
            {
                // Create the DataGridTemplateColumn
                DataGridTemplateColumn activityColumn = new DataGridTemplateColumn();
                activityColumn.Header = "活動";
                activityColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                // Create a DataTemplate
                DataTemplate dataTemplate = new DataTemplate();

                // Create a FrameworkElementFactory for the Grid (root of DataTemplate)
                FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));
                gridFactory.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                // Define the Grid Columns
                FrameworkElementFactory colDef1 = new FrameworkElementFactory(typeof(ColumnDefinition));
                FrameworkElementFactory colDef2 = new FrameworkElementFactory(typeof(ColumnDefinition));
                gridFactory.AppendChild(colDef1);
                gridFactory.AppendChild(colDef2);

                // Create the first Button (Delete)
                FrameworkElementFactory deleteButtonFactory = new FrameworkElementFactory(typeof(Button));
                deleteButtonFactory.SetValue(Button.ContentProperty, "\uE74D"); // Delete Icon
                deleteButtonFactory.SetValue(Button.WidthProperty, 20.0);
                deleteButtonFactory.SetValue(Button.MarginProperty, new Thickness(8, 0, 0, 0));
                deleteButtonFactory.SetValue(Button.BorderThicknessProperty, new Thickness(0));
                deleteButtonFactory.SetValue(Button.TagProperty, new Binding("."));
                deleteButtonFactory.SetValue(Button.FontFamilyProperty, new FontFamily("Segoe MDL2 Assets"));
                deleteButtonFactory.SetValue(Button.StyleProperty, Application.Current.Resources["DeleteStyle"]);
                deleteButtonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(ConfirmDelete));

                // Create the second Button (Copy)
                FrameworkElementFactory copyButtonFactory = new FrameworkElementFactory(typeof(Button));
                copyButtonFactory.SetValue(Button.ContentProperty, "\uE8C8"); // Copy Icon
                copyButtonFactory.SetValue(Button.WidthProperty, 20.0);
                copyButtonFactory.SetValue(Button.MarginProperty, new Thickness(8, 0, 0, 0));
                copyButtonFactory.SetValue(Button.BorderThicknessProperty, new Thickness(0));
                copyButtonFactory.SetValue(Button.TagProperty, new Binding("."));
                copyButtonFactory.SetValue(Button.FontFamilyProperty, new FontFamily("Segoe MDL2 Assets"));
                copyButtonFactory.SetValue(Button.StyleProperty, Application.Current.Resources["CopyStyle"]);
                copyButtonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(CopyContent));

                // Set the grid columns for buttons
                deleteButtonFactory.SetValue(Grid.ColumnProperty, 1);
                copyButtonFactory.SetValue(Grid.ColumnProperty, 0);

                // Append the buttons to the gridFactory
                gridFactory.AppendChild(deleteButtonFactory);
                gridFactory.AppendChild(copyButtonFactory);

                // Set the visual tree for DataTemplate
                dataTemplate.VisualTree = gridFactory;

                // Set the CellTemplate to the DataTemplate
                activityColumn.CellTemplate = dataTemplate;

                // Add the column to the DataGrid
                dataGrid.Columns.Add(activityColumn);
            }
            catch (Exception ex)
            {
                // Log the exception with the error level
                logger.Error(ex, "An error occurred while performing an operation.");
            }
            return dataGrid;
        }

        private void RefreshList()
        {
            Pagination();
            _showOsHealthCheck.Clear();
            foreach (var idGroup in _currentOsHealthCheckItem.GroupBy(b => b.HealthCheckId))
            {
                var newRow = new Dictionary<string, string>
                {
                    { "Id", idGroup.Key.ToString() } // Set ID
                };

                foreach (var item in _allHealthCheckItem)
                {
                    string headerName = $"{item.ItemName} ( {item.Unit} )";

                    string? itemValue = idGroup.Where(r => r.ItemId == item.ItemId).FirstOrDefault()?.ItemValue;
                    if (itemValue != null)
                        newRow[headerName] = itemValue;
                    else
                        newRow[headerName] = String.Empty;
                }
                //MessageBox.Show(string.Join(", ", newRow.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
                _showOsHealthCheck.Add(newRow);
            }
            CheckIfNoData();
        }

        private void Pagination()
        {
            //MessageBox.Show(_allHealthCheck.Count.ToString());
            _paginationOsHealthCheck = _currentOsHealthCheck.Skip(_currentPage * _pageSize).Take(_pageSize).ToList();
            _currentOsHealthCheckItem = _currentOsHealthCheckItemCollection.Join(_paginationOsHealthCheck, 
                    allItem => allItem.HealthCheckId,
                    paginationItem => paginationItem.Id,
                    (allItem, paginationItem) => allItem).ToList();
            txtPage.Text = $"Page {_currentPage + 1} of {Math.Ceiling((double)_currentOsHealthCheck.Count / _pageSize)}";
        }

        private void CheckIfNoData()
        {
            if (_currentOsHealthCheck.Count == 0)
            {
                CreateNotFound();
            }
            else
            {
                CreateFound();
            }
        }

        private void GetTextBoxData()
        {
            _searchID = searchID.Text ?? String.Empty;
        }

        private void ClearTextBox()
        {
            _searchID = String.Empty;
            searchID.Clear();
        }

        private void ClearPagination()
        {
            _currentPage = 0;
        }

        private void CreateDataGridStyle()
        {
            _headerStyle = new Style(typeof(DataGridColumnHeader));

            // Background
            _headerStyle.Setters.Add(new Setter(Control.BackgroundProperty, new DynamicResourceExtension("PrimaryBrush")));

            // Foreground
            _headerStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White));

            // Font Properties
            _headerStyle.Setters.Add(new Setter(Control.FontSizeProperty, 18.0));
            _headerStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));

            // Border
            _headerStyle.Setters.Add(new Setter(Control.BorderBrushProperty, Brushes.Transparent));
            _headerStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));

            // Horizontal Alignment
            _headerStyle.Setters.Add(new Setter(Control.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));

            // Custom Template
            ControlTemplate headerTemplate = new ControlTemplate(typeof(DataGridColumnHeader));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.BackgroundProperty, new DynamicResourceExtension("PrimaryBrush"));
            border.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
            border.SetValue(Border.BorderThicknessProperty, new Thickness(0));

            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            border.AppendChild(contentPresenter);
            headerTemplate.VisualTree = border;
            _headerStyle.Setters.Add(new Setter(Control.TemplateProperty, headerTemplate));

            _cellStyle = new Style(typeof(DataGridCell));

            // Border
            _cellStyle.Setters.Add(new Setter(Control.BorderBrushProperty, Brushes.Transparent));
            _cellStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));

            // Disable Focus Visual Style
            _cellStyle.Setters.Add(new Setter(Control.FocusVisualStyleProperty, null));

            // Foreground
            _cellStyle.Setters.Add(new Setter(Control.ForegroundProperty, new DynamicResourceExtension("DataGridBrush")));
        }
        #endregion behind

        #region CRUD
        private void Search(object sender, RoutedEventArgs e)
        {
            GetTextBoxData();
            if (_searchID != String.Empty)
            {
                _currentOsHealthCheck = _allOsHealthCheck.ToList();
                _currentOsHealthCheck = _currentOsHealthCheck.Where(r => r.UrbanOsId == _searchID).ToList();
            }
            else
            {
                _currentOsHealthCheck = new List<UrbanOsHealthCheckModel>();
            }
            ClearPagination();
            RefreshList();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            ClearPagination();
            RefreshList();
        }

        private void Update(object sender, RoutedEventArgs e)
        {

        }
        #endregion CRUD

        #region front

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
            if (_currentPage < (int)(Math.Ceiling((double)_currentOsHealthCheck.Count / _pageSize) - 1))
            {
                _currentPage++;
            }
            RefreshList();
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = (int)(Math.Ceiling((double)_currentOsHealthCheck.Count / _pageSize) - 1);
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
                //RefreshList();
                //MessageBox.Show(selectedString);
            }
        }
        private void myDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.Row.Item is Dictionary<string, string> modelItem)
                {
                    var updatedValue = ((TextBox)e.EditingElement).Text;
                    DataGridTextColumn column = (DataGridTextColumn)e.Column;
                    Binding binding = column.Binding as Binding;

                    if (binding != null)
                    {
                        string path = binding.Path.Path;
                        if (path != null)
                        {
                            path = path.Trim('[', ']');
                            modelItem[path] = updatedValue;
                            var match = Regex.Match(path, @"^\s*(\S+)\s*\(\s*(\S+)\s*\)\s*$");

                            if (match.Success)
                            {
                                string itemName = match.Groups[1].Value;
                                string unit = match.Groups[2].Value;
                                int id = int.Parse(modelItem["Id"]);

                                int itemId = _allHealthCheckItem.Where(item => item.ItemName == itemName && item.Unit == unit).FirstOrDefault().ItemId;

                                var existingItem = _modifyOsHealthCheckItemCollection.FirstOrDefault(item => item.ItemId == itemId && item.HealthCheckId == id);
                                if (existingItem != null)
                                    _modifyOsHealthCheckItemCollection.Remove(existingItem);
                                var currentExistingItem = _currentOsHealthCheckItemCollection.FirstOrDefault(item => item.ItemId == itemId && item.HealthCheckId == id);
                                UrbanOsHealthCheckItemModel updatedModel = new UrbanOsHealthCheckItemModel();
                                if (currentExistingItem != null)
                                {
                                    updatedModel = new UrbanOsHealthCheckItemModel
                                    { 
                                        Id = currentExistingItem.Id,
                                        ItemId = currentExistingItem.ItemId,
                                        HealthCheckId = currentExistingItem.HealthCheckId,
                                        ItemValue = updatedValue,
                                    };
                                    _currentOsHealthCheckItemCollection.Remove(currentExistingItem);
                                }
                                else if(updatedValue != "")
                                {
                                    updatedModel = new UrbanOsHealthCheckItemModel
                                    {
                                        Id = _maxId++,
                                        ItemId = itemId,
                                        HealthCheckId = id,
                                        ItemValue = updatedValue,
                                    };
                                }
                                if (updatedValue != "")
                                {
                                    _currentOsHealthCheckItemCollection.Add(updatedModel);
                                    _modifyOsHealthCheckItemCollection.Add(updatedModel);
                                }
                                _currentOsHealthCheckItemCollection = _currentOsHealthCheckItemCollection.OrderBy(r => r.ItemId).ToList();
                            }
                        }
                    }
                    MessageBox.Show(_modifyOsHealthCheckItemCollection.Count.ToString());
                    MessageBox.Show(_currentOsHealthCheckItemCollection.Count.ToString());
                    //MessageBox.Show(String.Join(", ", modelItem));
                }
                RefreshList();
            }
            catch (Exception ex)
            {
                // Log the exception with the error level
                logger.Error(ex, "An error occurred while performing an operation.");
            }
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {
            Button btnCopy = sender as Button;
            Dictionary<string, string> cpyModel = btnCopy.Tag as Dictionary<string,string>;
            MessageBox.Show(string.Join(", ", cpyModel));
        }

        private void CopyContent(object sender, RoutedEventArgs e)
        {
            Button btnCopy = sender as Button;
            Dictionary<string, string> cpyModel = btnCopy.Tag as Dictionary<string, string>;
            //MessageBox.Show(string.Join(", ", cpyModel));
            //MessageBox.Show(string.Join(", ", _textBoxes));

            foreach(var item in _textBoxes)
            {
                int itemId = item.Key;
                HealthCheckItemMasterModel model = _allHealthCheckItem.Where(r => r.ItemId == itemId).FirstOrDefault();
                string headerName = $"{model.ItemName} ( {model.Unit} )";
                if(cpyModel.ContainsKey(headerName))
                    item.Value.Text = cpyModel[headerName];
            }
        }
        #endregion front
    }
}
