using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
namespace AdminArchive.Classes;
/// <summary>Класс для поиска Фондов, Описей, Документов, Единиц хранения</summary>
public static class SearchClass
{
    ///<summary>Поиск фонда</summary><param name="Name">Наименование</param><param name="ShortName">Сокращенное наименование</param><param name="StartDate">Начальная дата</param><param name="EndDate">Конечная дата</param><param name="Category">Категория</param>
    public static ObservableCollection<Fond> SearchFond(string Name, string ShortName, string StartDate, string EndDate, int Category) // функция, которая вызывается при нажатии на кнопку "Поиск"
    {
        using ArchiveBdContext dc = new(); // создаем контекст базы данных
        IQueryable<Fond> query = dc.Fonds.Include(u => u.CategoryNavigation).OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal); // получаем фонды из базы данных и сортируем их
        var FilteredItems = query.Where(u =>
            (string.IsNullOrWhiteSpace(Name) || u.Name.Contains(Name))
            && (string.IsNullOrWhiteSpace(ShortName) || u.ShortName.Contains(ShortName))
            && (string.IsNullOrWhiteSpace(StartDate) || u.StartDate >= Convert.ToInt32(StartDate))
            && (string.IsNullOrWhiteSpace(EndDate) || u.EndDate <= Convert.ToInt32(EndDate))
            && (Category == -1 || u.Category == Category)); //Фильтрация
        return new ObservableCollection<Fond>(FilteredItems);
    }
    ///<summary>Поиск описи</summary><param name="Name">Наименование</param><param name="StartDate">Начальная дата</param><param name="EndDate">Конечная дата</param><param name="Category">Категория</param><param name="curFond">Фонд для фильтрации</param>
    public static ObservableCollection<Inventory> SearchInventory(string Name, string StartDate, string EndDate, int Category, Fond curFond = null) // функция, которая вызывается при нажатии на кнопку "Поиск"
    {
        using ArchiveBdContext dc = new(); // создаем контекст базы данных
        IQueryable<Inventory> query = dc.Inventories.Include(u => u.TypeNavigation).OrderBy(u => u.Number).ThenBy(u => u.Literal); //Загружаем список описей
        if (curFond != null) query = query.Where(u => u.Fond == curFond.Id);
        query = query.OrderBy(u => u.Number).ThenBy(u => u.Literal);
        var FilteredItems = query.Where(u => (string.IsNullOrWhiteSpace(Name) || u.Name.Contains(Name))
            && (string.IsNullOrWhiteSpace(StartDate) || u.StartDate >= Convert.ToInt32(StartDate))
            && (string.IsNullOrWhiteSpace(EndDate) || u.EndDate <= Convert.ToInt32(EndDate))
            && (Category == -1 || u.Category == Category));  //Фильтрация
        return new ObservableCollection<Inventory>(FilteredItems); // обновляем коллекцию фондов
    }
    ///<summary>Поиск единицы хранения</summary><param name="Name">Наименование</param><param name="StartDate">Начальная дата</param> <param name="EndDate">Конечная дата</param><param name="Category">Категория</param><param name="curInv">Текущая опись для фильтрации по ней</param>
    public static ObservableCollection<StorageUnit> SearchUnit(string Name, string StartDate, string EndDate, int Category, Inventory curInv = null) // функция, которая вызывается при нажатии на кнопку "Поиск"
    {
        using ArchiveBdContext dc = new(); // создаем контекст базы данных
        IQueryable<StorageUnit> query = dc.StorageUnits.Include(u => u.CategoryNavigation).OrderBy(u => u.Number).ThenBy(u => u.Literal); //Загружаем список единиц хранения
        if (curInv != null) query = query.Where(u => u.Inventory == curInv.Id); //Если указана опись, то фильтрует по описи
        query = query.OrderBy(u => u.Number).ThenBy(u => u.Literal);
        var FilteredItems = query.Where(u => (string.IsNullOrWhiteSpace(Name) || u.Title.Contains(Name))
            && (string.IsNullOrWhiteSpace(StartDate) || u.StartDate >= Convert.ToInt32(StartDate))
            && (string.IsNullOrWhiteSpace(EndDate) || u.EndDate <= Convert.ToInt32(EndDate))
            && (Category == -1 || u.Category == Category)); // фильтруем коллекцию
        return new ObservableCollection<StorageUnit>(FilteredItems); // овозвращаем коллекцию ;
    }
    /// <summary>Поиск документа</summary><param name="Title">Наименование</param><param name="Au">Подлинность</param><param name="Type">Тип</param><param name="date">Дата документа</param><param name="curUnit">Текущая единица хранения для фильтрации по ней</param>
    public static ObservableCollection<Document> SearchDocument(string Title, int Au, int Type, DateTime date, StorageUnit curUnit = null) // функция, которая вызывается при нажатии на кнопку "Поиск"
    {
        using ArchiveBdContext dc = new();
        IQueryable<Document> query = dc.Documents.Include(u => u.SecretCharNavigation).OrderBy(u => u.Number); //Загружаем список документов
        if (curUnit != null) query = query.Where(u => u.StorageUnit == curUnit.Id);
        var FilteredItems = query.Where(u => (string.IsNullOrWhiteSpace(Title) || u.Name.Contains(Title))
        && (Au == -1 || u.Authenticity == Au) && (Type == -1 || u.DocType == Type)
        && (date == DateTime.MinValue || u.Date == date)); // фируем фонды по категории
        return new ObservableCollection<Document>(FilteredItems); // возвращаем коллекцию
    }
}