using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SmartHealthTest.Database;
using SmartHealthTest.Models;
using System.Windows.Media;
using SmartHealthTest.Utilities;

namespace SmartHealthTest.Views
{
    public partial class CompanyPage : Page
    {
        private JsonDatabase<CompanyMasterModel> _companyDatabase;
        public ObservableCollection<CompanyMasterModel> CompanyData;
        private SolidColorBrush _colorBrush = new SolidColorBrush(Colors.LightYellow);
        private SolidColorBrush _altBrush;
        public CompanyPage()
        {
            InitializeComponent();
            _companyDatabase = new JsonDatabase<CompanyMasterModel>("company_master.json");
            LoadData();
            dgCompany.ItemsSource = CompanyData;
            CheckIfNoData();

            this.SizeChanged += (s, e) =>
            {
                dgCompany.MaxHeight = this.ActualHeight - 230; // 50% of window height
                dgCompany.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                dgCompany.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            };
        }

        private void CheckIfNoData()
        {
            if (dgCompany.Items.Count == 0)
            {
                dgCompany.Visibility = Visibility.Collapsed;
                NoDataGrid.Visibility = Visibility.Visible; // Show "No Data Found" message
            }
            else
            {
                dgCompany.Visibility = Visibility.Visible;
                NoDataGrid.Visibility = Visibility.Collapsed; // Hide message
            }
        }

        private void LoadData()
        {
            CompanyData = new ObservableCollection<CompanyMasterModel>(_companyDatabase.GetAll());
            if (Application.Current.Resources["WindowBackground"] is SolidColorBrush brush)
                _colorBrush = brush;
            _altBrush = Application.Current.Resources["AlternateBrush"] as SolidColorBrush ?? new SolidColorBrush(Colors.MintCream);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (searchID.Text == "" || newName.Text == "")
            {
                MessageBoxResult result = MessageBox.Show(MessageClass.UnfillDataMsg, MessageClass.UnfillDataTitle, MessageBoxButton.OK);
                if (searchID.Text == "")
                    searchID.Focus();
                else
                    newName.Focus();
            }
            else
            {
                List<CompanyMasterModel> CompanyMasterModels = _companyDatabase.GetAll();
                CompanyMasterModel attModel = CompanyMasterModels.FirstOrDefault(r => r.CompanyId == int.Parse(searchID.Text));
                CompanyMasterModel newAttributeModel = new CompanyMasterModel
                {
                    CompanyId = int.Parse(searchID.Text),
                    CompanyName = newName.Text
                };
                if (attModel != null)
                    CompanyMasterModels.Remove(attModel);
                CompanyMasterModels.Add(newAttributeModel);
                CompanyMasterModels = CompanyMasterModels.OrderBy(r => r.CompanyId).ToList();
                _companyDatabase.SaveAll(CompanyMasterModels);
                RefreshList();
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            List<CompanyMasterModel> CompanyMasterModels = new List<CompanyMasterModel>();
            if (searchID.Text != "" || newName.Text != "")
            {
                CompanyMasterModels = _companyDatabase.GetAll();
                if (searchID.Text != "")
                    CompanyMasterModels = CompanyMasterModels.Where(r => r.CompanyId == int.Parse(searchID.Text)).ToList();
                if (newName.Text != "")
                    CompanyMasterModels = CompanyMasterModels.Where(r => r.CompanyName.Contains(newName.Text)).ToList();
            }
            CompanyData.Clear();
            foreach (var attModel in CompanyMasterModels)
                CompanyData.Add(attModel);
            dgCompany.Items.Refresh();
            CheckIfNoData();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            searchID.Text = string.Empty;
            newName.Text = string.Empty;
            List<CompanyMasterModel> CompanyMasterModels = _companyDatabase.GetAll();
            CompanyData.Clear();
            foreach (var item in CompanyMasterModels)
            {
                CompanyData.Add(item);
            }
            dgCompany.Items.Refresh();
            CheckIfNoData();
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.Tag;
            CompanyMasterModel deleteAttributeModel = (CompanyMasterModel)item;

            MessageBoxResult result = MessageBox.Show(MessageClass.ConfirmDeleteMsg + deleteAttributeModel.CompanyName, MessageClass.ConfirmDeleteTitle, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                List<CompanyMasterModel> CompanyMasterModels = _companyDatabase.GetAll();
                CompanyMasterModel attModel = CompanyMasterModels.FirstOrDefault(r => r.CompanyId == deleteAttributeModel.CompanyId);
                if (attModel != null)
                    CompanyMasterModels.Remove(attModel);
                _companyDatabase.SaveAll(CompanyMasterModels);
                CompanyData.Remove(deleteAttributeModel);
            }
            dgCompany.Items.Refresh();
            CheckIfNoData();
        }

        private void dgCompany_LoadingRow(object sender, DataGridRowEventArgs e)
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

        public void RefreshList()
        {
            CompanyData.Clear();
            List<CompanyMasterModel> newCompanyMasterModels = _companyDatabase.GetAll();
            foreach (var item in newCompanyMasterModels)
            {
                CompanyData.Add(item);
            }
            dgCompany.Items.Refresh();
        }
    }
}
