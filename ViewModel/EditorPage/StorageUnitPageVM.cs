using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
namespace AdminArchive.ViewModel;
// Частичный класс ViewModel страницы хранения, наследующийся от базового класса PageBaseVM
partial class StorageUnitPageVM : PageBaseVM
{
    // Объявление приватных переменных
    private ObservableCollection<StorageUnit> _storageUnits; // Коллекция хранилищных единиц
    private StorageUnit _selectedItem; // Выбранная хранилищная единица
    private Inventory curInv; // Инвентаризация, к которой относится хранилищная единица
    private Fond curFond; // Фонд, к которому относится хранилищная единица
    private string? _unitName, _unitStartDate, _unitEndDate; // Название, дата начала и дата окончания хранилищной единиы
    private int _unitCategory = -1; // Категория хранилищной единицы
    // Объявление публичных свойств
    public ObservableCollection<StorageUnit> StorageUnits { get => _storageUnits; set { _storageUnits = value; OnPropertyChanged(); } } // Коллекция хранилищных единиц
    public StorageUnit SelectedItem { get => _selectedItem; set => _selectedItem = value; } // Выбранная хранилищная единица
    public ObservableCollection<UnitCategory> UnitCategories { get; set; } // Коллекция категорий хранилищных единиц
    public string? UnitName { get => _unitName; set { _unitName = value; OnPropertyChanged(); } } // Название хранилищной единицы
    public string? UnitStartDate { get => _unitStartDate; set { _unitStartDate = value; OnPropertyChanged(); } } // Дата начала хранилищной единицы
    public string? UnitEndDate { get => _unitEndDate; set { _unitEndDate = value; OnPropertyChanged(); } } // Дата окончания хранилищной единицы
    public int UnitCategory { get => _unitCategory; set { _unitCategory = value; OnPropertyChanged(); } } // Категория хранилищной единицы
    // Конструктор класса, принимающий инвентаризацию и фонд, к которым относится хранилиная единица
    public StorageUnitPageVM(Inventory inv, Fond fond)
    {
        curFond = fond; // Присвоение фонда
        curInv = inv; // Присвоение инвентаризации
        LoadData(); // Загрузка данных
    }
    // Конструктор класса по умолчанию
    public StorageUnitPageVM() { }
    // Метод загрузки данных
    private void LoadData()
    {
        using ArchiveBdContext dc = new(); // Создание контекста базы данных
        UnitCategories = new ObservableCollection<UnitCategory>(dc.UnitCategories); // Инициализация коллекции категорий хранилищных единиц
        UnitCategories.Insert(0, new UnitCategory { Name = "Все категории", Id = -1 }); // Добавление "Все категории" в коллекцию категорий
        UpdateData(); // Обновление данных
    }
    // Метод обновления данных
    public override void UpdateData()
    {
        StorageUnits = SearchClass.SearchUnit(UnitName, UnitStartDate, UnitEndDate, UnitCategory, curInv); // Обновление коллекции хранилищных единиц
    }
    // Метод перехода на предыдущую страницу
    protected override void GoBack()
    {
        InventoryPageVM vm = new(curFond); // Создание ViewModel страницы инвентаризации
        InventoryPage v = new() { DataContext = vm }; // Создание страницы инвентаризации
        Setting.mainFrame?.Navigate(v); // Переход на страницу инвентаризации
    }
    // Метод редактирования выбранной хранилищной единицы
    protected override void EditItem()
    {
        int index = StorageUnits.IndexOf(SelectedItem); // Получение индекса выбранной хранилищной единицы
        StorageUnitWindow Editor = new(); // Создание окна редактирования хранилищной единицы
        StorageUnitWindowVM vm = new(SelectedItem, this, index, StorageUnits, curInv); // Создание ViewModel окна редактирования хранилищной единицы
        Editor.DataContext = vm; // Присвоение ViewModel окну редактирования хранилищной единицы
        Editor.ShowDialog(); // Открытие окна редактирования хранилищной единицы
    }
    // Метод добавления хранилищной единицы
    protected override void AddItem()
    {
        StorageUnitWindowVM vm = new(this, StorageUnits, curInv); // Создание ViewModel окна добавления хранилищной единицы
        var newWindow = new StorageUnitWindow { DataContext = vm }; // Создание окна добавления хранилищной единицы
        newWindow.ShowDialog(); // Открытие окна добавления хранилищной единицы
    }
    // Метод открытия выбранной хранилищной единицы
    protected override void OpenItem()
    {
        if (SelectedItem != null)
        {
            try
            {
                DocumentPageVM vm = new(SelectedItem, curFond, curInv); // Создание ViewModel страницы документов
                DocumentPage v = new() { DataContext = vm }; // Создание страницы документов
                Setting.mainFrame?.Navigate(v); // Переход на страницу документов
            }
            catch (Exception ex) { ShowMessage(ex.ToString()); } // Обработка ошибки
        }
    }
    // Метод сброса поиска
    protected override void ResetSearch() { UpdateData(); }
    // Метод удаления выбранной хранилищной единицы
    protected override void RemoveItem() { RemoveCommand(SelectedItem); }
}