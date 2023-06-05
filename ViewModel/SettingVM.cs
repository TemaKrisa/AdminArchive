using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Wpf.Ui.Appearance;
namespace AdminArchive.ViewModel;
/// <summary>
/// ViewModel для настроек
/// </summary>
partial class SettingVM : BaseViewModel
{
    private int _selectedTheme;
    public int SelectedTheme { get { return _selectedTheme; } set { _selectedTheme = value; UpdateTheme(); } }
    public ICommand ThemeSet => new RelayCommand(UpdateTheme); // Команда для изменения темы
    public ICommand OpenHelp => new RelayCommand(Help); // Команда для открытия справки
    private void UpdateTheme()
    {
        switch (SelectedTheme)
        {
            case 0: Theme.Apply(ThemeType.Light); AppSettings.Default.Theme = "Light"; break; // Применение светлой темы
            case 1: Theme.Apply(ThemeType.Dark); AppSettings.Default.Theme = "Dark"; break; // Применение темной темы
        }
        AppSettings.Default.Save(); // Сохранение настроек
    }
    private void Help()
    {
        string appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName); // Получение пути к исполняемому файлу
        string helpFilePath = Path.Combine(appPath, "Help.chm"); // Получение пути к файлу справки
        Process.Start("hh.exe", helpFilePath); // Открытие файла справки
    }
}