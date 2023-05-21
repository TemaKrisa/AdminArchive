using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Wpf.Ui.Appearance;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel для Setting
    /// </summary>
    partial class SettingVM : BaseViewModel
    {
        private int _selectedTheme;
        public int SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                _selectedTheme = value;
                UpdateTheme(); // Вызов метода для смены цветовой темы
            }
        }

        public SettingVM() => SelectedTheme = Theme.GetAppTheme() == ThemeType.Light ? 0 : 1; //Получение текущей темы

        public ICommand ThemeSet => new RelayCommand(UpdateTheme);

        private void UpdateTheme()
        {
            switch (SelectedTheme)
            {
                case 0: Theme.Apply(ThemeType.Light); AppSettings.Default.Theme = "Light"; break;
                case 1: Theme.Apply(ThemeType.Dark); AppSettings.Default.Theme = "Dark";  break;
            }
            AppSettings.Default.Save();
        }
    }
}
