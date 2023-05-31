using AdminArchive.Classes;
using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
class StorageUnitWindowVM : EditBaseVM
{
    #region Переменные
    private int currentIndex;
    private ObservableCollection<StorageUnit> itemList;
    private ObservableCollection<UnitLog> _Log;
    private StorageUnit _selectedUnit = new();
    private Inventory curInv;
    private ObservableCollection<Feature> _unitFeatures, _unitFeaturesDelete, _features;
    private ObservableCollection<UnitRequiredWork> _requiredWorksDelete, _unitRequiredWorks;
    private Feature _editedFeature, _selectedFeature;
    private UnitRequiredWork _editedRequiredWork, _selectedRequiredWork;
    private UnitCompletedWork _editedCompletedWork, _selectedCompletedWork;
    private UnitCondition _editedCondition, _selectedCondition;
    private ObservableCollection<UnitCompletedWork> _completedWorksDelete, _completedWorks;
    private ObservableCollection<Model.UnitCondition> _conditionsDelete, _unitConditions;
    private ObservableCollection<Model.Condition> _conditions;
    private ObservableCollection<Work> _works;

    public ObservableCollection<Feature> Features { get => _features; set { _features = value; OnPropertyChanged(); } }
    public Feature SelectedFeature { get => _selectedFeature; set { _selectedFeature = value; OnPropertyChanged(); } }
    public Feature EditedFeature { get => _editedFeature; set { _editedFeature = value; OnPropertyChanged(); } }
    public UnitCondition SelectedCondition { get => _selectedCondition; set { _selectedCondition = value; OnPropertyChanged(); } }
    public UnitCondition EditedCondition { get => _editedCondition; set { _editedCondition = value; OnPropertyChanged(); } }
    public UnitCompletedWork SelectedCompletedWork { get => _selectedCompletedWork; set { _selectedCompletedWork = value; OnPropertyChanged(); } }
    public UnitCompletedWork EditedCompletedWork { get => _editedCompletedWork; set { _editedCompletedWork = value; OnPropertyChanged(); } }
    public UnitRequiredWork SelectedRequiredWork { get => _selectedRequiredWork; set { _selectedRequiredWork = value; OnPropertyChanged(); } }
    public UnitRequiredWork EditedRequiredWork { get => _editedRequiredWork; set { _editedRequiredWork = value; OnPropertyChanged(); } }
    public ObservableCollection<Feature> UnitFeatures { get => _unitFeatures; set { _unitFeatures = value; OnPropertyChanged(); } }
    public ObservableCollection<Feature> UnitFeaturesDelete { get => _unitFeaturesDelete; set { _unitFeaturesDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitRequiredWork> RequiredWorks { get => _unitRequiredWorks; set { _unitRequiredWorks = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitRequiredWork> RequiredWorksDelete { get => _requiredWorksDelete; set { _requiredWorksDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitCompletedWork> CompletedWorks { get => _completedWorks; set { _completedWorks = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitCompletedWork> CompletedWorksDelete { get => _completedWorksDelete; set { _completedWorksDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<Work> Works { get => _works; set { _works = value; OnPropertyChanged(); } }
    public ObservableCollection<Model.Condition> Conditions { get => _conditions; set { _conditions = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitCondition> ConditionsDelete { get => _conditionsDelete; set { _conditionsDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitCondition> UnitConditions { get => _unitConditions; set { _unitConditions = value; OnPropertyChanged(); } }
    public ObservableCollection<Acess> Acesses { get; set; }
    public ObservableCollection<SecretChar> SecretChars { get; set; }
    public ObservableCollection<Carrier> Carriers { get; set; }
    public ObservableCollection<UnitCategory> Categories { get; set; }
    public ObservableCollection<CharRestrict> CharRestricts { get; set; }
    public ObservableCollection<StorageUnit> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
    public StorageUnit SelectedItem { get => _selectedUnit; set { _selectedUnit = value; OnPropertyChanged(); } }
    private Visibility _featureVisibility = Visibility.Collapsed, _completedWorkVisibility = Visibility.Collapsed,
        _conditionVisibility = Visibility.Collapsed, _requiredWorkVisibility = Visibility.Collapsed;
    public Visibility FeatureVisibility { get => _featureVisibility; set { _featureVisibility = value; OnPropertyChanged(); } }
    public Visibility CompletedWorkVisibility { get => _completedWorkVisibility; set { _completedWorkVisibility = value; OnPropertyChanged(); } }
    public Visibility RequiredWorkVisibility { get => _requiredWorkVisibility; set { _requiredWorkVisibility = value; OnPropertyChanged(); } }
    public Visibility ConditionVisibility { get => _conditionVisibility; set { _conditionVisibility = value; OnPropertyChanged(); } }
    #endregion
    #region Команды
    public ICommand EditFeature => new RelayCommand(EditFeatureCommand);
    public ICommand AddFeature => new RelayCommand(AddFeatureCommand);
    public ICommand SaveFeature => new RelayCommand(SaveFeatureCommand);
    public ICommand RemoveFeature => new RelayCommand(RemoveFeatureCommand);
    public ICommand EditRequiredWork => new RelayCommand(EditRequiredWorkCommand);
    public ICommand AddRequiredWork => new RelayCommand(AddRequiredWorkCommand);
    public ICommand SaveRequiredWork => new RelayCommand(SaveRequiredWorkCommand);
    public ICommand RemoveRequiredWork => new RelayCommand(RemoveRequiredWorkCommand);
    public ICommand EditCompletedWork => new RelayCommand(EditCompletedWorkCommand);
    public ICommand AddCompletedWork => new RelayCommand(AddCompletedWorkCommand);
    public ICommand SaveCompletedWork => new RelayCommand(SaveCompletedWorkCommand);
    public ICommand RemoveCompletedWork => new RelayCommand(RemoveConditionCommand);
    public ICommand EditCondition => new RelayCommand(EditConditionCommand);
    public ICommand AddCondition => new RelayCommand(AddConditionCommand);
    public ICommand SaveCondition => new RelayCommand(SaveConditionCommand);
    public ICommand RemoveCondition => new RelayCommand(RemoveConditionCommand);
    #endregion
    #region Навигация
    private void CheckNav(int index) { IsFirst = index != 0; IsLast = index != ItemList.Count - 1; }
    protected override void GoNext() //Открытие следующей единицы хранения в списке
    {
        currentIndex++;
        SelectedItem = (currentIndex < ItemList.Count) ? ItemList[currentIndex] : SelectedItem;
        IsFirst = currentIndex != 0;
        IsLast = currentIndex != ItemList.Count - 1;
        FillTables();
    }
    private void CheckNav(string q)
    {
        if (q == "Add")
        {
            ItemList = pageVM.Fonds;
            CheckNav(pageVM.Fonds.IndexOf(SelectedItem));
        }
    }
    protected override void GoPrev() //Открытие прошлой единицы хранения в списке
    {
        currentIndex--;
        SelectedItem = (currentIndex >= 0) ? ItemList[currentIndex] : SelectedItem;
        IsFirst = currentIndex != 0;
        IsLast = currentIndex != ItemList.Count - 1;
        FillTables();
    }
    protected override void GoFirst() //Открытие первой единицы хранения в списке
    {
        SelectedItem = (ItemList.Count > 0) ? ItemList[0] : null;
        currentIndex = ItemList.IndexOf(SelectedItem);
        IsFirst = currentIndex != 0;
        IsLast = currentIndex != ItemList.Count - 1;
        FillTables();
    }
    protected override void GoLast() //Открытие последней единицы хранения в списке
    {
        SelectedItem = (ItemList.Count > 0) ? ItemList[^1] : null;
        currentIndex = ItemList.IndexOf(SelectedItem);
        IsFirst = currentIndex != 0;
        IsLast = currentIndex != ItemList.Count - 1;
        FillTables();
    }
    #endregion
    #region Инициализация
    public StorageUnitWindowVM(StorageUnit selUnit, StorageUnitPageVM vm, int selIndex, ObservableCollection<StorageUnit> items, Inventory inventory)
    {
        SelectedItem = selUnit;
        pageVM = vm;
        currentIndex = selIndex;
        curInv = inventory;
        ItemList = items;
        FillCollections();
    }
    public StorageUnitWindowVM(StorageUnitPageVM vm, ObservableCollection<StorageUnit> items, Inventory inventory)
    {
        ItemList = items;
        pageVM = vm;
        curInv = inventory;
        FillCollections();
        AddItem();
    }
    public StorageUnitWindowVM() { }
    #endregion
    private void FillTables() //Заполнение таблиц
    {
        using ArchiveBdContext dc = new();
        UnitConditions = new ObservableCollection<UnitCondition>(dc.UnitConditions.Where(u => u.Unit == SelectedItem.Id));
        RequiredWorks = new ObservableCollection<UnitRequiredWork>(dc.UnitRequiredWorks.Where(u => u.Unit == SelectedItem.Id));
        CompletedWorks = new ObservableCollection<UnitCompletedWork>(dc.UnitCompletedWorks.Where(u => u.Unit == SelectedItem.Id));
        UnitFeatures = new ObservableCollection<Feature>(dc.Features.Include(u => u.Units).Where(u => u.Units.Any(u => u.Id == SelectedItem.Id)));
    }
    protected override void FillCollections() //Заполнение списков перечислений
    {
        try
        {
            using ArchiveBdContext dc = new ArchiveBdContext();
            SecretChars = new ObservableCollection<SecretChar>(dc.SecretChars);
            Acesses = new ObservableCollection<Acess>(dc.Acesses);
            Carriers = new ObservableCollection<Carrier>(dc.Carriers);
            Categories = new ObservableCollection<UnitCategory>(dc.UnitCategories);
            CharRestricts = new ObservableCollection<CharRestrict>(dc.CharRestricts);
            if (SelectedItem != null)
            {
                FillTables();
                CheckNav(currentIndex);
            }
            else
            {
                UnitConditions = new ObservableCollection<UnitCondition>();
                ConditionsDelete = new ObservableCollection<UnitCondition>();
                RequiredWorks = new ObservableCollection<UnitRequiredWork>();
                RequiredWorksDelete = new ObservableCollection<UnitRequiredWork>();
                CompletedWorks = new ObservableCollection<UnitCompletedWork>();
                CompletedWorksDelete = new ObservableCollection<UnitCompletedWork>();
                UnitFeatures = new ObservableCollection<Feature>();
                UnitFeaturesDelete = new ObservableCollection<Feature>();
                AddItem();
            }
        }
        catch (Exception e) { ShowMessage(e.Message); }
    }
    protected override void AddItem() //Добавление новой единицы хранения
    {
        SelectedItem = new StorageUnit()
        {
            DocType = (int)curInv.DocType,
            Carrier = (int)curInv.Carrier,
            Acess = (int)curInv.Acess,
            SecretChar = curInv.SecretChar,
            CharRestrict = curInv.CharRestrict,
            Inventory = curInv.Id
        };
        CheckNav();
    }
    string q = "";
    private bool ValidateInput()
    {
        return true;
    }
    protected override void SaveItem() //Сохранение изменений
    {
        if (!ValidateInput()) return;
        UnitLog Log;
        using ArchiveBdContext dc = new();
        if (dc.StorageUnits.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal && u.Id != SelectedItem.Id))
        {
            ShowMessage("Единица хранения с таким номером уже существует");
            return;
        }
        using var transaction = dc.Database.BeginTransaction();
        try
        {
            if (!dc.StorageUnits.Contains(SelectedItem))
            {
                dc.StorageUnits.Add(SelectedItem);
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
            //Сохранение condition
            if (UnitConditions.Count != 0)
            {
                dc.UnitConditions.UpdateRange(UnitConditions.Where(uc => dc.UnitConditions.Any(u => u.Id == uc.Id)));
                foreach (var item in UnitConditions.Where(fn => !dc.UnitConditions.Any(u => u.Id == fn.Id)))
                {
                    UnitCondition newItem = new() { Condition = item.Condition, Note = item.Note, SheetsNumber = item.SheetsNumber, Unit = SelectedItem.Id };
                    dc.UnitConditions.Add(newItem);
                }
                dc.UnitConditions.RemoveRange(ConditionsDelete.Where(fnd => dc.UnitConditions.Any(u => u.Id == fnd.Id)));
            }
            //Сохранение незадокументированных периодов
            if (RequiredWorks.Count != 0)
            {
                dc.UnitRequiredWorks.UpdateRange(RequiredWorks.Where(uc => dc.UnitRequiredWorks.Any(u => u.Id == uc.Id)));
                foreach (var item in RequiredWorks.Where(rw => !dc.UnitRequiredWorks.Any(u => u.Id == rw.Id)))
                {
                    UnitRequiredWork newItem = new() { Work = item.Work, Note = item.Note, CheckDate = item.CheckDate, Unit = SelectedItem.Id };
                    dc.UnitRequiredWorks.Add(newItem);
                }
                dc.UnitRequiredWorks.RemoveRange(RequiredWorksDelete.Where(rwd => dc.UnitRequiredWorks.Any(u => u.Id == rwd.Id)));
            }
            if (CompletedWorks.Count != 0)
            {
                dc.UnitCompletedWorks.UpdateRange(CompletedWorks.Where(uc => dc.UnitCompletedWorks.Any(u => u.Id == uc.Id)));
                foreach (var item in CompletedWorks.Where(rw => !dc.UnitCompletedWorks.Any(u => u.Id == rw.Id)))
                {
                    UnitCompletedWork newItem = new() { Work = item.Work, Note = item.Note, Date = item.Date, Unit = SelectedItem.Id };
                    dc.UnitCompletedWorks.Add(newItem);
                }
                dc.UnitRequiredWorks.RemoveRange(RequiredWorksDelete.Where(rwd => dc.UnitRequiredWorks.Any(u => u.Id == rwd.Id)));
            }

            Log.Unit = SelectedItem.Id;
            dc.UnitLogs.Add(Log);
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
        Log = new ObservableCollection<UnitLog>(dc.UnitLogs.Where(u => u.Unit == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
    }
    protected override void CloseLog() //Закрытие пользовательских элементов управления
    {
        UCVisibility = Visibility.Collapsed;
        FeatureVisibility = Visibility.Collapsed;
        CompletedWorkVisibility = Visibility.Collapsed;
        RequiredWorkVisibility = Visibility.Collapsed;
        ConditionVisibility = Visibility.Collapsed;
    }
    #endregion
    #region особенности
    private void EditFeatureCommand()
    {
        using ArchiveBdContext dc = new();
        if (SelectedFeature != null)
        {
            Features = new ObservableCollection<Feature>();
            foreach (Feature f in dc.Features)
            {
                if (!UnitFeatures.Contains(f)) Features.Add(f);
            }
            Features.Add(SelectedFeature);
            FeatureVisibility = Visibility.Visible;
        }
    }
    private void AddFeatureCommand()
    {
        using ArchiveBdContext dc = new();
        Features = new ObservableCollection<Feature>();
        if (UnitFeatures != null)
            foreach (Feature f in UnitFeatures)
            {
                if (!UnitFeatures.Contains(f)) Features.Add(f);
            }
        FeatureVisibility = Visibility.Visible;
    }
    private void RemoveFeatureCommand()
    {
        if (SelectedFeature == null) return;
        UnitFeaturesDelete.Add(SelectedFeature);
        UnitFeatures.Remove(SelectedFeature);
    }
    private void SaveFeatureCommand()
    {
        var index = Features.IndexOf(Features.FirstOrDefault(u => u.Id == EditedFeature.Id));
        if (index == -1) Features.Add(EditedFeature);
        else Features[index] = EditedFeature;
        CloseLog();
    }
    #endregion
    #region RequiredWork
    private void EditRequiredWorkCommand() //+
    {
        if (SelectedRequiredWork != null)
        {
            EditedRequiredWork = new UnitRequiredWork()
            {
                Id = SelectedRequiredWork.Id,
                Note = SelectedRequiredWork.Note,
                Unit = SelectedItem.Id,
                CheckDate = SelectedRequiredWork.CheckDate,
                Work = SelectedRequiredWork.Work
            };
            CompletedWorkVisibility = Visibility.Visible;
        }
    }
    private void AddRequiredWorkCommand() //+
    {
        SelectedRequiredWork = new UnitRequiredWork();
        Action = ActionType.Add;
        EditRequiredWorkCommand();
    }
    private void RemoveRequiredWorkCommand() //+
    {
        if (SelectedRequiredWork == null) return;
        RequiredWorksDelete.Add(SelectedRequiredWork);
        RequiredWorks.Remove(SelectedRequiredWork);
    }
    private void SaveRequiredWorkCommand()
    {
        if (EditedRequiredWork != null)
            if (EditedRequiredWork.Work == null) ShowMessage("Выберите работу!");
            else
            {
                if (Action == ActionType.Add) { RequiredWorks.Add(EditedRequiredWork); }
                else RequiredWorks[RequiredWorks.IndexOf(RequiredWorks.FirstOrDefault(u => u.Id == EditedRequiredWork.Id))] = EditedRequiredWork;
                CloseLog();
            }
        Action = ActionType.Change;
    }
    #endregion
    #region CompletedWork
    private void EditCompletedWorkCommand()
    {
        if (SelectedCompletedWork != null)
        {
            EditedCompletedWork = new UnitCompletedWork()
            {
                Id = SelectedCompletedWork.Id,
                Note = SelectedCompletedWork.Note,
                Unit = SelectedItem.Id,
                Date = SelectedCompletedWork.Date,
                Work = SelectedCompletedWork.Work
            };
            CompletedWorkVisibility = Visibility.Visible;
        }
    }
    private void AddCompletedWorkCommand() //+
    {
        SelectedCompletedWork = new UnitCompletedWork();
        Action = ActionType.Add;
        EditCompletedWorkCommand();
    }
    private void RemoveCompletedWorkCommand() //+
    {
        if (SelectedCompletedWork == null) return;
        CompletedWorksDelete.Add(SelectedCompletedWork);
        CompletedWorks.Remove(SelectedCompletedWork);
    }
    private void SaveCompletedWorkCommand()
    {
        if (EditedCompletedWork != null)
            if (EditedCompletedWork.Work == null) ShowMessage("Выберите работу!");
            else
            {
                if (Action == ActionType.Add) { CompletedWorks.Add(EditedCompletedWork); }
                else CompletedWorks[CompletedWorks.IndexOf(CompletedWorks.FirstOrDefault(u => u.Id == EditedCompletedWork.Id))] = EditedCompletedWork;
                CloseLog();
            }
        Action = ActionType.Change;
    }
    #endregion
    #region Condition
    private void EditConditionCommand() //+
    {
        if (SelectedCondition != null)
        {
            EditedCondition = new UnitCondition()
            {
                Id = SelectedCondition.Id,
                Condition = SelectedCondition.Condition,
                Note = SelectedCondition.Note,
                SheetsNumber = SelectedCondition.SheetsNumber,
                Unit = SelectedItem.Id
            };
            ConditionVisibility = Visibility.Visible;
        }
    }
    private void AddConditionCommand() //Открытие юзерконтрола по добавлению состояния
    {
        SelectedCondition = new UnitCondition();
        Action = ActionType.Add;
        EditConditionCommand();
    }
    private void RemoveConditionCommand() //+
    {
        if (SelectedCondition == null) return;
        UnitConditions.Add(SelectedCondition);
        ConditionsDelete.Remove(SelectedCondition);
    }
    private void SaveConditionCommand() //Сохранение состояния
    {
        if (EditedCondition != null)
            if (EditedCondition.SheetsNumber == null) ShowMessage("Выберите работу!");
            else
            {
                if (Action == ActionType.Add) { UnitConditions.Add(EditedCondition); }
                else UnitConditions[UnitConditions.IndexOf(UnitConditions.FirstOrDefault(u => u.Id == EditedCondition.Id))] = EditedCondition;
                CloseLog();
            }
        Action = ActionType.Change;
    }
    #endregion
}
