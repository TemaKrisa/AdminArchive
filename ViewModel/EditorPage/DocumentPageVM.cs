using AdminArchive.Model;
using AdminArchive.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminArchive.ViewModel
{
    class DocumentPageVM : PageBaseVM
    {
        public ObservableCollection<Document>? Documents { get; set; }

        private ArchiveBdContext dc;

        private Document _selectedItem;
        public Document SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        private StorageUnit curUnit;
        public DocumentPageVM(StorageUnit unit)
        {
            curUnit = unit;
            dc = new ArchiveBdContext();
            UpdateData();
        }
        public DocumentPageVM() { }

        public void UpdateData()
        {
            Documents = new ObservableCollection<Document>(dc.Documents.Where(u => u.StorageUnit == curUnit.UnitId));
        }

        protected override void EditItem()
        {
            //DocumentWindow Editor = new();
            //DocumentWindowVM EditorVM = Editor.DataContext as DocumentWindowVM;
            //EditorVM.SelectedUnit = (SelectedItem as Document);
            //EditorVM.pageVM = this;
            //Editor.Show();
        }

        protected override void OpenItem()
        {

        }


        protected override void AddItem()
        {
            StorageUnitWindowVM vm = new();
            var newWindow = new StorageUnitWindow { DataContext = vm };
            newWindow.ShowDialog();
        }
    }
}
