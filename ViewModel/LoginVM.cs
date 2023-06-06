using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace AdminArchive.ViewModel;
/// <summary>ViewModel страницы входа в систему, наследующийся от базового класса BaseViewModel</summary>
partial class LoginVM : BaseViewModel
{
    // Поля для логина и пароля
    private string password, login;
    // Свойства для логина и пароля
    public string Password { get => password; set { password = value; OnPropertyChanged(); } }
    public string Login { get => login; set { login = value; OnPropertyChanged(); } }
    // Команда для авторизации
    public ICommand LoginCommand => new RelayCommand(Logining);
    // Метод авторизации
    private void Logining()
    {
        // Проверка на заполненность логина и пароля
        if (string.IsNullOrEmpty(Login)) { ShowMessage("Введите логин", "Авторизация"); }
        else if (string.IsNullOrEmpty(Password)) { ShowMessage("Введите пароль", "Авторизация"); }
        else
        {
            try
            {
                using ArchiveBdContext dc = new(); // Создание контекста базы данных
                var user = dc.Users.FirstOrDefault(u => u.Login == Login && u.Password == Password); // Поиск пользователя в базе данных
                if (user != null) // Если пользователь найден
                {
                    Setting.Usertype = user.Role; // Установка типа пользователя
                    Setting.UserID = user.Id; // Установка ID пользователя
                    Setting.containerFrame?.Navigate(new MainPage()); // Переход на главную страницу
                }
                else { ShowMessage("Неверный логин или пароль", "Авторизация"); } // Если пользователь не найден
            }
            catch (Exception ex) { ShowMessage($"Ошибка авторизации: {ex.Message}", "Авторизация"); } // Обработка ошибок
        }
    }
}