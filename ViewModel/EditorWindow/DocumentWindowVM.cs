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
    private Visibility _fileVisibility = Visibility.Collapsed, _featureVisibility = Visibility.Collapsed;
    private ObservableCollection<Document> itemList = new();
    private int currentIndex;
    private ObservableCollection<Feature> _docFeatures, _features;
    public Visibility FeatureVisibility { get => _featureVisibility; set { _featureVisibility = value; OnPropertyChanged(); } }
    public ObservableCollection<Feature> DocFeatures { get => _docFeatures; set { _docFeatures = value; OnPropertyChanged(); } }
    public ObservableCollection<Feature> Features { get => _features; set { _features = value; OnPropertyChanged(); } }
    private Feature _editedFeature, _selectedFeature;
    public Feature SelectedFeature { get => _selectedFeature; set { _selectedFeature = value; OnPropertyChanged(); } }
    public Feature EditedFeature { get => _editedFeature; set { _editedFeature = value; OnPropertyChanged(); } }
    private ObservableCollection<DocumentFile> docFiles, docFilesDelete;
    private StorageUnit storageUnit { get; set; }
    private DocumentFile addedFile, _editFile, _selFile = new();
    private Document? _selectedItem = new();
    private ObservableCollection<DocumentLog> _Log;
    public Visibility FileVisibility { get => _fileVisibility; set { _fileVisibility = value; OnPropertyChanged(); } }
    public ObservableCollection<SecretChar> SecretChar { get; set; }
    public ObservableCollection<DocType> DocType { get; set; }
    public ObservableCollection<Reproduction> Reproductions { get; set; }
    public ObservableCollection<DocumentLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
    public ObservableCollection<Document> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
    public ObservableCollection<Authenticity> Authenticities { get; set; }
    public Document? SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
    public DocumentFile EditFile { get => _editFile; set { _editFile = value; OnPropertyChanged(); } }
    public ObservableCollection<DocumentFile> DocFilesDelete { get => docFilesDelete; set { docFilesDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<DocumentFile> DocFiles { get => docFiles; set { docFiles = value; OnPropertyChanged(); } }
    public DocumentFile SelFile { get => _selFile; set { _selFile = value; OnPropertyChanged(); } }
    #endregion
    #region Навигация
    private void CheckNav(int index) //Определение доступности кнопок навигации
    {
        if (ItemList.Count == 0) { IsFirst = false; IsLast = false; }
        else { IsFirst = index != 0; IsLast = index != ItemList.Count - 1; }
    }
    protected override void GoNext()
    {
        try
        {
            currentIndex++;
            SelectedItem = (currentIndex < ItemList.Count) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0; IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }
        catch { IsLast = false; }
    }
    protected override void GoPrev()
    {
        currentIndex--;
        SelectedItem = (currentIndex >= 0) ? ItemList[currentIndex] : SelectedItem;
        IsFirst = currentIndex != 0;
        IsLast = currentIndex != ItemList.Count - 1;
        FillTables();
    }
    protected override void GoFirst()
    {
        SelectedItem = (ItemList.Count > 0) ? ItemList[0] : null;
        currentIndex = ItemList.IndexOf(SelectedItem);
        IsFirst = currentIndex != 0; IsLast = currentIndex != ItemList.Count - 1;
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
    #endregion
    #region Инициализация
    public DocumentWindowVM() { }
    public DocumentWindowVM(Document selDoc, dynamic vm, int selIndex, ObservableCollection<Document> items, StorageUnit unit)
    {
        SelectedItem = selDoc;
        pageVM = vm;
        currentIndex = selIndex;
        ItemList = items;
        storageUnit = unit;
        FillCollections();
    }
    public DocumentWindowVM(dynamic vm, ObservableCollection<Document> items, StorageUnit unit)
    {
        ItemList = items;
        pageVM = vm;
        storageUnit = unit;
        SelectedItem = null;
        FillCollections();
    }
    #endregion
    private void FillTables()
    {
        using ArchiveBdContext dc = new();
        DocFiles = new ObservableCollection<DocumentFile>(dc.DocumentFiles.Where(u => u.Document == SelectedItem.Id));
        DocFeatures = new ObservableCollection<Feature>(dc.Features.Include(u => u.Documents).Where(u => u.Documents.Any(u => u.Id == SelectedItem.Id)));
    }
    protected override void FillCollections() //Заполнение списковв
    {
        try
        {
            using ArchiveBdContext dc = new();
            DocType = new ObservableCollection<DocType>(dc.DocTypes);
            SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
            DocType = new ObservableCollection<DocType>(dc.DocTypes);
            Authenticities = new ObservableCollection<Authenticity>(dc.Authenticities);
            Reproductions = new ObservableCollection<Reproduction>(dc.Reproductions);
            DocFilesDelete = new ObservableCollection<DocumentFile>();
            if (SelectedItem != null)
            {
                FillTables();
                CheckNav(currentIndex);
            }
            else
            {
                DocFiles = new ObservableCollection<DocumentFile>();
                AddItem();
            }
        }
        catch (Exception e) { ShowMessage(e.Message); }
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
    protected bool ValidateInput(ArchiveBdContext dc)
    {
        if (String.IsNullOrWhiteSpace(SelectedItem.Name)) { ShowMessage("Введите наименование!"); return false; }
        else if (SelectedItem.DocType == 0) { ShowMessage("Введите вид документов!"); return false; }
        else if (SelectedItem.Reproduction == 0) { ShowMessage("Выберите способ воспроизведения"); return false; }
        else if (SelectedItem.Vol == 0) { ShowMessage("Введите обьем!"); return false; }
        else if (SelectedItem.VolStart == 0) { ShowMessage("Введите начальный номер страниц"); return false; }
        else if (SelectedItem.Number == 0) { ShowMessage("Введите номер!"); return false; }
        else if (SelectedItem.VolEnd == 0) { ShowMessage("Введите конечный номер страниц"); return false; }
        else if (dc.Documents.Any(u => u.Number == SelectedItem.Number && u.StorageUnit == SelectedItem.StorageUnit && u.Id != SelectedItem.Id)) { ShowMessage("Документ с таким номером уже существует"); return false; }
        return true;
    }
    protected override void SaveItem()
    {
        using ArchiveBdContext dc = new();
        DocumentLog Log;
        if (!ValidateInput(dc)) return;
        var q = string.Empty;
        using var transaction = dc.Database.BeginTransaction();
        try
        {
            if (!dc.Documents.Contains(SelectedItem))
            {
                dc.Documents.Add(SelectedItem);
                Log = new() { Activity = 1, Date = DateTime.Now, Document = SelectedItem.Id, User = 1 };
                q = "Add";
            }
            else
            {
                dc.Update(SelectedItem);
                Log = new() { Activity = 2, Date = DateTime.Now, Document = SelectedItem.Id, User = 1 };
            }
            dc.SaveChanges();
            UpdateAndAddItems(dc.DocumentFiles, DocFiles, DocFilesDelete, (item) => new DocumentFile { Document = SelectedItem.Id, Description = item.Description, Extension = item.Extension, File = item.File, FileName = item.FileName });
            Log.Document = SelectedItem.Id;
            dc.DocumentLogs.Add(Log);
            dc.SaveChanges();
            transaction.Commit();
            pageVM.UpdateData();
            if (q == "Add") { ItemList = pageVM.Documents; CheckNav(pageVM.Documents.IndexOf(SelectedItem)); } //Добавление элемента в коллекцию навигации
        }
        catch (Exception ex) { transaction.Rollback(); ShowMessage(ex.ToString()); }
    }
    protected override void OpenLog() //Открытие протокола
    {
        using ArchiveBdContext dc = new();
        Log = new ObservableCollection<DocumentLog>(dc.DocumentLogs.Where(u => u.Document == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
        UCVisibility = Visibility.Visible;
    }
    public ICommand ChooseFile => new RelayCommand(ChooseFiles);
    public ICommand SaveEditFile => new RelayCommand(SaveEditFileCommand);
    public ICommand SaveFile => new RelayCommand(SaveFiles);
    public ICommand OpenEditFile => new RelayCommand(OpenEditFileCommand);
    public ICommand AddEditFile => new RelayCommand(AddEditFileCommand);
    public ICommand DeleteFile => new RelayCommand(DeleteFileCommand);
    public ICommand EditFeature => new RelayCommand(EditFeatureCommand);
    public ICommand AddFeature => new RelayCommand(AddFeatureCommand);
    public ICommand SaveFeature => new RelayCommand(SaveFeatureCommand);
    public ICommand RemoveFeature => new RelayCommand(RemoveFeatureCommand);
    protected override void CloseLog() { UCVisibility = Visibility.Collapsed; FileVisibility = Visibility.Collapsed; }
    private void DeleteFileCommand()
    {
        if (SelFile != null || SelFile.Id == 0) { DocFiles.Remove(SelFile); }
        DocFilesDelete.Add(SelFile); DocFiles.Remove(SelFile);
    }
    private void OpenEditFileCommand()
    {
        Action = ActionType.Change;
        Index = DocFiles.IndexOf(SelFile);
        EditFile = new DocumentFile() { Id = SelFile.Id, Description = SelFile.Description, Document = SelFile.Document, File = SelFile.File, FileName = SelFile.FileName, Extension = SelFile.Extension };
        FileVisibility = Visibility.Visible;
    }
    private void AddEditFileCommand()
    {
        SelFile = new DocumentFile();
        OpenEditFileCommand();
        Action = ActionType.Add;
    }
    private void ChooseFiles()
    {
        OpenFileDialog openFileDialog = new() { Multiselect = false };
        if (openFileDialog.ShowDialog() == true) //Если пользователь выбрал файл
        {
            EditFile.FileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName); //Установка имени файла
            EditFile.File = File.ReadAllBytes(openFileDialog.FileName); // Записывание файла
            EditFile.Extension = Path.GetExtension(openFileDialog.FileName); //Установка расширения
            Action = ActionType.Change; //Установка действия на изменение
        }
    }
    private void SaveEditFileCommand()
    {
        if (EditFile != null)
        {
            if (EditFile.File == null) ShowMessage("Выберите файл!");
            else
            {
                if (Action == ActionType.Add) DocFiles.Add(EditFile);
                else DocFiles[Index] = EditFile;
                CloseLog();
                Action = ActionType.Change;
            }
        }
    }
    // Метод для сохранения файла
    private void SaveFiles()
    {
        if (SelFile.File is null) return;  // Проверяем, что файл не равен null
        SaveFileDialog saveFileDialog = new()
        {
            Title = SelFile.FileName,  // Устанавливаем заголовок диалога
            FileName = $"{SelFile.FileName}{SelFile.Extension}", // Устанавливаем имя файла по умолчанию
            Filter = $"Все файлы (*.*)|*.*|{SelFile.Extension.ToUpper()} файлы (*{SelFile.Extension})|*{SelFile.Extension}" // Устанавливаем фильтр для типов файлов
        };
        // Если пользователь нажал кноп "Сохранить" в диалоге сохранения файла, то записываем файл на диск
        if (saveFileDialog.ShowDialog() ?? false) System.IO.File.WriteAllBytes(saveFileDialog.FileName, SelFile.File);
    }
    #region Features
    private void EditFeatureCommand()
    {
        using ArchiveBdContext dc = new();
        Index = DocFeatures.IndexOf(SelectedFeature);
        EditedFeature = new Feature() { Id = SelectedFeature.Id, Name = SelectedFeature.Name };
        Features = new ObservableCollection<Feature>();
        var allFeatures = new ObservableCollection<Feature>(dc.Features.AsNoTracking());
        if (DocFeatures != null)
        {
            foreach (Feature f in allFeatures)
                if (!DocFeatures.Any(uf => uf.Id == f.Id)) Features.Add(f);
        }
        else Features = allFeatures;
        FeatureVisibility = Visibility.Visible;
    }
    private void SaveFeatureCommand()
    {
        if (EditedFeature != null)
            if (EditedFeature.Name == null) ShowMessage("Выберите работу!");
            else
            {
                if (Action == ActionType.Add) { DocFeatures.Add(EditedFeature); }
                else DocFeatures[Index] = EditedFeature;
                CloseLog();
                Action = ActionType.Change;
            }
    }
    private void AddFeatureCommand()
    {
        SelectedFeature = new Feature();
        EditFeatureCommand();
        Action = ActionType.Add;
    }
    private void RemoveFeatureCommand()
    {
        if (SelectedFeature == null) return;
        DocFeatures.Remove(SelectedFeature);
    }
    #endregion
}