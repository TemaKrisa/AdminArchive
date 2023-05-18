using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel для страницы документов
    /// </summary>
    class DocumentPageVM : PageBaseVM
    {
        public ObservableCollection<Document>? Documents { get; set; }
        private ArchiveBdContext dc;
        private Document _selectedItem;
        private StorageUnit curUnit;
        private Inventory curInv;
        private Fond curFond;
        public Document SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        public DocumentPageVM(StorageUnit unit,Fond fond, Inventory inventory)
        {
            dc = new ArchiveBdContext();
            curUnit = unit;
            curFond = fond;
            curInv = inventory;
            UpdateData();
        }

        public void UpdateData() =>
         Documents = new ObservableCollection<Document>(dc.Documents.Where(u => u.StorageUnit == curUnit.UnitId));

        protected override void GoBack()
        {
            StorageUnitPageVM vm = new(curInv,curFond);
            StorageUnitPage v = new() { DataContext = vm };
            FrameManager.mainFrame.Navigate(v);
        }

        protected override void EditItem()
        {
            DocumentWindow Editor = new();
            DocumentWindowVM EditorVM = Editor.DataContext as DocumentWindowVM;
            EditorVM.SelectedItem = (SelectedItem as Document);
            EditorVM.pageVM = this;
            Editor.Show();
        }

        protected override void AddItem()
        {
            DocumentWindowVM vm = new();
            var newWindow = new DocumentWindow { DataContext = vm };
            newWindow.ShowDialog();
        }

        protected override void OpenItem() {}
        public DocumentPageVM() { }
    }
}
