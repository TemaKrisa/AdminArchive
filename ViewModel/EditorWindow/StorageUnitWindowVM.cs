using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
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
    private StorageUnit _selectedUnit;
    private Inventory curInv;
    private ObservableCollection<Feature> _unitFeatures, _features;
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
    private void CheckNav(int index) //Определение доступности кнопок навигации
    {
        if (ItemList.Count == 0) { IsFirst = false; IsLast = false; }
        else { IsFirst = index != 0; IsLast = index != ItemList.Count - 1; }
    }
    protected override void GoNext() //Открытие следующей единицы хранения в списке
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
    protected override void GoPrev() //Открытие прошлой единицы хранения в списке
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
    public StorageUnitWindowVM(StorageUnit selUnit, dynamic vm, int selIndex, ObservableCollection<StorageUnit> items, Inventory inventory)
    {
        SelectedItem = selUnit;
        pageVM = vm;
        currentIndex = selIndex;
        curInv = inventory;
        ItemList = items;
        FillCollections();
    }
    public StorageUnitWindowVM(dynamic vm, ObservableCollection<StorageUnit> items, Inventory inventory)
    {
        ItemList = items;
        pageVM = vm;
        curInv = inventory;
        FillCollections();
    }
    public StorageUnitWindowVM() { }
    #endregion
    private void FillTables() //Заполнение таблиц
    {
        using ArchiveBdContext dc = new();
        UnitConditions = new ObservableCollection<UnitCondition>(dc.UnitConditions.Include(u => u.ConditionNavigation).Where(u => u.Unit == SelectedItem.Id));
        RequiredWorks = new ObservableCollection<UnitRequiredWork>(dc.UnitRequiredWorks.Include(u => u.WorkNavigation).Where(u => u.Unit == SelectedItem.Id));
        CompletedWorks = new ObservableCollection<UnitCompletedWork>(dc.UnitCompletedWorks.Include(u => u.WorkNavigation).Where(u => u.Unit == SelectedItem.Id));
        UnitFeatures = new ObservableCollection<Feature>(dc.Features.Include(u => u.Units).Where(u => u.Units.Any(u => u.Id == SelectedItem.Id)));
    }
    protected override void FillCollections() //Заполнение списков перечислений
    {
        try
        {
            using ArchiveBdContext dc = new ArchiveBdContext();
            SecretChars = new ObservableCollection<SecretChar>(dc.SecretChars);
            SecretChars.Insert(0, new SecretChar());
            Acesses = new ObservableCollection<Acess>(dc.Acesses);
            Carriers = new ObservableCollection<Carrier>(dc.Carriers);
            Categories = new ObservableCollection<UnitCategory>(dc.UnitCategories);
            CharRestricts = new ObservableCollection<CharRestrict>(dc.CharRestricts);
            Conditions = new ObservableCollection<Model.Condition>(dc.Conditions);
            Works = new ObservableCollection<Work>(dc.Works);
            CompletedWorksDelete = new ObservableCollection<UnitCompletedWork>();
            ConditionsDelete = new ObservableCollection<UnitCondition>();
            RequiredWorksDelete = new ObservableCollection<UnitRequiredWork>();
            if (SelectedItem != null) { CheckNav(currentIndex); }
            else { AddItem(); }
            FillTables();
        }
        catch (Exception e) { ShowMessage(e.Message); }
    }
    protected override void AddItem() //Добавление новой единицы хранения
    {
        SelectedItem = new StorageUnit() { DocType = (int)curInv.DocType, Carrier = (int)curInv.Carrier, Acess = (int)curInv.Acess, SecretChar = curInv.SecretChar, CharRestrict = curInv.CharRestrict, Inventory = curInv.Id };
        CheckNav();
    }
    private bool ValidateInput(ArchiveBdContext dc)
    {
        if (SelectedItem.Number == 0) { ShowMessage("Введите номер!"); return false; }
        else if (string.IsNullOrWhiteSpace(SelectedItem.Title)) { ShowMessage("Введите наименование!"); return false; }
        else if (SelectedItem.Carrier == 0) { ShowMessage("Выберите носитель!"); return false; }
        else if (SelectedItem.Category == 0) { ShowMessage("Выберите категорию!"); return false; }
        else if (SelectedItem.Acess == 0) { ShowMessage("Выберите доступ!"); return false; }
        else if (string.IsNullOrWhiteSpace(SelectedItem.Date)) { ShowMessage("Введите точные даты!"); return false; }
        else if (SelectedItem.StartDate <= 1000) { ShowMessage("Дата начала не может быть меньше 1000!"); return false; }
        else if (SelectedItem.EndDate <= 1000) { ShowMessage("Дата конца не может быть меньше 1000!"); return false; }
        else if (SelectedItem.EndDate < SelectedItem.StartDate) { ShowMessage("Начальная дата не может превышать конечную!"); return false; }
        else if (dc.StorageUnits.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal && u.Inventory == SelectedItem.Inventory && u.Id != SelectedItem.Id))
        { ShowMessage("Единица хранения с таким номером уже существует!"); return false; }
        else if (SelectedItem.SecretChar == 0) SelectedItem.SecretChar = null;
        return true;
    }
    protected override void SaveItem() //Сохранение изменений
    {
        using ArchiveBdContext dc = new();
        if (!ValidateInput(dc)) return;
        var q = string.Empty;
        UnitLog Log;
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
            UpdateAndAddItems(dc.UnitConditions, UnitConditions, ConditionsDelete, (item) => new UnitCondition { Condition = item.Condition, Note = item.Note, SheetsNumber = item.SheetsNumber, Unit = SelectedItem.Id });
            UpdateAndAddItems(dc.UnitRequiredWorks, RequiredWorks, RequiredWorksDelete, (item) => new UnitRequiredWork { Work = item.Work, Note = item.Note, CheckDate = item.CheckDate, Unit = SelectedItem.Id });
            UpdateAndAddItems(dc.UnitCompletedWorks, CompletedWorks, CompletedWorksDelete, (item) => new UnitCompletedWork { Work = item.Work, Note = item.Note, Date = item.Date, Unit = SelectedItem.Id });
            var idParam = new SqlParameter("@id", SelectedItem.Id);
            dc.Database.ExecuteSqlRaw($"Delete From StorageUnitFeatures Where Unit = @id", idParam);
            dc.SaveChanges();
            foreach (var feature in UnitFeatures)
            {
                var unitFeature = new Feature { Name = feature.Name, Id = feature.Id };
                SelectedItem.Features.Add(unitFeature);
            }
            Log.Unit = SelectedItem.Id;
            dc.UnitLogs.Add(Log);
            dc.SaveChanges();
            transaction.Commit();
            pageVM.UpdateData();
            if (q == "Add") { ItemList = pageVM.StorageUnits; CheckNav(pageVM.StorageUnits.IndexOf(SelectedItem)); } //Добавление элемента в коллекцию навигации
        }
        catch (Exception ex) { transaction.Rollback(); ShowMessage(ex.ToString()); }
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
        Index = UnitFeatures.IndexOf(SelectedFeature);
        EditedFeature = new Feature() { Id = SelectedFeature.Id, Name = SelectedFeature.Name };
        Features = new ObservableCollection<Feature>();
        var allFeatures = new ObservableCollection<Feature>(dc.Features.AsNoTracking());
        if (UnitFeatures != null)
        {
            foreach (Feature f in allFeatures)
                if (!UnitFeatures.Any(uf => uf.Id == f.Id)) Features.Add(f);
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
                if (Action == ActionType.Add) { UnitFeatures.Add(EditedFeature); }
                else UnitFeatures[Index] = EditedFeature;
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
        UnitFeatures.Remove(SelectedFeature);
    }

    #endregion
    #region RequiredWork
    private void AddRequiredWorkCommand()
    {
        SelectedRequiredWork = new UnitRequiredWork();
        EditRequiredWorkCommand();
        Action = ActionType.Add;
    }
    private void EditRequiredWorkCommand()
    {
        Action = ActionType.Change;
        EditedRequiredWork = new UnitRequiredWork() { Id = SelectedRequiredWork.Id, Note = SelectedRequiredWork.Note, Unit = SelectedRequiredWork.Id, CheckDate = SelectedRequiredWork.CheckDate, Work = SelectedRequiredWork.Work };
        RequiredWorkVisibility = Visibility.Visible;
    }
    private void SaveRequiredWorkCommand()
    {
        if (EditedRequiredWork != null)
            if (EditedRequiredWork.Work == 0) ShowMessage("Выберите работу!");
            else if (EditedRequiredWork.CheckDate == null) ShowMessage("Выберите дату!");
            else
            {
                EditedRequiredWork.WorkNavigation = Works.First(u => u.Id == EditedRequiredWork.Work);
                if (Action == ActionType.Add) { RequiredWorks.Add(EditedRequiredWork); }
                else RequiredWorks[Index] = EditedRequiredWork;
                CloseLog();
                Action = ActionType.Change;
            }
    }
    private void RemoveRequiredWorkCommand() //+
    {
        if (SelectedRequiredWork == null) return;
        RequiredWorksDelete.Add(SelectedRequiredWork);
        RequiredWorks.Remove(SelectedRequiredWork);
    }
    #endregion
    #region CompletedWork
    private void AddCompletedWorkCommand() //+
    {
        SelectedCompletedWork = new UnitCompletedWork();
        EditCompletedWorkCommand();
        Action = ActionType.Add;
    }
    private void EditCompletedWorkCommand()
    {
        Action = ActionType.Change;
        EditedCompletedWork = new UnitCompletedWork() { Id = SelectedCompletedWork.Id, Note = SelectedCompletedWork.Note, Unit = SelectedItem.Id, Date = SelectedCompletedWork.Date, Work = SelectedCompletedWork.Work };
        CompletedWorkVisibility = Visibility.Visible;
    }
    private void SaveCompletedWorkCommand()
    {
        if (EditedCompletedWork != null)
            if (EditedCompletedWork.Work == 0) ShowMessage("Выберите работу!");
            else if (EditedCompletedWork.Date == null) ShowMessage("Выберите дату!");
            else
            {
                EditedCompletedWork.WorkNavigation = Works.First(u => u.Id == EditedCompletedWork.Work);
                if (Action == ActionType.Add) { CompletedWorks.Add(EditedCompletedWork); }
                else CompletedWorks[Index] = EditedCompletedWork;
                CloseLog();
                Action = ActionType.Change;
            }
    }
    private void RemoveCompletedWorkCommand() //+
    {
        if (SelectedCompletedWork == null) return;
        CompletedWorksDelete.Add(SelectedCompletedWork);
        CompletedWorks.Remove(SelectedCompletedWork);
    }
    #endregion
    #region Condition
    private void EditConditionCommand() //+
    {
        Action = ActionType.Change;
        Index = UnitConditions.IndexOf(SelectedCondition);
        EditedCondition = new UnitCondition() { Id = SelectedCondition.Id, Condition = SelectedCondition.Condition, Note = SelectedCondition.Note, SheetsNumber = SelectedCondition.SheetsNumber, Unit = SelectedItem.Id };
        ConditionVisibility = Visibility.Visible;
    }
    private void AddConditionCommand() //Открытие юзерконтрола по добавлению состояния
    {
        SelectedCondition = new UnitCondition();
        EditConditionCommand();
        Action = ActionType.Add;
    }
    private void RemoveConditionCommand() //Удаление состояния
    {
        if (SelectedCondition == null) return;
        UnitConditions.Add(SelectedCondition);
        ConditionsDelete.Remove(SelectedCondition);
    }
    private void SaveConditionCommand() //Сохранение состояния
    {
        if (EditedCondition != null)
        {
            if (String.IsNullOrWhiteSpace(EditedCondition.SheetsNumber)) ShowMessage("Выберите номера строк!");
            else if (EditedCondition.Condition == 0) { ShowMessage("Выберите состояние!"); }
            else
            {
                EditedCondition.ConditionNavigation = Conditions.First(u => u.Id == EditedCondition.Condition);
                if (Action == ActionType.Add) { UnitConditions.Add(EditedCondition); }
                else UnitConditions[Index] = EditedCondition;
                CloseLog();
                Action = ActionType.Change;
            }
        }
    }
    #endregion
}