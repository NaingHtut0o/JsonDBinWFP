using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using SmartHealthTest.Database;
using SmartHealthTest.Models;

namespace SmartHealthTest.Views
{
    public partial class RegisterAttributePage : Page
    {
        private JsonDatabase<AttributeMasterModel> _attributeDatabase;
        private JsonDatabase<UrbanOsUsersModel> _userDatabase;
        private JsonDatabase<UrbanOsAttributesModel> _userAttributesDatabase;
        private List<AttributeMasterModel> AttributeData;
        private Dictionary<string, TextBox> textBoxes;
        private string currentOsId;
        private SolidColorBrush btnBrush;
        private SolidColorBrush prmBrush;
        private SolidColorBrush _colorBrush = new SolidColorBrush(Colors.LightYellow);

        public RegisterAttributePage()
        {
            InitializeComponent();
            _attributeDatabase = new JsonDatabase<AttributeMasterModel>("attribute_master.json");
            _userDatabase = new JsonDatabase<UrbanOsUsersModel>("urban_os_users.json");
            _userAttributesDatabase = new JsonDatabase<UrbanOsAttributesModel>("urban_os_attributes.json");
            LoadData();
            this.DataContext = this;
            CreateColumns();
            //dgAttributes.ItemsSource = AttributeData;
        }

        /* private void LoadData()
        {
            AttributeNames = new ObservableCollection<string>();
            AttributeData = _attributeDatabase.GetAll();
            foreach (var attribute in AttributeData)
            {
                AttributeNames.Add(attribute.AttributeName);
            }
            _attributeNames.ItemsSource = AttributeNames;
            //foreach(string name in AttributeNames)
            //{
            //    MessageBox.Show(name);
            //}
        } */

