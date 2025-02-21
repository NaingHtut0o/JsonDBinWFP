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

        public CompanyLinkPage()
        {
            InitializeComponent();
            _urbanOsLinkDatabase = new JsonDatabase<UrbanOsLinkModel>("urban_os_links.json");
            _companyDatabase = new JsonDatabase<CompanyMasterModel>("company_master.json");
            LoadData();
            dgCompanyLink.ItemsSource = companyLinks;
            CheckIfNoData();
            comboBoxData();
        }

        private void LoadData()
        {
            companyLinks = new ObservableCollection<CompanyLink>();
            GetAllUrbanOsLinks();
            GetAllCompanies();
            List<UrbanOsLinkModel> osLinks = allUrbanOsLinks;
            FromOsLinkToCompanyLink(osLinks);
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
            CheckIfNoData(); 
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearControls();
            FromOsLinkToCompanyLink(allUrbanOsLinks);
            CheckIfNoData();
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {

        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if(searchID.Text != "" || newName.SelectedValue != null || linkID.Text != "")
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
                UrbanOsLinkModel editLink = allUrbanOsLinks.FirstOrDefault();
            }
        }

        private void dgCompanyLink_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void CheckIfNoData()
        {
            if (dgCompanyLink.Items.Count == 0)
            {
                dgCompanyLink.Visibility = Visibility.Collapsed;
                NoDataGrid.Visibility = Visibility.Visible; // Show "No Data Found" message
            }
            else
            {
                dgCompanyLink.Visibility = Visibility.Visible;
                NoDataGrid.Visibility = Visibility.Collapsed; // Hide message
            }
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
