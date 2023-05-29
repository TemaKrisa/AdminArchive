using AdminArchive.Classes;
using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel
{
    internal class SearchPageVM : BaseViewModel
    {
        private ObservableCollection<Fond> _fonds; // коллекция фондов
        private string _name, _shortName, _startDate, _endDate; // названия и даты фондов
        private int _category; // категория фонда
        private Fond _selectedItem; // выбранный фонд

        public ObservableCollection<Category> Categories { get; set; } // коллекция категорий
        public ObservableCollection<Fond> Fonds // коллекция фондов
        { get => _fonds; set { _fonds = value; OnPropertyChanged(); } }
        public Fond SelectedItem
        { get => _selectedItem; set { _selectedItem = value; } }
        public string? FondName
        { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string? FondShortName
        { get => _shortName; set { _shortName = value; OnPropertyChanged(); } }
        public string? FondStartDate
        { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public string? FondEndDate
        { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }
        public int FondCategory
        { get => _category; set { _category = value; OnPropertyChanged(); } }

        public SearchPageVM()
        {

        }

        private void UpdateData()
        {
            using ArchiveBdContext dc = new(); // создаем контекст базы данных
            Fonds = new ObservableCollection<Fond>(dc.Fonds.Include(u => u.CategoryNavigation).OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal)); // получаем фонды из базы данных и сортируем их
            Categories = new ObservableCollection<Category>(dc.Categories); // получаем категории из базы данных
            Categories.Insert(0, new Category { Name = "Все категории", Id = -1 }); // добавляем категорию "Все категории" в начало списка
        }
        protected void SearchCommand() // функция, которая вызывается при нажатии на кнопку "Поиск"
        {
            Fonds = SearchClass.SearchFond(Fonds, FondName, FondShortName, FondStartDate, FondEndDate, FondCategory);
            UCVisibility = System.Windows.Visibility.Collapsed;
        }
        protected void ResetSearch() // функция, которая вызывается при нажатии на кнопку "Сбросить" и сбрасывает фильтры поиска
        { FondName = null; FondShortName = null; FondStartDate = null; FondEndDate = null; FondCategory = -1; UpdateData(); }
        protected void CloseSearchCommand() => UCVisibility = System.Windows.Visibility.Collapsed; // функция, которая вызывается при нажатии на кнопку "Закрыть поиск"
        protected void OpenSearchCommand() => UCVisibility = System.Windows.Visibility.Visible; // функция, которая вызывается при нажатии на кнопку "Открыть поиск"
    }
}
