using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.Classes
{
    public static class SearchClass
    {
        public static ObservableCollection<Fond> SearchFond(ObservableCollection<Fond> Fonds, string Name, string ShortName, string StartDate, string EndDate, int Category) // функция, которая вызывается при нажатии на кнопку "Поиск"
        {
            using ArchiveBdContext dc = new(); // создаем контекст базы данных
            Fonds = new ObservableCollection<Fond>(dc.Fonds.Include(u => u.CategoryNavigation).OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal)); // получаем фонды из базы данных и сортируем их
            var filteredFonds = Fonds.Where(u =>
                (string.IsNullOrWhiteSpace(Name) || u.Name.Contains(Name)) // фильтруем фонды по названию
                && (string.IsNullOrWhiteSpace(ShortName) || u.ShortName.Contains(ShortName)) // фильтруем фонды по короткому названию
                && (string.IsNullOrWhiteSpace(StartDate) || u.StartDate >= Convert.ToInt32(StartDate)) // фильтруем фонды по дате начала
                && (string.IsNullOrWhiteSpace(EndDate) || u.EndDate <= Convert.ToInt32(EndDate)) // фильтруем фонды по дате окончания
                && (Category == -1 || u.Category == Category)); // фируем фонды по категории
            Fonds = new ObservableCollection<Fond>(filteredFonds); // обновляем коллекцию фондов
            return Fonds;
        }
    }
}
