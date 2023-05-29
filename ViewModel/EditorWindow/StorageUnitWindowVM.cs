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
    private ObservableCollection<Model.Condition> _conditionsDelete, _unitConditions, _conditions;
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
    public ObservableCollection<UnitCompletedWork> CompletedWorks { get => CompletedWorks; set { CompletedWorks = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitCompletedWork> CompletedWorksDelete { get => _completedWorksDelete; set { _completedWorksDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<Work> Works { get => _works; set { _works = value; OnPropertyChanged(); } }
    public ObservableCollection<Feature> Conditions { get => _unitFeaturesDelete; set { _unitFeaturesDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<Feature> ConditionsDelete { get => _unitFeaturesDelete; set { _unitFeaturesDelete = value; OnPropertyChanged(); } }
    public ObservableCollection<Acess> Acesses { get; set; }
    public ObservableCollection<SecretChar> SecretChars { get; set; }
    public ObservableCollection<Carrier> Carriers { get; set; }
    public ObservableCollection<UnitCategory> Categories { get; set; }
    public ObservableCollection<CharRestrict> CharRestricts { get; set; }
    public ObservableCollection<StorageUnit> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
    public ObservableCollection<UnitLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
    public StorageUnit SelectedItem { get => _selectedUnit; set { _selectedUnit = value; OnPropertyChanged(); } }
    private Visibility _featureVisibility, _completedWorkVisibility, _conditionVisibility, _requiredWorkVisibility = Visibility.Collapsed;
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
    private void CheckNav() { IsFirst = false; IsLast = false; }
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
    protected override void GoFirst()
    {
        SelectedItem = (ItemList.Count > 0) ? ItemList[0] : null;
        currentIndex = ItemList.IndexOf(SelectedItem);
        IsFirst = currentIndex != 0;
        IsLast = currentIndex != ItemList.Count - 1;
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
    private void FillTables()
    {
        using ArchiveBdContext dc = new();
        UnitFeatures = new ObservableCollection<Feature>(dc.Features.Include(u => u.Units).Where(u => u.Units.Any(u => u.Id == SelectedItem.Id)));
        UnitFeaturesDelete = new ObservableCollection<Feature>();
    }
    protected override void FillCollections()
    {
        try
        {
            using ArchiveBdContext dc = new ArchiveBdContext();
            SecretChars = new ObservableCollection<SecretChar>(dc.SecretChars);
            Acesses = new ObservableCollection<Acess>(dc.Acesses);
            Carriers = new ObservableCollection<Carrier>(dc.Carriers);
            Categories = new ObservableCollection<UnitCategory>(dc.UnitCategories);
            CharRestricts = new ObservableCollection<CharRestrict>(dc.CharRestricts);
            if (SelectedItem.Number != null) { CheckNav(currentIndex); }
            else { AddItem(); }
        }
        catch (Exception e) { ShowMessage(e.Message); }
    }
    protected override void AddItem()
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
    protected override void SaveItem()
    {
        try
        {
            using ArchiveBdContext dc = new();
            if (!dc.StorageUnits.Any(u => u.Id == SelectedItem.Id))
                dc.StorageUnits.Add(SelectedItem);
            else dc.StorageUnits.Update(SelectedItem);
            dc.SaveChanges();
            pageVM.UpdateData();
        }
        catch (Exception ex) { ShowMessage(ex.ToString()); }
    }
    #region Протокол
    protected override void OpenLog() //Открытие протокола
    {
        using ArchiveBdContext dc = new();
        UCVisibility = Visibility.Visible;
        Log = new ObservableCollection<UnitLog>(dc.UnitLogs.Where(u => u.Unit == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
    }
    protected override void CloseLog()
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
            Features = new ObservableCollection<Feature>(dc.Features);
            Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
            Features.Add(SelectedFeature);
            FeatureVisibility = Visibility.Visible;
        }
    }
    private void AddFeatureCommand()
    {
        using ArchiveBdContext dc = new();
        Features = new ObservableCollection<Feature>(dc.Features);
        Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
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
    private void EditRequiredWorkCommand()
    {
        using ArchiveBdContext dc = new();
        if (SelectedFeature != null)
        {
            Features = new ObservableCollection<Feature>(dc.Features);
            Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
            Features.Add(SelectedFeature);
            FeatureVisibility = Visibility.Visible;
        }
    }
    private void AddRequiredWorkCommand()
    {
        using ArchiveBdContext dc = new();
        Features = new ObservableCollection<Feature>(dc.Features);
        Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
        FeatureVisibility = Visibility.Visible;
    }
    private void RemoveRequiredWorkCommand() //+
    {
        if (SelectedRequiredWork == null) return;
        RequiredWorksDelete.Add(SelectedRequiredWork);
        RequiredWorks.Remove(SelectedRequiredWork);
    }
    private void SaveRequiredWorkCommand()
    {
        var index = Features.IndexOf(Features.FirstOrDefault(u => u.Id == EditedFeature.Id));
        if (index == -1) Features.Add(EditedFeature);
        else Features[index] = EditedFeature;
        CloseLog();
    }
    #endregion
    #region CompletedWork
    private void EditCompletedWorkCommand()
    {
        using ArchiveBdContext dc = new();
        if (SelectedFeature != null)
        {
            Features = new ObservableCollection<Feature>(dc.Features);
            Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
            Features.Add(SelectedFeature);
            FeatureVisibility = Visibility.Visible;
        }
    }
    private void AddCompletedWorkCommand()
    {
        using ArchiveBdContext dc = new();
        Features = new ObservableCollection<Feature>(dc.Features);
        Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
        FeatureVisibility = Visibility.Visible;
    }
    private void RemoveCompletedWorkCommand() //+
    {
        if (SelectedCompletedWork == null) return;
        CompletedWorksDelete.Add(SelectedCompletedWork);
        CompletedWorks.Remove(SelectedCompletedWork);
    }
    private void SaveCompletedWorkCommand()
    {
        var index = Features.IndexOf(Features.FirstOrDefault(u => u.Id == EditedFeature.Id));
        if (index == -1) Features.Add(EditedFeature);
        else Features[index] = EditedFeature;
        CloseLog();
    }
    #endregion
    #region Condition
    private void EditConditionCommand()
    {
        using ArchiveBdContext dc = new();
        if (SelectedFeature != null)
        {
            Features = new ObservableCollection<Feature>(dc.Features);
            Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
            Features.Add(SelectedFeature);
            FeatureVisibility = Visibility.Visible;
        }
    }
    private void AddConditionCommand()
    {
        using ArchiveBdContext dc = new();
        Features = new ObservableCollection<Feature>(dc.Features);
        Features = new ObservableCollection<Feature>(Features.AsEnumerable().Except(UnitFeatures));
        FeatureVisibility = Visibility.Visible;
    }
    private void RemoveConditionCommand() //+
    {
        if (SelectedCondition == null) return;
        Conditions.Add(SelectedFeature);
        ConditionsDelete.Remove(SelectedFeature);
    }
    private void SaveConditionCommand()
    {
        var index = Features.IndexOf(Features.FirstOrDefault(u => u.Id == EditedFeature.Id));
        if (index == -1) Features.Add(EditedFeature);
        else Features[index] = EditedFeature;
        CloseLog();
    }
    #endregion
}
