using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
namespace AdminArchive.ViewModel;
partial class InventoryPageVM : PageBaseVM
{
    private ObservableCollection<Inventory> _inventories;
    private Fond curFond;
    private string _inventoryName, _inventoryStartDate, _inventoryEndDate;
    private int _inventoryCategory = -1;
    public string InventoryName { get => _inventoryName; set { _inventoryName = value; OnPropertyChanged(); } }
    public string InventoryStartDate { get => _inventoryStartDate; set { _inventoryStartDate = value; OnPropertyChanged(); } }
    public string InventoryEndDate { get => _inventoryEndDate; set { _inventoryEndDate = value; OnPropertyChanged(); } }
    public int InventoryCategory { get => _inventoryCategory; set { _inventoryCategory = value; OnPropertyChanged(); } }
    public ObservableCollection<Inventory> Inventories { get => _inventories; set { _inventories = value; OnPropertyChanged(); } }
    public ObservableCollection<Category> Categories { get; set; }
    public Inventory SelectedItem { get; set; }
    public InventoryPageVM(Fond fond) { using ArchiveBdContext dc = new(); curFond = fond; Categories = new ObservableCollection<Category>(dc.Categories); UpdateData(); Categories.Insert(0, new Category { Name = "Все категории", Id = -1 }); }
    public InventoryPageVM() { }
    public override void UpdateData() => Inventories = SearchClass.SearchInventory(InventoryName, InventoryStartDate, InventoryEndDate, InventoryCategory, curFond); /*new ObservableCollection<Inventory>(dc.Inventories.Include(u => u.TypeNavigation).Where(u => u.Fond == curFond.Id).OrderBy(u => u.Number).ThenBy(u => u.Literal));*/
    protected override void GoBack() { Setting.mainFrame.Navigate(new FundPage()); }
    protected override void OpenItem()
    {
        if (SelectedItem != null)
        {
            StorageUnitPageVM vm = new(SelectedItem, curFond);
            StorageUnitPage v = new() { DataContext = vm };
            Setting.mainFrame.Navigate(v);
        }
    }
    protected override void AddItem()
    {
        InventoryWindowVM viewModel = new(this, Inventories, curFond);
        InventoryWindow newWindow = new() { DataContext = viewModel };
        newWindow.ShowDialog();
    }
    protected override void EditItem()
    {
        int index = Inventories.IndexOf(SelectedItem);
        InventoryWindow newWindow = new();
        InventoryWindowVM viewModel = new((SelectedItem as Inventory), this, index, Inventories, curFond);
        newWindow.DataContext = viewModel;
        newWindow.ShowDialog();
    }
    protected override void ResetSearch() { InventoryName = null; InventoryStartDate = null; InventoryEndDate = null; InventoryCategory = -1; UCVisibility = System.Windows.Visibility.Collapsed; UpdateData(); }
    protected override void RemoveItem() { RemoveCommand(SelectedItem); }
}