        private void LoadData()
        {
            _attributeNames.RowDefinitions.Clear();
            AttributeData = _attributeDatabase.GetAll();
            textBoxes = new Dictionary<string, TextBox>();
            btnBrush = new SolidColorBrush(Colors.Black);
            prmBrush = new SolidColorBrush(Colors.DarkOrange);
            int i = 0;
            foreach (var attribute in AttributeData)
            {
                _attributeNames.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Grid grid = new Grid
                {
                    Margin = new Thickness(80, 10, 50, 0),
                    Height = 30,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                TextBlock textBlock = new TextBlock
                { 
                    Text = attribute.AttributeName,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Width = 75,
                };
                Grid.SetColumn(textBlock, 0);
                grid.Children.Add(textBlock);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                TextBox textBox = new TextBox
                {
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    Width = 430,
                    Margin = new Thickness(0, 0, 5, 0),
                    //Text = attribute.AttributeName,
                };
                textBoxes.Add(attribute.AttributeId, textBox);
                Grid.SetColumn(textBox, 1);
                grid.Children.Add(textBox);
                Grid.SetRow(grid, i);
                _attributeNames.Children.Add(grid);
                i++;
            }
            if (Application.Current.Resources["ButtonBrush"] is SolidColorBrush brush)
                btnBrush = brush as SolidColorBrush;
            if (Application.Current.Resources["PrimaryBrush"] is SolidColorBrush pbrush)
                prmBrush = pbrush as SolidColorBrush;
            if (Application.Current.Resources["WindowBackground"] is SolidColorBrush bbrush)
                _colorBrush = bbrush;
        }

        private void CreateColumns()
        {
            _headerGrid.ColumnDefinitions.Clear();
            int i = 0;
            foreach(var header in AttributeData)
            {
                _headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Border border = new Border
                {
                    //BorderThickness = new Thickness(1, 0, 1, 1),
                    //BorderBrush = new SolidColorBrush(Colors.White),
                    Child = new TextBlock
                    {
                        Text = " " + header.AttributeName,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White),
                    }
                };
                Grid.SetColumn(border, i);
                _headerGrid.Children.Add(border);
                i++;
            }
            _headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) });
            Border border1 = new Border
            {
                //BorderThickness = new Thickness(1, 0, 1, 1),
                //BorderBrush = new SolidColorBrush(Colors.White),
                Child = new TextBlock
                {
                    Text = " 活動",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White),
                }
            };
            Grid.SetColumn(border1, i);
            _headerGrid.Children.Add(border1);
            NotFound();
        }

        private void NotFound()
        {
            _bodyGrid.Children.Clear();
            _bodyGrid.ColumnDefinitions.Clear();
            _bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            TextBlock textBlock = new TextBlock
            {
                Text = "データ 無し",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 50,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red),
            };
            Grid.SetColumn(textBlock, 0);
            _bodyGrid.Children.Add(textBlock);
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            if (searchID.Text != "")
            {
                List<UrbanOsAttributesModel> allOsAttribute = _userAttributesDatabase.GetAll();
                allOsAttribute = allOsAttribute.Where(r => r.UrbanOsId == searchID.Text).ToList();
                currentOsId = searchID.Text;
                if (allOsAttribute.Count > 0)
                    RefreshColumns(allOsAttribute);
                else
                    NotFound();
            }
            else
                NotFound();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            AllClear();
        }

        private void RefreshColumns(List<UrbanOsAttributesModel> RegisterAttribute)
        {
            _bodyGrid.Children.Clear();
            _bodyGrid.RowDefinitions.Clear();
            _bodyGrid.ColumnDefinitions.Clear();
            int i = 0;
            _bodyGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            Grid grid = new Grid();
            foreach (var header in AttributeData)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                string attributeValue = String.Empty;
                attributeValue = RegisterAttribute.Where(r => r.AttributeId == header.AttributeId).Select(r => r.AttributeValue).FirstOrDefault();
                Border border = new Border
                {
                    BorderThickness = new Thickness(.5, 0, .5, .5),
                    BorderBrush = prmBrush,
                    Background = _colorBrush,
                    Child = new TextBlock
                    {
                        Text = " " + attributeValue,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.Black),
                    }
                };
                Grid.SetColumn(border, i);
                grid.Children.Add(border);
                i++;
                //MessageBox.Show(i.ToString());
                textBoxes[header.AttributeId].Text = attributeValue;
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Button deleteButton = new Button
            {
                Content = "\uE74D",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Width = 20,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Foreground = btnBrush,
            };
            deleteButton.Click += ConfirmDelete;
            if (Application.Current.Resources["CustomButtonTemplate"] is ControlTemplate buttonTemplate)
                deleteButton.Template = buttonTemplate;
            Border border1 = new Border
            {
                BorderThickness = new Thickness(.5, 0, .5, .5),
                Child = deleteButton,
                BorderBrush = prmBrush,
                Background = _colorBrush,
            };
            Grid.SetColumn(border1, i);
            grid.Children.Add(border1);
            Grid.SetRow(grid, 0);
            _bodyGrid.Children.Add(grid);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (searchID.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("You have to fill 都市OSID", "Fill In 都市OSID", MessageBoxButton.OK);
                searchID.Focus();
            }
            else
            {
                currentOsId = searchID.Text;
                List<UrbanOsAttributesModel> updateAttribute = _userAttributesDatabase.GetAll();
                int maxId = updateAttribute.Max(r => r.Id) + 1;
                foreach(var attribute in AttributeData)
                {
                    string attributeId = attribute.AttributeId;
                    string attributeValue = textBoxes[attributeId].Text;

                    UrbanOsAttributesModel currentUrban = updateAttribute.FirstOrDefault(r => r.AttributeId == attributeId && r.UrbanOsId == currentOsId);
                    if (currentUrban != null)
                        updateAttribute.Remove(currentUrban);

                    UrbanOsAttributesModel newUrban = new UrbanOsAttributesModel
                    {
                        Id = maxId,
                        AttributeId = attributeId,
                        AttributeValue = attributeValue,
                        UrbanOsId = currentOsId
                    };
                    updateAttribute.Add(newUrban);
                    maxId++;
                }
                updateAttribute = updateAttribute.OrderBy(r => r.Id).ToList();
                _userAttributesDatabase.SaveAll(updateAttribute);

                updateAttribute = updateAttribute.Where(r => r.UrbanOsId == currentOsId).ToList();
                RefreshColumns(updateAttribute);
            }
        }

        private void ConfirmDelete(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + currentOsId, "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                List<UrbanOsAttributesModel> updateAttribute = _userAttributesDatabase.GetAll();
                foreach (var attribute in AttributeData)
                {
                    string attributeId = attribute.AttributeId;

                    UrbanOsAttributesModel currentUrban = updateAttribute.FirstOrDefault(r => r.AttributeId == attributeId && r.UrbanOsId == currentOsId);
                    if (currentUrban != null)
                        updateAttribute.Remove(currentUrban);
                }
                updateAttribute = updateAttribute.OrderBy(r => r.Id).ToList();
                _userAttributesDatabase.SaveAll(updateAttribute);
                AllClear();
            }
        }

        private void AllClear()
        {
            searchID.Text = string.Empty;
            foreach (var attribute in AttributeData)
            {
                textBoxes[attribute.AttributeId].Text = string.Empty;
            }
            NotFound();
        }
    }
}
