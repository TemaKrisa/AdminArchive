using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
namespace AdminArchive.ViewModel;
/// <summary> ViewModel для страницы описей </summary>
partial class InventoryPageVM : PageBaseVM
{
    // Объявление приватных переменных
    private ObservableCollection<Inventory> _inventories; // Коллекция инвентаризационных единиц
    private Fond curFond; // Фонд, кому относится инвентаризация
    private string _inventoryName, _inventoryStartDate, _inventoryEndDate; // Название, дата начала и дата окончания инвентаризации
    private int _inventoryCategory = -1; // Категория инвентаризации
    // Объявление публичных свойств
    public string InventoryName { get => _inventoryName; set { _inventoryName = value; OnPropertyChanged(); } } // Название инвентаризации
    public string InventoryStartDate { get => _inventoryStartDate; set { _inventoryStartDate = value; OnPropertyChanged(); } } // Дата начала инвентаризации
    public string InventoryEndDate { get => _inventoryEndDate; set { _inventoryEndDate = value; OnPropertyChanged(); } } // Дата окончания инвентаризации
    public int InventoryCategory { get => _inventoryCategory; set { _inventoryCategory = value; OnPropertyChanged(); } } // Категория инвентаризации
    public ObservableCollection<Inventory> Inventories { get => _inventories; set { _inventories = value; OnPropertyChanged(); } } // Коллекция инвентаризационных единиц
    public ObservableCollection<Category> Categories { get; set; } // Коллекция категорий инвентаризации
    public Inventory SelectedItem { get; set; } // Выбраннаявентаризационная единица
    // Конструктор класса, принимающий фонд, к которому относится инвентаризация
    public InventoryPageVM(Fond fond)
    {
        using ArchiveBdContext dc = new(); // Создание контекста базы данных
        curFond = fond; // Присвоение фонда
        Categories = new ObservableCollection<Category>(dc.Categories); // Инициализация коллекции категорий инвентаризации
        UpdateData(); // Обновление данных
        Categories.Insert(0, new Category { Name = "Все категории", Id = -1 }); // Добавление "Все категории" в коллекцию категорий
    }
    // Конструктор класса по умолчанию
    public InventoryPageVM() { }
    // Метод обновления данных
    public override void UpdateData() => Inventories = SearchClass.SearchInventory(InventoryName, InventoryStartDate, InventoryEndDate, InventoryCategory, curFond);
    // Метод перехода на предыдущую страницу
    protected override void GoBack() { Setting.mainFrame.Navigate(new FundPage()); }
    // Метод открытия выбранной инвентаризационной единицы
    protected override void OpenItem()
    {
        if (SelectedItem != null)
        {
            StorageUnitPageVM vm = new(SelectedItem, curFond);
            StorageUnitPage v = new() { DataContext = vm };
            Setting.mainFrame.Navigate(v);
        }
    }
    // Метод добавления инвентаризационной единицы
    protected override void AddItem()
    {
        InventoryWindowVM viewModel = new(this, Inventories, curFond);
        InventoryWindow newWindow = new() { DataContext = viewModel };
        newWindow.ShowDialog();
    }
    // Метод редактирования выбранной инвентаризационной единицы
    protected override void EditItem()
    {
        int index = Inventories.IndexOf(SelectedItem);
        InventoryWindow newWindow = new();
        InventoryWindowVM viewModel = new((SelectedItem as Inventory), this, index, Inventories, curFond);
        newWindow.DataContext = viewModel;
        newWindow.ShowDialog();
    }
    // Метод сброса поиска
    protected override void ResetSearch() { InventoryName = null; InventoryStartDate = null; InventoryEndDate = null; InventoryCategory = -1; UCVisibility = System.Windows.Visibility.Collapsed; UpdateData(); }
    // Метод удаления выбранной инвентаризационной единицы
    protected override void RemoveItem() { RemoveCommand(SelectedItem); }
}