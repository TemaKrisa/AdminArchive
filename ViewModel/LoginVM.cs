using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    partial class LoginVM : BaseViewModel
    {
        private string password, login;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        public string Login { get => login; set { login = value; OnPropertyChanged(); } }

        public ICommand LoginCommand => new RelayCommand(Logining);

        private async void Logining()
        {
            if (Login == null) { ShowMessage("Введите логин", "Авторизация"); }
            else if (Password == null) { ShowMessage("Введите логин", "Авторизация"); }
            else
            {
                try
                {
                    using ArchiveBdContext dc = new();
                    var user = await dc.Users.FirstOrDefaultAsync(u => u.Login == Login && u.Password == Password);
                    if (user != null)
                    {
                        Setting.Usertype = user.Role;
                        Setting.UserID = user.Id;
                        Setting.containerFrame?.Navigate(new MainPage());
                    }
                    else { ShowMessage("Неверный логин или пароль", "Авторизация"); }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Ошибка авторизации: {ex.Message}", "Авторизация");
                }
            }

        }
    }
}
