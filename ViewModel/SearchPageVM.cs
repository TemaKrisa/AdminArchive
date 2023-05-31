using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
internal class SearchPageVM : BaseViewModel
{
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<UnitCategory> UnitCategories { get; set; }
    public ObservableCollection<Authenticity> Authenticities { get; set; }
    public ObservableCollection<DocType> DocTypes { get; set; }
    //свойства поиска фонда
    private string _fondName, _fondShortName, _fondStartDate, _fondEndDate;
    private int _fondCategory = -1;
    public string? FondName { get => _fondName; set { _fondName = value; OnPropertyChanged(); } }
    public string? FondShortName { get => _fondShortName; set { _fondShortName = value; OnPropertyChanged(); } }
    public string? FondStartDate { get => _fondStartDate; set { _fondStartDate = value; OnPropertyChanged(); } }
    public string? FondEndDate { get => _fondEndDate; set { _fondEndDate = value; OnPropertyChanged(); } }
    public int FondCategory { get => _fondCategory; set { _fondCategory = value; OnPropertyChanged(); } }
    //Свойства поиска описи
    private string _inventoryName, _inventoryStartDate, _inventoryEndDate;
    private int _inventoryCategory = -1;
    public string InventoryName { get => _inventoryName; set { _inventoryName = value; OnPropertyChanged(); } }
    public string InventoryStartDate { get => _inventoryStartDate; set { _inventoryStartDate = value; OnPropertyChanged(); } }
    public string InventoryEndDate { get => _inventoryEndDate; set { _inventoryEndDate = value; OnPropertyChanged(); } }
    public int InventoryCategory { get => _inventoryCategory; set { _inventoryCategory = value; OnPropertyChanged(); } }
    //Свойства единицы хранения
    private string _unitName, _unitStartDate, _unitEndDate;
    private int _unitCategory = -1;
    public string? UnitName { get => _unitName; set { _unitName = value; OnPropertyChanged(); } }
    public string? UnitStartDate { get => _unitStartDate; set { _unitStartDate = value; OnPropertyChanged(); } }
    public string? UnitEndDate { get => _unitEndDate; set { _unitEndDate = value; OnPropertyChanged(); } }
    public int UnitCategory { get => _unitCategory; set { _unitCategory = value; OnPropertyChanged(); } }
    //Свойства документа
    private string _docTitle;
    private int _docAu = -1, _docType = -1;
    private DateTime _docDate;
    public string? DocTitle { get => _docTitle; set { _docTitle = value; OnPropertyChanged(); } }
    public int DocAu { get => _docAu; set { _docAu = value; OnPropertyChanged(); } }
    public int DocType { get => _docType; set { _docType = value; OnPropertyChanged(); } }
    public DateTime DocDate { get => _docDate; set { _docDate = value; OnPropertyChanged(); } }
    //Коллекции Datagrid
    private ObservableCollection<Fond> _fonds;
    private ObservableCollection<Inventory> _inventories;
    private ObservableCollection<StorageUnit> _units;
    private ObservableCollection<Document> _documents;
    public ObservableCollection<Fond> Fonds { get => _fonds; set { _fonds = value; OnPropertyChanged(); } }
    public ObservableCollection<Inventory> Inventories { get => _inventories; set { _inventories = value; OnPropertyChanged(); } }
    public ObservableCollection<StorageUnit> Units { get => _units; set { _units = value; OnPropertyChanged(); } }
    public ObservableCollection<Document> Documents { get => _documents; set { _documents = value; OnPropertyChanged(); } }
    //Свойства выбранного элемента Datagrid
    private Fond _selectedFond;
    private StorageUnit _selectedUnit;
    private Inventory _selectedInventory;
    private Document _selectedDocument;
    public Fond SelectedFond { get => _selectedFond; set { _selectedFond = value; } }// выбранный фонд
    public Inventory SelectedInventory { get => _selectedInventory; set { _selectedInventory = value; } }
    public StorageUnit SelectedUnit { get => _selectedUnit; set { _selectedUnit = value; } }
    public Document SelectedDocument { get => _selectedDocument; set { _selectedDocument = value; } }
    //Комманды
    public ICommand OpenSearch => new RelayCommand<string>(OpenSearchCommand);
    public ICommand Search => new RelayCommand<string>(SearchCommand);
    public ICommand ResetSearch => new RelayCommand<string>(ResetSearchCommand);
    public ICommand OpenItem => new RelayCommand<string>(OpenItemCommand);
    public ICommand EditItem => new RelayCommand<string>(EditItemCommand);
    public ICommand CloseSearch => new RelayCommand<string>(CloseSearchCommand);
    public ICommand Update => new RelayCommand<string>(UpdateData);
    //Видимость юзерконтролов
    private Visibility _fondVisibility = Visibility.Collapsed, _inventoryVisibility = Visibility.Collapsed, _unitVisibility = Visibility.Collapsed, _documentVisibility = Visibility.Collapsed;
    public Visibility FondVisibility { get => _fondVisibility; set { _fondVisibility = value; OnPropertyChanged(); } }
    public Visibility UnitVisibility { get => _unitVisibility; set { _unitVisibility = value; OnPropertyChanged(); } }
    public Visibility InventoryVisibility { get => _inventoryVisibility; set { _inventoryVisibility = value; OnPropertyChanged(); } }
    public Visibility DocumentVisibility { get => _documentVisibility; set { _documentVisibility = value; OnPropertyChanged(); } }

