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
    public ObservableCollection<FondName> FondNames { get => fondNames; set { fondNames = value; OnPropertyChanged(nameof(FondNames)); } }
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
    public FundWindowVM(Fond selFond, FundPageVM vm, int selIndex, ObservableCollection<Fond> items)
    {
        SelectedItem = selFond;
        pageVM = vm;
        currentIndex = selIndex;
        ItemList = items;
        FillCollections();
    }
    public FundWindowVM(FundPageVM vm, ObservableCollection<Fond> items)
    { ItemList = items; pageVM = vm; SelectedItem = null; FillCollections(); }
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
            if (SelectedItem != null)
            {
                FillTables();
                CheckNav(currentIndex);
            }
            else AddItem();
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
    protected override void SaveItem() //Сохранение фонда
    {
        var c = SelectedItem.Category;
        FondLog Log;
        using ArchiveBdContext dc = new();
        if (SelectedItem.Movement == 1 && SelectedItem.MovementType == null) { ShowMessage("При выборе движения выбыл, также должен быть выбран тип движения!"); }
        else if (string.IsNullOrWhiteSpace(SelectedItem.Name)) { ShowMessage("Введите наименование фонда!"); }
        else if (string.IsNullOrWhiteSpace(SelectedItem.ShortName)) { ShowMessage("Введите сокращенное наименование фонда!"); }
        else if (string.IsNullOrWhiteSpace(SelectedItem.ReceiptDate.ToString())) { ShowMessage("Введите дату поступления!"); }
        else
        {
            using var transaction = dc.Database.BeginTransaction();
            try
            {
                if (!dc.Fonds.Contains(SelectedItem))
                {
                    if (dc.Fonds.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal && u.Index == SelectedItem.Index))
                    {
                        ShowMessage("Добавление фонда", "Фонд с таким номером уже существует");
                        return;
                    }
                    dc.Fonds.Add(SelectedItem);
                    Log = new() { Activity = 1, Date = DateTime.Now, Fond = SelectedItem.Id, User = 1 };
                }
                else
                {
                    dc.Update(SelectedItem);
                    Log = new() { Activity = 2, Date = DateTime.Now, Fond = SelectedItem.Id, User = 1 };
                }
                dc.FondLogs.Add(Log);
                //Сохранение переименований фондов
                dc.FondNames.UpdateRange(FondNames.Where(fn => dc.FondNames.Any(u => u.Id == fn.Id)));
                dc.FondNames.AddRange(FondNames.Where(fn => !dc.FondNames.Any(u => u.Id == fn.Id)));
                dc.FondNames.RemoveRange(FondNamesDelete.Where(fnd => dc.FondNames.Any(u => u.Id == fnd.Id)));
                //Сохранение незадокументированных периодов
                dc.UndocumentPeriods.UpdateRange(UndocumentPeriods.Where(up => dc.UndocumentPeriods.Any(u => u.Id == up.Id)));
                dc.UndocumentPeriods.AddRange(UndocumentPeriods.Where(up => !dc.UndocumentPeriods.Any(u => u.Id == up.Id)));
                dc.UndocumentPeriods.RemoveRange(UndocumentPeriodsDelete.Where(upd => dc.UndocumentPeriods.Any(u => u.Id == upd.Id)));
                SelectedItem.Category = c;
                dc.SaveChanges();
                transaction.Commit();
                pageVM.UpdateData();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ShowMessage(ex.ToString());
            }
        }
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
        if (SelectedName != null) { FondNamesDelete.Add(SelectedName); FondNames.Remove(SelectedName); CloseLog(); }
    }
    private void CreateRenameCommand() { SelectedName = new FondName() { Fond = SelectedItem.Id }; EditRenameCommand(); }
    private FondName editingName;
    public FondName EditingName { get => editingName; set { editingName = value; OnPropertyChanged(); } }
    private void SaveRenameCommand()
    {
        if (EditingName != null)
            if (EditingName.StartDate > EditingName.EndDate)
                ShowMessage("Начальная дата превышает конечную!");
            else if (String.IsNullOrWhiteSpace(EditingName.Name))
                ShowMessage("Введите наименование!");
            else
            {
                var index = FondNames.IndexOf(FondNames.FirstOrDefault(u => u.Id == EditingName.Id));
                if (index == -1) FondNames.Add(EditingName);
                else FondNames[index] = EditingName;
                CloseLog();
            }
    }
    private void EditRenameCommand()
    {
        if (SelectedName != null)
        {
            RenameVisibility = Visibility.Visible;
            EditingName = new FondName()
            {
                Id = SelectedName.Id,
                Fond = SelectedName.Fond,
                Name = SelectedName.Name,
                EndDate = SelectedName.EndDate,
                StartDate = SelectedName.StartDate,
                Note = SelectedName.Note
            };
        }
    }
    #endregion
    #region Незадокументированные периоды
    private void EditPeriodCommand()
    {
        PeriodVisibility = Visibility.Visible;
        EditingPeriod = new UndocumentPeriod()
        {
            Id = SelectedPeriod.Id,
            DocumentLocation = SelectedPeriod.DocumentLocation,
            StartDate = SelectedPeriod.StartDate,
            EndDate = SelectedPeriod.EndDate,
            Reason = SelectedPeriod.Reason,
            Fond = SelectedItem.Id,
            Note = SelectedPeriod.Note
        };
    }
    private void SavePeriodCommand()
    {
        if (EditingPeriod != null)
            if (EditingPeriod.StartDate > EditingPeriod.EndDate)
                ShowMessage("Начальная дата превышает конечную!");
            else
            {
                var index = UndocumentPeriods.IndexOf(UndocumentPeriods.FirstOrDefault(u => u.Id == EditingPeriod.Id));
                UndocumentPeriods[index >= 0 ? index : UndocumentPeriods.Count] = EditingPeriod;
                CloseLog();
            }
    }
    private void RemovePeriodCommand()
    {
        if (SelectedPeriod != null) return;
        UndocumentPeriodsDelete.Add(SelectedPeriod);
        UndocumentPeriods.Remove(SelectedPeriod);
        CloseLog();
    }
    private void CreatePeriodCommand()
    {
        SelectedPeriod = new UndocumentPeriod() { Fond = SelectedItem.Id };
        EditPeriodCommand();
    }
    #endregion
}
