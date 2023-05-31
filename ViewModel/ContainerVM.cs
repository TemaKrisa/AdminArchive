using AdminArchive.Classes;
using AdminArchive.View.Pages;
namespace AdminArchive.ViewModel;
partial class ContainerVM : BaseViewModel
{
    public ContainerVM()
    {
        switch (AppSettings.Default.Theme)
        {
            case "Dark": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark); break;
            case "Light": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light); break;
        }
        Setting.containerFrame?.Navigate(new LoginPage());
    }
}
