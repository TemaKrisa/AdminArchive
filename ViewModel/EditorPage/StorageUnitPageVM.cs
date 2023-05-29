using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel
{
    class StorageUnitPageVM : PageBaseVM
    {
        private ObservableCollection<StorageUnit> _storageUnits;
        public ObservableCollection<StorageUnit> StorageUnits
        {
            get => _storageUnits;
            set { _storageUnits = value; OnPropertyChanged(); }
        }

        private ArchiveBdContext dc;

        private StorageUnit _selectedItem;
        public StorageUnit SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        private Inventory curInv;
        private Fond curFond;
        public StorageUnitPageVM(Inventory inv, Fond fond)
        {
            curFond = fond;
            curInv = inv;
            dc = new ArchiveBdContext();
            UpdateData();
        }
        public StorageUnitPageVM() { }


        protected override void GoBack()
        {
            InventoryPageVM vm = new(curFond);
            InventoryPage v = new() { DataContext = vm };
            Setting.mainFrame?.Navigate(v);
        }

        public void UpdateData()
        {
            StorageUnits = new ObservableCollection<StorageUnit>(dc.StorageUnits.Where(u => u.Inventory == curInv.Id).OrderBy(u => u.Number).ThenBy(u => u.Literal).Include(u => u.CategoryNavigation));
        }

        protected override void EditItem()
        {
            int index = StorageUnits.IndexOf(SelectedItem);
            StorageUnitWindow Editor = new();
            StorageUnitWindowVM vm = new(SelectedItem, this, index, StorageUnits, curInv);
            Editor.DataContext = vm;
            Editor.ShowDialog();
        }
        protected override void AddItem()
        {
            StorageUnitWindowVM vm = new(this, StorageUnits, curInv);
            var newWindow = new StorageUnitWindow { DataContext = vm };
            newWindow.ShowDialog();
        }

        protected override void OpenItem()
        {
            if (SelectedItem != null)
            {
                DocumentPageVM vm = new(SelectedItem, curFond, curInv);
                DocumentPage v = new() { DataContext = vm };
                Setting.mainFrame?.Navigate(v);
            }
        }

        protected override void SearchCommand()
        {

        }

        protected override void ResetSearch() { UpdateData(); }
    }
}
