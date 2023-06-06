using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
namespace AdminArchive.ViewModel;
/// <summary>ViewModel страницы управления аккаунтами </summary>
class AccountVM : BaseViewModel
{
    #region Переменные
    private bool IsInitialized = false; // Флаг инициализации
    private ObservableCollection<User> users; // Коллекция пользователей
    private ObservableCollection<Role> roles; // Коллекция ролей
    public ObservableCollection<User> Users { get => users; set { users = value; OnPropertyChanged(); } } // Коллекция пользователей
    private User curUser, selectedUser; // Текущий пользователь и выбранный пользователь
    public User CurUser { get => curUser; set { curUser = value; OnPropertyChanged(); } } // Текущий пользователь
    public User SelectedUser { get => selectedUser; set { selectedUser = value; OnPropertyChanged(); } } // Выбранный пользователь
    public ObservableCollection<Role> Roles { get => roles; set { roles = value; OnPropertyChanged(); } } // Коллекция ролей
    #endregion
    protected void AddItem() // Метод открытия UserControl для добавления нового пользователя
    {
        UCVisibility = Visibility.Visible;
        CurUser = new User() { Role = 1 };
    }
    private void UpdateData() // Метод обновления данных
    {
        using ArchiveBdContext dc = new(); // Создание контекста базы данных
        Users = new ObservableCollection<User>(dc.Users.Include(u => u.RoleNavigation)); // Инициализация коллекции пользователей
    }
    private string CurLogin; // Логин текущего пользователя
    protected void OpenItem() // Метод открытия UserControl для редактирования выбранного пользователя
    {
        if (SelectedUser != null)
        {
            UCVisibility = Visibility.Visible;
            CurUser = selectedUser;
            CurLogin = CurUser.Login;
        }
    }
    public AccountVM() // Конструктор класса
    {
        if (!IsInitialized)
        {
            using ArchiveBdContext dc = new(); // Создание контекста базы данных
            Roles = new ObservableCollection<Role>(dc.Roles); // Инициализация коллекции ролей
            UpdateData(); // Обновление данных
        }
    }
    #region UserControl
    private RelayCommand? _add, _open, _save, _close; // Команды UserControl
    public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } } // Команда открытия UserControl для редактирования выбранного пользователя
    public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } } // Команда открытия UserControl для добавления нового пользователя
    public RelayCommand Save { get { return _save ??= new RelayCommand(SaveItem); } } // Команда сохранения изменений пользователя
    public RelayCommand Close { get { return _close ??= new RelayCommand(CloseUC); } } // Команда закрытия UserControl
    protected void SaveItem() // Метод сохранения изменений пользователя
    {
        string errorMessage = null;
        if (string.IsNullOrWhiteSpace(CurUser.Surname)) errorMessage = "Введите фамилию!";
        else if (string.IsNullOrWhiteSpace(CurUser.Name)) errorMessage = "Введите имя!";
        else if (string.IsNullOrWhiteSpace(CurUser.Password)) errorMessage = "Введите пароль!";
        if (errorMessage != null)
        {
            UpdateData();
            ShowMessage(errorMessage.ToString());
            return;
        }
        try
        {
            using ArchiveBdContext dc = new(); // Создание контекста базы данных
            if (!dc.Users.Contains(CurUser))
            {
                if (!dc.Users.Any(u => u.Login == curUser.Login))
                {
                    dc.Users.Add(CurUser);
                    ShowMessage("Добавление прошло успешно", "Добавление аккаунта");
                }
                else
                {
                    UpdateData();
                    ShowMessage("Аккаунт с таким логином уже существует!", "Добавление аккаунта");
                    return;
                }
            }
            else
            {
                if (CurUser.Login != CurLogin && dc.Users.Any(u => u.Login == curUser.Login))
                {
                    UpdateData();
                    ShowMessage("Аккаунт с таким логином уже существует!", "Изменение аккаунта");
                    return;
                }
                dc.Users.Update(CurUser);
                ShowMessage("Изменение прошло успешно", "Изменение аккаунта");
            }
            dc.SaveChanges();
            UpdateData();
            UCVisibility = Visibility.Collapsed;
        }
        catch (Exception ex) { ShowMessage($"Ошибка: {ex.Message}"); }
    }
    protected void CloseUC() // Метод закрытия UserControl
    { UpdateData(); UCVisibility = Visibility.Collapsed; }
    #endregion
}