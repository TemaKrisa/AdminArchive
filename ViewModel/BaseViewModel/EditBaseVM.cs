using CommunityToolkit.Mvvm.Input;

namespace AdminArchive.ViewModel
{
    internal abstract class EditBaseVM : BaseViewModel
    {
        private RelayCommand? _add, _open, _save, _close, _next, _prev, _last, _first;
        protected abstract void AddItem();
        protected abstract void SaveItem();
        protected abstract void OpenItem();
        protected abstract void OpenLog();       
        protected abstract void CloseLog();
        protected abstract void GoNext();
        protected abstract void GoPrev();
        protected abstract void GoLast();
        protected abstract void GoFirst();
        protected abstract void FillCollections();
        public RelayCommand Close { get { return _close ??= new RelayCommand(CloseLog); } }
        public RelayCommand Next { get { return _next ??= new RelayCommand(GoNext); } }
        public RelayCommand Prev { get { return _prev ??= new RelayCommand(GoPrev); } }
        public RelayCommand Last { get { return _last ??= new RelayCommand(GoLast); } }
        public RelayCommand First { get { return _first ??= new RelayCommand(GoFirst); } }
        public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
        public RelayCommand Save { get { return _save ??= new RelayCommand(SaveItem); } }
        public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } }
        public RelayCommand ShowLog { get { return _open ??= new RelayCommand(OpenLog); } }
    }
}
