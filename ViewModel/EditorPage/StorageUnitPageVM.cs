using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel
{
    class StorageUnitPageVM : PageBaseVM 
    {
        public ObservableCollection<StorageUnit>? StorageUnits { get; set; }

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
            FrameManager.mainFrame.Navigate(v);
        }

        public void UpdateData()
        {
            StorageUnits = new ObservableCollection<StorageUnit>(dc.StorageUnits.Where(u => u.Inventory == curInv.InventoryId));
        }

        protected override void EditItem()
        {
            StorageUnitWindow Editor = new();
            StorageUnitWindowVM? EditorVM = Editor.DataContext as StorageUnitWindowVM;
            EditorVM.SelectedUnit = (SelectedItem as StorageUnit);
            EditorVM.pageVM = this;
            Editor.Show();
        }

        protected override void OpenItem()
        {
            if (SelectedItem != null)
            {
                DocumentPageVM vm = new(SelectedItem,curFond,curInv);
                DocumentPage v = new() { DataContext = vm };
                FrameManager.mainFrame.Navigate(v);
            }
        }


        protected override void AddItem()
        {
            StorageUnitWindowVM vm = new();
            var newWindow = new StorageUnitWindow { DataContext = vm };
            newWindow.ShowDialog();
        }
    }
}
