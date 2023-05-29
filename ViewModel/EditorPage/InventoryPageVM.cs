using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel
{
    partial class InventoryPageVM : PageBaseVM
    {

        private ObservableCollection<Inventory> _inventories;
        private Fond curFond;
        public ObservableCollection<Inventory> Inventories
        { get => _inventories; set { _inventories = value; OnPropertyChanged(); } }
        public Inventory SelectedItem { get; set; }

        public InventoryPageVM(Fond fond)
        {
            curFond = fond;
            UpdateData();
        }
        public InventoryPageVM() { }
        public void UpdateData()
        {
            using ArchiveBdContext dc = new();
            Inventories = new ObservableCollection<Inventory>(dc.Inventories.Include(u => u.TypeNavigation).Where(u => u.Fond == curFond.Id).OrderBy(u => u.Number).ThenBy(u => u.Literal));
        }
        protected override void GoBack() { Setting.mainFrame.Navigate(new FundPage()); }
        protected override void OpenItem()
        {
            if (SelectedItem != null)
            {
                StorageUnitPageVM vm = new(SelectedItem, curFond);
                StorageUnitPage v = new() { DataContext = vm };
                Setting.mainFrame.Navigate(v);
            }
        }
        protected override void AddItem()// Define function that is called when a user clicks on the "Add" button
        {
            InventoryWindowVM viewModel = new(this, Inventories, curFond);
            InventoryWindow newWindow = new() { DataContext = viewModel };
            newWindow.ShowDialog();
        }
        protected override void EditItem() // Define function that is called when a user clicks on the "Edit" button
        {
            int index = Inventories.IndexOf(SelectedItem);
            InventoryWindow newWindow = new();
            InventoryWindowVM viewModel = new((SelectedItem as Inventory), this, index, Inventories, curFond);
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();
        }
        protected override void ResetSearch() { UpdateData(); }

        protected override void SearchCommand()
        {

        }
    }
}
