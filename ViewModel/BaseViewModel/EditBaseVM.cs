using CommunityToolkit.Mvvm.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel для редактирования Фондов, Описей, Ед.Хранения и Документов
    /// </summary>
    internal abstract class EditBaseVM : BaseViewModel
    {
        // Команды для добавления, сохранения, закрытия, отображения журнала изменений и навигации по элементам
        private RelayCommand? _add, _open, _save, _close, _next, _prev, _last, _first;
        // Свойства, определяющие, является ли текущий элемент первым или последним в коллекции
        private bool _isFirst, _isLast;
        public bool IsFirst
        {
            get => _isFirst;
            set { _isFirst = value; OnPropertyChanged(); }
        }
        public bool IsLast
        {
            get => _isLast;
            set { _isLast = value; OnPropertyChanged(); }
        }
        // Абстрактные методы, которые должны быть реализованы в производных классах
        protected abstract void AddItem();
        protected abstract void SaveItem();
        protected abstract void OpenLog();
        protected abstract void CloseLog();
        protected abstract void GoNext();
        protected abstract void GoPrev();
        protected abstract void GoLast();
        protected abstract void GoFirst();
        protected abstract void FillCollections();
        // Команды, определяемые через лямбда-выражения и вызывающие соответствующие методы
        public RelayCommand Close { get { return _close ??= new RelayCommand(CloseLog); } }
        public RelayCommand Next { get { return _next ??= new RelayCommand(GoNext); } }
        public RelayCommand Prev { get { return _prev ??= new RelayCommand(GoPrev); } }
        public RelayCommand Last { get { return _last ??= new RelayCommand(GoLast); } }
        public RelayCommand First { get { return _first ??= new RelayCommand(GoFirst); } }
        public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
        public RelayCommand Save { get { return _save ??= new RelayCommand(SaveItem); } }
        public RelayCommand ShowLog { get { return _open ??= new RelayCommand(OpenLog); } }
    }
}
