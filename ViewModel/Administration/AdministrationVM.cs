using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
/// <summary>
/// ViewModel для управления разделом управления.
/// </summary>
public partial class AdministrationVM : BaseViewModel
{
    private ObservableCollection<object> _items;
    private object _selectedItem;
    private string type;
    private int navType;
    public object SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
    public ObservableCollection<object> Items { get => _items; set { _items = value; OnPropertyChanged(); } }
    public string Type { get => type; set { type = value; OnPropertyChanged(); } }
    public int NavType { get => navType; set { navType = value; OnPropertyChanged(); } }
    public ICommand Add => new RelayCommand(AddCommand);
    public ICommand Edit => new RelayCommand(EditCommand);
    public ICommand Remove => new RelayCommand(RemoveCommand);
    public ICommand Open => new RelayCommand(OpenCommand);
    public ICommand Close => new RelayCommand(CloseCommand);
    public ICommand Navigator => new RelayCommand<object>(Navigate);
    public AdministrationVM()
    {
        Type = AdminArchive.Classes.Setting.AdminType;
        if (type == null) { Navigate(1); }
        UpdateData();
    }
    private void UpdateData()
    {
        using ArchiveBdContext dc = new();
        switch (AdminArchive.Classes.Setting.AdminNavType)
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
    private void Navigate(object parameter) //Навигация
    {
        NavType = Convert.ToInt16(parameter);
        AdminArchive.Classes.Setting.AdminType = NavType switch
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
        AdminArchive.Classes.Setting.AdminNavType = NavType;
        Setting.adminFrame?.Navigate(new AdministrationEditPage());
    }
    private void AddCommand()//Вызов добавления
    {
        SelectedItem = AdminArchive.Classes.Setting.AdminNavType switch
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
        UCVisibility = System.Windows.Visibility.Visible;
    }
    private void EditCommand()//Вызов изменения
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
                        using ArchiveBdContext dc = new();
                        if (dc.Entry(SelectedItem).State == EntityState.Detached) dc.Add(SelectedItem);
                        else dc.Update(SelectedItem);
                        dc.SaveChanges();
                        UpdateData();
                        UCVisibility = System.Windows.Visibility.Collapsed;
                    }
                    else ShowMessage("Введите наименование");
                }
                else ShowMessage("Введите наименование");
            }
            catch (Exception ex) { ShowMessage(ex.ToString()); }
        }
    }
    private void OpenCommand()//Открытие юзерконтрола
    {
        if (SelectedItem != null)
            UCVisibility = System.Windows.Visibility.Visible;
    }
    private void RemoveCommand() //Удаление
    {
        if (SelectedItem != null && MessageBoxs.ShowDialog("Вы точно хотите удалить", "Удаление", MessageBoxs.Buttons.YesNo) == "1")
            try
            {
                using ArchiveBdContext dc = new();
                dc.Remove(SelectedItem); dc.SaveChanges(); UpdateData();
                ShowMessage("Удаление прошло успешно", "Удаление");
            }
            catch { ShowMessage("Ошибка удаления", "Удаление"); }
    }
    private void CloseCommand() => UCVisibility = System.Windows.Visibility.Collapsed; //Закрытие юзерконтрола
}