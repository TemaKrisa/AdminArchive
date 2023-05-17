using AdminArchive.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdminArchive.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 

        private protected static void ShowMessage(string message) => 
            MessageBoxs.Show(message, "Внимание");

        private protected static void ShowMessage(string messasge, string title) =>
            MessageBoxs.Show(messasge, title);
    }
}
