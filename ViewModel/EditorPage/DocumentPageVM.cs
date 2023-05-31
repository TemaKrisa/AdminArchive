using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel;
/// <summary> ViewModel для страницы документов </summary>
class DocumentPageVM : PageBaseVM
{
    public ObservableCollection<Document>? Documents { get; set; }
    private ArchiveBdContext dc;
    private Document _selectedItem;
    private StorageUnit curUnit;
    private Inventory curInv;
    private Fond curFond;
    private string _docTitle;
    private int _docAu = -1, _docType = -1;
    private DateTime _docDate;
    public string? DocTitle { get => _docTitle; set { _docTitle = value; OnPropertyChanged(); } }
    public int DocAu { get => _docAu; set { _docAu = value; OnPropertyChanged(); } }
    public int DocType { get => _docType; set { _docType = value; OnPropertyChanged(); } }
    public DateTime DocDate { get => _docDate; set { _docDate = value; OnPropertyChanged(); } }
    public ObservableCollection<Authenticity> Authenticities { get; set; }
    public ObservableCollection<DocType> DocTypes { get; set; }
    public Document SelectedItem { get => _selectedItem; set => _selectedItem = value; }
    public DocumentPageVM(StorageUnit unit, Fond fond, Inventory inventory)
    {
        dc = new ArchiveBdContext();
        curUnit = unit;
        curFond = fond;
        curInv = inventory;
        UpdateData();
    }
    public override void UpdateData() => Documents = SearchClass.SearchDocument(DocTitle, DocAu, DocType, DocDate, curUnit);
    protected override void GoBack()
    {
        StorageUnitPageVM vm = new(curInv, curFond);
        Setting.mainFrame?.Navigate(new StorageUnitPage { DataContext = vm });
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
        DocumentWindowVM vm = new(this, Documents, curUnit);
        var newWindow = new DocumentWindow { DataContext = vm };
        newWindow.ShowDialog();
    }
    protected override void ResetSearch() { DocTitle = null; DocAu = -1; DocType = -1; DocDate = DateTime.MinValue; UpdateData(); }
    protected override void OpenItem() { }
    public DocumentPageVM() { }
}