using AdminArchive.Classes;
using AdminArchive.View.Pages;
namespace AdminArchive.ViewModel;
/// <summary> ViewModel контейнера приложения, наследующийся от базового класса BaseViewModel</summary>
partial class ContainerVM : BaseViewModel
{
    public ContainerVM()
    {
        // Применение темы оформления в зависимости от настроек приложения
        switch (AppSettings.Default.Theme)
        {
            case "Dark": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark); break;
            case "Light": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light); break;
        }
        // Навигация на страницу входа при запуске приложения
        Setting.containerFrame?.Navigate(new LoginPage());
    }
}