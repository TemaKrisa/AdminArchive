using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using System.Collections.ObjectModel;
namespace AdminArchive.ViewModel;
internal class FundPageVM : PageBaseVM
{
    #region Переменные
    private ObservableCollection<Fond> _fonds; // коллекция фондов
    private string _fondName, _fondShortName, _fondStartDate, _fondEndDate;
    private int _fondCategory = -1;
    public string? FondName { get => _fondName; set { _fondName = value; OnPropertyChanged(); } }
    public string? FondShortName { get => _fondShortName; set { _fondShortName = value; OnPropertyChanged(); } }
    public string? FondStartDate { get => _fondStartDate; set { _fondStartDate = value; OnPropertyChanged(); } }
    public string? FondEndDate { get => _fondEndDate; set { _fondEndDate = value; OnPropertyChanged(); } }
    public int FondCategory { get => _fondCategory; set { _fondCategory = value; OnPropertyChanged(); } }
    private Fond _selectedItem; // выбранный фонд
    public ObservableCollection<Category> Categories { get; set; } // коллекция категорий
    public ObservableCollection<Fond> Fonds { get => _fonds; set { _fonds = value; OnPropertyChanged(); } } // коллекция фондов
    public Fond SelectedItem { get => _selectedItem; set { _selectedItem = value; } }

    #endregion
    public FundPageVM()
    {
        LoadData();
    }
    private void LoadData()
    {
        using ArchiveBdContext dc = new(); // создаем контекст базы данных 
        Categories = new ObservableCollection<Category>(dc.Categories);
        Categories.Insert(0, new Category { Name = "Все категории", Id = -1 });
        UpdateData();
    }
    // функция для получения данных из базы данных и обновления представления
    public override void UpdateData() => Fonds = SearchClass.SearchFond(FondName, FondShortName, FondStartDate, FondEndDate, FondCategory);
    protected override void OpenItem()
    {
        if (SelectedItem != null) // если выбран элемент
        {
            InventoryPageVM vm = new(SelectedItem); // создаем ViewModel для окна редактирования описи
            InventoryPage v = new() { DataContext = vm }; // создаем окно редактирования описи
            Setting.mainFrame?.Navigate(v); // переходим в окно редактирования описи
        }
    }
    protected override void AddItem()
    {
        FundWindowVM viewModel = new(this, Fonds); // создаем ViewModel для окна добавления фонда
        FundWindow newWindow = new() { DataContext = viewModel }; // создаем окно добавления фонда
        newWindow.ShowDialog(); // открываем окно добавления фонда
    }
    protected override void EditItem()
    {
        int index = Fonds.IndexOf(SelectedItem); // получаем индекс выбранного фонда
        FundWindow newWindow = new(); // создаем окно редактирования фонда
        FundWindowVM viewModel = new((SelectedItem as Fond), this, index, Fonds); // создаем ViewModel для окна редактирования фонда
        newWindow.DataContext = viewModel; // устанавливаем ViewModel для окна редактирования фонда
        newWindow.ShowDialog(); // открываем окно редактирования фонда
    }
    #region Поиск
    protected override void ResetSearch() { FondName = null; FondShortName = null; FondStartDate = null; FondEndDate = null; FondCategory = -1; UpdateData(); }
    #endregion
    protected override void GoBack() { }
}