using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AdminArchive.ViewModel
{
    partial class InventoryPageVM : PageBaseVM
    {
        public ObservableCollection<Inventory>? Inventories { get; set; }
        
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
            Inventories = new ObservableCollection<Inventory>(dc.Inventories.Where(u=>u.Fond == curFond.FondId));
        }

        protected override void EditItem()
        {
            InventoryWindow Editor = new();
            InventoryWindowVM EditorVM = Editor.DataContext as InventoryWindowVM;
            EditorVM.SelectedInventory = (SelectedItem as Inventory);
            EditorVM.pageVM = this;
            Editor.Show();
        }

        protected override void OpenItem()
        {
            if (SelectedItem != null)
            {
                StorageUnitPageVM vm = new(SelectedItem);
                StorageUnitPage v = new() { DataContext = vm };
                FrameManager.mainFrame.Navigate(v);
            }
        }


        protected override void AddItem()
        {
            InventoryWindowVM vm = new();
            var newWindow = new InventoryWindow { DataContext = vm };
            newWindow.ShowDialog();
        }
    }
}
