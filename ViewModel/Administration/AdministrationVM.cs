using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
/// <summary>  ViewModel для управления разделом управления.  </summary>
public partial class AdministrationVM : BaseViewModel
{
    private ObservableCollection<object> _items; //Коллекция объектов, которые будут отображаться на странице.
    private object _selectedItem; //Выбранный объект из коллекции.
    private string type; //Тип административного раздела.
    private int navType; //Тип навигации по административному разу.
    public object SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } } //Свойство для доступа к выбранному объекту.
    public ObservableCollection<object> Items { get => _items; set { _items = value; OnPropertyChanged(); } } //Свойство для доступа к коллекции объектов.
    public string Type { get => type; set { type = value; OnPropertyChanged(); } } //Свойство для доступа к типу административного раздела.
    public int NavType { get => navType; set { navType = value; OnPropertyChanged(); } } //Свойство для доступа к типу навигации по административному разделу.
    ICommand Add => new RelayCommand(AddCommand); //Команда для вызова добавления объекта.
    public ICommand Edit => new RelayCommand(EditCommand); //Команда для вызова изменения объекта.
    public ICommand Remove => new RelayCommand(RemoveCommand); //Команда для удаления выбранного объекта.
    public ICommand Open => new RelayCommand(OpenCommand); //Команда для открытия выбранного объекта.
    public ICommand Close => new RelayCommand(CloseCommand); //Команда для закрытия выбранного объекта.
    public ICommand Navigator => new RelayCommand<object>(Navigate); //Команда для навигации по административному разделу.
    public AdministrationVM()
    {
        Type = AdminArchive.Classes.Setting.AdminType; //Установка типа административного раздела.
        if (type == null) { Navigate(1); } //Если тип не установлен, то переходим на первый раздел.
        UpdateData(); //Обновление данных.
    }
    private void UpdateData() //Обновление данных.
    {
        using ArchiveBdContext dc = new(); //Создание контекста базы данных.
        switch (AdminArchive.Classes.Setting.AdminNavType) //Выбор коллекции объектов в зависимости от типа навигации            {
        {
            case 1: Items = new ObservableCollection<object>(dc.UnitCategories); break;
            case 2: Items = new ObservableCollection<object>(dc.IncomeSources); break;
            case 3: Items = new ObservableCollection<object>(dc.HistoricalPeriods); break;
            case 4: Items = new ObservableCollection<object>(dc.MovementTypes); break;
            case 5: Items = new ObservableCollection<object>(dc.CharRestricts); break;
            case 6: Items = new ObservableCollection<object>(dc.Works); break;
            case 7: Items = new ObservableCollection<object>(dc.FondViews); break;
            case 8: Items = new ObservableCollection<object>(dc.ReceiptReasons); break;
            case 9: Items = new ObservableCollection<object>(dc.Carriers); break;
            case 10: Items = new ObservableCollection<object>(dc.Features); break;
        }
    }
    private void Navigate(object parameter) //Навигация по административному разделу.
    {
        NavType = Convert.ToInt16(parameter); //Установка типа навигации.
        AdminArchive.Classes.Setting.AdminType = NavType switch //Установка типа административного раздела в зависимости от типа навигации.
        {
            1 => "Категория ед.хр.",
            2 => "Источники поступления",
            3 => "Исторические периоды",
            4 => "Тип движения",
            5 => "Причины ограничения доступа",
            6 => "Работы",
            7 => "Вид фонда",
            8 => "Причины поступления",
            9 => "Носители",
            10 => "Особенности"
        };
        AdminArchive.Classes.Setting.AdminNavType = NavType; //Установка типа навигации в настройках.
        Setting.adminFrame?.Navigate(new AdministrationEditPage()); //Переход на страницу редактирования административного раздела.
    }
    private void AddCommand() //Вызов добавления объекта.
    {
        SelectedItem = AdminArchive.Classes.Setting.AdminNavType switch //Установка выбранного объекта в зависимости от типа навигации.
        {
            1 => new UnitCategory(),
            2 => new IncomeSource(),
            3 => new HistoricalPeriod(),
            4 => new MovementType(),
            5 => new CharRestrict(),
            6 => new Work(),
            7 => new FondView(),
            8 => new ReceiptReason(),
            9 => new Carrier(),
            10 => new Feature()
        };
        UCVisibility = System.Windows.Visibility.Visible; //Отображение юзерконтрола.
    }
    private void EditCommand() //Вызов изменения объекта.
    {
        if (SelectedItem != null)
        {
            try
            {
                object obj = SelectedItem;
                if (obj is { } && (obj.GetType().GetProperty("Name")?.GetValue(obj) as string) != null)
                {
                    string name = (obj as dynamic).Name;
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        using ArchiveBdContext dc = new(); //Создание контекста базы данных.
                        if (dc.Entry(SelectedItem).State == EntityState.Detached) dc.Add(SelectedItem); //Добавление объекта в контекст, если он не отслеживается.
                        else dc.Update(SelectedItem); //Обновление объекта в контексте, если он отслеживается.
                        dc.SaveChanges(); //Сохранение изменений в базе данных.
                        UpdateData(); //Обновление данных.
                        UCVisibility = System.Windows.Visibility.Collapsed; //Скрытие юзерконтрола.
                    }
                    else ShowMessage("Введите наименование"); //Вывод сообщения об ошибке, если не введено наименование объекта.
                }
                else ShowMessage("Введите наименование"); //Вывод сообщения об ошибке, если не введено наименование объекта.
            }
            catch (Exception ex) { ShowMessage(ex.ToString()); } //Вывод сообщения об ошибке, если произошла ошибка при изменении объекта.
        }
    }
    private void OpenCommand() //Открытие юзерконтрола.
    { if (SelectedItem != null) UCVisibility = System.Windows.Visibility.Visible; } //Отображение юзерконтрола. 
    private void RemoveCommand() //Удаление выбранного объекта.
    {
        if (SelectedItem != null && MessageBoxs.ShowDialog("Вы точно хотите удалить", "Удаление", MessageBoxs.Buttons.YesNo) == "1") //Если объект выбран и пользователь подтвердил удаление.
            try
            {
                using ArchiveBdContext dc = new(); //Создание контекста базы данных.
                dc.Remove(SelectedItem); dc.SaveChanges(); UpdateData(); //Удаление объекта из контекста, сохранение изменений и обновление данных.
                ShowMessage("Удаление прошло успешно", "Удаление"); //Вывод сообщения об успешном удалении объекта.
            }
            catch { ShowMessage("Ошибка удаления", "Удаление"); } //Вывод сообщения об ошибке при удалении объекта.
    }
    private void CloseCommand() => UCVisibility = System.Windows.Visibility.Collapsed; //Закрытие юзерконтрола.
}