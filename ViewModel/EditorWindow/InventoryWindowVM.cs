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
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }
        catch { IsLast = false; }
    }
    protected override void GoPrev()
    {
        try
        {
            currentIndex--;
            SelectedItem = (currentIndex >= 0) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }
        catch { IsFirst = false; }
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
    public InventoryWindowVM(Inventory selInv, dynamic vm, int selIndex, ObservableCollection<Inventory> items, Fond fond)
    {
        SelectedItem = selInv;
        pageVM = vm;
        curFond = fond;
        currentIndex = selIndex;
        ItemList = items;
        FillCollections();
    }
    public InventoryWindowVM(dynamic vm, ObservableCollection<Inventory> items, Fond fond)
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
            CharRestrict = curFond?.CharRestrict ?? null,
            Fond = curFond.Id
        };
        CheckNav();
    }
    protected bool ValidateInput(ArchiveBdContext dc)
    {
        if (SelectedItem.Movement == 1 && SelectedItem.MovementType == null) { ShowMessage("При выборе движения выбыл, также должен быть выбран тип движения!"); return false; }
        else if (string.IsNullOrWhiteSpace(SelectedItem.Name)) { ShowMessage("Введите наименование!"); return false; }
        else if (string.IsNullOrWhiteSpace(SelectedItem.Number)) { ShowMessage("Введите номер!"); return false; }
        else if (SelectedItem.Type == 0) { ShowMessage("Введите тип!"); return false; }
        else if (SelectedItem.Carrier == 0) { ShowMessage("Выберите носитель!"); return false; }
        else if (SelectedItem.Acess == 0) { ShowMessage("Выберите доступ!"); return false; }
        else if (SelectedItem.Movement == 0) { ShowMessage("Выберите движение!"); return false; }
        else if (SelectedItem.Category == 0) { ShowMessage("Выберите категорию!"); return false; }
        else if (SelectedItem.ReceiptReason == 0) { ShowMessage("Выберите основание поступления!"); return false; }
        else if (SelectedItem.IncomeSource == 0) { ShowMessage("Выберите источник поступления!"); return false; }
        else if (SelectedItem.StorageTime == 0) { ShowMessage("Введите срок хранения!"); return false; }
        else if (dc.Inventories.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal && u.Fond == SelectedItem.Fond && u.Id != SelectedItem.Id)) { ShowMessage("Опись с таким номером уже существует"); return false; }
        if (SelectedItem.Movement == 2) { SelectedItem.MovementType = null; }
        return true;
    }
    protected override void SaveItem()
    {
        using ArchiveBdContext dc = new();
        if (!ValidateInput(dc)) return;
        var q = string.Empty;
        InventoryLog Log;
        try
        {
            if (SelectedItem.Movement == 2) SelectedItem.MovementType = null;
            SelectedItem.Fond = curFond.Id;
            if (!dc.Inventories.Contains(SelectedItem))
            {
                dc.Inventories.Add(SelectedItem); q = "Add";
                Log = new() { Activity = 1, Date = DateTime.Now, Inventory = SelectedItem.Id, User = 1 };
            }
            else
            {
                dc.Update(SelectedItem);
                Log = new() { Activity = 2, Date = DateTime.Now, Inventory = SelectedItem.Id, User = 1 };
            }
            dc.SaveChanges();
            Log.Inventory = SelectedItem.Id;
            dc.InventoryLogs.Add(Log);
            dc.SaveChanges();
            pageVM.UpdateData(); //Обновление данных
            if (q == "Add") { ItemList = pageVM.Inventories; CheckNav(pageVM.Inventories.IndexOf(SelectedItem)); } //Добавление элемента в коллекцию навигации
        }
        catch (Exception ex) { ShowMessage(ex.ToString()); }
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
