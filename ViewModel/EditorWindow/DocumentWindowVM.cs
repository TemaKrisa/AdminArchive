using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    class DocumentWindowVM : EditBaseVM
    {

        #region Переменные
        public DocumentPageVM pageVM = new();
        
        private int currentIndex;

        private ArchiveBdContext dc;

        public ObservableCollection<SecretChar> SecretChar { get; set; }
        public ObservableCollection<DocType> DocType { get; set; }
        public ObservableCollection<Reproduction> Reproductions { get; set; }
        
        private ObservableCollection<DocumentFile> docFiles;


        private ObservableCollection<Document> itemList = new();
        public ObservableCollection<Document> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }

        private StorageUnit storageUnit { get; set; }
        public ObservableCollection<DocumentFile> DocFiles { get => docFiles;  set { docFiles = value; OnPropertyChanged(); } }

        public Document _selectedItem = new();
        public Document SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }

        public DocumentFile _selFile = new();

        public DocumentFile SelFile { get => _selFile; set { _selFile = value; OnPropertyChanged(); } }

        #endregion

        #region Навигация
        private void CheckNav(int index)
        {
            IsFirst = index != 0;
            IsLast = index != ItemList.Count - 1;
        }

        private void CheckNav() { IsFirst = false; IsLast = false; }

        protected override void GoNext()
        {
            currentIndex++;
            SelectedItem = (currentIndex < ItemList.Count) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }

        protected override void GoPrev()
        {
            currentIndex--;
            SelectedItem = (currentIndex >= 0) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }

        protected override void GoLast()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[^1] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }

        protected override void GoFirst()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[0] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }
        #endregion

        #region Инициализация
        public DocumentWindowVM() { }

        public DocumentWindowVM(Document selDoc, DocumentPageVM vm, int selIndex, ObservableCollection<Document> items, StorageUnit unit)
        {
            SelectedItem = selDoc;
            pageVM = vm;
            currentIndex = selIndex;
            ItemList = items;
            FillCollections();
        }

        public DocumentWindowVM(DocumentPageVM vm, ObservableCollection<Document> items, StorageUnit unit)
        {
            ItemList = items;
            pageVM = vm;
            storageUnit = unit;
            FillCollections();
        }
        #endregion

        private void FillTables()
        {
            AddedFl.File = dc.DocumentFiles.First().File;
        }



        protected override void FillCollections()
        {
            try
            {
                DocType = new ObservableCollection<DocType>(dc.DocTypes);
                SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
                DocFiles = new ObservableCollection<DocumentFile>(dc.DocumentFiles);
                DocType = new ObservableCollection<DocType>(dc.DocTypes);
                FillTables();
            }
            catch (Exception e)
            {
                ShowMessage(e.Message);
            }
        }
        protected override void AddItem()
        {
            SelectedItem = new Document();
        }



        protected override void SaveItem()
        {
            try
            {
                if (!dc.Documents.Any(u => u.DocumentId == SelectedItem.DocumentId))
                    dc.Documents.Add(SelectedItem);
                dc.SaveChanges();
                pageVM.UpdateData();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString());
            }
        }

        protected override void OpenLog() 
        {

        }

        protected override void CloseLog()
        {

        }

        public DocumentFile addedFile = new();

        public DocumentFile AddedFl
        {
            get => addedFile;
            set
            {
                addedFile = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChooseFile => new RelayCommand(ChooseFiles);

        private void ChooseFiles()
        {
            ArchiveBdContext dc = new();
            if (SelFile != null)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Multiselect = false
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    SelFile.File = File.ReadAllBytes(openFileDialog.FileName);
                    //SelFile.Document = SelectedItem.DocumentId;
                    SelFile.FileName = openFileDialog.FileName;
                    dc.Add(SelFile);
                    dc.SaveChanges();
                    DocFiles = new ObservableCollection<DocumentFile>(dc.DocumentFiles);
                }
            }
        }
    }
}
