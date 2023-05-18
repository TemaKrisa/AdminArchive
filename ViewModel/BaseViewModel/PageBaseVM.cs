using CommunityToolkit.Mvvm.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// Базовая ViewModel для страниц фондов, документов, описей, ед.хранения 
    /// </summary>
    internal abstract class PageBaseVM : BaseViewModel 
    {
        private RelayCommand _add, _open, _edit,_back;

        public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
        public RelayCommand Edit { get { return _edit ??= new RelayCommand(EditItem); } }
        public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } }
        public RelayCommand Back { get { return _back ??= new RelayCommand(GoBack); } }

        protected abstract void AddItem();
        protected abstract void GoBack();
        protected abstract void EditItem();
        protected abstract void OpenItem();
    }
}