    public void UpdateData()
    {
        Fonds = SearchClass.SearchFond(FondName, FondShortName, FondStartDate, FondEndDate, FondCategory);
        Inventories = SearchClass.SearchInventory(InventoryName, InventoryStartDate, InventoryEndDate, InventoryCategory);
        Units = SearchClass.SearchUnit(UnitName, UnitStartDate, UnitEndDate, UnitCategory);
        Documents = SearchClass.SearchDocument(DocTitle, DocAu, DocType, DocDate);
    }
    public void LoadData() //Загрузка списков 
    {
        using ArchiveBdContext dc = new(); // создаем контекст базы данных
        Categories = new ObservableCollection<Category>(dc.Categories);
        Categories.Insert(0, new Category { Name = "Все категории", Id = -1 });
        UnitCategories = new ObservableCollection<UnitCategory>(dc.UnitCategories);
        UnitCategories.Insert(0, new UnitCategory { Name = "Все категории", Id = -1 });
        Authenticities = new ObservableCollection<Authenticity>(dc.Authenticities);
        Authenticities.Insert(0, new Authenticity { Name = "Все категории", Id = -1 });
        DocTypes = new ObservableCollection<DocType>(dc.DocTypes);
        DocTypes.Insert(0, new DocType { Name = "Все категории", Id = -1 });
        UpdateData();
    }
    public void UpdateData(string type)
    {
        switch (type) //Обновление определенной коллекции
        {
            case "Fond": Fonds = SearchClass.SearchFond(FondName, FondShortName, FondStartDate, FondEndDate, FondCategory); break;
            case "Inventory": Inventories = SearchClass.SearchInventory(InventoryName, InventoryStartDate, InventoryEndDate, InventoryCategory); break;
            case "Unit": Units = SearchClass.SearchUnit(UnitName, UnitStartDate, UnitEndDate, UnitCategory); break;
            case "Document": Documents = SearchClass.SearchDocument(DocTitle, DocAu, DocType, DocDate); break;
        }
    }
    public SearchPageVM()
    {
        LoadData();
    }
    protected void CloseSearchCommand(string command)
    {
        switch (command)
        {
            case "Fond": FondVisibility = Visibility.Collapsed; break;
            case "Inventory": InventoryVisibility = Visibility.Collapsed; break;
            case "Unit": UnitVisibility = Visibility.Collapsed; break;
            case "Document": DocumentVisibility = Visibility.Collapsed; break;
        }
    }
    protected void OpenSearchCommand(string command)
    {
        switch (command)
        {
            case "Fond": FondVisibility = Visibility.Visible; break;
            case "Inventory": InventoryVisibility = Visibility.Visible; break;
            case "Unit": UnitVisibility = Visibility.Visible; break;
            case "Document": DocumentVisibility = Visibility.Visible; break;
        }
    }
    protected void SearchCommand(string command)
    {
        switch (command)
        {
            case "Fond": FondVisibility = Visibility.Collapsed; UpdateData(command); break;
            case "Inventory": InventoryVisibility = Visibility.Collapsed; UpdateData(command); break;
            case "Unit": UnitVisibility = Visibility.Collapsed; UpdateData(command); break;
            case "Document": DocumentVisibility = Visibility.Collapsed; UpdateData(command); break;
        }
    }
    protected void ResetSearchCommand(string command)
    {
        switch (command)//Сбросс свойств поиска и вызов обновления данных
        {
            case "Fond": FondName = null; FondShortName = null; FondStartDate = null; FondEndDate = null; FondCategory = -1; break;
            case "Inventory": InventoryName = null; InventoryCategory = -1; InventoryEndDate = null; InventoryStartDate = null; break;
            case "Unit": UnitCategory = -1; UnitEndDate = null; UnitStartDate = null; UnitName = null; break;
            case "Document": DocAu = -1; DocDate = DateTime.MinValue; DocTitle = null; DocType = -1; break;
        }
        UpdateData(command);
    }
    protected void OpenItemCommand(string command)
    {
        using ArchiveBdContext dc = new();
        switch (command)
        {
            case "Fond":
                if (SelectedFond != null) // если выбран элемент
                {
                    InventoryPageVM vm = new(SelectedFond); // создаем ViewModel для окна редактирования описи
                    InventoryPage v = new() { DataContext = vm }; // создаем окно редактирования описи
                    Setting.mainFrame?.Navigate(v); // переходим в окно редактирования описи
                }
                break;
            case "Inventory":
                if (SelectedInventory != null)
                {

                    Fond curFond = dc.Fonds.Where(u => u.Id == SelectedInventory.Id).Single();
                    StorageUnitPageVM vm = new(SelectedInventory, curFond);
                    StorageUnitPage v = new() { DataContext = vm };
                    Setting.mainFrame.Navigate(v);
                }
                break;
            case "Unit":
                if (SelectedUnit != null)
                {
                    Inventory curInv = dc.Inventories.Where(u => u.Id == SelectedUnit.Id).Single();
                    Fond curFond = dc.Fonds.Where(u => u.Id == curInv.Fond).Single();
                    DocumentPageVM vm = new(SelectedUnit, curFond, curInv);
                    DocumentPage v = new() { DataContext = vm };
                    Setting.mainFrame?.Navigate(v);
                }
                break;
        }
    }
    protected void EditItemCommand(string command) //изменение
    {
        int index;
        using ArchiveBdContext dc = new();
        switch (command)
        {
            case "Fond":
                index = Fonds.IndexOf(SelectedFond); // получаем индекс выбранного фонда
                FundWindow fundWindow = new(); // создаем окно редактирования фонда
                FundWindowVM FondVM = new((SelectedFond as Fond), this, index, Fonds); // создаем ViewModel для окна редактирования фонда
                fundWindow.DataContext = FondVM; // устанавливаем ViewModel для окна редактирования фонда
                fundWindow.ShowDialog(); // открываем окно редактирования фонда
                break;
            case "Inventory":
                Fond curFond = dc.Fonds.Where(u => u.Id == SelectedInventory.Id).Single();
                index = Inventories.IndexOf(SelectedInventory);
                InventoryWindow inventoryWindow = new();
                InventoryWindowVM inventoryVM = new((SelectedInventory as Inventory), this, index, Inventories, curFond);
                inventoryWindow.DataContext = inventoryVM;
                inventoryWindow.ShowDialog();
                break;
            case "Unit":

                break;
            case "Document":

                break;
        }
    }
}
