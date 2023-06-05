using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
namespace AdminArchive.ViewModel;
class StorageUnitPageVM : PageBaseVM
{
    private ObservableCollection<StorageUnit> _storageUnits;
    public ObservableCollection<StorageUnit> StorageUnits { get => _storageUnits; set { _storageUnits = value; OnPropertyChanged(); } }
    private StorageUnit _selectedItem;
    public StorageUnit SelectedItem { get => _selectedItem; set => _selectedItem = value; }
    private Inventory curInv;
    private Fond curFond;
    public ObservableCollection<UnitCategory> UnitCategories { get; set; }
    private string? _unitName, _unitStartDate, _unitEndDate;
    private int _unitCategory = -1;
    public string? UnitName { get => _unitName; set { _unitName = value; OnPropertyChanged(); } }
    public string? UnitStartDate { get => _unitStartDate; set { _unitStartDate = value; OnPropertyChanged(); } }
    public string? UnitEndDate { get => _unitEndDate; set { _unitEndDate = value; OnPropertyChanged(); } }
    public int UnitCategory { get => _unitCategory; set { _unitCategory = value; OnPropertyChanged(); } }

    public StorageUnitPageVM(Inventory inv, Fond fond)
    {
        curFond = fond;
        curInv = inv;
        LoadData();
    }
    public StorageUnitPageVM() { }
    protected override void GoBack()
    {
        InventoryPageVM vm = new(curFond);
        InventoryPage v = new() { DataContext = vm };
        Setting.mainFrame?.Navigate(v);
    }

    private void LoadData()
    {
        using ArchiveBdContext dc = new();
        UnitCategories = new ObservableCollection<UnitCategory>(dc.UnitCategories);
        UnitCategories.Insert(0, new UnitCategory { Name = "Все категории", Id = -1 });
        UpdateData();
    }
    public override void UpdateData()
    {
        StorageUnits = SearchClass.SearchUnit(UnitName, UnitStartDate, UnitEndDate, UnitCategory, curInv);
    }
    protected override void EditItem()
    {
        int index = StorageUnits.IndexOf(SelectedItem);
        StorageUnitWindow Editor = new();
        StorageUnitWindowVM vm = new(SelectedItem, this, index, StorageUnits, curInv);
        Editor.DataContext = vm;
        Editor.ShowDialog();
    }
    protected override void AddItem()
    {
        StorageUnitWindowVM vm = new(this, StorageUnits, curInv);
        var newWindow = new StorageUnitWindow { DataContext = vm };
        newWindow.ShowDialog();
    }
    protected override void OpenItem()
    {
        if (SelectedItem != null)
        {
            try
            {

                DocumentPageVM vm = new(SelectedItem, curFond, curInv);
                DocumentPage v = new() { DataContext = vm };
                Setting.mainFrame?.Navigate(v);
            }
            catch (Exception ex) { ShowMessage(ex.ToString()); }
        }
    }
    protected override void ResetSearch() { UpdateData(); }

    protected override void RemoveItem() { RemoveCommand(SelectedItem); }
}
