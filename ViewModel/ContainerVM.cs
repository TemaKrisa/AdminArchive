using AdminArchive.Classes;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    partial class ContainerVM : BaseViewModel
    {
        public ContainerVM()
        {
            switch (AppSettings.Default.Theme)
            {
                case "Dark": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark); break;
                case "Light": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light); break;
            }
            FrameManager.containerFrame.Navigate(new MainPage());
        }

        public ICommand SetLightTheme => new RelayCommand(LightTheme);
        public ICommand SetDarkTheme => new RelayCommand(DarkTheme);

        private void LightTheme() { Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light); }
        private void DarkTheme() { Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark); }
    }
}
