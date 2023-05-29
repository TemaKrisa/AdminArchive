using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
namespace AdminArchive.ViewModel;
class InventoryWindowVM : EditBaseVM
{
    #region Переменные
    private Inventory _selectedItem = new();
    private int currentIndex;
    private ObservableCollection<Inventory> itemList = new();
    private Fond curFond;
    private ObservableCollection<InventoryLog> _Log;
    public Inventory SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
    public ObservableCollection<InventoryLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
    public ObservableCollection<Movement> Movements { get; set; }
    public ObservableCollection<MovementType> MovementTypes { get; set; }
    public ObservableCollection<Acess> Acess { get; set; }
    public ObservableCollection<FondView> FondView { get; set; }
    public ObservableCollection<CharRestrict> CharRestrict { get; set; }
    public ObservableCollection<Carrier> Carriers { get; set; }
    public ObservableCollection<HistoricalPeriod> HistoricalPeriod { get; set; }
    public ObservableCollection<InventoryType> Types { get; set; }
    public ObservableCollection<ReceiptReason> ReceiptReasons { get; set; }
    public ObservableCollection<DocType> DocType { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<SecretChar> SecretChar { get; set; }
    public ObservableCollection<IncomeSource> IncomeSource { get; set; }
    public ObservableCollection<StorageTime> StorageTime { get; set; }
    public ObservableCollection<Inventory> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
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
        SelectedItem = (ItemList.Count > 0) ? ItemList[ItemList.Count - 1] : null;
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
    public InventoryWindowVM(Inventory selInv, InventoryPageVM vm, int selIndex, ObservableCollection<Inventory> items, Fond fond)
    {
        SelectedItem = selInv;
        pageVM = vm;
        curFond = fond;
        currentIndex = selIndex;
        ItemList = items;
        FillCollections();
    }
    public InventoryWindowVM(InventoryPageVM vm, ObservableCollection<Inventory> items, Fond fond)
    {
        curFond = fond;
        ItemList = items;
        pageVM = vm;
        FillCollections();
        AddItem();
    }
    public InventoryWindowVM() { }
    #endregion
    #region Заполнение полей
    private void FillTables() { }
    protected override void FillCollections()
    {
        try
        {
            using ArchiveBdContext dc = new();
            Acess = new ObservableCollection<Acess>(dc.Acesses);
            FondView = new ObservableCollection<FondView>(dc.FondViews);
            CharRestrict = new ObservableCollection<CharRestrict>(dc.CharRestricts);
            HistoricalPeriod = new ObservableCollection<HistoricalPeriod>(dc.HistoricalPeriods);
            DocType = new ObservableCollection<DocType>(dc.DocTypes);
            Categories = new ObservableCollection<Category>(dc.Categories);
            SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
            IncomeSource = new ObservableCollection<IncomeSource>(dc.IncomeSources);
            StorageTime = new ObservableCollection<StorageTime>(dc.StorageTimes);
            Movements = new ObservableCollection<Movement>(dc.Movements);
            ReceiptReasons = new ObservableCollection<ReceiptReason>(dc.ReceiptReasons);
            MovementTypes = new ObservableCollection<MovementType>(dc.MovementTypes);
            Carriers = new ObservableCollection<Carrier>(dc.Carriers);
            Types = new ObservableCollection<InventoryType>(dc.InventoryTypes);
            if (SelectedItem != null) CheckNav(currentIndex);
            else AddItem();
        }
        catch (Exception e) { ShowMessage(e.Message); }
    }
    #endregion
    protected override void AddItem()
    {
        SelectedItem = new Inventory()
        {
            Acess = curFond.Acess,
            Category = curFond.Category,
            Movement = curFond.Movement,
            SecretChar = curFond.SecretChar,
            DocType = curFond.DocType,
            IncomeSource = curFond.IncomeSource,
            MovementType = curFond?.MovementType ?? null,
            ReceiptReason = curFond.ReceiptReason,
            StorageTime = curFond.StorageTime,
            CharRestrict = curFond?.CharRestrict ?? null
        };
        CheckNav();
    }

    protected override void SaveItem()
    {
        try
        {
            InventoryLog Log;
            using ArchiveBdContext dc = new();
            if (SelectedItem.Movement == 2 && SelectedItem.MovementType == null) { ShowMessage("При выборе движения выбыл, также должен быть выбран тип движения!"); }
            else if (string.IsNullOrWhiteSpace(SelectedItem.Name)) { ShowMessage("Введите наименование описи!"); }
            else if (string.IsNullOrWhiteSpace(SelectedItem.Number)) { ShowMessage("Введите номер описи!"); }
            else
            {
                if (!dc.Inventories.Contains(SelectedItem))
                {
                    if (dc.Inventories.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal))
                    {
                        ShowMessage("Добавление описи", "Опись с таким номером уже существует");
                        return;
                    }
                    dc.Inventories.Add(SelectedItem);
                    dc.SaveChanges();
                    Log = new() { Activity = 1, Date = DateTime.Now, Inventory = SelectedItem.Id, User = 1 };
                }
                else
                {
                    dc.Update(SelectedItem);
                    dc.SaveChanges();
                    Log = new() { Activity = 2, Date = DateTime.Now, Inventory = SelectedItem.Id, User = 1 };
                }
                dc.InventoryLogs.Add(Log);
            }
            dc.SaveChanges();
            pageVM.UpdateData();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    #region Протокол
    protected override void OpenLog()
    {
        using ArchiveBdContext dc = new();
        UCVisibility = Visibility.Visible;
        Log = new ObservableCollection<InventoryLog>(dc.InventoryLogs.Where(u => u.Inventory == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
    }
    protected override void CloseLog() { UCVisibility = Visibility.Collapsed; }
    #endregion
}
