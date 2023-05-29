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
    private ArchiveBdContext dc = new();
    private ObservableCollection<object> _items;
    private object _selectedItem;
    private string type;
    private int navType;
    public object SelectedItem
    { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
    public ObservableCollection<object> Items
    { get => _items; set { _items = value; OnPropertyChanged(); } }
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
        if (type == null) Type = "Категории";
        UpdateData();
    }
    private void UpdateData()
    {
        switch (AdminArchive.Classes.Setting.AdminNavType)
        {
            case 1: Items = new ObservableCollection<object>(dc.Categories); break;
            case 2: Items = new ObservableCollection<object>(dc.IncomeSources); break;
            case 3: Items = new ObservableCollection<object>(dc.HistoricalPeriods); break;
        }
    }
    private void Navigate(object parameter)
    {
        NavType = Convert.ToInt16(parameter);
        AdminArchive.Classes.Setting.AdminType = NavType switch
        {
            1 => "Категории",
            2 => "Источники поступления",
            3 => "Исторические периоды"
        };
        AdminArchive.Classes.Setting.AdminNavType = NavType;
        Setting.adminFrame?.Navigate(new AdministrationEditPage());
    }
    private void AddCommand()//Добавление юзерконтрола
    {
        Items = Type switch
        {
            "Категории" => new ObservableCollection<object>(dc.Categories),
            "Источники поступления" => new ObservableCollection<object>(dc.Activities),
            "Исторические периоды" => new ObservableCollection<object>(dc.Acesses),
        };
        UCVisibility = System.Windows.Visibility.Visible;
    }
    private void EditCommand()//Изменение юзерконтрола
    {
        if (SelectedItem != null)
        {
            try
            {
                if (dc.Entry(SelectedItem).State == EntityState.Detached) dc.Add(SelectedItem);
                else dc.Update(SelectedItem);
                dc.SaveChanges();
                UpdateData();
                UCVisibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            { ShowMessage(ex.ToString()); }
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
                dc.Remove(SelectedItem); dc.SaveChanges(); UpdateData();
                ShowMessage("Удаление прошло успешно", "Удаление");
            }
            catch { ShowMessage("Ошибка удаления", "Удаление"); }
    }
    private void CloseCommand() => UCVisibility = System.Windows.Visibility.Collapsed; //Закрытие юзерконтрола
}
