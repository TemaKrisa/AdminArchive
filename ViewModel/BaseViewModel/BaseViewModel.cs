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
        // Метод OnPropertyChanged вызывает событие PropertyChanged, если свойство изменилось.
        // [CallerMemberName] позволяет получить имя свойства, вызвавшего метод, без явного указания его имени.
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // Метод ShowMessage отображает диалоговое окно с сообщением об ошибке.
        private protected static void ShowMessage(string message) =>
            MessageBoxs.ShowDialog(message, "Внимание", MessageBoxs.Buttons.OK, MessageBoxs.Icon.Error);
        // Перегруженный метод ShowMessage отображает диалоговое окно с сообщением и заголовком.
        private protected static void ShowMessage(string messasge, string title) =>
            MessageBoxs.ShowDialog(messasge, title);
        // Свойство UCVisibility определяет видимость элемента управления.
        private Visibility _uCVisibility = Visibility.Collapsed;
        public Visibility UCVisibility
        { get => _uCVisibility; set { _uCVisibility = value; OnPropertyChanged(); } }
    }
}
