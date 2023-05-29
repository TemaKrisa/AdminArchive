using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel;
internal class FundPageVM : PageBaseVM
{
    #region Переменные
    private ObservableCollection<Fond> _fonds; // коллекция фондов
    private string _name, _shortName, _startDate, _endDate; // названия и даты фондов
    private int _category; // категория фонда
    private Fond _selectedItem; // выбранный фонд
    public ObservableCollection<Category> Categories { get; set; } // коллекция категорий
    public ObservableCollection<Fond> Fonds // коллекция фондов
    { get => _fonds; set { _fonds = value; OnPropertyChanged(); } }
    public Fond SelectedItem
    { get => _selectedItem; set { _selectedItem = value; } }
    public string? Name
    { get => _name; set { _name = value; OnPropertyChanged(); } }
    public string? ShortName
    { get => _shortName; set { _shortName = value; OnPropertyChanged(); } }
    public string? StartDate
    { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
    public string? EndDate
    { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }
    public int Category
    { get => _category; set { _category = value; OnPropertyChanged(); } }
    #endregion

    public FundPageVM()
    {
        Category = -1; // устанавливаем значение категории по умолчанию
        UpdateData(); // обновляем данные
    }

    public void UpdateData() // функция для получения данных из базы данных и обновления представления
    {
        using ArchiveBdContext dc = new(); // создаем контекст базы данных
        Fonds = new ObservableCollection<Fond>(dc.Fonds.Include(u => u.CategoryNavigation).OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal)); // получаем фонды из базы данных и сортируем их
        Categories = new ObservableCollection<Category>(dc.Categories); // получаем категории из базы данных
        Categories.Insert(0, new Category { Name = "Все категории", Id = -1 }); // добавляем категорию "Все категории" в начало списка
    }

    protected override void OpenItem() // функция, которая вызывается при щелчке на элементе, чтобы открыть его
    {
        if (SelectedItem != null) // если выбран элемент
        {
            InventoryPageVM vm = new(SelectedItem); // создаем ViewModel для окна редактирования описи
            InventoryPage v = new() { DataContext = vm }; // создаем окно редактирования описи
            Setting.mainFrame?.Navigate(v); // переходим в окно редактирования описи
        }
    }
    protected override void AddItem() // функция, которая вызывается при нажатии на кнопку "Добавить"
    {
        FundWindowVM viewModel = new(this, Fonds); // создаем ViewModel для окна добавления фонда
        FundWindow newWindow = new() { DataContext = viewModel }; // создаем окно добавления фонда
        newWindow.ShowDialog(); // открываем окно добавления фонда
    }
    protected override void EditItem() // функция, которая вызывается при нажатии на кнопку "Редактировать"
    {
        int index = Fonds.IndexOf(SelectedItem); // получаем индекс выбранного фонда
        FundWindow newWindow = new(); // создаем окно редактирования фонда
        FundWindowVM viewModel = new((SelectedItem as Fond), this, index, Fonds); // создаем ViewModel для окна редактирования фонда
        newWindow.DataContext = viewModel; // устанавливаем ViewModel для окна редактирования фонда
        newWindow.ShowDialog(); // открываем окно редактирования фонда
    }

    #region Поиск
    protected override void SearchCommand() // функция, которая вызывается при нажатии на кнопку "Поиск"
    {
        Fonds = SearchClass.SearchFond(Fonds, Name, ShortName, StartDate, EndDate, Category);
        UCVisibility = System.Windows.Visibility.Collapsed;
    }
    protected override void ResetSearch() // функция, которая вызывается при нажатии на кнопку "Сбросить" и сбрасывает фильтры поиска
    { Name = null; ShortName = null; StartDate = null; EndDate = null; Category = -1; UpdateData(); }
    #endregion
    protected override void GoBack() { }
}