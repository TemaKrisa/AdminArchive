using AdminArchive.Classes;
using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
class FundWindowVM : EditBaseVM
{
    #region Переменные
    private ObservableCollection<FondLog> _Log;
    private ObservableCollection<MovementType> movementTypes;
    private ObservableCollection<Fond> itemList;
    private ObservableCollection<UndocumentPeriod> undocPeriodDelete, _undocumentPeriods;
    private ObservableCollection<FondName> fondNames, fondNamesDelete;
    private Visibility _renameVisibility = Visibility.Collapsed, _periodVisibility = Visibility.Collapsed;
    private int currentIndex;
    private UndocumentPeriod _selectedPeriod, editingPeriod;
    private FondName _selectedName;
    private Fond _selectedItem;
    public Visibility PeriodVisibility { get => _periodVisibility; set { _periodVisibility = value; OnPropertyChanged(); } }
    public Visibility RenameVisibility { get => _renameVisibility; set { _renameVisibility = value; OnPropertyChanged(); } }
    public FondName SelectedName { get => _selectedName; set { _selectedName = value; OnPropertyChanged(); } }
    public UndocumentPeriod EditingPeriod { get => editingPeriod; set { editingPeriod = value; OnPropertyChanged(); } }
    public Fond SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
    public UndocumentPeriod SelectedPeriod { get => _selectedPeriod; set { _selectedPeriod = value; OnPropertyChanged(); } }
    public ObservableCollection<FondName> FondNames { get => fondNames; set { fondNames = value; OnPropertyChanged(); } }
    public ObservableCollection<MovementType> MovementTypes { get => movementTypes; set { movementTypes = value; OnPropertyChanged(); } }
    public ObservableCollection<Fond> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
    public ObservableCollection<FondLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
    public ObservableCollection<FondName> FondNamesDelete { get => fondNamesDelete; set { fondNamesDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<UndocumentPeriod> UndocumentPeriodsDelete { get => undocPeriodDelete; set { undocPeriodDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<UndocumentPeriod> UndocumentPeriods { get => _undocumentPeriods; set { _undocumentPeriods = value; OnPropertyChanged(); } }
    public ObservableCollection<Acess> Acess { get; set; }
    public ObservableCollection<FondView> FondView { get; set; }
    public ObservableCollection<CharRestrict> CharRestrict { get; set; }
    public ObservableCollection<HistoricalPeriod> HistoricalPeriod { get; set; }
    public ObservableCollection<FondType> FondType { get; set; }
    public ObservableCollection<DocType> DocType { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<SecretChar> SecretChar { get; set; }
    public ObservableCollection<IncomeSource> IncomeSource { get; set; }
    public ObservableCollection<Ownership> Ownership { get; set; }
    public ObservableCollection<StorageTime> StorageTime { get; set; }
    public ObservableCollection<ReceiptReason> ReceiptReasons { get; set; }
    public ObservableCollection<Movement> Movements { get; set; }
    #endregion
    #region Команды
    public ICommand RemoveName => new RelayCommand(RemoveNameCommand);
    public ICommand SaveName => new RelayCommand(SaveRenameCommand);
    public ICommand EditRename => new RelayCommand(EditRenameCommand);
    public ICommand CreateRename => new RelayCommand(CreateRenameCommand);
    public ICommand RemovePeriod => new RelayCommand(RemovePeriodCommand);
    public ICommand SavePeriod => new RelayCommand(SavePeriodCommand);
    public ICommand EditPeriod => new RelayCommand(EditPeriodCommand);
    public ICommand CreatePeriod => new RelayCommand(CreatePeriodCommand);
    #endregion
    #region Навигация
    private void CheckNav(int index) { IsFirst = index != 0; IsLast = index != ItemList.Count - 1; }
    private void CheckNav(string q)
    {
        if (q == "Add")
        {
            ItemList = pageVM.Fonds;
            CheckNav(pageVM.Fonds.IndexOf(SelectedItem));
        }
    }
    protected override void GoNext()
    {
        currentIndex++;
        SelectedItem = (currentIndex < ItemList.Count) ? ItemList[currentIndex] : SelectedItem;
        IsFirst = currentIndex != 0; IsLast = currentIndex != ItemList.Count - 1;
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
    public FundWindowVM(Fond selFond, dynamic vm, int selIndex, ObservableCollection<Fond> items)
    {
        SelectedItem = selFond;
        pageVM = vm;
        currentIndex = selIndex;
        ItemList = items;
        FillCollections();
    }
    public FundWindowVM(dynamic vm, ObservableCollection<Fond> items) { ItemList = items; pageVM = vm; SelectedItem = null; FillCollections(); }
    public FundWindowVM() { }
    #endregion
    #region Заполнение коллекций
    protected override void FillCollections()
    {
        try
        {
            using ArchiveBdContext dc = new();
            Acess = new ObservableCollection<Acess>(dc.Acesses);
            FondView = new ObservableCollection<FondView>(dc.FondViews);
            CharRestrict = new ObservableCollection<CharRestrict>(dc.CharRestricts);
            HistoricalPeriod = new ObservableCollection<HistoricalPeriod>(dc.HistoricalPeriods);
            FondType = new ObservableCollection<FondType>(dc.FondTypes);
            DocType = new ObservableCollection<DocType>(dc.DocTypes);
            Categories = new ObservableCollection<Category>(dc.Categories);
            SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
            IncomeSource = new ObservableCollection<IncomeSource>(dc.IncomeSources);
            Ownership = new ObservableCollection<Ownership>(dc.Ownerships);
            StorageTime = new ObservableCollection<StorageTime>(dc.StorageTimes);
            Movements = new ObservableCollection<Movement>(dc.Movements);
            MovementTypes = new ObservableCollection<MovementType>(dc.MovementTypes);
            ReceiptReasons = new ObservableCollection<ReceiptReason>(dc.ReceiptReasons);
            FondNamesDelete = new ObservableCollection<FondName>();
            UndocumentPeriodsDelete = new ObservableCollection<UndocumentPeriod>();
            if (SelectedItem != null)
            {
                FillTables();
                CheckNav(currentIndex);
            }
            else
            {
                FondNames = new ObservableCollection<FondName>();
                UndocumentPeriods = new ObservableCollection<UndocumentPeriod>();
                FondNamesDelete = new ObservableCollection<FondName>();
                UndocumentPeriodsDelete = new ObservableCollection<UndocumentPeriod>();
                AddItem();
            }
        }
        catch (Exception e)
        { ShowMessage(e.Message); }
    }
    private void FillTables()
    {
        using ArchiveBdContext dc = new();
        FondNames = new ObservableCollection<FondName>(dc.FondNames.Where(u => u.Fond == SelectedItem.Id));
        UndocumentPeriods = new ObservableCollection<UndocumentPeriod>(dc.UndocumentPeriods.Where(u => u.Fond == SelectedItem.Id));
        FondNamesDelete = new ObservableCollection<FondName>();
        UndocumentPeriodsDelete = new ObservableCollection<UndocumentPeriod>();
    }
    #endregion
    protected override void AddItem() //Создание нового фонда с заполнением полей
    {
        SelectedItem = new Fond() { Acess = 1, Category = 4, View = 2, Movement = 2, SecretChar = 1, HistoricalPeriod = 2, StorageTime = 1, IncomeSource = 6 };
        CheckNav();
    }
    string q = "";
    private bool ValidateInput()
    {
        if (SelectedItem.Movement == 1 && SelectedItem.MovementType == null) { ShowMessage("При выборе движения выбыл, также должен быть выбран тип движения!"); return false; }
        else if (string.IsNullOrWhiteSpace(SelectedItem.Name)) { ShowMessage("Введите наименование фонда!"); return false; }
        else if (string.IsNullOrWhiteSpace(SelectedItem.ShortName)) { ShowMessage("Введите сокращенное наименование фонда!"); return false; }
        else if (SelectedItem.OwnerShip == 0) { ShowMessage("Выберите собственность фонда!"); return false; }
        else if (SelectedItem.DocType == 0) { ShowMessage("Выберите тип документов!"); return false; }
        else if (SelectedItem.View == 0) { ShowMessage("Выберите вид!"); return false; }
        else if (SelectedItem.Type == 0) { ShowMessage("Выберите тип фонда!"); return false; }
        else if (SelectedItem.ReceiptReason == 0) { ShowMessage("Выберите источник поступления!"); return false; }
        return true;
    }
    protected override void SaveItem() //Сохранение фонда
    {
        if (!ValidateInput()) return;
        var c = SelectedItem.Category;
        FondLog Log;
        using ArchiveBdContext dc = new();
        if (dc.Fonds.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal && u.Index == SelectedItem.Index && u.Id != SelectedItem.Id))
        {
            ShowMessage("Фонд с таким номером уже существует");
            return;
        }
        using var transaction = dc.Database.BeginTransaction();
        try
        {
            if (!dc.Fonds.Contains(SelectedItem))
            {
                dc.Fonds.Add(SelectedItem);
                Log = new() { Activity = 1, Date = DateTime.Now, User = 1 };
                q = "Add";
            }
            else
            {
                dc.Update(SelectedItem);
                Log = new() { Activity = 2, Date = DateTime.Now, User = 1 };
                q = "";
            }
            dc.SaveChanges();
            //Сохранение переименований фондов
            //if (FondNames.Count != 0)
            //{
            //    dc.FondNames.UpdateRange(FondNames.Where(fn => dc.FondNames.Any(u => u.Id == fn.Id)));
            //    foreach (var item in FondNames.Where(fn => !dc.FondNames.Any(u => u.Id == fn.Id)))
            //    {
            //        FondName f = new() { Name = item.Name, Fond = SelectedItem.Id, EndDate = item.EndDate, StartDate = item.StartDate, Note = item.Note };
            //        dc.FondNames.Add(f);
            //    }
            //    dc.FondNames.RemoveRange(FondNamesDelete.Where(fnd => dc.FondNames.Any(u => u.Id == fnd.Id)));
            //}
            UpdateAndAddItems(dc.FondNames, FondNames, FondNamesDelete, (item) => new UnitCompletedWork { Work = item.Work, Note = item.Note, Date = item.Date, Unit = SelectedItem.Id });
            //Сохранение незадокументированных периодов
            if (UndocumentPeriods.Count != 0)
            {
                dc.UndocumentPeriods.UpdateRange(UndocumentPeriods.Where(up => dc.UndocumentPeriods.Any(u => u.Id == up.Id)));
                dc.UndocumentPeriods.AddRange(UndocumentPeriods.Where(up => !dc.UndocumentPeriods.Any(u => u.Id == up.Id)));
                foreach (var item in UndocumentPeriods.Where(fn => !dc.UndocumentPeriods.Any(u => u.Id == fn.Id)))
                {
                    UndocumentPeriod p = new() { Fond = SelectedItem.Id, EndDate = item.EndDate, StartDate = item.StartDate, Note = item.Note, Reason = item.Reason, DocumentLocation = item.DocumentLocation };
                    dc.UndocumentPeriods.Add(p);
                }
                dc.UndocumentPeriods.RemoveRange(UndocumentPeriodsDelete.Where(upd => dc.UndocumentPeriods.Any(u => u.Id == upd.Id)));
            }
            SelectedItem.Category = c;
            Log.Fond = SelectedItem.Id;
            dc.FondLogs.Add(Log);
            dc.SaveChanges();
            transaction.Commit();
            CheckNav(q);
            pageVM.UpdateData();
            q = "";
        }
        catch (Exception ex) { transaction.Rollback(); ShowMessage(ex.ToString()); }
    }

    private void UpdateAndAddItems<T>(DbSet<T> dbSet, ObservableCollection<T> items, ObservableCollection<T> itemsToDelete, Func<T, T> createNewItem) where T : class, IHasId
    {
        if (items.Count == 0) return;
        dbSet.UpdateRange(items.Where(item => dbSet.Any(u => u.Id == item.Id)));
        foreach (var item in items.Where(item => !dbSet.Any(u => u.Id == item.Id)))
        {
            dbSet.Add(createNewItem(item));
        }
        dbSet.RemoveRange(itemsToDelete.Where(item => dbSet.Any(u => u.Id == item.Id)));
    }
    #region Протокол
    protected override void OpenLog() //Открытие протокола
    {
        using ArchiveBdContext dc = new();
        UCVisibility = Visibility.Visible;
        Log = new ObservableCollection<FondLog>(dc.FondLogs.Where(u => u.Fond == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
    }
    protected override void CloseLog() //Закрытие юзерконтролов
    {
        RenameVisibility = Visibility.Collapsed;
        UCVisibility = Visibility.Collapsed;
        PeriodVisibility = Visibility.Collapsed;
    }
    #endregion
    #region Переименования фондов
    private void RemoveNameCommand()
    {
        if (SelectedName == null) return;
        FondNamesDelete.Add(SelectedName); FondNames.Remove(SelectedName); CloseLog();
    }
    private void CreateRenameCommand()
    {
        Action = ActionType.Add;
        SelectedName = new FondName();
        EditRenameCommand();
    }
    private FondName editingName;
    public FondName EditingName { get => editingName; set { editingName = value; OnPropertyChanged(); } }
    private void SaveRenameCommand()
    {
        if (EditingName != null)
            if (EditingName.StartDate > EditingName.EndDate) ShowMessage("Начальная дата превышает конечную!");
            else
            {
                if (Action == ActionType.Add) { FondNames.Add(EditingName); }
                else
                    FondNames[FondNames.IndexOf(FondNames.FirstOrDefault(u => u.Id == EditingName.Id))] = EditingName;
                CloseLog();
            }
        Action = ActionType.Change;
    }
    private void EditRenameCommand()
    {
        if (SelectedName != null)
        {
            EditingName = new FondName()
            {
                Id = SelectedName.Id,
                Fond = SelectedName.Fond,
                Name = SelectedName.Name,
                EndDate = SelectedName.EndDate,
                StartDate = SelectedName.StartDate,
                Note = SelectedName.Note
            };
            RenameVisibility = Visibility.Visible;
        }
    }
    #endregion
    #region Незадокументированные периоды
    private void EditPeriodCommand()
    {
        EditingPeriod = new UndocumentPeriod() { Id = SelectedPeriod.Id, DocumentLocation = SelectedPeriod.DocumentLocation, StartDate = SelectedPeriod.StartDate, EndDate = SelectedPeriod.EndDate, Reason = SelectedPeriod.Reason, Fond = SelectedItem.Id, Note = SelectedPeriod.Note, };
        PeriodVisibility = Visibility.Visible;
    }
    private void SavePeriodCommand()
    {
        if (EditingPeriod != null)
            if (EditingPeriod.StartDate > EditingPeriod.EndDate) ShowMessage("Начальная дата превышает конечную!");
            else
            {
                if (Action == ActionType.Add) { UndocumentPeriods.Add(EditingPeriod); }
                else
                    UndocumentPeriods[UndocumentPeriods.IndexOf(UndocumentPeriods.FirstOrDefault(u => u.Id == EditingPeriod.Id))] = EditingPeriod;
                CloseLog();
            }
        Action = ActionType.Change;
    }
    private void RemovePeriodCommand()
    {
        if (SelectedPeriod == null || SelectedPeriod.Id == 0) return;
        UndocumentPeriodsDelete.Add(SelectedPeriod); UndocumentPeriods.Remove(SelectedPeriod); CloseLog();
    }
    private void CreatePeriodCommand()
    {
        SelectedPeriod = new UndocumentPeriod();
        Action = ActionType.Add;
        EditPeriodCommand();
    }
    #endregion
}
