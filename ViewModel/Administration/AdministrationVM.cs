using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel для управления разделом управления.
    /// </summary>
    public partial class AdministrationVM : BaseViewModel
    {
        private ArchiveBdContext dc = new();

        private ObservableCollection<object> _items;
        private object _selectedItem;

        public object SelectedItem
        { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
        public ObservableCollection<object> Items
        { get => _items; set { _items = value; OnPropertyChanged(); } }

        private string type;
        public string Type { get => type; set { type = value; OnPropertyChanged(); } }
        private string navType;
        public string NavType { get => navType; set { navType = value; OnPropertyChanged(); } }

        public AdministrationVM()
        {
            Type = AdminArchive.Classes.Setting.AdminType;
            if (type == null) Type = "Категории";
            UpdateData();
        }


        private void UpdateData()
        {
            switch (Type)
            {
                case "Категории": Items = new ObservableCollection<object>(dc.Categories); break;
                case "Источники поступления": Items = new ObservableCollection<object>(dc.IncomeSources); break;
                case "Исторические периоды": Items = new ObservableCollection<object>(dc.HistoricalPeriods); break;
            }
        }
        public ICommand Navigator => new RelayCommand<string>(Navigate);

        private void Navigate(string parameter)
        {
            NavType = parameter;
            switch (NavType)
            {
                case "1": AdminArchive.Classes.Setting.AdminType = "Категории"; break;
                case "2": AdminArchive.Classes.Setting.AdminType = "Источники поступления"; break;
                case "3": AdminArchive.Classes.Setting.AdminType = "Исторические периоды"; break;
            }

            Setting.adminFrame?.Navigate(new AdministrationEditPage());
        }

        public ICommand Add => new RelayCommand(AddCommand);
        public ICommand Edit => new RelayCommand(EditCommand);
        public ICommand Remove => new RelayCommand(RemoveCommand);
        public ICommand Open => new RelayCommand(OpenCommand);
        public ICommand Close => new RelayCommand(CloseCommand);
        private void AddCommand()
        {
            Items = Type switch
            {
                "Категории" => new ObservableCollection<object>(dc.Categories),
                "Источники поступления" => new ObservableCollection<object>(dc.Activities),
                "Исторические периоды" => new ObservableCollection<object>(dc.Acesses),
            };
            UCVisibility = System.Windows.Visibility.Visible;
        }
        private void EditCommand()
        {
            if (SelectedItem != null)
            {
                try
                {
                    if (dc.Entry(SelectedItem).State == EntityState.Detached)
                        dc.Add(SelectedItem);
                    else
                        dc.Update(SelectedItem);
                    dc.SaveChanges();
                    UpdateData();
                    UCVisibility = System.Windows.Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.ToString());
                }
            }
        }

        private void OpenCommand()
        {
            if (SelectedItem != null)
                UCVisibility = System.Windows.Visibility.Visible;
        }

        private void RemoveCommand()
        {
            if (SelectedItem != null)
            {
                dc.Remove(SelectedItem);
                dc.SaveChanges();
                UpdateData();
            }
        }
        private void CloseCommand() => UCVisibility = System.Windows.Visibility.Collapsed;
    }
}
