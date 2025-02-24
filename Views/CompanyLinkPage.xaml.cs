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
    public partial class CompanyLinkPage : Page
    {
        private JsonDatabase<UrbanOsLinkModel> _urbanOsLinkDatabase;
        private JsonDatabase<CompanyMasterModel> _companyDatabase;
        private List<UrbanOsLinkModel> allUrbanOsLinks;
        private List<CompanyMasterModel> allCompanies;
        public ObservableCollection<CompanyLink> companyLinks;
        private SolidColorBrush _colorBrush = new SolidColorBrush(Colors.LightYellow);
        private SolidColorBrush _altBrush = new SolidColorBrush(Colors.MintCream);
        private SolidColorBrush _prmBrush = new SolidColorBrush(Colors.DarkOrange);
        private SolidColorBrush _btnBrush = new SolidColorBrush(Colors.DarkOrange);

        public CompanyLinkPage()
        {
            InitializeComponent();
            _urbanOsLinkDatabase = new JsonDatabase<UrbanOsLinkModel>("urban_os_links.json");
            _companyDatabase = new JsonDatabase<CompanyMasterModel>("company_master.json");
            LoadData();
            //dgCompanyLink.ItemsSource = companyLinks;
            CheckIfNoData(allUrbanOsLinks);
            comboBoxData();
        }

        private void LoadData()
        {
            companyLinks = new ObservableCollection<CompanyLink>();
            GetAllUrbanOsLinks();
            GetAllCompanies();
            List<UrbanOsLinkModel> osLinks = allUrbanOsLinks;
            FromOsLinkToCompanyLink(osLinks);
            if (Application.Current.Resources["WindowBackground"] is SolidColorBrush brush)
                _colorBrush = brush;
            if (Application.Current.Resources["AlternateBrush"] is SolidColorBrush abrush)
                _altBrush = abrush;
            if (Application.Current.Resources["PrimaryBrush"] is SolidColorBrush pbrush)
                _prmBrush = pbrush;
            if (Application.Current.Resources["ButtonBrush"] is SolidColorBrush bbrush)
                _btnBrush = bbrush;
        }

        #region page_functions
        private void Search(object sender, RoutedEventArgs e)
        {
            List<UrbanOsLinkModel> searchCompanyLinks = new List<UrbanOsLinkModel>();
            if(searchID.Text != "" || newName.SelectedValue != null || linkID.Text != "")
            {
                searchCompanyLinks = allUrbanOsLinks;
                if(searchID.Text != "")
                    searchCompanyLinks = searchCompanyLinks.Where(r => r.UrbanOsId.Contains(searchID.Text)).ToList();
                if(newName.SelectedValue != null)
                    searchCompanyLinks = searchCompanyLinks.Where(r => r.CompanyId == (int)newName.SelectedValue).ToList();
                if(linkID.Text != "")
                    searchCompanyLinks = searchCompanyLinks.Where(r => r.ExternalId.Contains(linkID.Text)).ToList();
            }
            FromOsLinkToCompanyLink(searchCompanyLinks);
            CheckIfNoData(searchCompanyLinks); 
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearControls();
            FromOsLinkToCompanyLink(allUrbanOsLinks);
            CheckIfNoData(allUrbanOsLinks);
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var id = button?.Tag;
            UrbanOsLinkModel delLinkModel = allUrbanOsLinks.Where(r => r.Id == (int)id).FirstOrDefault();


            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + delLinkModel.ExternalId, "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (delLinkModel != null)
                    allUrbanOsLinks.Remove(delLinkModel);
                _urbanOsLinkDatabase.SaveAll(allUrbanOsLinks);
            }
            FromOsLinkToCompanyLink(allUrbanOsLinks);
            CheckIfNoData(allUrbanOsLinks);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if(searchID.Text == "" || newName.SelectedValue == null || linkID.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("You have to fill all data", "Fill All Data", MessageBoxButton.OK);
                if (searchID.Text == "")
                    searchID.Focus();
                else if(newName.SelectedValue == null)
                    newName.Focus();
                else
                    linkID.Focus();
            }
            else
            {
                int maxId = allUrbanOsLinks.OrderByDescending(r => r.Id).First().Id + 1;
                UrbanOsLinkModel editLink = allUrbanOsLinks.Where(r => r.UrbanOsId == searchID.Text && r.CompanyId == (int)newName.SelectedValue).FirstOrDefault();
                if (editLink != null)
                {
                    allUrbanOsLinks.Remove(editLink);
                    maxId = editLink.Id;
                }
                UrbanOsLinkModel newLinkModel = new UrbanOsLinkModel
                {
                    Id = maxId,
                    UrbanOsId = searchID.Text,
                    CompanyId = (int)newName.SelectedValue,
                    ExternalId = linkID.Text,
                };
                allUrbanOsLinks.Add(newLinkModel);
                allUrbanOsLinks = allUrbanOsLinks.OrderBy(r => r.Id).ToList();
                _urbanOsLinkDatabase.SaveAll(allUrbanOsLinks);
                FromOsLinkToCompanyLink(allUrbanOsLinks);
                CheckIfNoData(allUrbanOsLinks);
            }
        }

        private void dgCompanyLink_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() % 2 == 0)
            {
                e.Row.Background = _colorBrush;
            }
            else
            {
                e.Row.Background = Brushes.MintCream;
            }
        }

        #endregion page_functions
        #region private_functions

        private void GetAllUrbanOsLinks()
        {
            allUrbanOsLinks = _urbanOsLinkDatabase.GetAll();
        }

        private void GetAllCompanies()
        {
            allCompanies = _companyDatabase.GetAll();
        }

        //private void RefreshList(List<CompanyLink> list)
        //{
        //    companyLinks.Clear();
        //    foreach (CompanyLink link in list)
        //        companyLinks.Add(link);
        //    dgCompanyLink.Items.Refresh();
        //    CheckIfNoData();
        //}

        private void CheckIfNoData(List<UrbanOsLinkModel> OsLink)
        {
            if (OsLink.Count == 0)
                NotFound();
            else
                Found();
        }

        private void NotFound()
        {
            dgCompanyLink.Children.Clear();
            dgCompanyLink.RowDefinitions.Clear();
            dgCompanyLink.ColumnDefinitions.Clear();
            dgCompanyLink.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            TextBlock textBlock = new TextBlock
            {
                Text = "データ 無し",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 50,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red),
            };
            Grid.SetColumn(textBlock, 0);
            dgCompanyLink.Children.Add(textBlock);
        }

        private void Found()
        {
            dgCompanyLink.Children.Clear();
            dgCompanyLink.ColumnDefinitions.Clear();
            dgCompanyLink.RowDefinitions.Clear();
            int i = 0;
            foreach(CompanyLink companyLink in companyLinks)
            {
                dgCompanyLink.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Star) });
                TextBlock idBlock = new TextBlock
                {
                    Text = companyLink.Id.ToString(),
                };
                Border border = attachBorder(idBlock);
                Grid.SetColumn(border, 0);
                grid.Children.Add(border);

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
                TextBlock osidBlock = createTextBlock(" " + companyLink.UrbanOsId);
                Border border1 = attachBorder(osidBlock);
                Grid.SetColumn(border1, 1);
                grid.Children.Add(border1);

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                TextBlock comBlock = createTextBlock(" " + companyLink.CompanyName);
                Border border2 = attachBorder(comBlock);
                Grid.SetColumn(border2, 2);
                grid.Children.Add(border2);

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
                TextBlock extBlock = createTextBlock(" " + companyLink.ExternalId);
                Border border3 = attachBorder(extBlock);
                Grid.SetColumn(border3, 3);
                grid.Children.Add(border3);

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                Button btnDel = new Button
                {
                    Content = "\uE74D",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Width = 20,
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderThickness = new Thickness(0),
                    Foreground = _btnBrush,
                    Tag = companyLink.Id,
                };
                btnDel.Click += ConfirmDelete;
                Border border4 = attachBorder(btnDel);
                Grid.SetColumn(border4, 4);
                grid.Children.Add(border4);
                
                if(i%2 ==0)
                    grid.Background = _colorBrush;
                else
                    grid.Background = _altBrush;
                Grid.SetRow(grid, i);
                dgCompanyLink.Children.Add(grid);
                i++;
            }
        }

        private Border attachBorder(dynamic child)
        {
            Border border = new Border
            {
                BorderThickness = new Thickness(.5, 0, .5, .5),
                BorderBrush = _prmBrush,
                Child = child,
            };
            return border;
        }

        private TextBlock createTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
            };
            return textBlock;
        }

        private void FromOsLinkToCompanyLink(List<UrbanOsLinkModel> osLink)
        {
            companyLinks.Clear();
            foreach (var urbanLink in osLink)
            {
                int companyId = urbanLink.CompanyId;
                string companyName = allCompanies.Where(r => r.CompanyId == companyId).First().CompanyName;
                CompanyLink companyLink = new CompanyLink(urbanLink.Id, urbanLink.UrbanOsId, companyName, urbanLink.ExternalId);
                companyLinks.Add(companyLink);
            }
        }

        private void comboBoxData()
        {
            newName.ItemsSource = allCompanies;
            newName.Background = new SolidColorBrush(Colors.DarkOrange);
            newName.DisplayMemberPath = "DisplayName";
            newName.SelectedValuePath = "CompanyId";
        }

        private void ClearControls()
        {
            searchID.Text = string.Empty;
            newName.SelectedValue = null;
            newName.Text = String.Empty;
            linkID.Text = string.Empty;
        }
        #endregion private_functions
    }

    public class CompanyLink
    {
        public int Id { get; set; }
        public string UrbanOsId { get; set; }
        public string CompanyName { get; set; }
        public string ExternalId { get; set; }

        public CompanyLink(int id,string urbanOsId, string companyName, string externalId)
        {
            Id = id;
            UrbanOsId = urbanOsId;
            CompanyName = companyName;
            ExternalId = externalId;
        }
    }
}
