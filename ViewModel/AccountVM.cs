using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace AdminArchive.ViewModel
{
    class AccountVM : BaseViewModel
    {
        #region Переменные
        private bool IsInitialized = false;
        private ObservableCollection<User> users;
        private ObservableCollection<Role> roles;
        public ObservableCollection<User> Users //Список отображающий пользователей
        { get => users; set { users = value; OnPropertyChanged(); } }

        private User curUser, selectedUser;
        public User CurUser
        { get => curUser; set { curUser = value; OnPropertyChanged(); } }

        public User SelectedUser
        { get => selectedUser; set { selectedUser = value; OnPropertyChanged(); } }

        public ObservableCollection<Role> Roles
        { get => roles; set { roles = value; OnPropertyChanged(); } }
        #endregion
        protected void AddItem() //Открытие UserControl
        {
            UCVisibility = Visibility.Visible;
            CurUser = new User() { Role = 1 };
        }

        private void UpdateData()
        {
            using ArchiveBdContext dc = new();
            Users = new ObservableCollection<User>(dc.Users.Include(u => u.RoleNavigation));
        }

        private string CurLogin;
        protected void OpenItem() //Открытие UserControl и передача выбранного пользователя
        {
            if (SelectedUser != null)
            {
                UCVisibility = Visibility.Visible;
                CurUser = selectedUser;
                CurLogin = CurUser.Login;
            }
        }
        public AccountVM()
        {
            if (!IsInitialized)
            {
                using ArchiveBdContext dc = new();
                Roles = new ObservableCollection<Role>(dc.Roles);
                UpdateData();
            }
        }

        #region UserControl
        private RelayCommand? _add, _open, _save, _close;
        public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } }
        public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
        public RelayCommand Save { get { return _save ??= new RelayCommand(SaveItem); } }
        public RelayCommand Close { get { return _close ??= new RelayCommand(CloseUC); } }

        protected void SaveItem() //Сохранение элемента из UserControl
        {
            if (string.IsNullOrWhiteSpace(CurUser.Surname)) ShowMessage("Введите фамилию!");
            else if (string.IsNullOrWhiteSpace(CurUser.Name)) ShowMessage("Введите имя!");
            else if (string.IsNullOrWhiteSpace(CurUser.Password)) ShowMessage("Введите пароль!");
            else
                try
                {
                    using ArchiveBdContext dc = new();
                    if (!dc.Users.Contains(CurUser))
                    {
                        if (!dc.Users.Any(u => u.Login == curUser.Login)) dc.Users.Add(CurUser);
                        else
                        {
                            ShowMessage("Аккаунт с таким логином уже существует!", "Добавление аккаунта");
                            return;
                        }
                        ShowMessage("Добавление прошло успешно", "Добавление аккаунта");
                    }
                    else
                    {
                        if (CurUser.Login != CurLogin && dc.Users.Any(u => u.Login == curUser.Login))
                        {
                            ShowMessage("Аккаунт с таким логином уже существует!", "Изменение аккаунта");
                            return;
                        }
                        dc.Users.Update(CurUser);
                        ShowMessage("Изменение прошло успешно", "Изменение аккаунта");
                    }
                    dc.SaveChanges();
                    UCVisibility = Visibility.Collapsed;
                    UpdateData();
                }
                catch (Exception ex) { ShowMessage($"Ошибка: {ex.Message}"); }
        }



        protected void CloseUC() => UCVisibility = Visibility.Collapsed;

        #endregion
    }
}
