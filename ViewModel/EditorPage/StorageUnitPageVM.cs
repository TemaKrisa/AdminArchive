using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdminArchive.ViewModel
{
    class StorageUnitPageVM : PageBaseVM 
    {
        public ObservableCollection<StorageUnit> StorageUnits { get; set; }

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
            StorageUnits = new ObservableCollection<StorageUnit>(dc.StorageUnits.Where(u => u.Inventory == curInv.Id).OrderBy(u=>u.Number).ThenBy(u=>u.Literal).Include(u=>u.CategoryNavigation));
        }

        protected override void EditItem()
        {
            int index = StorageUnits.IndexOf(SelectedItem);
            StorageUnitWindow Editor = new();
            StorageUnitWindowVM vm = new(SelectedItem, this, index, StorageUnits,curFond);
            Editor.DataContext = vm;
            Editor.ShowDialog();
        }
        protected override void AddItem()
        {
            StorageUnitWindowVM vm = new(this, StorageUnits, curFond);
            var newWindow = new StorageUnitWindow { DataContext = vm };
            newWindow.ShowDialog();
        }

        protected override void OpenItem()
        {
            if (SelectedItem != null)
            {
                DocumentPageVM vm = new(SelectedItem,curFond,curInv);
                DocumentPage v = new() { DataContext = vm };
                Setting.mainFrame?.Navigate(v);
            }
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
