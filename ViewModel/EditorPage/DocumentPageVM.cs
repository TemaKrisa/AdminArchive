using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
using System.Linq;

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
         Documents = new ObservableCollection<Document>(dc.Documents.Where(u => u.StorageUnit == curUnit.Id));

        protected override void GoBack()
        {
            StorageUnitPageVM vm = new(curInv,curFond);
            Setting.mainFrame?.Navigate(new StorageUnitPage{ DataContext = vm });
        }

        protected override void EditItem()
        {
            int index = Documents.IndexOf(SelectedItem);
            DocumentWindow newWindow = new();
            DocumentWindowVM? viewModel = new((SelectedItem as Document), this, index, Documents, curUnit);
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();
        }

        protected override void AddItem()
        {
            DocumentWindowVM vm = new(this,Documents,curUnit);
            var newWindow = new DocumentWindow { DataContext = vm };
            newWindow.ShowDialog();
        }

        protected override void OpenItem() {}

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

        public DocumentPageVM() { }
    }
}
