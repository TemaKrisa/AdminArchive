using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
class DocumentWindowVM : EditBaseVM
{
    #region Переменные
    public dynamic pageVM;
    private Visibility _fileVisibility = Visibility.Collapsed;
    private ObservableCollection<Document> itemList = new();
    private int currentIndex;
    private ObservableCollection<DocumentFile> docFiles, docFilesDelete;
    private StorageUnit storageUnit { get; set; }
    private DocumentFile addedFile, _editFile, _selFile = new();
    private Document _selectedItem = new();
    private ObservableCollection<DocumentLog> _Log;
    public Visibility FileVisibility { get => _fileVisibility; set { _fileVisibility = value; OnPropertyChanged(); } }
    public ObservableCollection<SecretChar> SecretChar { get; set; }
    public ObservableCollection<DocType> DocType { get; set; }
    public ObservableCollection<Reproduction> Reproductions { get; set; }
    public ObservableCollection<DocumentLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
    public ObservableCollection<Document> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
    public ObservableCollection<Authenticity> Authenticities { get; set; }
    public Document SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
    public DocumentFile EditFile { get => _editFile; set { _editFile = value; OnPropertyChanged(); } }
    public ObservableCollection<DocumentFile> DocFilesDelete { get => docFilesDelete; set { docFilesDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<DocumentFile> DocFiles { get => docFiles; set { docFiles = value; OnPropertyChanged(); } }
    public DocumentFile SelFile { get => _selFile; set { _selFile = value; OnPropertyChanged(); } }
    #endregion
    #region Навигация
    private void CheckNav(int index) { IsFirst = index != 0; IsLast = index != ItemList.Count - 1; }
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
        SelectedItem = selDoc; pageVM = vm; currentIndex = selIndex; ItemList = items; storageUnit = unit;
        FillCollections();
    }
    public DocumentWindowVM(DocumentPageVM vm, ObservableCollection<Document> items, StorageUnit unit)
    {
        ItemList = items; pageVM = vm; storageUnit = unit;
        FillCollections();
    }
    #endregion
    private void FillTables()
    {
        using ArchiveBdContext dc = new();
        DocFiles = new ObservableCollection<DocumentFile>(dc.DocumentFiles.Where(u => u.Document == SelectedItem.Id));
        DocFilesDelete = new ObservableCollection<DocumentFile>();
    }
    protected override void FillCollections()
    {
        try
        {
            using ArchiveBdContext dc = new();
            DocType = new ObservableCollection<DocType>(dc.DocTypes);
            SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
            DocType = new ObservableCollection<DocType>(dc.DocTypes);
            Authenticities = new ObservableCollection<Authenticity>(dc.Authenticities);
            Reproductions = new ObservableCollection<Reproduction>(dc.Reproductions);
            if (SelectedItem != null) CheckNav(currentIndex);
            else AddItem();
        }
        catch (Exception e)
        {
            ShowMessage(e.Message);
        }
    }
    protected override void AddItem()
    {
        SelectedItem = new Document()
        {
            SecretChar = storageUnit.SecretChar.HasValue ? storageUnit.SecretChar.Value : default(int),
            StorageUnit = storageUnit.Id,
            DocType = (int)storageUnit.DocType
        };
        FillTables();
    }
    protected override void SaveItem()
    {
        using ArchiveBdContext dc = new();
        DocumentLog Log;
        try
        {
            using var transaction = dc.Database.BeginTransaction();

            if (!dc.Documents.Contains(SelectedItem))
            {
                if (dc.Fonds.Any(u => u.Number == SelectedItem.Number))
                { ShowMessage("Документ с таким номером уже существует"); return; }
                dc.Documents.Add(SelectedItem);
                Log = new() { Activity = 1, Date = DateTime.Now, Document = SelectedItem.Id, User = 1 };
            }
            else
            {
                dc.Update(SelectedItem);
                Log = new() { Activity = 2, Date = DateTime.Now, Document = SelectedItem.Id, User = 1 };
            }
            dc.SaveChanges();
            dc.DocumentFiles.UpdateRange(DocFiles.Where(up => dc.DocumentFiles.Any(u => u.Id == up.Id)));
            dc.DocumentFiles.AddRange(DocFiles.Where(up => !dc.DocumentFiles.Any(u => u.Id == up.Id)));
            dc.RemoveRange(DocFilesDelete.Where(u => dc.DocumentFiles.Any(v => v.Id == u.Id)));
            dc.SaveChanges();
            transaction.Commit();
            pageVM.UpdateData();
        }
        catch (Exception ex)
        { ShowMessage(ex.ToString()); }
    }
    protected override void OpenLog() //Открытие протокола
    {
        using ArchiveBdContext dc = new();
        UCVisibility = Visibility.Visible;
        Log = new ObservableCollection<DocumentLog>(dc.DocumentLogs.Where(u => u.Document == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
    }
    public ICommand ChooseFile => new RelayCommand(ChooseFiles);
    public ICommand SaveEditFile => new RelayCommand(SaveEditFileCommand);
    public ICommand SaveFile => new RelayCommand(SaveFiles);
    public ICommand OpenEditFile => new RelayCommand(OpenEditFileCommand);
    public ICommand AddEditFile => new RelayCommand(AddEditFileCommand);
    public ICommand DeleteFile => new RelayCommand(DeleteFileCommand);
    protected override void CloseLog() { UCVisibility = Visibility.Collapsed; FileVisibility = Visibility.Collapsed; }

    private void DeleteFileCommand()
    {
        if (SelFile != null)
        {
            DocFilesDelete.Add(SelFile);
            DocFiles.Remove(SelFile);
        }
    }
    private void OpenEditFileCommand()
    {
        EditFile = new DocumentFile()
        {
            Id = SelFile.Id,
            Description = SelFile.Description,
            Document = SelFile.Document,
            File = SelFile.File,
            FileName = SelFile.FileName,
            Extension = SelFile.Extension
        };
        FileVisibility = Visibility.Visible;
    }
    private void AddEditFileCommand()
    {
        EditFile = new DocumentFile() { Document = SelectedItem.Id };
        FileVisibility = Visibility.Visible;
    }
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
                if (EditFile.FileName != null) EditFile.FileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                EditFile.File = File.ReadAllBytes(openFileDialog.FileName);
                EditFile.Extension = Path.GetExtension(openFileDialog.FileName);
            }
        }
    }
    private void SaveEditFileCommand()
    {
        if (EditFile != null)
        {
            if (EditFile.File == null) ShowMessage("Выберите файл!");
            else
            {
                var index = DocFiles.IndexOf(DocFiles.FirstOrDefault(u => u.Id == EditFile.Id));
                if (index == -1) DocFiles.Add(EditFile);
                else DocFiles[index] = EditFile;
                CloseLog();
            }
        }
    }
    private void SaveFiles()
    {
        if (SelFile.File != null)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Title = SelFile.FileName,
                FileName = SelFile.FileName + "" + SelFile.Extension,
                Filter = "All files (*.*)|*.*|" + SelFile.Extension.ToUpper() + " files (*" + SelFile.Extension + ")|*" +
                SelFile.Extension
            };
            if (saveFileDialog.ShowDialog() == true) System.IO.File.WriteAllBytes(saveFileDialog.FileName, SelFile.File);
        }
    }
}
