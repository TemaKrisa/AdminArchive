using CommunityToolkit.Mvvm.Input;

namespace AdminArchive.ViewModel
{
    internal abstract class PageBaseVM : BaseViewModel
    {
        private RelayCommand _add, _open, _edit;

        public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
        public RelayCommand Edit { get { return _edit ??= new RelayCommand(EditItem); } }
        public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } }

        protected abstract void AddItem();
        protected abstract void EditItem();
        protected abstract void OpenItem();
    }
}
