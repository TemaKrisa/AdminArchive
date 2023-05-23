using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdminArchive.ViewModel
{
    partial class InventoryPageVM : PageBaseVM
    {

        private ObservableCollection<Inventory> _inventories;
        public ObservableCollection<Inventory> Inventories
        {
            get => _inventories; 
            set
            { _inventories = value; OnPropertyChanged(); }
        } // Define an ObservableCollection of Fonds

        private ArchiveBdContext dc;

        private Inventory _selectedItem;
        public Inventory SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        private Fond curFond;
        public InventoryPageVM(Fond fond)
        {
            curFond = fond;
            dc = new ArchiveBdContext();
            UpdateData();
        }
        public InventoryPageVM() { }

        public void UpdateData()
        {
            dc = new ArchiveBdContext();
            Inventories = new ObservableCollection<Inventory>(dc.Inventories.Where(u => u.Fond == curFond.Id).OrderBy(u => u.Number).ThenBy(u => u.Literal));
        }

        protected override void GoBack() { Setting.mainFrame.Navigate(new FundPage()); }

        protected override void OpenItem()
        {
            if (SelectedItem != null)
            {
                StorageUnitPageVM vm = new(SelectedItem,curFond);
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

        protected override void SearchCommand()
        {
            throw new System.NotImplementedException();
        }

        protected override void ResetSearch()
        {
            throw new System.NotImplementedException();
        }

        protected override void CloseSearchCommand()
        {
            throw new System.NotImplementedException();
        }

        protected override void OpenSearchCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}
