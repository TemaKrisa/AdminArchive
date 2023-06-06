using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel;
/// <summary> ViewModel для страницы документов </summary>
class DocumentPageVM : PageBaseVM
{
    private ObservableCollection<Document> _documents; // Коллекция документов
    public ObservableCollection<Document>? Documents { get => _documents; set { _documents = value; OnPropertyChanged(); } } // Свойство для доступа к коллекции документов
    private ArchiveBdContext dc; // Контекст базы данных
    private Document _selectedItem; // Выбранный элемент
    private StorageUnit curUnit; // Текущая единица хранения
    private Inventory curInv; // Текущий номер
    private Fond curFond; // Текущий фонд
    private string _docTitle; // Название документа
    private int _docAu = -1, _docType = -1;
    private DateTime? _docDate = null; // Дата документа
    public string? DocTitle { get => _docTitle; set { _docTitle = value; OnPropertyChanged(); } } // Свойство для доступа к названию документа
    public int DocAu { get => _docAu; set { _docAu = value; OnPropertyChanged(); } }
    public int DocType { get => _docType; set { _docType = value; OnPropertyChanged(); } } // Свойство для доступа к типу документа
    public DateTime? DocDate { get => _docDate; set { _docDate = value; OnPropertyChanged(); } } // Свойство для доступа к дате документа
    public ObservableCollection<Authenticity> Authenticities { get; set; } // Коллекция подлинностей
    public ObservableCollection<DocType> DocTypes { get; set; } // Коллекция типов документов
    public Document SelectedItem { get => _selectedItem; set => _selectedItem = value; } // Свойство для доступа к выбранному элементу
    public DocumentPageVM(StorageUnit unit, Fond fond, Inventory inventory)
    {
        dc = new ArchiveBdContext(); // Инициализация контекста базы данных
        curUnit = unit; // Присвоение текущей единицы хранения
        curFond = fond; // Присвоение текущего фонда
        curInv = inventory; // Присвоение текущего инвентарного номера
        Authenticities = new ObservableCollection<Authenticity>(dc.Authenticities); // Инициализация коллекции подлинностей
        Authenticities.Insert(0, new Authenticity { Name = "Все подлинности", Id = -1 }); // Добавление элемента "Все подлинности" в коллекцию подлинностей
        DocTypes = new ObservableCollection<DocType>(dc.DocTypes); // Инициализация коллекции типов документов
        DocTypes.Insert(0, new DocType { Name = "Все виды", Id = -1 }); // Добавление элемента "Все виды" в коллекцию типов документов
        UpdateData(); // Обновление данных на странице
    }
    public override void UpdateData() => Documents = SearchClass.SearchDocument(DocTitle, DocAu, DocType, DocDate, curUnit); // Обновление данных на странице
    protected override void GoBack() // Метод для возврата на предыдущую страницу
    {
        StorageUnitPageVM vm = new(curInv, curFond); // Создание экземпляра ViewModel для страницы единиц хранения
        Setting.mainFrame?.Navigate(new StorageUnitPage { DataContext = vm }); // Переход на страницу единиц хранения
    }
    protected override void EditItem() // Метод для редактирования элемента
    {
        int index = Documents.IndexOf(SelectedItem); // Получение индекса выбранного элемента
        DocumentWindow newWindow = new(); // Создание нового окна для редактирования документа
        DocumentWindowVM? viewModel = new((SelectedItem as Document), this, index, Documents, curUnit); // Создание экземпляра ViewModel для окна редактирования документа
        newWindow.DataContext = viewModel; // Присвоение ViewModel окну редактирования документа
        newWindow.ShowDialog(); // Открытие окна редактирования документа
    }
    protected override void AddItem() // Метод для добавления элемента
    {
        DocumentWindowVM vm = new(this, Documents, curUnit); // Создание экземпляра ViewModel для окна добавления документа
        var newWindow = new DocumentWindow { DataContext = vm }; // Создание нового окна для добавления документа
        newWindow.ShowDialog(); // Открытие окна добавления документа
    }
    protected override void ResetSearch() { DocTitle = null; DocAu = -1; DocType = -1; DocDate = DateTime.MinValue; UpdateData(); } // Метод для сброса фильтров поиска
    protected override void OpenItem() { } // Метод для открытия элемента
    public DocumentPageVM() { } // Конструктор по умолчанию
    protected override void RemoveItem() { RemoveCommand(SelectedItem); } // Метод для удаления элемента
}
