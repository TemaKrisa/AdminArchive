using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    partial class LoginVM : BaseViewModel
    {
        private string _login;
        private string _password;
        public string Login
        { get => _login;  set { _login = value; OnPropertyChanged(); } }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand => new RelayCommand(Logining);

        private void Logining()
        {
            using (var dc = new ArchiveBdContext())
            {
                var user = dc.Users.FirstOrDefault(u => u.Login == Login && u.Password == Password);
                if (user != null)
                {
                    Setting.Usertype = user.Role;
                    Setting.UserID = user.Id;
                    Setting.containerFrame?.Navigate(new MainPage());
                }
                else ShowMessage("Неверный логин или пароль", "Авторизация");
            }
        }
    }
}
