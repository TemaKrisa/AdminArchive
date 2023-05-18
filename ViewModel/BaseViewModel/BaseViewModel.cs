using AdminArchive.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// Базовая ViewModel
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 

        private protected static void ShowMessage(string message) => 
            MessageBoxs.ShowDialog("Внимание", message, MessageBoxs.Buttons.OK, MessageBoxs.Icon.Error); 

        private protected static void ShowMessage(string messasge, string title) =>
            MessageBoxs.ShowDialog(messasge, title);        

        private Visibility _uCVisibility = Visibility.Collapsed;
        public Visibility UCVisibility
        {
            get { return _uCVisibility; }
            set { _uCVisibility = value; OnPropertyChanged(); }
        }
    }
}
