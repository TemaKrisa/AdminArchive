using AdminArchive.Classes;
using AdminArchive.View.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace AdminArchive.ViewModel
{
    partial class ContainerVM : BaseViewModel
    {
        private ObservableCollection<MenuItem> _trayMenuItems;
        public ObservableCollection<MenuItem> TrayMenuItems
        {
            get => _trayMenuItems;
            set { _trayMenuItems = value; OnPropertyChanged(); }
        }

        public ContainerVM()
        {
            switch (AppSettings.Default.Theme)
            {
                case "Dark": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark); break;
                case "Light": Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light); break;
            }
            FrameManager.containerFrame.Navigate(new MainPage());

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home",
                    Command = SetLightTheme
                }

                        //                <ContextMenu>
                        //    <ui:MenuItem Header="Тема" >
                        //        <MenuItem Header="Светлая" Tag="Light" Command="{Binding SetLightTheme}" />
                        //        <MenuItem Header="Тёмная" Tag="Dark" Command="{Binding SetDarkTheme}"  />
                        //    </ui:MenuItem>
                        //    <MenuItem Header="Справка" Tag="Help"  />
                        //</ContextMenu>-->
            };
        }




        public ICommand SetLightTheme => new RelayCommand(LightTheme);
        public ICommand SetDarkTheme => new RelayCommand(DarkTheme);

        private void LightTheme() { Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light); }
        private void DarkTheme() { Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark); }


    }
}
